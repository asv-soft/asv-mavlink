using System;
using System.Collections.Generic;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrRecordFileMetadata:ISizedSpanSerializable
{
    public AsvSdrRecordPayload Info { get; } = new();

    public IList<AsvSdrRecordTagPayload> Tags { get; } = new List<AsvSdrRecordTagPayload>();

    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        Info.Deserialize(ref buffer);
        var count = BinSerialize.ReadUShort(ref buffer);
        Tags.Clear();
        for (var i = 0; i < count; i++)
        {
            var tag = new AsvSdrRecordTagPayload();
            tag.Deserialize(ref buffer);
            Tags.Add(tag);
        }
    }

    public void Serialize(ref Span<byte> buffer)
    {
        Info.Serialize(ref buffer);
        BinSerialize.WriteUShort(ref buffer,(ushort)Tags.Count);
        foreach (var tag in Tags)
        {
            tag.Serialize(ref buffer);
        }
    }

    public int GetByteSize()
    {
        var size = Info.GetByteSize()  + sizeof(ushort);
        if (Tags != null)
        {
            size += Tags.Sum(x => x.GetByteSize());
        }
        return size;
    }
}