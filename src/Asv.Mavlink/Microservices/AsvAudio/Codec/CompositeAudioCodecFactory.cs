using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;


public interface IAudioCodecFactoryPart
{
    AsvAudioCodec Codec { get; }
    IEnumerable<AudioCodecInfo> AvailableCodecs { get; }
    IAudioEncoder CreateEncoder(AudioCodecInfo codecInfo);
    IAudioDecoder CreateDecoder(AudioCodecInfo codecInfo);
}

public class CompositeAudioCodecFactory : IAudioCodecFactory
{
    private readonly ImmutableDictionary<AsvAudioCodec,IAudioCodecFactoryPart> _parts;

    public CompositeAudioCodecFactory(IEnumerable<IAudioCodecFactoryPart> parts)
    {
        _parts = parts.ToImmutableDictionary(x => x.Codec);
    }
   

    public IAudioEncoder CreateEncoder(AudioCodecInfo info)
    {
        if (_parts.TryGetValue(info.Codec, out var part) == false)
        {
            throw new Exception($"Codec {info.Name} not supported");
        }
        return part.CreateEncoder(info);
    }

    public IAudioDecoder CreateDecoder(AudioCodecInfo info)
    {
        if (_parts.TryGetValue(info.Codec, out var part) == false)
        {
            throw new Exception($"Codec {info.Name} not supported");
        }
        return part.CreateDecoder(info);
    }

    public IEnumerable<AudioCodecInfo> AvailableCodecs => _parts.SelectMany(x=>x.Value.AvailableCodecs);

    public bool TryFindCodec(AsvAudioPcmFormat format, int sampleRate, int channels, AsvAudioCodec codec,
        byte codecAdditionalParam, out AudioCodecInfo codecInfo)
    {
        if (_parts.TryGetValue(codec, out var part) == false)
        {
            throw new Exception($"Codec {codec} not supported");
        }
        codecInfo = part.AvailableCodecs.FirstOrDefault(x => x.IsEqual(format, sampleRate, channels,codec, codecAdditionalParam));
        return codecInfo != null;
    }
}