using System;
using System.Reactive.Concurrency;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public abstract class ArduVehicle:VehicleClient
{
    protected ArduVehicle(
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        VehicleClientConfig config, 
        IPacketSequenceCalculator seq, 
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null)
        : base(connection, identity, config, seq,timeProvider, scheduler, logFactory)
    {
    }
}