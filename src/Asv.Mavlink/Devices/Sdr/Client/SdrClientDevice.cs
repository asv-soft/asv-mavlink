using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.Sdr;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;
using MavCmd = Asv.Mavlink.V2.AsvSdr.MavCmd;

namespace Asv.Mavlink;

public class SdrClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvSdrClientExConfig SdrEx { get; set; } = new();
}
public class SdrClientDevice : ClientDevice, ISdrClientDevice
{
    public SdrClientDevice(IMavlinkV2Connection connection, MavlinkClientIdentity identity, SdrClientDeviceConfig config, IPacketSequenceCalculator seq, IScheduler scheduler) : base(connection, identity, config, seq, scheduler)
    {
        Command = new CommandClient(connection, identity, seq, config.Command, scheduler).DisposeItWith(Disposable);
        Sdr = new AsvSdrClientEx(new AsvSdrClient(connection, identity, seq, scheduler), Heartbeat, Command,config.SdrEx).DisposeItWith(Disposable);
    }
    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("SdrPayload");
    }
    public override DeviceClass Class => DeviceClass.SdrPayload;
    public IAsvSdrClientEx Sdr { get; }
    public ICommandClient Command { get; }

    
}