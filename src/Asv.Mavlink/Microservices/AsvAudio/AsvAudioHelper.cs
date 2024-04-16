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

    public static string GetConfigName(AsvAudioCodec codec)
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

        return "";
    }

}
