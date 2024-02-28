using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public static class AsvAudioHelper
{
    public const int MaxPacketStreamData = AsvAudioStreamPayload.DataMaxItemsCount;
    
    public const int DeviceNameLength = 8;

    public const int DeviceNameMaxLength = 16;
    private const string DeviceNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex RecordNameRegex = new(DeviceNameRegexString, RegexOptions.Compiled);
    
    public static void CheckDeviceName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Device name is empty");
        if (name.Length > DeviceNameMaxLength)
            throw new Exception($"Record name is too long. Max length is {DeviceNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{DeviceNameRegexString}')");
    }

    public static int GetSampleRate(AsvAudioSampleRate rate)
    {
        return rate switch
        {
            AsvAudioSampleRate.AsvAudioSampleRate8000Hz => 8000,
            AsvAudioSampleRate.AsvAudioSampleRate11025Hz => 11_025,
            AsvAudioSampleRate.AsvAudioSampleRate12000Hz => 12_000,
            AsvAudioSampleRate.AsvAudioSampleRate16000Hz => 16_000,
            AsvAudioSampleRate.AsvAudioSampleRate22050Hz => 22_050,
            AsvAudioSampleRate.AsvAudioSampleRate24000Hz => 24_000,
            AsvAudioSampleRate.AsvAudioSampleRate44100Hz => 44_100,
            AsvAudioSampleRate.AsvAudioSampleRate48000Hz => 48_000,
            AsvAudioSampleRate.AsvAudioSampleRate88200Hz => 88_200,
            AsvAudioSampleRate.AsvAudioSampleRate96000Hz => 96_000,
            AsvAudioSampleRate.AsvAudioSampleRate176400Hz => 176_400,
            AsvAudioSampleRate.AsvAudioSampleRate192000Hz => 192_000,
            AsvAudioSampleRate.AsvAudioSampleRate328800Hz => 328_800,
            AsvAudioSampleRate.AsvAudioSampleRate384000Hz => 384_000,
            _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
        };
    }

    public static int GetChannels(AsvAudioChannel channel)
    {
        return channel switch
        {
            AsvAudioChannel.AsvAudioChannelMono => 1,
            AsvAudioChannel.AsvAudioChannelStereo => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }

}