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

public class ArduPlaneClientDevice : ArduVehicleClientDevice
{
    private readonly ILogger<ArduPlaneClientDevice> _logger1;

    public ArduPlaneClientDevice(MavlinkClientDeviceId identity,
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext core) : base(identity, config, extenders, core)
    {
        _logger1 = core.LoggerFactory.CreateLogger<ArduPlaneClientDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        ICommandClient? cmd = null;
        IPositionClientEx? pos = null;
        IParamsClient? param = null;
        IHeartbeatClient? hb = null;
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            switch (microservice)
            {
                case IHeartbeatClient heartbeat:
                    hb = heartbeat;
                    break;
                case IParamsClient parameters:
                    param = parameters;
                    break;
                case ICommandClient command:
                    cmd = command;
                    break;
                case IPositionClientEx position:
                    pos = position;
                    break;
            }

            yield return microservice;
        }
        if (param == null) yield break;
        if (cmd == null) yield break;
        if (pos == null) yield break;
        
        if (hb == null)
        {
            _logger1.ZLogWarning($"{Id} {nameof(HeartbeatClient)} microservice not found");
            yield break;
        }
        
        var value = await param.Read("Q_ENABLE",cancel).ConfigureAwait(false);
        var quadPlaneEnabled = value.ParamValue != 0;
        if (quadPlaneEnabled)
        {
            var mode = new ArduQuadPlaneModeClient(hb, cmd);
            yield return mode;
            yield return new ArduQuadPlaneControlClient(hb, mode,pos);    
        }
        else
        {
            var mode = new ArduPlaneModeClient(hb, cmd);
            yield return mode;
            yield return new ArduPlaneControlClient(hb, mode,pos);
        }
        
    }
}