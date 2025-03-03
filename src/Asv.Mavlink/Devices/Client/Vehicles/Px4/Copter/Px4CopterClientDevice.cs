using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class Px4CopterClientDevice : Px4VehicleClientDevice
{
    private readonly ILogger _logger;

    public Px4CopterClientDevice(MavlinkClientDeviceId identity,
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext core) : base(identity, config, extenders, core)
    {
        _logger = core.LoggerFactory.CreateLogger<Px4CopterClientDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        ICommandClient? cmd = null;
        HeartbeatClient? hb = null;
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            if (microservice is HeartbeatClient heartbeat)
            {
                hb = heartbeat;
            }
            if (microservice is ICommandClient command)
            {
                cmd = command;
            }
            yield return microservice;
        }
       
        if (cmd == null) yield break;
        
        if (hb == null)
        {
            _logger.ZLogWarning($"{Id} {nameof(HeartbeatClient)} microservice not found");
            yield break;
        }
        yield return new Px4CopterModeClient(hb,cmd);
        
    }
}