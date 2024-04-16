using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public class RawCodecFactoryPart:IAudioCodecFactory
{
    public IAudioEncoder CreateEncoder(AsvAudioCodec codec)
    {
        return new RawCodec();
    }

    public IAudioDecoder CreateDecoder(AsvAudioCodec codec)
    {
        return new RawCodec();
    }

    public IEnumerable<AsvAudioCodec> AvailableCodecs
    {
        get
        {
            yield return RawCodec.CodecId;
        }
    }
}

public class RawCodec:IAudioEncoder,IAudioDecoder
{
    public const AsvAudioCodec CodecId = AsvAudioCodec.AsvAudioCodecRaw8000Mono;
    
    public AsvAudioCodec Codec => AsvAudioCodec.AsvAudioCodecRaw8000Mono;

    public void Encode(byte[] inputData, int inputSize, byte[] outputData, int outputSize, out int encodedSize)
    {
        encodedSize = inputSize;
        inputData.CopyTo(outputData,0);
    }

    public int MaxEncodedSize => 58_650;
    
    public void Decode(byte[] inputData, int inputSize, byte[] outputData, out int decodedSize)
    {
        decodedSize = inputSize;
        inputData.CopyTo(outputData,0);
    }

    public int MaxDecodedSize => 58_650;
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}