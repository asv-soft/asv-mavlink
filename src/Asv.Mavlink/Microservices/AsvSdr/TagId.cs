using System;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class TagId:IEquatable<TagId>
{
    public TagId(Guid tagGuid, Guid recordGuid)
    {
        TagGuid = tagGuid;
        RecordGuid = recordGuid;
    }

    internal TagId(AsvSdrRecordTagDeleteResponsePayload payload)
    {
        RecordGuid = new Guid(payload.RecordGuid);
        TagGuid = new Guid(payload.TagGuid);
    }

    internal TagId(AsvSdrRecordTagPayload payload)
    {
        RecordGuid = new Guid(payload.RecordGuid);
        TagGuid = new Guid(payload.TagGuid);
    }

    public TagId(AsvSdrRecordTagDeleteRequestPayload payload)
    {
        RecordGuid = new Guid(payload.RecordGuid);
        TagGuid = new Guid(payload.TagGuid);
    }

    public Guid RecordGuid { get; }
    public Guid TagGuid { get;  }

    public override string ToString()
    {
        return $"TAG:{RecordGuid:N}.{TagGuid:N}";
    }

    public bool Equals(TagId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return RecordGuid.Equals(other.RecordGuid) && TagGuid.Equals(other.TagGuid);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TagId)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RecordGuid, TagGuid);
    }

    public static bool operator ==(TagId left, TagId right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TagId left, TagId right)
    {
        return !Equals(left, right);
    }
}