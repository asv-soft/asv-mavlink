using System.Collections.Generic;
using System.Diagnostics;

namespace Asv.Mavlink;

public class ArduCopterClientDeviceV2: ArduVehicleClientDevice
{
    public ArduCopterClientDeviceV2(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core) 
        : base(identity, deviceConfig, core, DeviceClass.Copter)
    {
        
    }

    protected override IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices()
    {
        ICommandClient? cmd = null;
        IPositionClientEx? pos = null;
        foreach (var client in base.CreateMicroservices())
        {
            if (client is ICommandClient command)
            {
                cmd = command;
            }
            if (client is IPositionClientEx position)
            {
                pos = position;
            }
            yield return client;
        }
        
        Debug.Assert(cmd != null);
        Debug.Assert(pos != null);
        
        var mode = new ArduCopterModeClient(Heartbeat, cmd);
        yield return mode;
        yield return new ArduCopterControlClient(Heartbeat, mode,pos);
    }
}