using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

public delegate Task<MavResult> EnableRadioDelegate(uint frequencyHz, AsvRadioModulation modulation,
    float referenceRxPowerDbm, float txPowerDbm, AsvAudioCodec codec, CancellationToken cancel);

public delegate Task<MavResult> DisableRadioDelegate(CancellationToken cancel);
public interface IAsvRadioServerEx
{
    IAsvRadioServer Base { get; }
    ReadOnlyReactiveProperty<AsvRadioCustomMode> CustomMode { get; }
    EnableRadioDelegate EnableRadio { set; }
    DisableRadioDelegate DisableRadio { set; }

    void Start();
}