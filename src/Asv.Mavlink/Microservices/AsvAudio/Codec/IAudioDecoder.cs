using System;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioDecoder:IDisposable
{   
    AsvAudioCodec Codec { get; }
    void Decode(byte[] inputData, int inputSize, byte[] outputData, out int decodedSize);   
    int MaxDecodedSize { get; }     
}