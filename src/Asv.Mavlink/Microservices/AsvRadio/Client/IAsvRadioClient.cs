using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRadio;

namespace Asv.Mavlink;

public interface IAsvRadioClient
{
    IObservable<AsvRadioStatusPayload> Status { get; }
    Task<AsvRadioCapabilitiesResponsePayload> RequestCapabilities(CancellationToken cancel = default);

    Task<AsvRadioCodecCapabilitiesResponsePayload> RequestCodecCapabilities(ushort skip = 0, byte count = byte.MaxValue,
        CancellationToken cancel = default);
}