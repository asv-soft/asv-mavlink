#nullable enable
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public class AdsbClientDeviceConfig : ClientDeviceConfig
{
    public AdsbVehicleClientConfig Adsb { get; set; } = new();
}

public class AdsbClientDevice : ClientDevice, IAdsbClientDevice
{
    public AdsbClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        AdsbClientDeviceConfig config,
        IScheduler? scheduler = null) : base(connection, identity, config, seq, scheduler)
    {
        Adsb = new AdsbVehicleClient(connection, identity, seq, config.Adsb, scheduler).DisposeItWith(Disposable);
    }

    protected override string DefaultName => $"ADSB [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    public override DeviceClass Class => DeviceClass.Adsb;
    public IAdsbVehicleClient Adsb { get; }
}