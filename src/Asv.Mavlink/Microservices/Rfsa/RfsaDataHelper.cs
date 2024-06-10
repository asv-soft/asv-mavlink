using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.AsvRfsa;

namespace Asv.Mavlink;

public static class RfsaHelper
{
    
    public const int SignalNameMaxLength = 16;
    private const string SignalNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex SignalNameRegex = new(SignalNameRegexString, RegexOptions.Compiled);
    
    public static void CheckSignalName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Signal name is empty");
        if (name.Length > SignalNameMaxLength)
            throw new Exception($"Signal name is too long. Max length is {SignalNameMaxLength}");
        if (SignalNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Signal name '{name}' not match regex '{SignalNameRegexString}')");
    }
    
    public static void CheckSignalAxisName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Signal axis name is empty");
        if (name.Length > SignalNameMaxLength)
            throw new Exception($"Signal axis name is too long. Max length is {SignalNameMaxLength}");
        
    }
    
    public static void WriteSignalMeasure(ref Span<byte> span, SignalInfo info, float value )
    {
        switch (info.Format)
        {
            case AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat8bit:
                BinSerialize.Write8BitRange(ref span, info.MinX, info.MaxX, value);
                break;
            case AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat16bit:
                BinSerialize.Write16BitRange(ref span, info.MinX, info.MaxX, value);
                break;
            case AsvRfsaSignalFormat.AsvSdrSignalFormatFloat:
                BinSerialize.WriteFloat(ref span,value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static float ReadSignalMeasure(ref ReadOnlySpan<byte> span, SignalInfo info)
    {
        return info.Format switch
        {
            AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat8bit => BinSerialize.Read8BitRange(ref span, info.MinX,
                info.MaxX),
            AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat16bit => BinSerialize.Read16BitRange(ref span, info.MinX,
                info.MaxX),
            AsvRfsaSignalFormat.AsvSdrSignalFormatFloat => BinSerialize.ReadFloat(ref span),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
   
    
    public static byte GetByteSizeOneMeasure(AsvRfsaSignalFormat format)
    {   
        return format switch
        {
            AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat8bit => 1,
            AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat16bit => 2,
            AsvRfsaSignalFormat.AsvSdrSignalFormatFloat => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}