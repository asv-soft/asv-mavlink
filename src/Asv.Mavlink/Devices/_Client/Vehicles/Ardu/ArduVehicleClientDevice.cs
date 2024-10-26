namespace Asv.Mavlink;

public class ArduVehicleClientDevice:VehicleClientDevice
{
    protected ArduVehicleClientDevice(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core, DeviceClass @class) 
        : base(identity, deviceConfig, core, @class)
    {
        
    }
}