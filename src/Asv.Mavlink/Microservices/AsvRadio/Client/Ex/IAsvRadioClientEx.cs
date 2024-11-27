using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.Common;

using Asv.Mavlink.AsvRadio;

using R3;

namespace Asv.Mavlink;

public interface IAsvRadioClientEx:IMavlinkMicroserviceClient
{
    ReadOnlyReactiveProperty<AsvRadioCustomMode> CustomMode { get; }
    IAsvRadioClient Base { get; }

    ReadOnlyReactiveProperty<AsvRadioCapabilities?> Capabilities { get; }
    
    Task<MavResult> EnableRadio(uint frequencyHz, AsvRadioModulation modulation, float referenceRxPowerDbm,
        float txPowerDbm, AsvAudioCodec codec, CancellationToken cancel = default);
    Task<MavResult> DisableRadio(CancellationToken cancel = default);
    
    Task<AsvRadioCapabilities> GetCapabilities(CancellationToken cancel = default);
    Task<ISet<AsvAudioCodec>> GetCodecsCapabilities(CancellationToken cancel = default);

}