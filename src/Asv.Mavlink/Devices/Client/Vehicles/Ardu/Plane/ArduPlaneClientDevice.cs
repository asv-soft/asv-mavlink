using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class ArduPlaneClientDevice : ArduVehicleClientDevice
{
    private readonly ILogger<ArduPlaneClientDevice> _logger;

    public ArduPlaneClientDevice(
        MavlinkClientDeviceId identity,
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext core) 
        : base(identity, config, extenders, core)
    {
        _logger = core.LoggerFactory.CreateLogger<ArduPlaneClientDevice>();
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

        var value = await paramsClientEx.ReadOnce("Q_ENABLE", cancel).ConfigureAwait(false);
        var quadPlaneEnabled = (int)value != 0;
        if (quadPlaneEnabled)
        {
            var mode = new ArduQuadPlaneModeClient(heartbeatClient, commandClient);
            yield return mode;
            yield return new ArduQuadPlaneControlClient(heartbeatClient, mode,positionClientEx);   
            yield return new ArduQuadPlaneFrameClient(heartbeatClient, paramsClientEx);
        }
        else
        {
            var mode = new ArduPlaneModeClient(heartbeatClient, commandClient);
            yield return mode;
            yield return new ArduPlaneControlClient(heartbeatClient, mode,positionClientEx);
        }
    }
}