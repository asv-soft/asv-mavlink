#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using R3;
using ZLogger;
using ObservableExtensions = System.ObservableExtensions;

namespace Asv.Mavlink;

public class ClientDeviceConfig
{
    public HeartbeatClientConfig Heartbeat { get; set; } = new();
    public int RequestInitDataDelayAfterFailMs { get; set; } = 5000;
}

public abstract class ClientDevice: IClientDevice,IDisposable, IAsyncDisposable
{
    private readonly ClientDeviceConfig _config;
    private readonly RxValueBehaviour<InitState> _onInit;
    private readonly RxValueBehaviour<string> _name;
    private bool _needToRequestAgain = true;
    private int _isTryReconnectInProgress;
    private readonly ILogger _loggerBase;
    private readonly HeartbeatClient _heartbeat;
    private readonly StatusTextClient _statusText;
    private readonly IDisposable _subscribe1;
    private readonly IDisposable _subscribe2;
    private ITimer? _reconnectionTimer;

    protected ClientDevice(MavlinkClientIdentity identity, ClientDeviceConfig config, ICoreServices core)
    {
        _loggerBase = core.Log.CreateLogger<ClientDevice>();
        _config = config;
        
        Identity = identity;
        Core = core;
        
        _heartbeat = new HeartbeatClient(identity, config.Heartbeat, core);
        _onInit = new RxValueBehaviour<InitState>(InitState.WaitConnection);
        _subscribe1 = Heartbeat.Link.DistinctUntilChanged()
            .Where(s => s == LinkState.Disconnected)
            .Subscribe(_ => _needToRequestAgain = true);

        _subscribe2 = Heartbeat.Link.DistinctUntilChanged()
            .Where(_ => _needToRequestAgain)
            .Where(s => s == LinkState.Connected)
            .Subscribe(_ => TryReconnect(null));
        _statusText = new StatusTextClient(identity,core);
        _name = new RxValueBehaviour<string>(string.Empty);
    }

    protected virtual string DefaultName => $"{Class:G} [{Identity.Target.SystemId:00},{Identity.Target.ComponentId:00}]";
    
    private async void TryReconnect(object? state)
    {
        if (Interlocked.Exchange(ref _isTryReconnectInProgress, 1) != 0)
        {
            _loggerBase.LogTrace("Skip double reconnect");
            return;
        }
        _onInit.OnNext(InitState.InProgress);
        try
        {
            await InternalInit().ConfigureAwait(false);
            _name.OnNext(DefaultName);
            _onInit.OnNext(InitState.Complete);
            _needToRequestAgain = false;
        }
        catch (Exception e)
        {
            _loggerBase.ZLogError(e, $"Error to init device:{e.Message}");
            _onInit.OnNext(InitState.Failed);

            _reconnectionTimer = Core.TimeProvider.CreateTimer(TryReconnect, null,
                TimeSpan.FromMilliseconds(_config.RequestInitDataDelayAfterFailMs),Timeout.InfiniteTimeSpan);
        }
        finally
        {
            Interlocked.Exchange(ref _isTryReconnectInProgress, 0);
        }
    }

    protected abstract Task InternalInit();
    public ICoreServices Core { get; }
    public abstract DeviceClass Class { get; }
    public IRxValue<string> Name => _name;
    protected IRxEditableValue<string> EditableName => _name;
    public IHeartbeatClient Heartbeat => _heartbeat;
    public IStatusTextClient StatusText => _statusText;
    public ushort FullId => Heartbeat.FullId;
    public MavlinkClientIdentity Identity { get; }
    public IRxValue<InitState> OnInit => _onInit;

    #region Dispose and AsyncDispose

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onInit.Dispose();
            _name.Dispose();
            _heartbeat.Dispose();
            _statusText.Dispose();
            _subscribe1.Dispose();
            _subscribe2.Dispose();
            _reconnectionTimer?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onInit).ConfigureAwait(false);
        await CastAndDispose(_name).ConfigureAwait(false);
        await CastAndDispose(_heartbeat).ConfigureAwait(false);
        await CastAndDispose(_statusText).ConfigureAwait(false);
        await CastAndDispose(_subscribe1).ConfigureAwait(false);
        await CastAndDispose(_subscribe2).ConfigureAwait(false);
        if (_reconnectionTimer != null) await _reconnectionTimer.DisposeAsync().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    #endregion
}