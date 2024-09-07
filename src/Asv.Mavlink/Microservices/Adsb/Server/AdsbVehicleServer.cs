using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;



public class AdsbVehicleServer : MavlinkMicroserviceServer, IAdsbVehicleServer
{
    public AdsbVehicleServer(
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq, 
        IScheduler? rxScheduler = null,
        ILogger? logger = null) 
        : base("ADSB", connection, identity, seq, rxScheduler,logger)
    {
    }
    
    public Task Send(Action<AdsbVehiclePayload> fillCallback)
    {
        return InternalSend<AdsbVehiclePacket>(p => fillCallback(p.Payload));
    }
}