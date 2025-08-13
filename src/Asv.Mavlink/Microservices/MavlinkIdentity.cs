using System;

namespace Asv.Mavlink;

public class MavlinkIdentity : IEquatable<MavlinkIdentity>
{
    private readonly byte _componentId;
    private readonly byte _systemId;

    public MavlinkIdentity(ushort fullId)
    {
        FullId = fullId;
        MavlinkHelper.ConvertFromId(fullId, out _componentId, out _systemId);
    }

    public MavlinkIdentity(byte systemId, byte componentId)
    {
        FullId = MavlinkHelper.ConvertToFullId(componentId, systemId);
        _componentId = componentId;
        _systemId = systemId;
    }

    public ushort FullId { get; }

    public byte ComponentId => _componentId;

    public byte SystemId => _systemId;

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
    
    public static MavlinkIdentity Parse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value), "Mavlink identity cannot be null or empty");
        value = value.Trim('[', ']', ' ', '\t', '\r', '\n');
        var parts = value.Split('.');
        if (parts.Length != 2) throw new ArgumentException("Mavlink identity must be in format 'systemId.componentId'", nameof(value));
        if (!byte.TryParse(parts[0], out var systemId)) throw new ArgumentException("Invalid systemId in mavlink identity", nameof(value));
        if (!byte.TryParse(parts[1], out var componentId)) throw new ArgumentException("Invalid componentId in mavlink identity", nameof(value));
        return new MavlinkIdentity(systemId, componentId);
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