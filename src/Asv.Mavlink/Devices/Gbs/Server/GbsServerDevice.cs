using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class GbsServerDeviceConfig
{
    public MavlinkHeartbeatServerConfig Heartbeat { get; set; } = new();
    public AsvGbsServerConfig Gbs { get; set; } = new();
}
public class GbsServerDevice:DisposableOnceWithCancel, IGbsServerDevice
{
    public GbsServerDevice(IAsvGbsCommon impl, IMavlinkV2Connection connection,
        IPacketSequenceCalculator seq, 
        MavlinkServerIdentity identity,
        GbsServerDeviceConfig config,
        IScheduler scheduler )
    {
        Heartbeat = new HeartbeatServer(connection,seq,identity,config.Heartbeat,scheduler).DisposeItWith(Disposable);
        Command = new CommandServer(connection,seq,identity,scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(Command).DisposeItWith(Disposable);
        CommandIntEx = new CommandIntServerEx(Command).DisposeItWith(Disposable);
        var gbs = new AsvGbsServer(connection, seq, identity, config.Gbs, scheduler).DisposeItWith(Disposable);
        Gbs = new AsvGbsExServer(gbs,Heartbeat,CommandLongEx,impl).DisposeItWith(Disposable);
    }

    public void Start()
    {
        Heartbeat.Start();
        Gbs.Base.Start();
    }
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public ICommandServerEx<CommandIntPacket> CommandIntEx { get; }
    public IHeartbeatServer Heartbeat { get; }
    public ICommandServer Command { get; }
    public IAsvGbsServerEx Gbs { get; }
}