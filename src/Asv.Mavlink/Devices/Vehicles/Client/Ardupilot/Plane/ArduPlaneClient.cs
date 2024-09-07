#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class ArduPlaneClient : ArduVehicle
{
    private readonly ILogger _logger;
    private string _vehicleClassName;
    private const string ClassNamePlane = "ArduPlane";
    private const string ClassNameQuadPlane = "ArduQuadPlane";
    private bool _isQuadPlane;

    public ArduPlaneClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        VehicleClientConfig config,
        IPacketSequenceCalculator seq, 
        IScheduler? scheduler = null,
        ILogger? logger = null) : base(connection, identity, config, seq, scheduler,logger)
    {
    }

    protected override async Task InternalInit()
    {
        await base.InternalInit().ConfigureAwait(false);
        var qEnable =
            ArdupilotFrameTypeHelper.ParseFrameClass((sbyte)await Params.ReadOnce("Q_ENABLE", DisposeCancel)
                .ConfigureAwait(false));
        if (qEnable > 0) _isQuadPlane = true;
        _vehicleClassName = _isQuadPlane ? ClassNameQuadPlane : ClassNamePlane;
    }

    protected override string DefaultName => $"{_vehicleClassName} [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";

    public override DeviceClass Class => DeviceClass.Plane;
    
    protected override Task<IReadOnlyCollection<ParamDescription>> GetParamDescription()
    {
        return Task.FromResult((IReadOnlyCollection<ParamDescription>)new List<ParamDescription>());
    }

    public override async Task EnsureInGuidedMode(CancellationToken cancel)
    {
        if (!await CheckGuidedMode(cancel).ConfigureAwait(false))
        {
            await SetVehicleMode(ArdupilotPlaneMode.Guided, cancel).ConfigureAwait(false);
        }
    }

    public override Task<bool> CheckGuidedMode(CancellationToken cancel)
    {
        return Task.FromResult(
            Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            Heartbeat.RawHeartbeat.Value.CustomMode == (int)PlaneMode.PlaneModeGuided);
    }

    public override Task<bool> CheckAutoMode(CancellationToken cancel)
    {
        return Task.FromResult(
            Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            Heartbeat.RawHeartbeat.Value.CustomMode == (int)PlaneMode.PlaneModeAuto);
    }

    public override async Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        await EnsureInAutoMode(cancel).ConfigureAwait(false);
        await Position.SetTarget(point, cancel).ConfigureAwait(false);
    }
    
    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        return Commands.DoSetMode(1, (uint)PlaneMode.PlaneModeAuto, 0, cancel);
    }

    public override async Task DoLand(CancellationToken cancel = default)
    {
        _logger.LogInformation("=> Land manually");
        await SetVehicleMode(ArdupilotPlaneMode.Qland, cancel).ConfigureAwait(false);
    }

    public override Task DoRtl(CancellationToken cancel = default)
    {
        return SetVehicleMode(_isQuadPlane ? ArdupilotPlaneMode.Qrtl : ArdupilotPlaneMode.Rtl, cancel);
    }

    public override async Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        await EnsureInGuidedMode(cancel).ConfigureAwait(false);
        await Position.ArmDisarm(true, cancel).ConfigureAwait(false);
        await Position.TakeOff(altInMeters, cancel).ConfigureAwait(false);
    }

    public override IEnumerable<IVehicleMode> AvailableModes => ArdupilotPlaneMode.AllModes;

    protected override IVehicleMode? InternalInterpretMode(HeartbeatPayload heartbeatPayload)
    {
        return AvailableModes.Cast<ArdupilotPlaneMode>()
            .FirstOrDefault(m => m.CustomMode == (PlaneMode)heartbeatPayload.CustomMode);
    }

    public override Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default)
    {
        if (mode is not ArdupilotPlaneMode)
            throw new Exception($"Invalid mode. Only {nameof(ArdupilotPlaneMode)} supported");
        return Commands.DoSetMode(1, (uint)((ArdupilotPlaneMode)mode).CustomMode, 0, cancel);
    }
    

    public override async Task EnsureInAutoMode(CancellationToken cancel)
    {
        if (!await CheckAutoMode(cancel).ConfigureAwait(false))
        {
            await SetVehicleMode(ArdupilotPlaneMode.Auto, cancel).ConfigureAwait(false);
        }
    }
}