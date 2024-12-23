#nullable enable
using System;
using System.Diagnostics;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Common;



namespace Asv.Mavlink;

public class CommonRecordData
{
    public uint DataIndex { get; set; }
    public Guid RecordGuid { get; set; }
    public DateTime TimeUnixUsec { get; set; }
    public ulong TotalFreq { get; set; }
    public int GnssLat { get; set; }
    public int GnssLon { get; set; }
    public int GnssAlt { get; set; }
    public int GnssAltEllipsoid { get; set; }
    public uint GnssHAcc { get; set; }
    public uint GnssVAcc { get; set; }
    public uint GnssVelAcc { get; set; }
    public int Lat { get; set; }
    public int Lon { get; set; }
    public int Alt { get; set; }
    public int RelativeAlt { get; set; }
    public float Roll { get; set; }
    public float Pitch { get; set; }
    public float Yaw { get; set; }
    public ushort GnssEph { get; set; }
    public ushort GnssEpv { get; set; }
    public ushort GnssVel { get; set; }
    public short Vx { get; set; }
    public short Vy { get; set; }
    public short Vz { get; set; }
    public ushort Hdg { get; set; }
    public GpsFixType GnssFixType { get; set; }
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
            metadata.Tags?.Remove(itemToDelete);
            if (metadata.Tags != null)
            {
                metadata.Info.TagCount = (ushort)metadata.Tags.Count;
            }
            result = true;
        });
        return result;
    }

    public static void Write(this IListDataFile<AsvSdrRecordFileMetadata> self, AsvSdrRecordPayload record)
    {
        self.ReadMetadata().Info.CopyTo(record);
    }

    public static bool TryReadCommonRecordData(MavlinkMessage packet, out CommonRecordData? data)
    {
        var type = (AsvSdrCustomMode)packet.Id;
        data = null;
        switch (type)
        {
            case AsvSdrCustomMode.AsvSdrCustomModeLlz:
                var gp = packet as AsvSdrRecordDataLlzPacket;
                Debug.Assert(gp != null);
                data = new CommonRecordData
                {
                    DataIndex = gp.Payload.DataIndex,
                    RecordGuid = MavlinkTypesHelper.GetGuid(gp.Payload.RecordGuid),
                    TimeUnixUsec = MavlinkTypesHelper.FromUnixTimeUs(gp.Payload.TimeUnixUsec),
                    TotalFreq = gp.Payload.TotalFreq,
                    GnssLat = gp.Payload.GnssLat,
                    GnssLon = gp.Payload.GnssLon,
                    GnssAlt = gp.Payload.GnssAlt,
                    GnssAltEllipsoid = gp.Payload.GnssAltEllipsoid,
                    GnssHAcc = gp.Payload.GnssHAcc,
                    GnssVAcc = gp.Payload.GnssVAcc,
                    GnssVelAcc = gp.Payload.GnssVelAcc,
                    Lat = gp.Payload.Lat,
                    Lon = gp.Payload.Lon,
                    Alt = gp.Payload.Alt,
                    RelativeAlt = gp.Payload.RelativeAlt,
                    Roll = gp.Payload.Roll,
                    Pitch = gp.Payload.Pitch,
                    Yaw = gp.Payload.Yaw,
                    GnssEph = gp.Payload.GnssEph,
                    GnssEpv = gp.Payload.GnssEpv,
                    GnssVel = gp.Payload.GnssVel,
                    Vx = gp.Payload.Vx,
                    Vy = gp.Payload.Vy,
                    Vz = gp.Payload.Vz,
                    Hdg = gp.Payload.Hdg,
                    GnssFixType = gp.Payload.GnssFixType,
                    
                };
                return true;
            case AsvSdrCustomMode.AsvSdrCustomModeGp:
                var llz = packet as AsvSdrRecordDataGpPacket;
                Debug.Assert(llz != null);
                data = new CommonRecordData
                {
                    
                    DataIndex = llz.Payload.DataIndex,
                    RecordGuid = MavlinkTypesHelper.GetGuid(llz.Payload.RecordGuid),
                    TimeUnixUsec = MavlinkTypesHelper.FromUnixTimeUs(llz.Payload.TimeUnixUsec),
                    TotalFreq = llz.Payload.TotalFreq,
                    GnssLat = llz.Payload.GnssLat,
                    GnssLon = llz.Payload.GnssLon,
                    GnssAlt = llz.Payload.GnssAlt,
                    GnssAltEllipsoid = llz.Payload.GnssAltEllipsoid,
                    GnssHAcc = llz.Payload.GnssHAcc,
                    GnssVAcc = llz.Payload.GnssVAcc,
                    GnssVelAcc = llz.Payload.GnssVelAcc,
                    Lat = llz.Payload.Lat,
                    Lon = llz.Payload.Lon,
                    Alt = llz.Payload.Alt,
                    RelativeAlt = llz.Payload.RelativeAlt,
                    Roll = llz.Payload.Roll,
                    Pitch = llz.Payload.Pitch,
                    Yaw = llz.Payload.Yaw,
                    GnssEph = llz.Payload.GnssEph,
                    GnssEpv = llz.Payload.GnssEpv,
                    GnssVel = llz.Payload.GnssVel,
                    Vx = llz.Payload.Vx,
                    Vy = llz.Payload.Vy,
                    Vz = llz.Payload.Vz,
                    Hdg = llz.Payload.Hdg,
                    GnssFixType = llz.Payload.GnssFixType,
                };
                return true;
            case AsvSdrCustomMode.AsvSdrCustomModeVor:
                var vor = packet as AsvSdrRecordDataVorPacket;
                Debug.Assert(vor != null);
                data = new CommonRecordData
                {
                    DataIndex = vor.Payload.DataIndex,
                    RecordGuid = MavlinkTypesHelper.GetGuid(vor.Payload.RecordGuid),
                    TimeUnixUsec = MavlinkTypesHelper.FromUnixTimeUs(vor.Payload.TimeUnixUsec),
                    TotalFreq = vor.Payload.TotalFreq,
                    GnssLat = vor.Payload.GnssLat,
                    GnssLon = vor.Payload.GnssLon,
                    GnssAlt = vor.Payload.GnssAlt,
                    GnssAltEllipsoid = vor.Payload.GnssAltEllipsoid,
                    GnssHAcc = vor.Payload.GnssHAcc,
                    GnssVAcc = vor.Payload.GnssVAcc,
                    GnssVelAcc = vor.Payload.GnssVelAcc,
                    Lat = vor.Payload.Lat,
                    Lon = vor.Payload.Lon,
                    Alt = vor.Payload.Alt,
                    RelativeAlt = vor.Payload.RelativeAlt,
                    Roll = vor.Payload.Roll,
                    Pitch = vor.Payload.Pitch,
                    Yaw = vor.Payload.Yaw,
                    GnssEph = vor.Payload.GnssEph,
                    GnssEpv = vor.Payload.GnssEpv,
                    GnssVel = vor.Payload.GnssVel,
                    Vx = vor.Payload.Vx,
                    Vy = vor.Payload.Vy,
                    Vz = vor.Payload.Vz,
                    Hdg = vor.Payload.Hdg,
                    GnssFixType = vor.Payload.GnssFixType,
                };
                return true;
            case AsvSdrCustomMode.AsvSdrCustomModeIdle:
            default:
                return false;
        }
    }
    
    public static bool TryReadDataIndex(MavlinkMessage packet, out uint index)
    {
        var type = (AsvSdrCustomMode)packet.Id;
        switch (type)
        {
            case AsvSdrCustomMode.AsvSdrCustomModeLlz:
                var gp = packet as AsvSdrRecordDataLlzPacket;
                Debug.Assert(gp != null);
                index = gp.Payload.DataIndex;
                return true;
            case AsvSdrCustomMode.AsvSdrCustomModeGp:
                var llz = packet as AsvSdrRecordDataGpPacket;
                Debug.Assert(llz != null);
                index = llz.Payload.DataIndex;
                return true;
            case AsvSdrCustomMode.AsvSdrCustomModeVor:
                var vor = packet as AsvSdrRecordDataVorPacket;
                Debug.Assert(vor != null);
                index = vor.Payload.DataIndex;
                return true;
            case AsvSdrCustomMode.AsvSdrCustomModeIdle:
            default:
                index = 0;
                return false;
        }
    }
    
    public static bool Write(this IListDataFile<AsvSdrRecordFileMetadata> self, MavlinkV2Message<IPayload> packet)
    {
        if (TryReadDataIndex(packet, out var dataIndex) == false) return false;
        self.Write(dataIndex, packet.Payload);
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