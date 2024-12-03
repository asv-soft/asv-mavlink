using System.Collections.Immutable;
using Asv.IO;

namespace Asv.Mavlink;

public class Px4PlaneClientDevice:Px4VehicleClientDevice
{
    public Px4PlaneClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        ICoreServices core) 
        : base(identity,config,extenders,core)
    {
        
    }
}