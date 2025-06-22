using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using R3;

namespace Asv.Mavlink;

public interface IAsvRsgaClient:IMavlinkMicroserviceClient
{
    ReadOnlyReactiveProperty<AsvRsgaCompatibilityResponsePayload?> OnCompatibilityResponse { get; }
    Task<AsvRsgaCompatibilityResponsePayload> GetCompatibilities(CancellationToken cancel = default);
    
    Observable<MavlinkMessage> DeviceMessages { get; }
}