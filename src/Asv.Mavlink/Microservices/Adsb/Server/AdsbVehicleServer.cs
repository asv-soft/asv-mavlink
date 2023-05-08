using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class AdsbVehicleServerConfig
{
    public int RateMs { get; set; } = 1000;
}

public class AdsbVehicleServer : MavlinkMicroserviceServer, IAdsbVehicleServer
{
    private readonly AdsbVehicleServerConfig _config;
    private readonly MavlinkPacketTransponder<AdsbVehiclePacket, AdsbVehiclePayload> _transponder;
    
    public AdsbVehicleServer(IMavlinkV2Connection connection, MavlinkServerIdentity identity, 
        IPacketSequenceCalculator seq, IScheduler rxScheduler, AdsbVehicleServerConfig config) : base("ADSB", connection, identity, seq, rxScheduler)
    {
        _config = config;
        _transponder = new MavlinkPacketTransponder<AdsbVehiclePacket, AdsbVehiclePayload>(connection, identity, seq)
            .DisposeItWith(Disposable);
    }

    public void Start()
    {
        _transponder.Start(TimeSpan.FromMilliseconds(_config.RateMs));
    }

    public void Set(Action<AdsbVehiclePayload> changeCallback)
    {
        _transponder.Set(changeCallback);
    }
}