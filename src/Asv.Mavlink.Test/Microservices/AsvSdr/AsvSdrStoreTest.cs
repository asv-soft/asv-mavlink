using System;
using System.IO;
using System.Linq;
using Asv.Mavlink.V2.AsvSdr;
using Newtonsoft.Json;
using Xunit;

namespace Asv.Mavlink.Test;

public class AsvSdrStoreTest
{
    [Fact]
    public static void Read_write_tag()
    {
        var mem = new MemoryStream();
        IListDataFile<AsvSdrRecordFileMetadata> write = new AsvSdrRecordFile(mem,false);
        var tagId = Guid.NewGuid();
        var recId = Guid.NewGuid();
        var value = new byte[8];
        var name = "Tag1";
        var type = AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64;
        Random.Shared.NextBytes(value);
        write.WriteTag(tagId,recId, type,name,value);
        write.Dispose();
        
        IListDataFile<AsvSdrRecordFileMetadata> read = new AsvSdrRecordFile(mem,false);
        Assert.Equal(1, read.ReadMetadata().Tags.Count);
        Assert.Equal(tagId, MavlinkTypesHelper.GetGuid(read.ReadMetadata().Tags[0].TagGuid));
        Assert.Equal(recId, MavlinkTypesHelper.GetGuid(read.ReadMetadata().Tags[0].RecordGuid));
        Assert.Equal(name, MavlinkTypesHelper.GetString(read.ReadMetadata().Tags[0].TagName));
        Assert.Equal(type, read.ReadMetadata().Tags[0].TagType);
        Assert.Equal(value, read.ReadMetadata().Tags[0].TagValue);

        var tag = read.GetTagIds(0, 65535).FirstOrDefault();
        Assert.Equal(tagId,tag);
        var items = read.GetTagIds(1, 65535);
        Assert.Empty(items);
        var tag2 = read.GetTagIds(1, 65535).FirstOrDefault();
        Assert.Equal(Guid.Empty,tag2);
    }
}