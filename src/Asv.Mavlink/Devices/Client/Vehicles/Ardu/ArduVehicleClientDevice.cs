using System.Collections.Immutable;
using Asv.IO;

namespace Asv.Mavlink;

public class ArduVehicleClientDevice:VehicleClientDevice
{
    protected ArduVehicleClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        
    }
}