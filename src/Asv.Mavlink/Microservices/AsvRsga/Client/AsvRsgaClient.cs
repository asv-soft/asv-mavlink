using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;
using R3;

namespace Asv.Mavlink;

public class AsvRsgaClient: MavlinkMicroserviceClient, IAsvRsgaClient
{
    private uint _requestCounter;
    public AsvRsgaClient(MavlinkClientIdentity identity, ICoreServices core) 
        : base("RSGA", identity, core)
    {
        OnCompatibilityResponse = InternalFilter<AsvRsgaCompatibilityResponsePacket>()
            .Select(x => x?.Payload)
            .ToReadOnlyReactiveProperty();
    }

    public ReadOnlyReactiveProperty<AsvRsgaCompatibilityResponsePayload?> OnCompatibilityResponse { get; }

    public Task<AsvRsgaCompatibilityResponsePayload> GetCompatibilities(CancellationToken cancel)
    {
        var reqId = GenerateRequestIndex();
        return InternalCall<AsvRsgaCompatibilityResponsePayload, AsvRsgaCompatibilityRequestPacket,
                AsvRsgaCompatibilityResponsePacket>(
                x =>
                {
                    x.Payload.TargetSystem = Identity.Target.SystemId;
                    x.Payload.TargetComponent = Identity.Target.ComponentId;
                    x.Payload.RequestId = reqId;
                }, x => x.Payload.RequestId == reqId, x => x.Payload, cancel: cancel);
    }
    private ushort GenerateRequestIndex()
    {
        return (ushort)(Interlocked.Increment(ref _requestCounter)%ushort.MaxValue);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            OnCompatibilityResponse.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (OnCompatibilityResponse is IAsyncDisposable onCompatibilityResponseAsyncDisposable)
            await onCompatibilityResponseAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            OnCompatibilityResponse.Dispose();

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion

}