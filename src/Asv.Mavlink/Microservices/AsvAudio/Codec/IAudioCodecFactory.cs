using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioCodecFactory
{
    IAudioEncoder CreateEncoder(AsvAudioCodec codec);
    IAudioDecoder CreateDecoder(AsvAudioCodec codec);
    IEnumerable<AsvAudioCodec> AvailableCodecs { get; }

}