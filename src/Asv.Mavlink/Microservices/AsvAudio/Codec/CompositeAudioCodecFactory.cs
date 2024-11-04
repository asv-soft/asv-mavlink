using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using R3;

namespace Asv.Mavlink;



public class CompositeAudioCodecFactory : IAudioCodecFactory
{
    private readonly ImmutableArray<IAudioCodecFactory> _parts;

    public CompositeAudioCodecFactory(IEnumerable<IAudioCodecFactory> parts)
    {
        _parts = [..parts];
        //check codec unique
        var codecs = _parts.GroupBy(x => x.AvailableCodecs).Where(x => x.Count() > 1).ToImmutableArray();
        if (codecs.Any())
        {
            throw new Exception($"Codec {codecs.First().Key} duplicated");
        }
    }


    public IAudioEncoder CreateEncoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input)
    {
        var result = _parts.FirstOrDefault(part => part.AvailableCodecs.Contains(codec))?.CreateEncoder(codec, input);
        if (result == null) throw new Exception($"Codec {codec} not supported");
        return result;
    }

    public IAudioDecoder CreateDecoder(AsvAudioCodec codec, Observable<ReadOnlyMemory<byte>> input)
    {
        var result = _parts.FirstOrDefault(part => part.AvailableCodecs.Contains(codec))?.CreateDecoder(codec, input);
        if (result == null) throw new Exception($"Codec {codec} not supported");
        return result;
    }

    public IEnumerable<AsvAudioCodec> AvailableCodecs => _parts.SelectMany(x => x.AvailableCodecs);
}