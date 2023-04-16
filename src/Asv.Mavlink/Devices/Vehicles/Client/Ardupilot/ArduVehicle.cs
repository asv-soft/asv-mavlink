using System.Reactive.Concurrency;

namespace Asv.Mavlink;

public abstract class ArduVehicle:VehicleClient
{
    protected ArduVehicle(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config, IPacketSequenceCalculator seq, IScheduler scheduler) : base(connection, identity, config, seq, scheduler)
    {
    }

}