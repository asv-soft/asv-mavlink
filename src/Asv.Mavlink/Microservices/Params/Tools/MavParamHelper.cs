using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public static class MavParamHelper
{
    
    public const int ParamNameMaxLength = 16;
    private const string ParamNameRegexString = "^[A-Za-z][A-Za-z0-9_]{2,16}$";
    private static readonly Regex RecordNameRegex = new(ParamNameRegexString, RegexOptions.Compiled);
    
    public static void CheckParamName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Param name is empty");
        if (name.Length > ParamNameMaxLength) throw new Exception($"Param name is too long. Max length is {ParamNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Param name '{name}' not match regex '{ParamNameRegexString}')");
    }
   
    public static IMavParamEncoding GetEncoding(MavParamEncodingType type)
    {
        return type switch
        {
            MavParamEncodingType.ByteWiseEncoding => ByteWiseEncoding,
            MavParamEncodingType.CStyleEncoding => CStyleEncoding,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    public static IEnumerable<MavParamEncodingType> EncodingTypes
    {
        get
        {
            yield return MavParamEncodingType.ByteWiseEncoding;
            yield return MavParamEncodingType.CStyleEncoding;    
        }
    }
    public static IEnumerable<IMavParamEncoding> Encodings => EncodingTypes.Select(GetEncoding);

    public static IMavParamEncoding ByteWiseEncoding { get; } = new MavParamByteWiseEncoding();
    public static IMavParamEncoding CStyleEncoding { get; } = new MavParamCStyleEncoding();

    public static MavParamValue ReadFromConfig(IConfiguration config, string key, IMavParamTypeMetadata type)
    {
        switch (type.Type)
        {
            case MavParamType.MavParamTypeUint8:
                return new MavParamValue(config.Get(key, (byte)type.DefaultValue));
            case MavParamType.MavParamTypeInt8:
                return new MavParamValue(config.Get(key, (sbyte)type.DefaultValue));
            case MavParamType.MavParamTypeUint16:
                return new MavParamValue(config.Get(key, (ushort)type.DefaultValue));
            case MavParamType.MavParamTypeInt16:
                return new MavParamValue(config.Get(key, (short)type.DefaultValue));
            case MavParamType.MavParamTypeUint32:
                return new MavParamValue(config.Get(key, (uint)type.DefaultValue));
            case MavParamType.MavParamTypeInt32:
                return new MavParamValue(config.Get(key, (int)type.DefaultValue));
            case MavParamType.MavParamTypeReal32:
                return new MavParamValue(config.Get(key, (float)type.DefaultValue));
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    public static void WriteToConfig(IConfiguration config, string key, MavParamValue value)
    {
        switch (value.Type)
        {
            case MavParamType.MavParamTypeUint8:
                config.Set(key, (byte)value);
                break;
            case MavParamType.MavParamTypeInt8:
                config.Set(key, (sbyte)value);
                break;
            case MavParamType.MavParamTypeUint16:
                config.Set(key, (ushort)value);
                break;
            case MavParamType.MavParamTypeInt16:
                config.Set(key, (short)value);
                break;
            case MavParamType.MavParamTypeUint32:
                config.Set(key, (uint)value);
                break;
            case MavParamType.MavParamTypeInt32:
                config.Set(key, (int)value);
                break;
            case MavParamType.MavParamTypeReal32:
                config.Set(key, (float)value);
                break;
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
    
}

public enum MavParamEncodingType
{
    /// <summary>
    /// The parameter's bytes are copied directly into the bytes used for the field. A 32-bit integer is sent as 32 bits of data.
    /// </summary>
    ByteWiseEncoding,
    /// <summary>
    /// The parameter value is converted to a float. This may result in some loss of precision as a float can represent an integer with up to 24 bits of pecision.
    /// </summary>
    CStyleEncoding
}