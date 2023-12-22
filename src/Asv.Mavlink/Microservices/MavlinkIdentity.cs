namespace Asv.Mavlink;

public struct MavlinkIdentity(byte systemId, byte componentId)
{
    public byte ComponentId { get; } = componentId;
    public byte SystemId { get; } = systemId;
}