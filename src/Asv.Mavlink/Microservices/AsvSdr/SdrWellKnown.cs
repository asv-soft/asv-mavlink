using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public static class SdrWellKnown
{
    public const int RecordTagValueMaxLength = 4;
    public const int RecordNameMaxLength = 27;
    private const string RecordNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,27}$";
    private static readonly Regex RecordNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);
    
    
    public const int RecordTagNameMaxLength = 16;
    private const string RecordTagNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex RecordTagNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);
    

    public static void CheckRecordName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (name.Length > RecordNameMaxLength) throw new Exception($"Record name is too long. Max length is {SdrWellKnown.RecordNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{RecordNameRegexString}')");
    }
    
    public static void CheckTagName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Tag name is empty");
        if (name.Length > RecordNameMaxLength) throw new Exception($"Tag name is too long. Max length is {SdrWellKnown.RecordNameMaxLength}");
        if (RecordTagNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Tag name '{name}' not match regex '{RecordTagNameRegexString}')");
    }
    
    public static float GetCommandParamValue(ushort recordIndex, AsvSdrRecordTagFlag asvSdrRecordTagFlagForCurrent, AsvSdrRecordTagType tagType)
    {
        var array = new byte[4];
        BitConverter.TryWriteBytes(array, recordIndex);
        array[2] = (byte)asvSdrRecordTagFlagForCurrent;
        array[3] = (byte)tagType;
        return BitConverter.ToSingle(array);
    }
    public static void ParseCommandParamValue(float paramValue, out ushort recordIndex, out AsvSdrRecordTagFlag asvSdrRecordTagFlagForCurrent, out AsvSdrRecordTagType tagType)
    {
        var value = BitConverter.GetBytes(paramValue);
        recordIndex = BitConverter.ToUInt16(value, 0);
        asvSdrRecordTagFlagForCurrent = (AsvSdrRecordTagFlag)value[2];
        tagType = (AsvSdrRecordTagType)value[3];
    }
    
    

    
}