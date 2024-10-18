using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;

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
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null)
        : base(connection, seq, identity, config, timeProvider, scheduler, logFactory)
    {
        var command = new CommandServer(connection,seq,identity,timeProvider, scheduler, logFactory).DisposeItWith(Disposable);
        Adsb = new AdsbVehicleServer(connection, identity, seq, timeProvider, scheduler, logFactory);
        Heartbeat.Set(p => p.Type = MavType.MavTypeAdsb);
    }

    public IAdsbVehicleServer Adsb { get; }
}