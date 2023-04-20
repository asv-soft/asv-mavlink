using System;
using System.Text;
using Asv.IO;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrServerRecordTag:ISizedSpanSerializable
{
    public AsvSdrServerRecordTag(AsvSdrRecordTagType tagType, string name, byte[] valueArray)
    {
        TagType = tagType;
        Name = name;
        ValueArray = valueArray;
    }


    public AsvSdrRecordTagType TagType { get; }
    public string Name { get; }
    public byte[] ValueArray { get; }

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