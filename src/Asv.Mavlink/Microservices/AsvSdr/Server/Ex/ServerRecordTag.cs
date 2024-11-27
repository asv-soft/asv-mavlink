using System;
using Asv.IO;
using Asv.Mavlink.AsvSdr;


namespace Asv.Mavlink;

public class ServerRecordTag:ISizedSpanSerializable
{
    public ServerRecordTag(Guid id, AsvSdrRecordTagType type, string name, byte[] value)
    {
        Type = type;
        Name = name;
        Value = value;
    }


    public AsvSdrRecordTagType Type { get; }
    public string Name { get; }
    public byte[] Value { get; }

    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        throw new NotImplementedException();
    }

    public void Serialize(ref Span<byte> buffer)
    {
        throw new NotImplementedException();
    }

    public int GetByteSize()
    {
        throw new NotImplementedException();
    }
}