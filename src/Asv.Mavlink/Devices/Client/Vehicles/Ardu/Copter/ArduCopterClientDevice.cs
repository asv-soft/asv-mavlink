using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class ArduCopterClientDevice : ArduVehicleClientDevice
{
    private readonly ILogger<ArduCopterClientDevice> _logger;

    public ArduCopterClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity, config, extenders, core)
    {
        _logger = core.LoggerFactory.CreateLogger<ArduCopterClientDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        ICommandClient? commandClient = null;
        IPositionClientEx? positionClientEx = null;
        IParamsClientEx? paramsClientEx = null;
        IHeartbeatClient? heartbeatClient = null;
        
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            switch (microservice)
            {
                case ICommandClient client:
                    commandClient = client;
                    break;
                case IPositionClientEx client:
                    positionClientEx = client;
                    break;
                case IParamsClientEx client:
                    paramsClientEx = client;
                    break;
                case IHeartbeatClient client:
                    heartbeatClient = client;
                    break;
            }
            
            yield return microservice;
        }

        if (commandClient == null)
        {
            _logger.ZLogWarning($"{Id}: command microservice not found");
            yield break;
        }
        if (positionClientEx == null)
        {
            _logger.ZLogWarning($"{Id}: positionEx microservice not found");
            yield break;
        }
        if (paramsClientEx == null)
        {
            _logger.ZLogWarning($"{Id}: paramsEx microservice not found");
            yield break;
        }
        if (heartbeatClient == null)
        {
            _logger.ZLogWarning($"{Id}: heartbeat microservice not found");
            yield break;
        }
        
        var mode = new ArduCopterModeClient(heartbeatClient, commandClient);
        yield return mode;
        yield return new ArduCopterControlClient(heartbeatClient, mode, positionClientEx);
        yield return new ArduCopterFrameClient(heartbeatClient, paramsClientEx);
    }
}