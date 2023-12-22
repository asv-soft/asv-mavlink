using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class AdsbServerDeviceConfig : ServerDeviceConfig
{
    
}

public class AdsbServerDevice : ServerDevice, IAdsbServerDevice
{
    public AdsbServerDevice(IMavlinkV2Connection connection, 
        IPacketSequenceCalculator seq, 
        MavlinkIdentity identity, 
        AdsbServerDeviceConfig config, 
        IScheduler scheduler) : base(connection, seq, identity, config, scheduler)
    {
        var command = new CommandServer(connection,seq,identity,scheduler).DisposeItWith(Disposable);
        Adsb = new AdsbVehicleServer(connection, identity, seq, scheduler);
        Heartbeat.Set(_ => _.Type = MavType.MavTypeAdsb);
    }

    public IAdsbVehicleServer Adsb { get; }
}