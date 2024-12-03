using System.Collections.Immutable;
using Asv.IO;

namespace Asv.Mavlink;

public class Px4VehicleClientDevice:VehicleClientDevice
{
    public Px4VehicleClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        ICoreServices core) 
        : base(identity,config,extenders,core)
    {
        
    }
}