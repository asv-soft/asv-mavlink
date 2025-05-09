﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;



public class AdsbVehicleServer(MavlinkIdentity identity, IMavlinkContext core)
    : MavlinkMicroserviceServer(AdsbHelper.MicroserviceName, identity, core), IAdsbVehicleServer
{
    public ValueTask Send(Action<AdsbVehiclePayload> fillCallback, CancellationToken cancel)
    {
        return InternalSend<AdsbVehiclePacket>(p => fillCallback(p.Payload), cancel);
    }
}