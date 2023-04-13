using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class GbsServerDeviceConfig:ServerDeviceConfig
{
    public AsvGbsServerConfig Gbs { get; set; } = new();
}
public class GbsServerDevice:ServerDevice, IGbsServerDevice
{
    public GbsServerDevice(IMavlinkV2Connection connection,
        MavlinkServerIdentity identity,
        IPacketSequenceCalculator seq,
        IScheduler scheduler,
        GbsServerDeviceConfig config,
        IAsvGbsCommon impl) : base(connection, seq, identity, config, scheduler)
    {
        var command = new CommandServer(connection,seq,identity,scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(command).DisposeItWith(Disposable);
        var gbs = new AsvGbsServer(connection, seq, identity, config.Gbs, scheduler).DisposeItWith(Disposable);
        Gbs = new AsvGbsExServer(gbs,Heartbeat,CommandLongEx,impl).DisposeItWith(Disposable);
    }

    public override void Start()
    {
        base.Start();
        Gbs.Base.Start();
    }
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public IAsvGbsServerEx Gbs { get; }
}