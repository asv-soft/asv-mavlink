#nullable enable
using System;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;


/// <summary>
///  ASV_AUDIO_CHANNEL
/// </summary>
public enum AsvAudioChannel:uint
{
    /// <summary>
    /// Mono, 1 channel.
    /// ASV_AUDIO_CHANNEL_MONO
    /// </summary>
    AsvAudioChannelMono = 1,
    /// <summary>
    /// Stereo, 2 channel.
    /// ASV_AUDIO_CHANNEL_STEREO
    /// </summary>
    AsvAudioChannelStereo = 2,
}

/// <summary>
/// Count of bit per sample for decoded.
///  ASV_AUDIO_PCM_FORMAT
/// </summary>
public enum AsvAudioPcmFormat:uint
{
    /// <summary>
    /// 8 bit per sample.
    /// ASV_AUDIO_PCM_FORMAT_INT8
    /// </summary>
    AsvAudioPcmFormatInt8 = 1,
    /// <summary>
    /// 16 bit per sample.
    /// ASV_AUDIO_PCM_FORMAT_INT16
    /// </summary>
    AsvAudioPcmFormatInt16 = 2,
}

public class AudioCodecInfo: IEquatable<AudioCodecInfo>
{
    public static AudioCodecInfo Create(AsvAudioCodec codec)
    {
        switch (codec)
        {
            case AsvAudioCodec.AsvAudioCodecUnknown:
                break;
            case AsvAudioCodec.AsvAudioCodecReserved255:
                break;
            case AsvAudioCodec.AsvAudioCodecRaw8000Mono:
                break;
            case AsvAudioCodec.AsvAudioCodecOpus8000Mono:
                break;
            case AsvAudioCodec.AsvAudioCodecAac:
                break;
            case AsvAudioCodec.AsvAudioCodecPcmu:
                break;
            case AsvAudioCodec.AsvAudioCodecPcma:
                break;
            case AsvAudioCodec.AsvAudioCodecSpeex:
                break;
            case AsvAudioCodec.AsvAudioCodecIlbc:
                break;
            case AsvAudioCodec.AsvAudioCodecG722:
                break;
            case AsvAudioCodec.AsvAudioCodecL16:
                break;
            case AsvAudioCodec.AsvAudioCodecReserved:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(codec), codec, null);
        }
        throw new ArgumentOutOfRangeException(nameof(codec), codec, null);
    }
    
    private AudioCodecInfo(AsvAudioCodec codec, AsvAudioPcmFormat format, uint sampleRate,
        AsvAudioChannel channels, string name)
    {
        Codec = codec;
        Format = format;
        SampleRate = sampleRate;
        Channel = channels;
        Name = name;
    }

    public string Name { get; }
    public AsvAudioCodec Codec { get; }
    public AsvAudioPcmFormat Format { get; }
    public uint SampleRate { get; }
    public AsvAudioChannel Channel { get; }

    public bool Equals(AudioCodecInfo? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Codec == other.Codec && Format == other.Format && SampleRate == other.SampleRate && Channel == other.Channel;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AudioCodecInfo)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, (int)Codec, (int)Format, SampleRate, (int)Channel);
    }

    public static bool operator ==(AudioCodecInfo left, AudioCodecInfo right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AudioCodecInfo left, AudioCodecInfo right)
    {
        return !Equals(left, right);
    }
}