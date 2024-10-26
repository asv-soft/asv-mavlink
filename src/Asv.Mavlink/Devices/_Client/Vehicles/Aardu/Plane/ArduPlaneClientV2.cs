using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public class ArduPlaneClientV2: ArduVehicleClient
{
    protected ArduPlaneClientV2(MavlinkClientIdentity identity, VehicleClientV2Config config, ICoreServices core) 
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
        yield return new ArduPlaneModeClient(Heartbeat, cmd);
    }
}