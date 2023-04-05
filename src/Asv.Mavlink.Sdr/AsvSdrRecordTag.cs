using System;
using System.Linq;
using System.Text;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrRecordTag : ICloneable
{
    private const int MaxNameLength = 16;
    
    internal AsvSdrRecordTag(AsvSdrRecordTagPayload payload)
    {
        Name = GetName(payload);
        Type = payload.TagType;
        RawValue = payload.TagValue;
    }
    
    private AsvSdrRecordTag(string name,AsvSdrRecordTagType type)
    {
        if (name.Length > MaxNameLength) throw new ArgumentOutOfRangeException(nameof(name), $"Max length is {MaxNameLength} chars");      
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
        
    public ulong GetUint64() => BitConverter.ToUInt64(RawValue,0); 
    public long GetInt64() => BitConverter.ToInt64(RawValue,0);
    public double GetReal64() => BitConverter.ToDouble(RawValue,0);
    public string GetString() => new(RawValue.Select(_=>(char)_).Where(_ => _ != '\0').ToArray());
        
    private static string GetName(AsvSdrRecordTagPayload payload) => new(payload.TagName.Where(_ => _ != '\0').ToArray());
    
    public object Clone()
    {
        return MemberwiseClone();
    }
}