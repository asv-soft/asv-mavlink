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

public class AsvRsgaClient: MavlinkMicroserviceClient, IAsvRsgaClient
{
    private readonly Subject<AsvRsgaCompatibilityResponsePayload> _onCompatibilityRequest;
    private uint _requestCounter;
    private readonly IDisposable _onCompatibilityRequestSubscribe;


    public AsvRsgaClient(MavlinkClientIdentity identity, ICoreServices core) 
        : base("RSGA", identity, core)
    {
        _onCompatibilityRequest = new Subject<AsvRsgaCompatibilityResponsePayload>();
        _onCompatibilityRequestSubscribe = InternalFilter<AsvRsgaCompatibilityResponsePacket>()
            .Select(x => x.Payload)
            .Subscribe(_onCompatibilityRequest);
    }

    public IObservable<AsvRsgaCompatibilityResponsePayload> OnCompatibilityResponse => _onCompatibilityRequest;

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

    public override void Dispose()
    {
        _onCompatibilityRequestSubscribe.Dispose();
        _onCompatibilityRequest.Dispose();
        base.Dispose();
    }
}