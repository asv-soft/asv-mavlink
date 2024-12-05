using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRadio;
using R3;

namespace Asv.Mavlink;

public interface IAsvRadioClient:IMavlinkMicroserviceClient
{
    ReadOnlyReactiveProperty<AsvRadioStatusPayload?> Status { get; }
    Task<AsvRadioCapabilitiesResponsePayload> RequestCapabilities(CancellationToken cancel = default);

    Task<AsvRadioCodecCapabilitiesResponsePayload> RequestCodecCapabilities(ushort skip = 0, byte count = byte.MaxValue,
        CancellationToken cancel = default);
}