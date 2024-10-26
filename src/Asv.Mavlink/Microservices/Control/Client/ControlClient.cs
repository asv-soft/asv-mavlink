using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public abstract class ControlClient(MavlinkClientIdentity identity, ICoreServices core)
    : MavlinkMicroserviceClient("CRTL", identity, core), IControlClient
{
    public abstract Task EnsureInAutoMode(CancellationToken cancel = default);
    public abstract ValueTask<bool> CheckAutoMode(CancellationToken cancel);
    public abstract Task EnsureInGuidedMode(CancellationToken cancel = default);
    public abstract Task<bool> CheckGuidedMode(CancellationToken cancel = default);
    public abstract Task GoTo(GeoPoint point, CancellationToken cancel = default);
    public abstract Task DoLand(CancellationToken cancel = default);
    public abstract Task DoRtl(CancellationToken cancel = default);
    public abstract Task SetAutoMode(CancellationToken cancel = default);
    public abstract Task TakeOff(double altInMeters, CancellationToken cancel = default);
}