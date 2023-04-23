using System;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrClientRecordTag
{
    internal AsvSdrClientRecordTag(TagId id, AsvSdrRecordTagPayload payload)
    {
        Id = id;
        Type = payload.TagType;
        RawValue = payload.TagValue;
        Name = MavlinkTypesHelper.GetString(payload.TagName);
    }

    public string Name { get; }
    public TagId Id { get; }
    public byte[] RawValue { get; }
    
    public AsvSdrRecordTagType Type { get; }

    public ulong GetUint64() => BitConverter.ToUInt64(RawValue,0); 
    public long GetInt64() => BitConverter.ToInt64(RawValue,0);
    public double GetReal64() => BitConverter.ToDouble(RawValue,0);
    public string GetString() => MavlinkTypesHelper.GetString(RawValue);

    public override string ToString()
    {
        return Type switch
        {
            AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64 => $"{Name}:{GetUint64()}",
            AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64 => $"{Name}:{GetInt64()}",
            AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64 => $"{Name}:{GetReal64()}",
            AsvSdrRecordTagType.AsvSdrRecordTagTypeString8 => $"{Name}:{GetString()}",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}