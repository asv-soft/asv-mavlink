using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public class RawCodecFactoryPart:IAudioCodecFactoryPart
{
    public AsvAudioCodec Codec => AsvAudioCodec.AsvAudioCodecRaw;

    public IEnumerable<AudioCodecInfo> AvailableCodecs
    {
        get
        {
            yield break;
        }
    }

    public IAudioEncoder CreateEncoder(AudioCodecInfo codecInfo)
    {
        throw new System.NotImplementedException();
    }

    public IAudioDecoder CreateDecoder(AudioCodecInfo codecInfo)
    {
        throw new System.NotImplementedException();
    }

}

public class RawCodec:IAudioEncoder,IAudioDecoder,IDisposable
{
    public void Encode(byte[] inputData, int inputSize, byte[] outputData, int outputSize, out int encodedSize)
    {
        throw new NotImplementedException();
    }

    public int MaxEncodedSize { get; }
    
    public void Decode(byte[] inputData, int inputSize, byte[] outputData, out int decodedSize)
    {
        throw new NotImplementedException();
    }

    public int MaxDecodedSize { get; }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}