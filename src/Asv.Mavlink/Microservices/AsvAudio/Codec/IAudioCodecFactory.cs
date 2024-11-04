using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;
using R3;

namespace Asv.Mavlink;

public interface IAudioCodecFactory
{
    IAudioEncoder CreateEncoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input);
    IAudioDecoder CreateDecoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input);
    IEnumerable<AsvAudioCodec> AvailableCodecs { get; }

}