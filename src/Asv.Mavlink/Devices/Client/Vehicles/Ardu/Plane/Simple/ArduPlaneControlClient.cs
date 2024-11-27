using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.Ardupilotmega;

using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class ArduPlaneControlClient(
    IHeartbeatClient heartbeat,
    IModeClient mode,
    IPositionClientEx pos)
    : ControlClient(heartbeat.Identity, heartbeat.Core)
{
    private readonly ILogger<ArduCopterControlClient> _logger = heartbeat.Core.Log.CreateLogger<ArduCopterControlClient>();
    public override ValueTask<bool> IsAutoMode(CancellationToken cancel = default)
    {
        if (heartbeat.RawHeartbeat.CurrentValue == null) return ValueTask.FromResult(false);
        return ValueTask.FromResult(heartbeat.RawHeartbeat.CurrentValue.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
                                    heartbeat.RawHeartbeat.CurrentValue.CustomMode == (int)PlaneMode.PlaneModeAuto);
    }

    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        _logger.LogInformation("Set auto mode");
        return mode.SetMode(ArduPlaneModeClient.Auto, cancel);
    }

    public override ValueTask<bool> IsGuidedMode(CancellationToken cancel = default)
    {
        if (heartbeat.RawHeartbeat.CurrentValue == null) return ValueTask.FromResult(false);
        return ValueTask.FromResult(
            heartbeat.RawHeartbeat.CurrentValue.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            heartbeat.RawHeartbeat.CurrentValue.CustomMode == (int)PlaneMode.PlaneModeGuided);
    }

    public override Task SetGuidedMode(CancellationToken cancel = default)
    {
        _logger.LogInformation("Set guided mode");
        return mode.SetMode(ArduPlaneModeClient.Guided, cancel);
    }

    public override async Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"GoTo({point})");
        await this.EnsureAutoMode(cancel).ConfigureAwait(false);
        await pos.SetTarget(point, cancel).ConfigureAwait(false);
    }

    public override Task DoLand(CancellationToken cancel = default)
    {
        throw new NotSupportedException("Land not supported for Plane vehicles");
    }

    public override async Task DoRtl(CancellationToken cancel = default)
    {
        _logger.LogInformation("DoRtl");
        await this.EnsureGuidedMode(cancel).ConfigureAwait(false);
        await mode.SetMode(ArduPlaneModeClient.Rtl, cancel).ConfigureAwait(false);
    }

    public override async Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"TakeOff(altitude:{altInMeters:F2})");
        await this.EnsureGuidedMode(cancel).ConfigureAwait(false);
        await pos.ArmDisarm(true, cancel).ConfigureAwait(false);
        await pos.TakeOff(altInMeters,  cancel).ConfigureAwait(false);
    }
}