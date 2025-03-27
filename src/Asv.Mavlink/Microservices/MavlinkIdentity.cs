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

public class MavlinkClientIdentity :IEquatable<MavlinkClientIdentity>
{
    public MavlinkClientIdentity(byte systemId, byte componentId, byte targetSystemId, byte targetComponentId)
    {
        Self = new MavlinkIdentity(systemId, componentId);
        Target = new MavlinkIdentity(targetSystemId, targetComponentId);
    }

    public MavlinkClientIdentity(MavlinkIdentity self, MavlinkIdentity target)
    {
        Self = self;
        Target = target;
    }

    public MavlinkIdentity Self { get; }
    public MavlinkIdentity Target { get; }
    
    public override string ToString()
    {
        return $"{Target}";
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