namespace Asv.Mavlink;

public class Px4VehicleClientV2:VehicleClientV2
{
    protected Px4VehicleClientV2(MavlinkClientIdentity identity, VehicleClientV2Config config, ICoreServices core, DeviceClass @class) 
        : base(identity, config, core, @class)
    {
        
    }
}