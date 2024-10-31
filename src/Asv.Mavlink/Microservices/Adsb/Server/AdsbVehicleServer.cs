using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;



public class AdsbVehicleServer(MavlinkIdentity identity, ICoreServices core)
    : MavlinkMicroserviceServer("ADSB", identity, core), IAdsbVehicleServer
{
    public Task Send(Action<AdsbVehiclePayload> fillCallback)
    {
        return InternalSend<AdsbVehiclePacket>(p => fillCallback(p.Payload));
    }
}