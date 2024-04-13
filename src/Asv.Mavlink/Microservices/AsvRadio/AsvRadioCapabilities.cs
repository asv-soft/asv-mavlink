using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;

namespace Asv.Mavlink;


public class AsvRadioCodecCapabilities
{
    public AsvAudioCodec Codec { get; set; }
    public IReadOnlySet<byte> SupportedOptions { get; set; }
}

public class AsvRadioCapabilities
{
    public uint MinFrequencyHz { get; set; } = 10_000_000;
    public uint MaxFrequencyHz { get; set; } = 1_000_000_000;
    public float MinReferencePowerDbm { get; set; } = -100;
    public float MaxReferencePowerDbm { get; set; } = 10;
    public float MinTxPowerDbm { get; set; } = -100;
    public float MaxTxPowerDbm { get; set; } = 10;
    public IReadOnlySet<AsvRadioModulation> SupportedModulations { get; set; }
    public IReadOnlyDictionary<AsvAudioCodec,AsvRadioCodecCapabilities> SupportedCodecs { get; set; }
}