using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using R3;

namespace Asv.Mavlink;

public class AsvRsgaServer:MavlinkMicroserviceServer,IAsvRsgaServer
{
    private readonly Subject<AsvRsgaCompatibilityRequestPayload> _onCompatibilityRequest;
    private readonly IDisposable _subscribe;

    public AsvRsgaServer(MavlinkIdentity identity,ICoreServices core)
        : base("RSGA", identity, core)
    {
        _onCompatibilityRequest = new Subject<AsvRsgaCompatibilityRequestPayload>();
        _subscribe = InternalFilter<AsvRsgaCompatibilityRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Select(x => x.Payload)
            .Subscribe(_onCompatibilityRequest.AsObserver());

    }

    public Observable<AsvRsgaCompatibilityRequestPayload> OnCompatibilityRequest => _onCompatibilityRequest;

    public ValueTask SendCompatibilityResponse(Action<AsvRsgaCompatibilityResponsePayload> fillCallback, CancellationToken cancel = default)
    {
        return InternalSend<AsvRsgaCompatibilityResponsePacket>(x => fillCallback(x.Payload), cancel);
    }
    
    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onCompatibilityRequest.Dispose();
            _subscribe.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onCompatibilityRequest).ConfigureAwait(false);
        await CastAndDispose(_subscribe).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion

}