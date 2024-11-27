using Asv.Mavlink.AsvSdr;
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

    public ulong GetUint64() => AsvSdrHelper.GetTagValueAsUInt64(RawValue, Type);
    public long GetInt64() => AsvSdrHelper.GetTagValueAsInt64(RawValue, Type);
    public double GetReal64() => AsvSdrHelper.GetTagValueAsReal64(RawValue,Type);
    public string GetString() => AsvSdrHelper.GetTagValueAsString(RawValue,Type);

    public override string ToString() => AsvSdrHelper.PrintTag(Name, Type, RawValue);

    public void CopyTo(AsvSdrRecordTagPayload dest)
    {
        MavlinkTypesHelper.SetString(dest.TagName, Name);
        dest.TagType = Type;
        RawValue.CopyTo(dest.TagValue, 0);
        MavlinkTypesHelper.SetGuid(dest.RecordGuid, Id.RecordGuid);
        MavlinkTypesHelper.SetGuid(dest.TagGuid, Id.TagGuid);
    }
}