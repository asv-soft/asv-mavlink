using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;



public class AdsbVehicleServer : MavlinkMicroserviceServer, IAdsbVehicleServer
{
    public AdsbVehicleServer(IMavlinkV2Connection connection, MavlinkServerIdentity identity, 
        IPacketSequenceCalculator seq, IScheduler rxScheduler) : base("ADSB", connection, identity, seq, rxScheduler)
    {
    }
    
    public Task Send(Action<AdsbVehiclePayload> fillCallback)
    {
        return InternalSend<AdsbVehiclePacket>(_ => fillCallback(_.Payload));
    }
}