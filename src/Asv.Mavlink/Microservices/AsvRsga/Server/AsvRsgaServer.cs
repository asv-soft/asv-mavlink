using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvRsga;
using Microsoft.Extensions.Logging;

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
            .Subscribe(_onCompatibilityRequest);

    }

    public IObservable<AsvRsgaCompatibilityRequestPayload> OnCompatibilityRequest => _onCompatibilityRequest;

    public Task SendCompatibilityResponse(Action<AsvRsgaCompatibilityResponsePayload> fillCallback, CancellationToken cancel = default)
    {
        return InternalSend<AsvRsgaCompatibilityResponsePacket>(x => fillCallback(x.Payload), cancel);
    }

    public override void Dispose()
    {
        _onCompatibilityRequest.Dispose();
        _subscribe.Dispose();
        base.Dispose();
    }
}