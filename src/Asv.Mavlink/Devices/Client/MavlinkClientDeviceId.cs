using System;
using Asv.IO;

namespace Asv.Mavlink;



public class MavlinkClientDeviceId(string deviceClass, MavlinkClientIdentity id)
    : DeviceId(deviceClass), IEquatable<MavlinkClientDeviceId>
{
    public bool Equals(MavlinkClientDeviceId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is MavlinkClientDeviceId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id);
    }

    public static bool operator ==(MavlinkClientDeviceId? left, MavlinkClientDeviceId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MavlinkClientDeviceId? left, MavlinkClientDeviceId? right)
    {
        return !Equals(left, right);
    }

    public MavlinkClientIdentity Id { get; } = id;

    public override string AsString()
    {
        return $"{DeviceClass}{Id}";
    }
}