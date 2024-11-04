using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRadio;
using R3;

namespace Asv.Mavlink;

public interface IAsvRadioServer:IMavlinkMicroserviceServer
{
    void Start();
    void Set(Action<AsvRadioStatusPayload> changeCallback);
    ReadOnlyReactiveProperty<AsvRadioCapabilitiesRequestPayload?> OnCapabilitiesRequest { get; }
    Task SendCapabilitiesResponse(Action<AsvRadioCapabilitiesResponsePayload> setValueCallback, CancellationToken cancel = default);
    ReadOnlyReactiveProperty<AsvRadioCodecCapabilitiesRequestPayload> OnCodecCapabilitiesRequest { get; }
    Task SendCodecCapabilitiesRequest(Action<AsvRadioCodecCapabilitiesResponsePayload> setValueCallback, CancellationToken cancel = default);
    
}