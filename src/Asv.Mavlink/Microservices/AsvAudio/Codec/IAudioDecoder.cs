using System;

namespace Asv.Mavlink;

public interface IAudioDecoder:IDisposable
{   
    void Decode(byte[] inputData, int inputSize, byte[] outputData, out int decodedSize);   
    int MaxDecodedSize { get; }     
}