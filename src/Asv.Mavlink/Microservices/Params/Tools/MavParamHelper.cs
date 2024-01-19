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