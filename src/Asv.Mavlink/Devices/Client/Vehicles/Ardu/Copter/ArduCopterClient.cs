using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class ArduCopterClientDevice: ArduVehicleClientDevice
{
    private readonly ILogger _logger;

    public ArduCopterClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        _logger = core.LoggerFactory.CreateLogger<ArduCopterClientDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        ICommandClient? cmd = null;
        IPositionClientEx? pos = null;
        IHeartbeatClient? hb = null;
        
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
            if (microservice is IPositionClientEx position)
            {
                pos = position;
            }
            yield return microservice;
        }

        if (cmd == null)
        {
            _logger.ZLogWarning($"{Id}: command microservice not found");
            yield break;
        }

        if (pos == null)
        {
            _logger.ZLogWarning($"{Id}: position microservice not found");
            yield break;
        }
        if (hb == null)
        {
            _logger.ZLogWarning($"{Id}: heartbeat microservice not found");
            yield break;
        }
        var mode = new ArduCopterModeClient(hb, cmd);
        yield return mode;
        yield return new ArduCopterControlClient(hb, mode, pos);
    }
}