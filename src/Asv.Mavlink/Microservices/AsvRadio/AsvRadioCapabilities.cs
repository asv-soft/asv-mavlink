using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;

namespace Asv.Mavlink;


public class AsvRadioCapabilitiesBuilder
{
    private readonly AsvRadioCapabilities _capabilities = new AsvRadioCapabilities();

    public AsvRadioCapabilitiesBuilder SetFrequencyHz(uint min, uint max)
    {
        _capabilities.MinFrequencyHz = min;
        _capabilities.MaxFrequencyHz = max;
        return this;
    }
    public AsvRadioCapabilitiesBuilder SetReferencePowerDbm(float min, float max)
    {
        _capabilities.MinReferencePowerDbm = min;
        _capabilities.MaxReferencePowerDbm = max;
        return this;
    }
    public AsvRadioCapabilitiesBuilder SetTxPowerDbm(float min, float max)
    {
        _capabilities.MinTxPowerDbm = min;
        _capabilities.MaxTxPowerDbm = max;
        return this;
    }

    public AsvRadioCapabilitiesBuilder SetSupportedModulations(params AsvRadioModulation[] modulations)
    {
        foreach (var modulation in modulations)
        {
            _capabilities.InternalSupportedModulations.Add(modulation);
        }
        return this;
    }
    public AsvRadioCapabilitiesBuilder SetSupportedModulations(IEnumerable<AsvRadioModulation> modulations)
    {
        foreach (var modulation in modulations)
        {
            _capabilities.InternalSupportedModulations.Add(modulation);
        }
        return this;
    }
    public AsvRadioCapabilities Build()
    {
        return _capabilities;
    }

}

public class AsvRadioCapabilities
{
    public static AsvRadioCapabilities Empty => new AsvRadioCapabilitiesBuilder().Build();
    
    internal readonly HashSet<AsvRadioModulation> InternalSupportedModulations = new();

    internal AsvRadioCapabilities()
    {
        
    }
    
    public uint MinFrequencyHz { get; internal set; } = 10_000_000;
    public uint MaxFrequencyHz { get; internal set; } = 1_000_000_000;
    public float MinReferencePowerDbm { get; internal set; } = -100;
    public float MaxReferencePowerDbm { get; internal set; } = 10;
    public float MinTxPowerDbm { get; internal set; } = -100;
    public float MaxTxPowerDbm { get; internal set; } = 10;

    public IReadOnlySet<AsvRadioModulation> SupportedModulations => InternalSupportedModulations;
}