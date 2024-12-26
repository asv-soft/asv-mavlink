using System;
using System.Linq;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public static class MavParamExtHelper
{
    public const int ParamExtNameMaxLength = 16;
    private const string ParamExtNameRegexString = "^[A-Za-z][A-Za-z0-9_]{2,16}$";
    private static readonly Regex RecordNameRegex = new(ParamExtNameRegexString, RegexOptions.Compiled);

    public static void CheckParamName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new MavlinkException(
            "Param name is empty");
        if (name.Length > ParamExtNameMaxLength)
            throw new MavlinkException(
                $"Param name is too long. Max length is {ParamExtNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Param name '{name}' not match regex '{ParamExtNameRegexString}')");
    }
    
    public static MavParamExtValue CreateFromBuffer(char[] buffer, MavParamExtType type) => 
        CreateFromBuffer(buffer.Select(ch => (byte)ch).ToArray(), type);

    public static MavParamExtValue CreateFromBuffer(byte[] buffer, MavParamExtType type) =>
        type switch
        {
            MavParamExtType.MavParamExtTypeUint8 => new MavParamExtValue(buffer[0]),
            MavParamExtType.MavParamExtTypeInt8 => new MavParamExtValue((sbyte)buffer[0]),
            MavParamExtType.MavParamExtTypeUint16 => new MavParamExtValue(BitConverter.ToUInt16(buffer, 0)),
            MavParamExtType.MavParamExtTypeInt16 => new MavParamExtValue(BitConverter.ToInt16(buffer, 0)),
            MavParamExtType.MavParamExtTypeUint32 => new MavParamExtValue(BitConverter.ToUInt32(buffer, 0)),
            MavParamExtType.MavParamExtTypeInt32 => new MavParamExtValue(BitConverter.ToInt32(buffer, 0)),
            MavParamExtType.MavParamExtTypeReal32 => new MavParamExtValue(BitConverter.ToSingle(buffer, 0)),
            MavParamExtType.MavParamExtTypeUint64 => new MavParamExtValue(BitConverter.ToUInt64(buffer, 0)),
            MavParamExtType.MavParamExtTypeInt64 => new MavParamExtValue(BitConverter.ToInt64(buffer, 0)),
            MavParamExtType.MavParamExtTypeReal64 => new MavParamExtValue(BitConverter.ToDouble(buffer, 0)),
            MavParamExtType.MavParamExtTypeCustom => new MavParamExtValue(buffer),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported MavParamExtType value")
        };
}