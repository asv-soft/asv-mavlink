using System;
using System.Text;
using Asv.Mavlink.Sdr;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrRecordTag
{
    private AsvSdrRecordTag(string name,AsvSdrRecordTagType type)
    {
        if (name.Length > SdrWellKnown.RecordTagNameMaxLength) throw new ArgumentOutOfRangeException(nameof(name), $"Max length is {SdrWellKnown.RecordTagNameMaxLength} chars");      
        Name = name;
        Type = type;
        RawValue = Array.Empty<byte>();
    }
        
    public AsvSdrRecordTag(string name, ulong value):this(name,AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64)
    {
        RawValue = BitConverter.GetBytes(value);
    }
        
    public AsvSdrRecordTag(string name, long value):this(name,AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64)
    {
        RawValue = BitConverter.GetBytes(value);
    }
        
    public AsvSdrRecordTag(string name, double value):this(name,AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64)
    {
        RawValue = BitConverter.GetBytes(value);
    }
        
    public AsvSdrRecordTag(string name, string value):this(name,AsvSdrRecordTagType.AsvSdrRecordTagTypeString)
    {
        if (value.Length > 8) throw new ArgumentOutOfRangeException(nameof(value), "Max length is 8 chars");
        RawValue = new byte[8];
        Encoding.ASCII.GetBytes(value).CopyTo(RawValue, 0);;
    }
    public byte[] RawValue { get; }
    public string Name { get; }
    public AsvSdrRecordTagType Type { get; }
}