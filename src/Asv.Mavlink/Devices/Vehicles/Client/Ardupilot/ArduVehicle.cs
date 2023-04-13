using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public abstract class ArduVehicle:VehicleClient
{
    protected ArduVehicle(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config, IPacketSequenceCalculator seq, IScheduler scheduler) : base(connection, identity, config, seq, scheduler)
    {
    }

}