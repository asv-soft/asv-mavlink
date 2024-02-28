using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public class RawCodecFactoryPart:IAudioCodecFactoryPart
{
    public static AudioCodecInfo Raw8KHz16BitMono => new(AsvAudioCodec.AsvAudioCodecRaw, 0, AsvAudioPcmFormat.AsvAudioPcmFormatInt16, AsvAudioSampleRate.AsvAudioSampleRate8000Hz, AsvAudioChannel.AsvAudioChannelMono, "Raw 8kHz 16bit mono");
    
    public AsvAudioCodec Codec => AsvAudioCodec.AsvAudioCodecRaw;

    public IEnumerable<AudioCodecInfo> AvailableCodecs
    {
        get
        {
            yield return Raw8KHz16BitMono;
        }
    }

    public IAudioEncoder CreateEncoder(AudioCodecInfo codecInfo)
    {
        return new RawCodec(codecInfo);
    }

    public IAudioDecoder CreateDecoder(AudioCodecInfo codecInfo)
    {
        return new RawCodec(codecInfo);
    }

}

public class RawCodec:IAudioEncoder,IAudioDecoder,IDisposable
{
    private readonly AudioCodecInfo _codecInfo;

    public RawCodec(AudioCodecInfo codecInfo)
    {
        _codecInfo = codecInfo;
    }

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