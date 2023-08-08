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
    public static void ReadRecordInfo(this IListDataFile<AsvSdrRecordFileMetadata> self,AsvSdrRecordPayload dest)
    {
        self.ReadMetadata().Info.CopyTo(dest);
    }
    public static void WriteRecordInfo(this IListDataFile<AsvSdrRecordFileMetadata> self,AsvSdrRecordPayload src)
    {
        self.EditMetadata(x => src.CopyTo(x.Info));
    }

    public static bool ReadTag(this IListDataFile<AsvSdrRecordFileMetadata> self, Guid tagId, AsvSdrRecordTagPayload dest)
    {
        var tagList = self.ReadMetadata().Tags;
        var tagItem = tagList?.FirstOrDefault(x => new Guid(x.TagGuid).Equals(tagId));
        if (tagItem == null) return false;
        tagItem.CopyTo(dest);
        return true;
    }

    public static void WriteTag(this IListDataFile<AsvSdrRecordFileMetadata> self, Guid tagId, Guid recId, AsvSdrRecordTagType type, string name, byte[] value)
    {
        var payload = new AsvSdrRecordTagPayload();
        MavlinkTypesHelper.SetGuid(payload.RecordGuid,recId);
        MavlinkTypesHelper.SetGuid(payload.TagGuid,tagId);
        payload.TagType = type;
        MavlinkTypesHelper.SetString(payload.TagName,name);
        value.CopyTo(payload.TagValue,0);
        self.WriteTag(tagId,payload);
        
    }
    public static void WriteTag(this IListDataFile<AsvSdrRecordFileMetadata> self, Guid tagId, AsvSdrRecordTagPayload src)
    {
        self.EditMetadata(x =>
        {
            var tag = x.Tags.FirstOrDefault(x => MavlinkTypesHelper.GetGuid(x.TagGuid).Equals(tagId));
            if (tag == null)
            {
                tag = new AsvSdrRecordTagPayload();
                x.Tags.Add(tag);
            }
            src.CopyTo(tag);
            x.Info.TagCount = (ushort)x.Tags.Count;
        });
    }
    public static Guid[] GetTagIds(this IListDataFile<AsvSdrRecordFileMetadata> self, ushort skip, ushort count)
    {
        var tags = self.ReadMetadata().Tags;
        return tags?.Skip(skip).Take(count).Select(x=>MavlinkTypesHelper.GetGuid(x.TagGuid)).ToArray() ?? Array.Empty<Guid>();
    }
    
    public static bool DeleteTag(this IListDataFile<AsvSdrRecordFileMetadata> self, Guid tagId)
    {
        var result = false;
        self.EditMetadata(metadata =>
        {
            var itemToDelete = metadata.Tags?.FirstOrDefault(x => new Guid(x.TagGuid).Equals(tagId));
            if (itemToDelete == null) return;
            metadata.Tags.Remove(itemToDelete);
            metadata.Info.TagCount = (ushort)metadata.Tags.Count;
            result = true;
        });
        return result;
    }

    public static void Write(this IListDataFile<AsvSdrRecordFileMetadata> self, AsvSdrRecordPayload record)
    {
        self.ReadMetadata().Info.CopyTo(record);
    }
    
    public static bool Write(this IListDataFile<AsvSdrRecordFileMetadata> self, IPacketV2<IPayload> packet)
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
    
    public static void Write(this IListDataFile<AsvSdrRecordFileMetadata> self, AsvSdrRecordDataGpPayload src)
    {
        self.Write(src.DataIndex, src);
    }
    public static void Write(this IListDataFile<AsvSdrRecordFileMetadata> self, AsvSdrRecordDataLlzPayload src)
    {
        self.Write(src.DataIndex, src);
    }
    public static void Write(this IListDataFile<AsvSdrRecordFileMetadata> self, AsvSdrRecordDataVorPayload src)
    {
        self.Write(src.DataIndex, src);
    }
        
}