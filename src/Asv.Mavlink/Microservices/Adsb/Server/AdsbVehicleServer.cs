using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;



public class AdsbVehicleServer(MavlinkIdentity identity, ICoreServices core)
    : MavlinkMicroserviceServer("ADSB", identity, core), IAdsbVehicleServer
{
    public Task Send(Action<AdsbVehiclePayload> fillCallback)
    {
        return InternalSend<AdsbVehiclePacket>(p => fillCallback(p.Payload));
    }
}