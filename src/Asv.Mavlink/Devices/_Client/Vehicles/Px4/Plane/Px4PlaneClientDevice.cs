namespace Asv.Mavlink;

public class Px4PlaneClientDevice:Px4VehicleClientDevice
{
    public Px4PlaneClientDevice(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core) 
        : base(identity, deviceConfig, core, DeviceClass.Plane)
    {
        
    }
}