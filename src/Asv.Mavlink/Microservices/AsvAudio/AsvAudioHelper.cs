#nullable enable
using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.Mavlink.AsvAudio;


namespace Asv.Mavlink;

public static class AsvAudioHelper
{
    public const int MaxPacketStreamData = AsvAudioStreamPayload.DataMaxItemsCount;
    public const int DeviceNameMaxLength = 16;
    private const string DeviceNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex RecordNameRegex = new(DeviceNameRegexString, RegexOptions.Compiled);
    
    public static void CheckDeviceName(string name)
    {
        var err = GetDeviceNameError(name);
        if (err != null)
        {
            throw new ArgumentException(err);
        }
    }
    
    public static bool IsDeviceNameValid(string? name)
    {
        if (name == null) return false;
        if (name.IsNullOrWhiteSpace()) return false;
        if (name.Length > DeviceNameMaxLength) return false;
        if (RecordNameRegex.IsMatch(name) == false) return false;
        return true;
    }

    public static string? GetDeviceNameError(string? name)
    {
        if (name == null || name.IsNullOrWhiteSpace())
        {
            return "Device name is empty";
        }
        if (name.Length > DeviceNameMaxLength)
        {
            return $"Record name is too long. Max length is {DeviceNameMaxLength}";
        }
        if (RecordNameRegex.IsMatch(name) == false)
        {
            return $"Record name '{name}' not match regex '{DeviceNameRegexString}')";
        }
        return null;
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
