using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRadio;
using R3;

namespace Asv.Mavlink;

public interface IAsvRadioServer:IMavlinkMicroserviceServer
{
    void Start();
    void Set(Action<AsvRadioStatusPayload> changeCallback);
    Observable<AsvRadioCapabilitiesRequestPayload?> OnCapabilitiesRequest { get; }
    ValueTask SendCapabilitiesResponse(Action<AsvRadioCapabilitiesResponsePayload> setValueCallback,
        CancellationToken cancel = default);
    Observable<AsvRadioCodecCapabilitiesRequestPayload?> OnCodecCapabilitiesRequest { get; }
    ValueTask SendCodecCapabilitiesRequest(Action<AsvRadioCodecCapabilitiesResponsePayload> setValueCallback,
        CancellationToken cancel = default);
    
}