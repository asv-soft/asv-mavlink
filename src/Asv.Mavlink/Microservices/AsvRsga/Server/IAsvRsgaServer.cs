using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.V2.AsvRsga;
using R3;

namespace Asv.Mavlink;



public interface IAsvRsgaServer:IMavlinkMicroserviceServer
{
    Observable<AsvRsgaCompatibilityRequestPayload> OnCompatibilityRequest { get; }
    Task SendCompatibilityResponse(Action<AsvRsgaCompatibilityResponsePayload> fillCallback, CancellationToken cancel = default);
}