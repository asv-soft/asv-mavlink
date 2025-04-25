using System;
using System.Collections.Immutable;
using Asv.IO;

namespace Asv.Mavlink;

public abstract class MavlinkMessage : IProtocolMessage<int>
{
    private ProtocolTags _tags = [];
    
    public abstract ImmutableArray<MavlinkFieldInfo> Fields { get; }
    public abstract string GetFormatMessage();

    public abstract void Deserialize(ref ReadOnlySpan<byte> buffer);

    public abstract void Serialize(ref Span<byte> buffer);

    public abstract IPayload GetPayload();

    public abstract int GetByteSize();

    public ref ProtocolTags Tags => ref _tags;

    public string GetIdAsString() => Id.ToString();

    public abstract ProtocolInfo Protocol { get; }
    
    public abstract string Name { get; }
    public abstract int Id { get; }
    
    public abstract byte GetCrcExtra();
    
    public byte Sequence { get; set; }
    
    public byte SystemId { get; set; }
    
    public byte ComponentId { get; set; }
    
    public MavlinkIdentity FullId => new(SystemId, ComponentId);
    
}