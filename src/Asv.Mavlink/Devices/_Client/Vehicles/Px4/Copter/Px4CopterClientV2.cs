using System.Collections.Generic;
using System.Diagnostics;

namespace Asv.Mavlink;

public class Px4CopterClientV2:Px4VehicleClientV2
{
    public Px4CopterClientV2(MavlinkClientIdentity identity, VehicleClientV2Config config, ICoreServices core) 
        : base(identity, config, core, DeviceClass.Copter)
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