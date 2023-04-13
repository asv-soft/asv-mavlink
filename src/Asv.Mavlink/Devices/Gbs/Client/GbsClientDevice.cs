using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public class GbsClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
}
public class GbsClientDevice : ClientDevice, IGbsClientDevice
{
    public GbsClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, 
        IScheduler scheduler,
        GbsClientDeviceConfig config) : base(connection, identity,config, seq, scheduler)
    {
        Command = new CommandClient(connection, identity, seq, config.Command, scheduler).DisposeItWith(Disposable);
        var gbs = new AsvGbsClient(connection,identity,seq,scheduler).DisposeItWith(Disposable);
        Gbs = new AsvGbsExClient(gbs,Heartbeat,Command).DisposeItWith(Disposable);
    }
    public ICommandClient Command { get; }
    public IAsvGbsExClient Gbs { get; }
    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("GBS");
    }

    public override DeviceClass Class => DeviceClass.GbsRtk;
}