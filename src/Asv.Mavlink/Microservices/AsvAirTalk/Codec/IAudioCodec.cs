using System;

namespace Asv.Mavlink;

public interface IAudioCodec:IDisposable
{   
    void Encode(byte[] inputData, int inputSize, byte[] outputData, int outputSize, out int encodedSize);   
    int MaxEncodedSize { get; }
}