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

    public AsvRsgaServer(
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq, 
        TimeProvider? timeProvider = null,
        IScheduler? rxScheduler = null,
        ILoggerFactory? logFactory = null)
        : base("RSGA", connection, identity, seq,timeProvider, rxScheduler,logFactory)
    {
        _onCompatibilityRequest = new Subject<AsvRsgaCompatibilityRequestPayload>().DisposeItWith(Disposable);
        InternalFilter<AsvRsgaCompatibilityRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Select(x => x.Payload)
            .Subscribe(_onCompatibilityRequest).DisposeItWith(Disposable);

    }

    public IObservable<AsvRsgaCompatibilityRequestPayload> OnCompatibilityRequest => _onCompatibilityRequest;

    public Task SendCompatilityResponse(Action<AsvRsgaCompatibilityResponsePayload> fillCallback, CancellationToken cancel = default)
    {
        return InternalSend<AsvRsgaCompatibilityResponsePacket>(x => fillCallback(x.Payload), cancel);
    }
}