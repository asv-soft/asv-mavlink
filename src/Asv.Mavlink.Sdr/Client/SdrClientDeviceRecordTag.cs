using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink.Sdr;

public class SdrClientDeviceRecordTag
{
    internal SdrClientDeviceRecordTag(AsvSdrRecordTagPayload payload)
    {
        Name = MavlinkTypesHelper.GetString(payload.TagName);
        Type = payload.TagType;
        RawValue = payload.TagValue;
        TagIndex = payload.TagIndex;
        RecordIndex = payload.RecordIndex;
    }
    public ushort RecordIndex { get; }
    public ushort TagIndex { get; }
    public byte[] RawValue { get; }
    public string Name { get; }
    public AsvSdrRecordTagType Type { get; }

    public ulong GetUint64() => BitConverter.ToUInt64(RawValue,0); 
    public long GetInt64() => BitConverter.ToInt64(RawValue,0);
    public double GetReal64() => BitConverter.ToDouble(RawValue,0);
    public string GetString() => MavlinkTypesHelper.GetString(RawValue);
}