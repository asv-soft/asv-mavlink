using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;

namespace Asv.Mavlink.Client;

public interface IAsvRadioClient
{
    IObservable<AsvRadioStatusPayload> Status { get; }
    Task<AsvRadioCapabilitiesResponsePayload> RequestCapabilities(CancellationToken cancel = default);
    Task<AsvRadioCodecCfgResponsePayload> RequestCodecCfg(AsvAudioCodec codec, CancellationToken cancel = default);
}