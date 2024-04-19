using System;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioEncoder:IDisposable
{   
    AsvAudioCodec Codec { get; }
    void Encode(ReadOnlyMemory<byte> input, Memory<byte> output, out int encodedSize);   
    int MaxEncodedSize { get; }
}