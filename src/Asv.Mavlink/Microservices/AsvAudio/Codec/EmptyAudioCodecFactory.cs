using System;
using System.Collections.Generic;
using Asv.Mavlink.AsvAudio;
using R3;

namespace Asv.Mavlink;

public class EmptyAudioCodecFactory:IAudioCodecFactory
{
    private static EmptyAudioCodecFactory? _instance;
    public static IAudioCodecFactory Instance => _instance ??= new EmptyAudioCodecFactory();
    private EmptyAudioCodecFactory()
    {
        
    }
    
    public IAudioEncoder CreateEncoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input)
    {
        throw new NotImplementedException();
    }

    public IAudioDecoder CreateDecoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AsvAudioCodec> AvailableCodecs => Array.Empty<AsvAudioCodec>();
}