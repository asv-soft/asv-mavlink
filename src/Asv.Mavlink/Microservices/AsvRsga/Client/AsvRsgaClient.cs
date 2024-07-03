using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvRsga;

namespace Asv.Mavlink;

public class AsvRsgaClient: MavlinkMicroserviceClient, IAsvRsgaClient
{
    private readonly Subject<AsvRsgaCompatibilityResponsePayload> _onCompatibilityRequest;
    private uint _requestCounter;


    public AsvRsgaClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq) 
        : base("RSGA", connection, identity, seq)
    {
        _onCompatibilityRequest = new Subject<AsvRsgaCompatibilityResponsePayload>().DisposeItWith(Disposable);
        InternalFilter<AsvRsgaCompatibilityResponsePacket>()
            .Select(x => x.Payload)
            .Subscribe(_onCompatibilityRequest).DisposeItWith(Disposable);
    }

    public IObservable<AsvRsgaCompatibilityResponsePayload> OnCompatibilityResponse => _onCompatibilityRequest;

    public Task<AsvRsgaCompatibilityResponsePayload> GetCompatibilities(CancellationToken cancel)
    {
        var reqId = GenerateRequestIndex();
        return InternalCall<AsvRsgaCompatibilityResponsePayload, AsvRsgaCompatibilityRequestPacket,
                AsvRsgaCompatibilityResponsePacket>(
                x =>
                {
                    x.Payload.TargetSystem = Identity.TargetSystemId;
                    x.Payload.TargetComponent = Identity.TargetComponentId;
                    x.Payload.RequestId = reqId;
                }, x => x.Payload.RequestId == reqId, x => x.Payload, cancel: cancel);
    }
    private ushort GenerateRequestIndex()
    {
        return (ushort)(Interlocked.Increment(ref _requestCounter)%ushort.MaxValue);
    }

}