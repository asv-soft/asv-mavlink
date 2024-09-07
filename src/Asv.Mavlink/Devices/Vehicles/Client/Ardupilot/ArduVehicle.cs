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
        IScheduler? scheduler = null,
        ILogger? logger = null) 
        : base(connection, identity, config, seq, scheduler,logger)
    {
    }
}