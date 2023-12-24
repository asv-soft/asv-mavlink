namespace Asv.Mavlink;

public struct MavlinkIdentity
{
    public MavlinkIdentity(byte systemId, byte componentId)
    {
        SystemId = systemId;
        ComponentId = componentId;
    }
    public byte ComponentId { get; }
    public byte SystemId { get; }
}