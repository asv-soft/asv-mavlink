using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioCodecFactory
{
    IAudioEncoder CreateEncoder(AudioCodecInfo info);
    IAudioDecoder CreateDecoder(AudioCodecInfo info);
    IEnumerable<AudioCodecInfo> AvailableCodecs { get; }
    bool TryFindCodec(AsvAudioPcmFormat format, AsvAudioSampleRate sampleRate, AsvAudioChannel channels, AsvAudioCodec codec,
        byte codecAdditionalParam, out AudioCodecInfo codecInfo); 
}