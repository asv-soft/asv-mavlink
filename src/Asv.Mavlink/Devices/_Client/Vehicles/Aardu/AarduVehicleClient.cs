namespace Asv.Mavlink;

public class ArduVehicleClient:VehicleClientV2
{
    protected ArduVehicleClient(MavlinkClientIdentity identity, VehicleClientV2Config config, ICoreServices core, DeviceClass @class) 
        : base(identity, config, core, @class)
    {
        
    }
}