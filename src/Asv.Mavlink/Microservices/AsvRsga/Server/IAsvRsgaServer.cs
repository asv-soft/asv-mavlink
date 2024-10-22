using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;

namespace Asv.Mavlink;



public interface IAsvRsgaServer:IMavlinkMicroserviceServer
{
    IObservable<AsvRsgaCompatibilityRequestPayload> OnCompatibilityRequest { get; }
    Task SendCompatibilityResponse(Action<AsvRsgaCompatibilityResponsePayload> fillCallback, CancellationToken cancel = default);
}