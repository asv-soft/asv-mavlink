#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using NLog;

namespace Asv.Mavlink;

public class ArduPlaneClient:ArduVehicle
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 
    
    public ArduPlaneClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config, IPacketSequenceCalculator seq, IScheduler? scheduler = null) : base(connection, identity, config, seq, scheduler)
    {
        
    }

    protected override string DefaultName => $"ArduPlane [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";

    public override DeviceClass Class => DeviceClass.Plane;
    protected override Task<IReadOnlyCollection<ParamDescription>> GetParamDescription()
    {
        // TODO: Read from device by FTP or load from XML file
        return Task.FromResult((IReadOnlyCollection<ParamDescription>)new List<ParamDescription>());
    }

    public override async Task EnsureInGuidedMode(CancellationToken cancel)
    {
        if (!await CheckGuidedMode(cancel).ConfigureAwait(false))
        {
            await SetVehicleMode(ArdupilotPlaneMode.Guided, cancel).ConfigureAwait(false);
        }
    }
    
    public override async Task EnsureInAutoMode(CancellationToken cancel)
    {
        if (!await CheckAutoMode(cancel).ConfigureAwait(false))
        {
            await SetVehicleMode(ArdupilotPlaneMode.Auto, cancel).ConfigureAwait(false);
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
        return Commands.DoSetMode(1, (uint)PlaneMode.PlaneModeAuto, 0,cancel);
    }

    public override IEnumerable<IVehicleMode> AvailableModes => ArdupilotPlaneMode.AllModes;
    protected override IVehicleMode? InternalInterpretMode(HeartbeatPayload heartbeatPayload)
    {
        return AvailableModes.Cast<ArdupilotPlaneMode>()
            .FirstOrDefault(_ => _.CustomMode == (PlaneMode)heartbeatPayload.CustomMode);
    }

    public override Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default)
    {
        if (mode is not ArdupilotPlaneMode) throw new Exception($"Invalid mode. Only {nameof(ArdupilotPlaneMode)} supported");
        return Commands.DoSetMode(1, (uint)((ArdupilotPlaneMode)mode).CustomMode, 0,cancel);
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
        return SetVehicleMode(ArdupilotPlaneMode.Rtl, cancel);
    }
    
    public override async Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        Logger.Info($"=> QTakeOff(altitude:{altInMeters:F2})");
        await EnsureInAutoMode(cancel).ConfigureAwait(false);
        await Position.ArmDisarm(true, cancel).ConfigureAwait(false);
        await Position.QTakeOff(altInMeters,  cancel).ConfigureAwait(false);
    }
}