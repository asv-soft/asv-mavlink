using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public class ArduPlaneClientDevice: ArduVehicleClientDevice
{

    public ArduPlaneClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        ICommandClient? cmd = null;
        IPositionClientEx? pos = null;
        IParamsClient? param = null;
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            if (microservice is IParamsClient parameters)
            {
                param = parameters;
            }
            if (microservice is ICommandClient command)
            {
                cmd = command;
            }
            if (microservice is IPositionClientEx position)
            {
                pos = position;
            }
            yield return microservice;
        }
        if (param == null) yield break;
        if (cmd == null) yield break;
        if (pos == null) yield break;
        
        var value = await param.Read("Q_ENABLE",cancel).ConfigureAwait(false);
        var quadPlaneEnabled = value.ParamValue != 0;
        if (quadPlaneEnabled)
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