using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioCodecFactory
{
    IAudioEncoder CreateEncoder(AsvAudioCodec codec, IObservable<ReadOnlyMemory<byte>> input);
    IAudioDecoder CreateDecoder(AsvAudioCodec codec, IObservable<ReadOnlyMemory<byte>> input);
    IEnumerable<AsvAudioCodec> AvailableCodecs { get; }

}