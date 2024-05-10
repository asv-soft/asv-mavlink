using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public class RawCodecFactoryPart:IAudioCodecFactory
{
    

    public IAudioEncoder CreateEncoder(AsvAudioCodec codec, IObservable<ReadOnlyMemory<byte>> input)
    {
        return new RawCodec(input);
    }

    public IAudioDecoder CreateDecoder(AsvAudioCodec codec, IObservable<ReadOnlyMemory<byte>> input)
    {
        return new RawCodec(input);
    }

    public IEnumerable<AsvAudioCodec> AvailableCodecs
    {
        get
        {
            yield return RawCodec.CodecId;
        }
    }
}

public class RawCodec: DisposableOnceWithCancel,IAudioEncoder,IAudioDecoder
{
    private readonly Subject<ReadOnlyMemory<byte>> _output;
    public const AsvAudioCodec CodecId = AsvAudioCodec.AsvAudioCodecRaw8000Mono;
    
    public AsvAudioCodec Codec => AsvAudioCodec.AsvAudioCodecRaw8000Mono;

    public RawCodec(IObservable<ReadOnlyMemory<byte>> src)
    {
        _output = new Subject<ReadOnlyMemory<byte>>().DisposeItWith(Disposable);
        src.Subscribe(_output);
    }


    public IDisposable Subscribe(IObserver<ReadOnlyMemory<byte>> observer)
    {
        return _output.Subscribe(observer);
    }
}