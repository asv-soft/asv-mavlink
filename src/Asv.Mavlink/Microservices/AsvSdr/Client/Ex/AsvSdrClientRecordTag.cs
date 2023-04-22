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
    }
    public TagId Id { get; }
    public byte[] RawValue { get; }
    
    public AsvSdrRecordTagType Type { get; }

    public ulong GetUint64() => BitConverter.ToUInt64(RawValue,0); 
    public long GetInt64() => BitConverter.ToInt64(RawValue,0);
    public double GetReal64() => BitConverter.ToDouble(RawValue,0);
    public string GetString() => MavlinkTypesHelper.GetString(RawValue);
}