using System;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioEncoder:IDisposable
{   
    AsvAudioCodec Codec { get; }
    void Encode(byte[] inputData, int inputSize, byte[] outputData, int outputSize, out int encodedSize);   
    int MaxEncodedSize { get; }
}