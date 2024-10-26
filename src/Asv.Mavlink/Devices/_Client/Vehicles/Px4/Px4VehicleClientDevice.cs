namespace Asv.Mavlink;

public class Px4VehicleClientDevice:VehicleClientDevice
{
    public Px4VehicleClientDevice(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core, DeviceClass @class) 
        : base(identity, deviceConfig, core, @class)
    {
        
    }
}