#nullable enable
using System;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public class AudioCodecInfo: IEquatable<AudioCodecInfo>
{
    public AudioCodecInfo(AsvAudioCodec codec, byte codecAdditionalParam, AsvAudioPcmFormat format, AsvAudioSampleRate sampleRate,
        AsvAudioChannel channels, string name)
    {
        Codec = codec;
        CodecConfig = codecAdditionalParam;
        Format = format;
        SampleRate = sampleRate;
        Channel = channels;
        Name = name;
    }

    public string Name { get; }
    public AsvAudioCodec Codec { get; }
    public byte CodecConfig { get; }

    public AsvAudioPcmFormat Format { get; }
    public AsvAudioSampleRate SampleRate { get; }
    public AsvAudioChannel Channel { get; }
    

    public override string ToString()
    {
        return Name;
    }

    public bool IsEqual(AsvAudioPcmFormat format, AsvAudioSampleRate sampleRate, AsvAudioChannel channels, AsvAudioCodec codec, byte codecAdditionalParam)
    {
        return Format == format && SampleRate == sampleRate && Channel == channels && Codec == codec && CodecConfig == codecAdditionalParam;
    }

    public bool Equals(AudioCodecInfo? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Codec == other.Codec && CodecConfig == other.CodecConfig && Format == other.Format && SampleRate == other.SampleRate && Channel == other.Channel;
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
        return HashCode.Combine((int)Codec, CodecConfig, (int)Format, SampleRate, Channel);
    }

    public static bool operator ==(AudioCodecInfo? left, AudioCodecInfo? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AudioCodecInfo? left, AudioCodecInfo? right)
    {
        return !Equals(left, right);
    }
}