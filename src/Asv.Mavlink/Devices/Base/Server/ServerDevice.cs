using System.Reactive.Concurrency;
using Asv.Common;

namespace Asv.Mavlink;

public class ServerDeviceConfig
{
    public MavlinkHeartbeatServerConfig Heartbeat { get; set; } = new();
    public StatusTextLoggerConfig StatusText { get; set; } = new();
}

public class ServerDevice: DisposableOnceWithCancel, IServerDevice
{
    public ServerDevice(IMavlinkV2Connection connection,
        IPacketSequenceCalculator seq, 
        MavlinkServerIdentity identity,
        ServerDeviceConfig config,
        IScheduler scheduler)
    {
        Connection = connection;
        Seq = seq;
        Identity = identity;
        Scheduler = scheduler;
        Heartbeat = new HeartbeatServer(connection, seq, identity, config.Heartbeat, scheduler).DisposeItWith(Disposable);
        StatusText = new StatusTextServer(connection, seq, identity, config.StatusText).DisposeItWith(Disposable);
    }
    public IMavlinkV2Connection Connection { get; }
    public IPacketSequenceCalculator Seq { get; }
    public IScheduler Scheduler { get; }
    public MavlinkServerIdentity Identity { get; }
    public IStatusTextServer StatusText { get; }
    public IHeartbeatServer Heartbeat { get; }
    
    public virtual void Start()
    {
        Heartbeat.Start();
    }
}