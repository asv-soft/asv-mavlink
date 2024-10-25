#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

public class ClientDeviceBaseConfig
{
    public int RequestInitDataDelayAfterFailMs { get; set; } = 5000;
    public HeartbeatClientConfig Heartbeat { get; set; } = new();
   
}

public abstract class ClientDevice: IClientDevice, IDisposable,IAsyncDisposable
{
    private readonly ClientDeviceBaseConfig _config;
    private readonly ICoreServices _core;
    private readonly ReactiveProperty<InitState> _initState;
    private readonly ReactiveProperty<string> _name;
    private bool _needToRequestAgain = true;
    private int _isTryReconnectInProgress;
    private readonly ILogger _loggerBase;
    private readonly HeartbeatClient _heartbeat;
    private readonly IDisposable _subscribe1;
    private readonly IDisposable _subscribe2;
    private ITimer? _reconnectionTimer;
    private readonly CancellationTokenSource _disposableCancel = new();
    private ImmutableArray<IMavlinkMicroserviceClient> _microservices = ImmutableArray<IMavlinkMicroserviceClient>.Empty;

    protected ClientDevice(MavlinkClientIdentity identity, ClientDeviceBaseConfig config, ICoreServices core, DeviceClass deviceClass)
    {
        _loggerBase = core.Log.CreateLogger<ClientDevice>();
        _config = config;
        _core = core;
        Class = deviceClass;

        Identity = identity;
        _heartbeat = new HeartbeatClient(identity, config.Heartbeat, core);
        _initState = new ReactiveProperty<InitState>(Mavlink.InitState.WaitConnection);
        _subscribe1 = Heartbeat.Link.DistinctUntilChanged()
            .Where(s => s == LinkState.Disconnected)
            .Subscribe(_ => _needToRequestAgain = true);

        _subscribe2 = Heartbeat.Link.DistinctUntilChanged()
            .Where(_ => _needToRequestAgain)
            .Where(s => s == LinkState.Connected)
            .Subscribe(_ => TryReconnect(null));
        _name = new ReactiveProperty<string>($"{Class:G}[{Identity.Target.SystemId:00},{Identity.Target.ComponentId:00}]");
    }

    protected ICoreServices Core => _core;
    
    private async void TryReconnect(object? state)
    {
        if (Interlocked.Exchange(ref _isTryReconnectInProgress, 1) != 0)
        {
            _loggerBase.LogTrace("Skip double reconnect");
            return;
        }
        _initState.OnNext(Mavlink.InitState.InProgress);
        List<IMavlinkMicroserviceClient> microservices = new();
        try
        {
            await InitBeforeMicroservices(DisposableCancel).ConfigureAwait(false);
            
            foreach (var client in CreateMicroservices())
            {
                microservices.Add(client);
                await client.Init(DisposableCancel).ConfigureAwait(false);
            }
            _initState.OnNext(Mavlink.InitState.Complete);
            _needToRequestAgain = false;
            microservices.Add(_heartbeat);
            _microservices = [..microservices];
            await InitAfterMicroservices(DisposableCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _loggerBase.ZLogError(e, $"Error to init device:{e.Message}");
            _initState.OnNext(Mavlink.InitState.Failed);
            _reconnectionTimer = _core.TimeProvider.CreateTimer(TryReconnect, null,
                TimeSpan.FromMilliseconds(_config.RequestInitDataDelayAfterFailMs),Timeout.InfiniteTimeSpan);
            DisposeMicroservices(microservices);
        }
        finally
        {
            Interlocked.Exchange(ref _isTryReconnectInProgress, 0);
        }
    }
    protected CancellationToken DisposableCancel => _disposableCancel.Token;
    /// <summary>
    /// This method is called before the microservices are created
    /// Can be called multiple times, if initialization fails.
    /// </summary>
    protected abstract Task InitBeforeMicroservices(CancellationToken cancel);
    /// <summary>
    /// Defines an abstract method that should be implemented to create a collection of microservices associated with the client device.
    /// Can be called multiple times, if initialization fails.
    /// </summary>
    /// <returns>
    /// An enumerable collection of IMavlinkMicroserviceClient instances representing the microservices.
    /// </returns>
    protected abstract IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices();

    /// <summary>
    /// This method is called after the microservices have been created and initialized.
    /// Can be called multiple times, if initialization fails.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected abstract Task InitAfterMicroservices(CancellationToken cancel);

    public DeviceClass Class { get; }
    public ReadOnlyReactiveProperty<string> Name => _name;
    protected void UpdateDeviceName(string name)
    {
        _name.OnNext(name);
    }
    public IHeartbeatClient Heartbeat => _heartbeat;
    public IEnumerable<IMavlinkMicroserviceClient> Microservices => _microservices;
    public MavlinkClientIdentity Identity { get; }
    public ReadOnlyReactiveProperty<InitState> InitState => _initState;

    #region Dispose and AsyncDispose
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeMicroservices(_microservices);
            _initState.Dispose();
            _name.Dispose();
            _heartbeat.Dispose();
            _subscribe1.Dispose();
            _subscribe2.Dispose();
            _reconnectionTimer?.Dispose();
            _disposableCancel.Cancel(false);
            _disposableCancel.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await DisposeAsyncMicroservices(_microservices).ConfigureAwait(false);
        await CastAndDispose(_initState).ConfigureAwait(false);
        await CastAndDispose(_name).ConfigureAwait(false);
        await CastAndDispose(_heartbeat).ConfigureAwait(false);
        await CastAndDispose(_subscribe1).ConfigureAwait(false);
        await CastAndDispose(_subscribe2).ConfigureAwait(false);
        if (_reconnectionTimer != null) await _reconnectionTimer.DisposeAsync().ConfigureAwait(false);
        _disposableCancel.Cancel(false);
        await CastAndDispose(_disposableCancel).ConfigureAwait(false);
    }

    private static async ValueTask CastAndDispose(IDisposable resource)
    {
        if (resource is IAsyncDisposable resourceAsyncDisposable)
            await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            resource.Dispose();
    }

    private void DisposeMicroservices(IEnumerable<IMavlinkMicroserviceClient> microservices)
    {
        foreach (var microservice in microservices)
        {
            try
            {
                (microservice as IDisposable)?.Dispose();
            }
            catch (Exception e)
            {
                _loggerBase.ZLogError(e, $"Error to dispose microservice:{e.Message}");
            }
        }
    }
    private async ValueTask DisposeAsyncMicroservices(IEnumerable<IMavlinkMicroserviceClient> microservices)
    {
        foreach (var microservice in microservices)
        {
            try
            {
                if (microservice is IDisposable disposable)
                {
                    await CastAndDispose(disposable).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _loggerBase.ZLogError(e, $"Error to dispose microservice:{e.Message}");
            }
        }
    }
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
    
    #endregion
}