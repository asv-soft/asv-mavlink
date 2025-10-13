using System;
using Asv.IO;

namespace Asv.Mavlink;

public abstract class MavlinkMessage : IProtocolMessage<int>
{
    private ProtocolTags _tags = [];

    public abstract void Deserialize(ref ReadOnlySpan<byte> buffer);

    public abstract void Serialize(ref Span<byte> buffer);

    public abstract IPayload GetPayload();

    public abstract int GetByteSize();

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public ref ProtocolTags Tags => ref _tags;

    public virtual string GetIdAsString() => Id.ToString();

    public abstract ProtocolInfo Protocol { get; }
    
    public abstract string Name { get; }
    public abstract int Id { get; }
    
    public abstract byte GetCrcExtra();
    
    public byte Sequence { get; set; }
    
    public byte SystemId { get; set; }
    
    public byte ComponentId { get; set; }
    
    public virtual bool TrySetTargetId(byte systemId, byte componentId)
    {
        return false;
    }
    
    public virtual bool TryGetTargetId(out byte systemId, out byte componentId)
    {
        systemId = 0;
        componentId = 0;
        return false;
    }
    
    public MavlinkIdentity FullId => new(SystemId, ComponentId);
    
}