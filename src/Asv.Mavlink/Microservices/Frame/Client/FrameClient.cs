using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public abstract class FrameClient(MavlinkClientIdentity identity, IMavlinkContext core)
    : MavlinkMicroserviceClient("FRAME", identity, core), IFrameClient
{
    public abstract IReadOnlyObservableDictionary<string, IMotorFrame> MotorFrames { get; }
    
    public abstract ValueTask LoadAvailableFrames(CancellationToken cancel = default);

    public abstract Task SetFrame(IMotorFrame motorFrameToSet, CancellationToken cancel = default);
    
    public abstract Task<IMotorFrame?> GetCurrentFrame(CancellationToken cancel = default);
}