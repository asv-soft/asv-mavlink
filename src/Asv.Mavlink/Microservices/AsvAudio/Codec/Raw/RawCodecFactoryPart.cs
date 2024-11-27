using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using R3;

namespace Asv.Mavlink;

public class RawCodecFactoryPart:IAudioCodecFactory
{
    public const AsvAudioCodec CodecId = AsvAudioCodec.AsvAudioCodecRaw8000Mono;
    public IAudioEncoder CreateEncoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input)
    {
        return new RawEncoder(input);
    }

    public IAudioDecoder CreateDecoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input)
    {
        return new RawDecoder(input);
    }

    public IEnumerable<AsvAudioCodec> AvailableCodecs
    {
        get
        {
            yield return CodecId;
        }
    }
}

public class RawDecoder : IAudioDecoder
{
    
    public RawDecoder(Observable<ReadOnlyMemory<byte>> input)
    {
        Output = input;
    }

    public Observable<ReadOnlyMemory<byte>> Output { get; }
    public AsvAudioCodec Codec => RawCodecFactoryPart.CodecId;

    public void Dispose()
    {
        
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}

public class RawEncoder : IAudioEncoder
{
    public RawEncoder(Observable<ReadOnlyMemory<byte>> input)
    {
        Output = input;
    }

    public void Dispose()
    {
        
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.CompletedTask);
    }

    public Observable<ReadOnlyMemory<byte>> Output { get; }
    public AsvAudioCodec Codec => RawCodecFactoryPart.CodecId;
}
