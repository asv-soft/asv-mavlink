using System.Collections.Immutable;
using Asv.IO;

namespace Asv.Mavlink;

public class Px4PlaneClientDevice(
    MavlinkClientDeviceId identity,
    VehicleClientDeviceConfig config,
    ImmutableArray<IClientDeviceExtender> extenders,
    IMavlinkContext core)
    : Px4VehicleClientDevice(identity, config, extenders, core);