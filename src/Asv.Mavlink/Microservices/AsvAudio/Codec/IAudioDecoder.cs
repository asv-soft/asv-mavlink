using System;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioDecoder:IDisposable
{   
    AsvAudioCodec Codec { get; }
    void Decode(ReadOnlyMemory<byte> input, Memory<byte> output,out int decodedSize);   
    int MaxDecodedSize { get; }     
}