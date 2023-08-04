using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public static class AsvSdrHelper
{
    public const int RecordTagValueMaxLength = 8;
    
    public const int RecordNameMaxLength = 28;
    private const string RecordNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,28}$";
    private static readonly Regex RecordNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);
    
    
    public const int RecordTagNameMaxLength = 16;
    private const string RecordTagNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex RecordTagNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);
    
    public static void CheckRecordName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (name.Length > RecordNameMaxLength) throw new Exception($"Record name is too long. Max length is {RecordNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{RecordNameRegexString}')");
    }
    
    public static void CheckTagName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Tag name is empty");
        if (name.Length > RecordTagNameMaxLength) throw new Exception($"Tag name is too long. Max length is {RecordTagNameMaxLength}");
        if (RecordTagNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Tag name '{name}' not match regex '{RecordTagNameRegexString}')");
    }
    
    
    

    
}