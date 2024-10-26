using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class ArduCopterControlClient(
    IHeartbeatClient heartbeat,
    IModeClient mode,
    IPositionClientEx pos)
    : ControlClient(heartbeat.Identity, heartbeat.Core)
{
    private readonly ILogger<ArduCopterControlClient> _logger = heartbeat.Core.Log.CreateLogger<ArduCopterControlClient>();

    public override ValueTask<bool> IsAutoMode(CancellationToken cancel = default)
    {
        return ValueTask.FromResult(heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
                                    heartbeat.RawHeartbeat.Value.CustomMode == (int)CopterMode.CopterModeAuto);
    }

    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        _logger.LogInformation("Set auto mode");
        return mode.SetMode(ArduCopterModeClient.Auto, cancel);
    }

    public override ValueTask<bool> IsGuidedMode(CancellationToken cancel = default)
    {
        return ValueTask.FromResult(
            heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            heartbeat.RawHeartbeat.Value.CustomMode == (int)CopterMode.CopterModeGuided);
    }

    public override Task SetGuidedMode(CancellationToken cancel = default)
    {
        _logger.LogInformation("Set guided mode");
        return mode.SetMode(ArduCopterModeClient.Guided, cancel);
    }

    public override async Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"GoTo({point})");
        await this.EnsureGuidedMode(cancel).ConfigureAwait(false);
        await pos.SetTarget(point, cancel).ConfigureAwait(false);
    }

    public override async Task DoLand(CancellationToken cancel = default)
    {
        _logger.LogInformation("DoLand");
        await this.EnsureGuidedMode(cancel).ConfigureAwait(false);
        await mode.SetMode(ArduCopterModeClient.Land, cancel).ConfigureAwait(false);
    }

    public override async Task DoRtl(CancellationToken cancel = default)
    {
        _logger.LogInformation("DoRtl");
        await this.EnsureGuidedMode(cancel).ConfigureAwait(false);
        await mode.SetMode(ArduCopterModeClient.Rtl, cancel).ConfigureAwait(false);
    }

    public override async Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"TakeOff(altitude:{altInMeters:F2})");
        await this.EnsureGuidedMode(cancel).ConfigureAwait(false);
        await pos.ArmDisarm(true, cancel).ConfigureAwait(false);
        await pos.TakeOff(altInMeters,  cancel).ConfigureAwait(false);
    }
}