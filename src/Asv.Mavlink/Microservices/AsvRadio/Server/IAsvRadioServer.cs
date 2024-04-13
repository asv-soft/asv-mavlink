using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRadio;

namespace Asv.Mavlink;

public interface IAsvRadioServer
{
    void Start();
    void Set(Action<AsvRadioStatusPayload> changeCallback);
    IObservable<AsvRadioCapabilitiesRequestPayload> OnCapabilitiesRequest { get; }
    Task SendCapabilitiesResponse(Action<AsvRadioCapabilitiesResponsePayload> setValueCallback, CancellationToken cancel = default);
    IObservable<AsvRadioCodecCfgRequestPayload> OnCodecCfgRequest { get; }
    Task SendCodecCfgResponse(Action<AsvRadioCodecCfgResponsePayload> setValueCallback, CancellationToken cancel = default);
    
}