using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class GbsClientDeviceConfig
{
    public HeartbeatClientConfig Heartbeat { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvGbsExClientConfig Gbs { get; set; } = new();
}
public class GbsClientDevice : DisposableOnceWithCancel, IGbsClientDevice
{
    public GbsClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, 
        IScheduler scheduler,
        GbsClientDeviceConfig config)
    {
        Heartbeat = new HeartbeatClient(connection, identity, seq, scheduler, config.Heartbeat);
        Command = new CommandClient(connection, identity, seq, config.Command, scheduler).DisposeItWith(Disposable);
        Gbs = new AsvGbsClient(connection,identity,seq,scheduler).DisposeItWith(Disposable);
        GbsExClient = new AsvGbsExClient(Gbs,Heartbeat,Command,config.Gbs).DisposeItWith(Disposable);;
    }
    public IHeartbeatClient Heartbeat { get; }
    public ICommandClient Command { get; }
    public IAsvGbsClient Gbs { get; }
    public IAsvGbsExClient GbsExClient { get; }
}