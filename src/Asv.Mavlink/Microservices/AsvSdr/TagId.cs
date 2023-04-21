using System;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class TagId:IEquatable<TagId>
{
    public TagId(string tagName, string recordName)
    {
        SdrWellKnown.CheckTagName(tagName);
        SdrWellKnown.CheckRecordName(recordName);
        TagName = tagName;
        RecordName = recordName;
    }

    internal TagId(AsvSdrRecordTagDeleteResponsePayload payload)
    {
        RecordName = MavlinkTypesHelper.GetString(payload.RecordName);
        TagName = MavlinkTypesHelper.GetString(payload.TagName);
    }

    internal TagId(AsvSdrRecordTagPayload payload)
    {
        RecordName = MavlinkTypesHelper.GetString(payload.RecordName);
        TagName = MavlinkTypesHelper.GetString(payload.TagName);
    }

    public TagId(AsvSdrRecordTagDeleteRequestPayload payload)
    {
        RecordName = MavlinkTypesHelper.GetString(payload.RecordName);
        TagName = MavlinkTypesHelper.GetString(payload.TagName);
    }

    public string RecordName { get; }
    public string TagName { get;  }

    public bool Equals(TagId other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(RecordName, other.RecordName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(TagName, other.TagName, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TagId)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(RecordName, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add(TagName, StringComparer.InvariantCultureIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(TagId left, TagId right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TagId left, TagId right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"TAG:{RecordName}.{TagName}";
    }
}