using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink;

public class ClientDeviceConfig
{
    public HeartbeatClientConfig Heartbeat { get; set; } = new();
    public int RequestInitDataDelayAfterFailMs { get; set; } = 5000;
}

public abstract class ClientDevice: DisposableOnceWithCancel, IClientDevice
{
    private readonly ClientDeviceConfig _config;
    private readonly RxValue<InitState> _onInit;
    private readonly RxValue<string> _name;
    private bool _needToRequestAgain = true;
    private int _isRequestInfoIsInProgressOrAlreadySuccess;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    

    protected ClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        ClientDeviceConfig config,
        IPacketSequenceCalculator seq, 
        IScheduler scheduler)
    {
        Connection = connection;
        _config = config;
        Identity = identity;
        Seq = seq;
        Scheduler = scheduler;

        Heartbeat = new HeartbeatClient(connection,identity,seq,scheduler,config.Heartbeat)
            .DisposeItWith(Disposable);
        
        _onInit = new RxValue<InitState>(InitState.WaitConnection)
            .DisposeItWith(Disposable);
        Heartbeat.Link.DistinctUntilChanged()
            .Where(_ => _ == LinkState.Disconnected)
            .Subscribe(_ => _needToRequestAgain = true).DisposeItWith(Disposable);
        
        Heartbeat.Link.DistinctUntilChanged().Where(_ => _needToRequestAgain).Where(_ => _ == LinkState.Connected)
            // only one time
            .Delay(TimeSpan.FromMilliseconds(100)).Subscribe(_ => TryReconnect()).DisposeItWith(Disposable);
        StatusText = new StatusTextClient(connection,identity,seq,scheduler).DisposeItWith(Disposable);
        _name = new RxValue<string>().DisposeItWith(Disposable);
    }

    private async void TryReconnect()
    {
        if (Interlocked.CompareExchange(ref _isRequestInfoIsInProgressOrAlreadySuccess,1,0) == 1) return;
        _onInit.OnNext(InitState.InProgress);
        try
        {
            _name.OnNext(await GetCustomName(DisposeCancel).ConfigureAwait(false));
            await InternalInit().ConfigureAwait(false);
            _onInit.OnNext(InitState.Complete);
            _needToRequestAgain = false;
        }
        catch (Exception e)
        {
            if (IsDisposed) return; // no need to replay since the instance was already disposed
            Logger.Error( $"Error to init device:{e.Message}");
            _onInit.OnNext(InitState.Failed);
            Observable.Timer(TimeSpan.FromMilliseconds(_config.RequestInitDataDelayAfterFailMs))
                .Subscribe(_ => TryReconnect()).DisposeItWith(Disposable);
        }
        finally
        {
            Interlocked.Exchange(ref _isRequestInfoIsInProgressOrAlreadySuccess, 0);
        }
            
    }

    protected abstract Task InternalInit();
    protected abstract Task<string> GetCustomName(CancellationToken cancel);

    public abstract DeviceClass Class { get; }
    
    public IRxValue<string> Name => _name;
    public IHeartbeatClient Heartbeat { get; }
    public IStatusTextClient StatusText { get; }
    public ushort FullId => Heartbeat.FullId;
    public IMavlinkV2Connection Connection { get; }
    public MavlinkClientIdentity Identity { get; }
    public IPacketSequenceCalculator Seq { get; }
    public IScheduler Scheduler { get; }
    public IRxValue<InitState> OnInit => _onInit;
}