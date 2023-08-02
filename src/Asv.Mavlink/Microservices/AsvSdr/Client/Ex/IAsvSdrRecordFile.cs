using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public interface IAsvSdrRecordFile:IListDataFile<AsvSdrRecordFileMetadata>
{
    
}

public class AsvSdrRecordFile :ListDataFile<AsvSdrRecordFileMetadata>, IAsvSdrRecordFile
{
    public static readonly ListDataFileHeader FileFormat = new()
    {
        Version = "1.0.0",
        Type = "AsvSdrRecordFile",
        MetadataMaxSize = 1024 * 4,
        ItemMaxSize = 256,
    };

    public AsvSdrRecordFile(Stream stream, bool disposeSteam) : base(stream, FileFormat, disposeSteam)
    {
        
    }
}

public class AsvSdrRecordFileMetadata:ISizedSpanSerializable
{
    public AsvSdrRecordPayload Info { get; } = new();
    
    public IList<AsvSdrRecordTagPayload> Tags { get; set; }

    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        Info.Deserialize(ref buffer);
        var count = BinSerialize.ReadUShort(ref buffer);
        Tags ??= new List<AsvSdrRecordTagPayload>(count);
        Tags.Clear();
        for (var i = 0; i < count; i++)
        {
            var tag = new AsvSdrRecordTagPayload();
            tag.Deserialize(ref buffer);
        }
    }

    public void Serialize(ref Span<byte> buffer)
    {
        Info.Serialize(ref buffer);
        Tags ??= new List<AsvSdrRecordTagPayload>();
        BinSerialize.WriteUShort(ref buffer,(ushort)Tags.Count);
        foreach (var tag in Tags)
        {
            tag.Serialize(ref buffer);
        }
    }

    public int GetByteSize()
    {
        var size = Info.GetCurrentByteSize()  + sizeof(ushort);
        if (Tags != null)
        {
            size += Tags.Sum(_ => _.GetCurrentByteSize());
        }
        return size;
    }
}
