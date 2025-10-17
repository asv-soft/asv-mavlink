using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public abstract class FrameClient(MavlinkClientIdentity identity, IMavlinkContext core)
    : MavlinkMicroserviceClient("FRAME", identity, core), IFrameClient
{
    public abstract IReadOnlyObservableDictionary<string, IMotorFrame> MotorFrames { get; }
    
    public abstract ReadOnlyReactiveProperty<IMotorFrame?> CurrentMotorFrame { get; }
    
    public abstract ValueTask RefreshAvailableFrames(CancellationToken cancel = default);

    public abstract Task SetFrame(IMotorFrame motorFrameToSet, CancellationToken cancel = default);
    
    public abstract Task RefreshCurrentFrame(CancellationToken cancel = default);

    protected override async Task InternalInit(CancellationToken cancel)
    {
        await base.InternalInit(cancel).ConfigureAwait(false);
        await RefreshAvailableFrames(cancel).ConfigureAwait(false);
        await RefreshCurrentFrame(cancel).ConfigureAwait(false);
    }
}