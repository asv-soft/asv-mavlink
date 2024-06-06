using System.Reactive.Concurrency;

namespace Asv.Mavlink;

public abstract class Px4VehicleClient(
    IMavlinkV2Connection connection,
    MavlinkClientIdentity identity,
    VehicleClientConfig config,
    IPacketSequenceCalculator seq,
    IScheduler scheduler)
    : VehicleClient(connection, identity, config, seq, scheduler);