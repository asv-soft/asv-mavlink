using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public class Px4CopterClientDevice:Px4VehicleClientDevice
{
    public Px4CopterClientDevice(MavlinkClientDeviceId identity, 
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
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            if (microservice is ICommandClient command)
            {
                cmd = command;
            }
            yield return microservice;
        }
       
        if (cmd == null) yield break;
        
        yield return new Px4CopterModeClient(Heartbeat,cmd);
        
    }
}