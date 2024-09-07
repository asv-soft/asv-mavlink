using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;

namespace Asv.Mavlink;

public interface IAsvRsgaClient
{
    IObservable<AsvRsgaCompatibilityResponsePayload> OnCompatibilityResponse { get; }
    Task<AsvRsgaCompatibilityResponsePayload> GetCompatibilities(CancellationToken cancel = default);
}