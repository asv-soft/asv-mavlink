using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Microsoft.Extensions.Logging;

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
        MavlinkIdentity identity,
        ServerDeviceConfig config,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null)
    {
        Connection = connection;
        Seq = seq;
        Identity = identity;
        Scheduler = scheduler ?? System.Reactive.Concurrency.Scheduler.Default;
        Heartbeat =
            new HeartbeatServer(connection, seq, identity, config.Heartbeat, timeProvider, scheduler, logFactory).DisposeItWith(Disposable);
        StatusText = new StatusTextServer(connection, seq, identity, config.StatusText, timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
    }
    public IMavlinkV2Connection Connection { get; }
    public IPacketSequenceCalculator Seq { get; }
    public IScheduler Scheduler { get; }
    public MavlinkIdentity Identity { get; }
    public IStatusTextServer StatusText { get; }
    public IHeartbeatServer Heartbeat { get; }
    
    public virtual void Start()
    {
        Heartbeat.Start();
    }
}