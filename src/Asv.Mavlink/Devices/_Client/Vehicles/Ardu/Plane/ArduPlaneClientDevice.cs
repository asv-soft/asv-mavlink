using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public class ArduPlaneClientDevice: ArduVehicleClientDevice
{
    private readonly VehicleClientDeviceConfig _deviceConfig;
    private bool _quadPlaneEnabled;

    public ArduPlaneClientDevice(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core) 
        : base(identity, deviceConfig, core, DeviceClass.Copter)
    {
        _deviceConfig = deviceConfig;
    }

    protected override async Task InitBeforeMicroservices(CancellationToken cancel)
    {
        var param = new ParamsClient(Identity,_deviceConfig.Params,Core);
        var value = await param.Read("Q_ENABLE",cancel).ConfigureAwait(false);
        _quadPlaneEnabled = value.ParamValue != 0;
        await base.InitBeforeMicroservices(cancel).ConfigureAwait(false);
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
        if (_quadPlaneEnabled)
        {
            var mode = new ArduQuadPlaneModeClient(Heartbeat, cmd);
            yield return mode;
            yield return new ArduQuadPlaneControlClient(Heartbeat, mode,pos);    
        }
        else
        {
            var mode = new ArduPlaneModeClient(Heartbeat, cmd);
            yield return mode;
            yield return new ArduPlaneControlClient(Heartbeat, mode,pos);
        }
        
    }
}