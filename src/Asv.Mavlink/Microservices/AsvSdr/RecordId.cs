using System;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class RecordId:IEquatable<RecordId>
{
    public static RecordId Empty { get; } = new("NOT_RECORDED");
    
    public RecordId(string name)
    {
        SdrWellKnown.CheckRecordName(name);
        Name = name;
    }

    internal RecordId(AsvSdrRecordPayload payload)
    {
        Name = MavlinkTypesHelper.GetString(payload.RecordName);
    }

    internal RecordId(AsvSdrRecordDeleteResponsePayload payload)
    {
        Name = MavlinkTypesHelper.GetString(payload.RecordName);
    }

    public RecordId(AsvSdrRecordTagRequestPayload payload)
    {
        Name = MavlinkTypesHelper.GetString(payload.RecordName);
    }

    public RecordId(AsvSdrRecordDeleteRequestPayload payload)
    {
        Name = MavlinkTypesHelper.GetString(payload.RecordName);
    }

    public RecordId(AsvSdrRecordDataRequestPayload payload)
    {
        Name = MavlinkTypesHelper.GetString(payload.RecordName);
    }

    public string Name { get; }


    public bool Equals(RecordId other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RecordId)obj);
    }

    public override int GetHashCode()
    {
        return (Name != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name) : 0);
    }

    public static bool operator ==(RecordId left, RecordId right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RecordId left, RecordId right)
    {
        return !Equals(left, right);
    }

    public bool IsEqualName(string recordName)
    {
        return string.Equals(Name, recordName, StringComparison.InvariantCultureIgnoreCase);
    }

    public override string ToString()
    {
        return $"RECORD:{Name}";
    }
}