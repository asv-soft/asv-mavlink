#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class ClientDeviceConfig
{
    public HeartbeatClientConfig Heartbeat { get; set; } = new();
    public int RequestInitDataDelayAfterFailMs { get; set; } = 5000;
}

public abstract class ClientDevice: DisposableOnceWithCancel, IClientDevice
{
    private readonly ClientDeviceConfig _config;
    private readonly IScheduler? _scheduler;
    private readonly RxValueBehaviour<InitState> _onInit;
    private readonly RxValueBehaviour<string> _name;
    private bool _needToRequestAgain = true;
    private int _isRequestInfoIsInProgressOrAlreadySuccess;
    private readonly ILogger _loggerBase;

    protected ClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        ClientDeviceConfig config,
        IPacketSequenceCalculator seq,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null, 
        ILoggerFactory? logFactory = null)
    {
        logFactory ??= NullLoggerFactory.Instance;
        _loggerBase = logFactory.CreateLogger<ClientDevice>();
        Connection = connection;
        _config = config;
        _scheduler = scheduler;
        Identity = identity;
        Seq = seq;

        Heartbeat = new HeartbeatClient(connection,identity,seq,config.Heartbeat,timeProvider, scheduler, logFactory)
            .DisposeItWith(Disposable);
        
        _onInit = new RxValueBehaviour<InitState>(InitState.WaitConnection)
            .DisposeItWith(Disposable);
        Heartbeat.Link.DistinctUntilChanged()
            .Where(s => s == LinkState.Disconnected)
            .Subscribe(_ => _needToRequestAgain = true).DisposeItWith(Disposable);
        
        if (scheduler != null)
        {
            Heartbeat.Link.DistinctUntilChanged()
                .Where(_ => _needToRequestAgain)
                .Where(s => s == LinkState.Connected)
                // only one time
                .Delay(TimeSpan.FromMilliseconds(100),scheduler)
                .Subscribe(_ => TryReconnect())
                .DisposeItWith(Disposable);
        }
        else
        {
            Heartbeat.Link
                .DistinctUntilChanged()
                .Where(_ => _needToRequestAgain)
                .Where(s => s == LinkState.Connected)
                // only one time
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => TryReconnect())
                .DisposeItWith(Disposable);
        }
       
        StatusText = new StatusTextClient(connection,identity,seq).DisposeItWith(Disposable);
        _name = new RxValueBehaviour<string>(string.Empty).DisposeItWith(Disposable);
    }

    protected abstract string DefaultName { get; }
    
    private async void TryReconnect()
    {
        if (Interlocked.CompareExchange(ref _isRequestInfoIsInProgressOrAlreadySuccess,1,0) == 1) return;
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
            if (IsDisposed) return; // no need to replay since the instance was already disposed
            _loggerBase.ZLogError(e, $"Error to init device:{e.Message}");
            _onInit.OnNext(InitState.Failed);
            if (_scheduler != null)
            {
                Observable.Timer(TimeSpan.FromMilliseconds(_config.RequestInitDataDelayAfterFailMs),_scheduler)
                    .Subscribe(_ => TryReconnect()).DisposeItWith(Disposable);    
            }
            else
            {
                Observable.Timer(TimeSpan.FromMilliseconds(_config.RequestInitDataDelayAfterFailMs))
                    .Subscribe(_ => TryReconnect()).DisposeItWith(Disposable);
            }
            
        }
        finally
        {
            Interlocked.Exchange(ref _isRequestInfoIsInProgressOrAlreadySuccess, 0);
        }
            
    }

    protected abstract Task InternalInit();
    public abstract DeviceClass Class { get; }
    public IRxValue<string> Name => _name;
    protected IRxEditableValue<string> EditableName => _name;
    public IHeartbeatClient Heartbeat { get; }
    public IStatusTextClient StatusText { get; }
    public ushort FullId => Heartbeat.FullId;
    public IMavlinkV2Connection Connection { get; }
    public MavlinkClientIdentity Identity { get; }
    public IPacketSequenceCalculator Seq { get; }
    public IRxValue<InitState> OnInit => _onInit;
}