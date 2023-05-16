using System.Reactive.Concurrency;
using System.Threading;
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
        IScheduler scheduler,
        AdsbClientDeviceConfig config) : base(connection, identity, config, seq, scheduler)
    {
        Adsb = new AdsbVehicleClient(connection, identity, seq, config.Adsb, scheduler).DisposeItWith(Disposable);
    }

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("ADSB");
    }

    public override DeviceClass Class => DeviceClass.Adsb;
    public IAdsbVehicleClient Adsb { get; }
}