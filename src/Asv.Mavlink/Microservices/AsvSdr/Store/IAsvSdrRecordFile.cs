using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public interface IAsvSdrStore : IListDataStore<AsvSdrRecordFileMetadata, Guid>
{
    
}

public interface IAsvSdrRecordFile:IListDataFile<AsvSdrRecordFileMetadata>
{
    
}

public static class AsvSdrRecordFileHelper
{
    public static void ReadRecordInfo(this IAsvSdrRecordFile self,AsvSdrRecordPayload dest)
    {
        self.ReadMetadata().Info.CopyTo(dest);
    }
    public static void WriteRecordInfo(this IAsvSdrRecordFile self,AsvSdrRecordPayload src)
    {
        self.EditMetadata(x => src.CopyTo(x.Info));
    }

    public static bool ReadTag(this IAsvSdrRecordFile self, Guid tagId, AsvSdrRecordTagPayload dest)
    {
        var tagList = self.ReadMetadata().Tags;
        var tagItem = tagList?.FirstOrDefault(x => new Guid(x.TagGuid).Equals(tagId));
        if (tagItem == null) return false;
        tagItem.CopyTo(dest);
        return true;
    }
    public static void WriteTag(this IAsvSdrRecordFile self, Guid tagId, AsvSdrRecordTagPayload src)
    {
        self.EditMetadata(x =>
        {
            var tag = x.Tags.FirstOrDefault(x => new Guid(x.TagGuid).Equals(tagId));
            if (tag == null)
            {
                tag = new AsvSdrRecordTagPayload();
                x.Tags.Add(tag);
            }
            src.CopyTo(tag);
        });
    }
    public static IEnumerable<Guid> GetTagIds(this IAsvSdrRecordFile self, ushort skip, ushort count)
    {
        return self.ReadMetadata().Tags?.Skip(skip).Take(count).Select(x=>new Guid(x.TagGuid)) ?? Array.Empty<Guid>();
    }
    
    public static bool DeleteTag(this IAsvSdrRecordFile self, Guid tagId)
    {
        var result = false;
        self.EditMetadata(metadata =>
        {
            var itemToDelete = metadata.Tags?.FirstOrDefault(x => new Guid(x.TagGuid).Equals(tagId));
            if (itemToDelete == null) return;
            metadata.Tags.Remove(itemToDelete);
            result = true;
        });
        return result;
    }
    
    public static bool Write(this IAsvSdrRecordFile self, IPacketV2<IPayload> packet)
    {
        var type = (AsvSdrCustomMode)packet.MessageId;
        switch (type)
        {
            case AsvSdrCustomMode.AsvSdrCustomModeLlz:
                var gp = packet as AsvSdrRecordDataGpPacket;
                Debug.Assert(gp != null);
                self.Write(gp.Payload);
                break;
            case AsvSdrCustomMode.AsvSdrCustomModeGp:
                var llz = packet as AsvSdrRecordDataLlzPacket;
                Debug.Assert(llz != null);
                self.Write(llz.Payload);
                break;
            case AsvSdrCustomMode.AsvSdrCustomModeVor:
                var vor = packet as AsvSdrRecordDataVorPacket;
                Debug.Assert(vor != null);
                self.Write(vor.Payload);
                break;
            case AsvSdrCustomMode.AsvSdrCustomModeIdle:
            default:
                return false;
        }

        return true;
    }
    
    public static void Write(this IAsvSdrRecordFile self, AsvSdrRecordDataGpPayload src)
    {
        self.Write(src.DataIndex, src);
    }
    public static void Write(this IAsvSdrRecordFile self, AsvSdrRecordDataLlzPayload src)
    {
        self.Write(src.DataIndex, src);
    }
    public static void Write(this IAsvSdrRecordFile self, AsvSdrRecordDataVorPayload src)
    {
        self.Write(src.DataIndex, src);
    }
        
}