using System.Collections.Generic;
using System.Diagnostics;

namespace Asv.Mavlink;

public class Px4CopterClientDevice:Px4VehicleClientDevice
{
    public Px4CopterClientDevice(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core) 
        : base(identity, deviceConfig, core, DeviceClass.Copter)
    {
        
    }
    
    protected override IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices()
    {
        ICommandClient? cmd = null;
        foreach (var client in base.CreateMicroservices())
        {
            if (client is ICommandClient command)
            {
                cmd = command;
            }
            yield return client;
        }
        Debug.Assert(cmd != null);
        yield return new Px4CopterModeClient(Heartbeat, cmd);
    }
}