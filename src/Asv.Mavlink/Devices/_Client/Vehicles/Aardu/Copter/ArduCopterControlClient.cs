using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class ArduCopterControlClient :ControlClient
{
    private readonly IHeartbeatClient _heartbeat;

    public ArduCopterControlClient(MavlinkClientIdentity identity, IHeartbeatClient heartbeat, ICoreServices core) : base(identity, core)
    {
        _heartbeat = heartbeat;
    }

    public override ValueTask<bool> CheckAutoMode(CancellationToken cancel)
    {
        return ValueTask.FromResult(_heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
                                    _heartbeat.RawHeartbeat.Value.CustomMode == (int)CopterMode.CopterModeAuto);
    }

    public override Task EnsureInGuidedMode(CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }

    public override Task<bool> CheckGuidedMode(CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }

    public override Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task DoLand(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task DoRtl(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }
}