using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public class ArduCopterClientDevice: ArduVehicleClientDevice
{
    public ArduCopterClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        ICoreServices core) 
        : base(identity,config,extenders,core)
    {
        
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        ICommandClient? cmd = null;
        IPositionClientEx? pos = null;
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
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
        if (cmd == null) yield break;
        if (pos == null) yield break;
        
        var mode = new ArduCopterModeClient(Heartbeat, cmd);
        yield return mode;
        yield return new ArduCopterControlClient(Heartbeat, mode, pos);
    }
}