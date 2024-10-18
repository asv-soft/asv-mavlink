using System;
using System.Reactive.Concurrency;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public abstract class Px4VehicleClient(
    IMavlinkV2Connection connection,
    MavlinkClientIdentity identity,
    VehicleClientConfig config,
    IPacketSequenceCalculator seq,
    TimeProvider? timeProvider = null,
    IScheduler? scheduler = null,
    ILoggerFactory? logFactory = null)
    : VehicleClient(connection, identity, config, seq,timeProvider, scheduler,logFactory);