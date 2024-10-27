using System;

namespace Asv.Mavlink;

public class MavlinkIdentity(byte systemId, byte componentId) : IEquatable<MavlinkIdentity>
{
    public ushort FullId { get; } = MavlinkHelper.ConvertToFullId(componentId, systemId);
    public byte ComponentId { get; } = componentId;
    public byte SystemId { get; } = systemId;

    public bool Equals(MavlinkIdentity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ComponentId == other.ComponentId && SystemId == other.SystemId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MavlinkIdentity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ComponentId, SystemId);
    }

    public static bool operator ==(MavlinkIdentity? left, MavlinkIdentity? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MavlinkIdentity? left, MavlinkIdentity? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"[{SystemId}.{ComponentId}]";
    }
}

public class MavlinkClientIdentity(byte systemId, byte componentId, byte targetSystemId, byte targetComponentId):IEquatable<MavlinkClientIdentity>
{
    public MavlinkIdentity Self { get; } = new(systemId, componentId);
    public MavlinkIdentity Target { get; } = new(targetSystemId, targetComponentId);
    
    public override string ToString()
    {
        return $"{Self}=>{Target}";
    }

    public bool Equals(MavlinkClientIdentity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Self.Equals(other.Self) && Target.Equals(other.Target);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MavlinkClientIdentity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Self, Target);
    }

    public static bool operator ==(MavlinkClientIdentity? left, MavlinkClientIdentity? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MavlinkClientIdentity? left, MavlinkClientIdentity? right)
    {
        return !Equals(left, right);
    }
}