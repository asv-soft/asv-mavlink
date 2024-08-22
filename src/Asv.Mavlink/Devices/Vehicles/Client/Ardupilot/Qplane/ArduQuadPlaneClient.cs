using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using NLog;

namespace Asv.Mavlink.Qplane;

public class ArduQuadPlaneClient : ArduVehicle
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public ArduQuadPlaneClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config,
        IPacketSequenceCalculator seq, IScheduler scheduler) : base(connection, identity, config, seq, scheduler)
    {
    }
    
    protected override async Task InternalInit()
    {
        await base.InternalInit().ConfigureAwait(false);
        if (Params.IsInit == false) return;
        try
        {
            var frameClass =
                ArdupilotFrameTypeHelper.ParseFrameClass((sbyte)await Params.ReadOnce("FRAME_CLASS", DisposeCancel)
                    .ConfigureAwait(false));
            var frameType =
                ArdupilotFrameTypeHelper.ParseFrameType((sbyte)await Params.ReadOnce("FRAME_TYPE", DisposeCancel)
                    .ConfigureAwait(false));
            Params.Filter("BRD_SERIAL_NUM")
                .Select(serial => ArdupilotFrameTypeHelper.GenerateName(frameClass, frameType, (int)serial))
                .Subscribe(EditableName)
                .DisposeItWith(Disposable);
            ;
            await Params.ReadOnce("BRD_SERIAL_NUM", DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Logger.Error($"Error to get vehicle name:{e.Message}");
        }
    }

    protected override string DefaultName =>
        $"ArduQuadPlane [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";

    public override DeviceClass Class => DeviceClass.Plane;

    protected override Task<IReadOnlyCollection<ParamDescription>> GetParamDescription()
    {
        return Task.FromResult((IReadOnlyCollection<ParamDescription>)new List<ParamDescription>());
    }

    public override async  Task EnsureInGuidedMode(CancellationToken cancel)
    {
        if (!await CheckGuidedMode(cancel).ConfigureAwait(false))
        {
            await SetVehicleMode(ArduQuadPlaneMode.Guided, cancel).ConfigureAwait(false);
        }
    }

    public override Task<bool> CheckGuidedMode(CancellationToken cancel)
    {
        return Task.FromResult(
            Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            Heartbeat.RawHeartbeat.Value.CustomMode == (int)PlaneMode.PlaneModeGuided);
    }

    public override async Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        await EnsureInAutoMode(cancel).ConfigureAwait(false);
        await Position.SetTarget(point, cancel).ConfigureAwait(false);
    }

    public override async Task DoLand(CancellationToken cancel = default)
    {
        Logger.Info("=> Land manualy");           
        await EnsureInAutoMode(cancel).ConfigureAwait(false);
        await Position.QLand(NavVtolLandOptions.NavVtolLandOptionsHoverDescent, double.NaN, cancel).ConfigureAwait(false);
        await Position.ArmDisarm(false, cancel).ConfigureAwait(false);
    }

    public override Task DoRtl(CancellationToken cancel = default)
    {
        return SetVehicleMode(ArduQuadPlaneMode.Qrtl, cancel);
    }

    public override async Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        await EnsureInGuidedMode(cancel).ConfigureAwait(false);
        await Position.ArmDisarm(true, cancel).ConfigureAwait(false);
        await Position.TakeOff(altInMeters,  cancel).ConfigureAwait(false);
    }

    public override IEnumerable<IVehicleMode> AvailableModes { get; }

    protected override IVehicleMode? InternalInterpretMode(HeartbeatPayload heartbeatPayload)
    {
        return AvailableModes.Cast<ArduQuadPlaneMode>()
            .FirstOrDefault(_ => _.CustomMode == (PlaneMode)heartbeatPayload.CustomMode);
    }

    public override Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default)
    {
        if (mode is not ArduQuadPlaneMode)
            throw new Exception($"Invalid mode. Only {nameof(ArduQuadPlaneMode)} supported");
        return Commands.DoSetMode(1, (uint)((ArduQuadPlaneMode)mode).CustomMode, 0, cancel);
    }

    public override Task<bool> CheckAutoMode(CancellationToken cancel)
    {
        return Task.FromResult(
            Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            Heartbeat.RawHeartbeat.Value.CustomMode == (int)PlaneMode.PlaneModeAuto);
    }

    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        return Commands.DoSetMode(1, (uint)PlaneMode.PlaneModeAuto, 0, cancel);
    }

    public override async Task EnsureInAutoMode(CancellationToken cancel)
    {
        if (!await CheckAutoMode(cancel).ConfigureAwait(false))
        {
            await SetVehicleMode(ArduQuadPlaneMode.Auto, cancel).ConfigureAwait(false);
        }
    }
}