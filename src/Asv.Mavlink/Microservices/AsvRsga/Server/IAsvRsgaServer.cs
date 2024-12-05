using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using R3;

namespace Asv.Mavlink;



public interface IAsvRsgaServer:IMavlinkMicroserviceServer
{
    Observable<AsvRsgaCompatibilityRequestPayload> OnCompatibilityRequest { get; }
    ValueTask SendCompatibilityResponse(Action<AsvRsgaCompatibilityResponsePayload> fillCallback,
        CancellationToken cancel = default);
}