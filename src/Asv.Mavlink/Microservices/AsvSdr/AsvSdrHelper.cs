using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.AsvSdr.MavCmd;

namespace Asv.Mavlink;

public static class AsvSdrHelper
{
    public const int RecordTagValueLength = 8;

    public const int RecordNameMaxLength = 28;
    private const string RecordNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,28}$";
    private static readonly Regex RecordNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);


    public const int RecordTagNameMaxLength = 16;
    private const string RecordTagNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex RecordTagNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);

    public static void CheckRecordName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (name.Length > RecordNameMaxLength)
            throw new Exception($"Record name is too long. Max length is {RecordNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{RecordNameRegexString}')");
    }

    public static void CheckTagName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Tag name is empty");
        if (name.Length > RecordTagNameMaxLength)
            throw new Exception($"Tag name is too long. Max length is {RecordTagNameMaxLength}");
        if (RecordTagNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Tag name '{name}' not match regex '{RecordTagNameRegexString}')");
    }


    public static readonly ListDataFileFormat FileFormat = new()
    {
        Version = "1.0.0",
        Type = "AsvSdrRecordFile",
        MetadataMaxSize =
            78 /*size of AsvSdrRecordPayload */ + sizeof(ushort) /* size of tag list */ +
            100 * 57 /* max 100 tag * size of AsvSdrRecordTagPayload */,
        ItemMaxSize = 256,
    };

    public static readonly IHierarchicalStoreFormat<Guid, IListDataFile<AsvSdrRecordFileMetadata>> StoreFormat =
        new AsvSdrListDataStoreFormat();

    #region Set mode

    public static void SetArgsForSdrSetMode(CommandLongPayload item, AsvSdrCustomMode mode, ulong frequencyHz,
        float recordRate, uint sendingThinningRatio)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        item.Param2 = BitConverter.ToSingle(freqArray, 0);
        item.Param3 = BitConverter.ToSingle(freqArray, 4);
        item.Param4 = recordRate;
        item.Param5 = BitConverter.ToSingle(BitConverter.GetBytes(sendingThinningRatio));
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }

    public static void SetArgsForSdrSetMode(MissionItemIntPayload payload, AsvSdrCustomMode mode, ulong frequencyHz,
        float recordRate, uint sendingThinningRatio)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        payload.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode;
        payload.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        payload.Param2 = BitConverter.ToSingle(freqArray, 0);
        payload.Param3 = BitConverter.ToSingle(freqArray, 4);
        payload.Param4 = recordRate;
        payload.X = BitConverter.ToInt32(BitConverter.GetBytes(sendingThinningRatio));
        payload.Y = 0;
        payload.Z = Single.NaN;
    }
    public static void GetArgsForSdrSetMode(CommandLongPayload item, out AsvSdrCustomMode mode, out ulong frequencyHz,
        out float recordRate, out uint sendingThinningRatio)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrSetMode}");
        mode = (AsvSdrCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        var freqArray = new byte[8];
        BitConverter.GetBytes(item.Param2).CopyTo(freqArray,0);
        BitConverter.GetBytes(item.Param3).CopyTo(freqArray,4);
        frequencyHz = BitConverter.ToUInt64(freqArray,0);
        recordRate = item.Param4;
        sendingThinningRatio = BitConverter.ToUInt32(BitConverter.GetBytes(item.Param5));
    }

    public static void GetArgsForSdrSetMode(ServerMissionItem item, out AsvSdrCustomMode mode,  out ulong frequencyHz,
        out float recordRate, out uint sendingThinningRatio)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrSetMode}");
        mode = (AsvSdrCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        var freqArray = new byte[8];
        BitConverter.GetBytes(item.Param2).CopyTo(freqArray,0);
        BitConverter.GetBytes(item.Param3).CopyTo(freqArray,4);
        frequencyHz = BitConverter.ToUInt64(freqArray,0);
        recordRate = item.Param4;
        sendingThinningRatio = BitConverter.ToUInt32(BitConverter.GetBytes(item.X));
    }

    #endregion

    #region StartRecord

    public static void SetArgsForSdrStartRecord(MissionItemIntPayload payload, string recordName)
    {
        CheckRecordName(recordName);
        var nameArray = new byte[RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray, recordName);
        payload.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStartRecord;
        payload.Param1 = BitConverter.ToSingle(nameArray, 0);
        payload.Param2 = BitConverter.ToSingle(nameArray, 4);
        payload.Param3 = BitConverter.ToSingle(nameArray, 8);
        payload.Param4 = BitConverter.ToSingle(nameArray, 12);
        payload.X = BitConverter.ToInt32(nameArray, 16);
        payload.Y = BitConverter.ToInt32(nameArray, 20);
        payload.Z = BitConverter.ToSingle(nameArray, 24);
    }
    
    public static void GetArgsForSdrStartRecord(ServerMissionItem payload, out string recordName)
    {
        if (payload.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStartRecord)
            throw new ArgumentException($"Command {payload.Command} is not {MavCmd.MavCmdAsvSdrStartRecord}");
        var nameArray = new byte[AsvSdrHelper.RecordNameMaxLength];
        BitConverter.GetBytes(payload.Param1).CopyTo(nameArray,0);
        BitConverter.GetBytes(payload.Param2).CopyTo(nameArray,4);
        BitConverter.GetBytes(payload.Param3).CopyTo(nameArray,8);
        BitConverter.GetBytes(payload.Param4).CopyTo(nameArray,12);
        BitConverter.GetBytes(payload.X).CopyTo(nameArray,16);
        BitConverter.GetBytes(payload.Y).CopyTo(nameArray,20);
        BitConverter.GetBytes(payload.Z).CopyTo(nameArray,24);
        recordName = MavlinkTypesHelper.GetString(nameArray);
        CheckRecordName(recordName);
    }

    public static void SetArgsForSdrStartRecord(CommandLongPayload item, string recordName)
    {
        CheckRecordName(recordName);
        var nameArray = new byte[RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray, recordName);
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStartRecord;
        item.Param1 = BitConverter.ToSingle(nameArray, 0);
        item.Param2 = BitConverter.ToSingle(nameArray, 4);
        item.Param3 = BitConverter.ToSingle(nameArray, 8);
        item.Param4 = BitConverter.ToSingle(nameArray, 12);
        item.Param5 = BitConverter.ToSingle(nameArray, 16);
        item.Param6 = BitConverter.ToSingle(nameArray, 20);
        item.Param7 = BitConverter.ToSingle(nameArray, 24);
    }

    public static void GetArgsForSdrStartRecord(CommandLongPayload payload, out string recordName)
    {
        if (payload.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStartRecord)
            throw new ArgumentException($"Command {payload.Command} is not {MavCmd.MavCmdAsvSdrStartRecord}");
        var nameArray = new byte[RecordNameMaxLength];
        BitConverter.GetBytes(payload.Param1).CopyTo(nameArray,0);
        BitConverter.GetBytes(payload.Param2).CopyTo(nameArray,4);
        BitConverter.GetBytes(payload.Param3).CopyTo(nameArray,8);
        BitConverter.GetBytes(payload.Param4).CopyTo(nameArray,12);
        BitConverter.GetBytes(payload.Param5).CopyTo(nameArray,16);
        BitConverter.GetBytes(payload.Param6).CopyTo(nameArray,20);
        BitConverter.GetBytes(payload.Param7).CopyTo(nameArray,24);
        recordName = MavlinkTypesHelper.GetString(nameArray);
        CheckRecordName(recordName);
    }

    #endregion

    #region StopRecord

    public static void SetArgsForSdrStopRecord(MissionItemIntPayload payload)
    {
        payload.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStopRecord;
        payload.Param1 = Single.NaN;
        payload.Param2 = Single.NaN;
        payload.Param3 = Single.NaN;
        payload.Param4 = Single.NaN;
        payload.X = 0;
        payload.Y = 0;
        payload.Z = Single.NaN;
    }
    
    public static void SetArgsForSdrStopRecord(CommandLongPayload item)
    {
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStopRecord;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    
    public static void GetArgsForSdrStopRecord(ServerMissionItem payload)
    {
        if (payload.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStopRecord)
            throw new ArgumentException($"Command {payload.Command} is not {MavCmd.MavCmdAsvSdrStopRecord}");
    }
   
    public static void GetArgsForSdrStopRecord(CommandLongPayload payload)
    {
        if (payload.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStopRecord)
            throw new ArgumentException($"Command {payload.Command} is not {MavCmd.MavCmdAsvSdrStopRecord}");
    }
    
    #endregion

    #region CurrentRecordSetTag

    public static void SetArgsForSdrCurrentRecordSetTag(CommandLongPayload item, string tagName, AsvSdrRecordTagType type, byte[] rawValue)
    {
        CheckTagName(tagName);
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException(nameof(rawValue), $"Tag value array must be {RecordTagValueLength} bytes length");
        var nameArray = new byte[RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray, tagName);
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetRecordTag;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)type));
        item.Param2 = BitConverter.ToSingle(nameArray, 0);
        item.Param3 = BitConverter.ToSingle(nameArray, 4);
        item.Param4 = BitConverter.ToSingle(nameArray, 8);
        item.Param5 = BitConverter.ToSingle(nameArray, 12);
        item.Param6 = BitConverter.ToSingle(rawValue,0);
        item.Param7 = BitConverter.ToSingle(rawValue,4);
    }
    public static void SetArgsForSdrCurrentRecordSetTag(MissionItemIntPayload payload,string tagName, AsvSdrRecordTagType type, byte[] rawValue)
    {
        CheckTagName(tagName);
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException(nameof(rawValue), $"Tag value array must be {RecordTagValueLength} bytes length");
        var nameArray = new byte[RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray, tagName);
        payload.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetRecordTag;
        payload.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)type));
        payload.Param2 = BitConverter.ToSingle(nameArray, 0);
        payload.Param3 = BitConverter.ToSingle(nameArray, 4);
        payload.Param4 = BitConverter.ToSingle(nameArray, 8);
        payload.X = BitConverter.ToInt32(nameArray, 12);
        payload.Y = BitConverter.ToInt32(rawValue, 0);
        payload.Z = BitConverter.ToSingle(rawValue, 4);
    }

    public static void GetArgsForSdrCurrentRecordSetTag(CommandLongPayload item, out string tagName,
        out AsvSdrRecordTagType type,out  byte[] valueArray)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetRecordTag)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrSetRecordTag}");
        type = (AsvSdrRecordTagType)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        var nameArray = new byte[RecordTagNameMaxLength];
        BitConverter.GetBytes(item.Param2).CopyTo(nameArray,0);
        BitConverter.GetBytes(item.Param3).CopyTo(nameArray,4);
        BitConverter.GetBytes(item.Param4).CopyTo(nameArray,8);
        BitConverter.GetBytes(item.Param5).CopyTo(nameArray,12);
        tagName = MavlinkTypesHelper.GetString(nameArray); 
        CheckTagName(tagName);
        valueArray = new byte[RecordTagValueLength];
        BitConverter.GetBytes(item.Param6).CopyTo(valueArray,0);
        BitConverter.GetBytes(item.Param7).CopyTo(valueArray,4);
    }
    public static void GetArgsForSdrCurrentRecordSetTag(ServerMissionItem item, out string tagName,
        out AsvSdrRecordTagType type,out  byte[] valueArray)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetRecordTag)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrSetRecordTag}");
        type = (AsvSdrRecordTagType)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        var nameArray = new byte[RecordTagNameMaxLength];
        BitConverter.GetBytes(item.Param2).CopyTo(nameArray,0);
        BitConverter.GetBytes(item.Param3).CopyTo(nameArray,4);
        BitConverter.GetBytes(item.Param4).CopyTo(nameArray,8);
        BitConverter.GetBytes(item.X).CopyTo(nameArray,12);
        tagName = MavlinkTypesHelper.GetString(nameArray); 
        CheckTagName(tagName);
        valueArray = new byte[RecordTagValueLength];
        BitConverter.GetBytes(item.Y).CopyTo(valueArray,0);
        BitConverter.GetBytes(item.Z).CopyTo(valueArray,4);
    }

    #endregion

    #region SystemControlAction

    public static void SetArgsForSdrSystemControlAction(CommandLongPayload item,AsvSdrSystemControlAction action)
    {   
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSystemControlAction;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)action));
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    
    public static void SetArgsForSdrSystemControlAction(MissionItemIntPayload item,AsvSdrSystemControlAction action)
    {   
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSystemControlAction;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)action));
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.X = 0;
        item.Y = 0;
        item.Z = Single.NaN;
    }

    public static void GetArgsForSdrSystemControlAction(CommandLongPayload item, out AsvSdrSystemControlAction action)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSystemControlAction)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrSystemControlAction}");
        action = (AsvSdrSystemControlAction)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        
    }
    
    public static void GetArgsForSdrSystemControlAction(ServerMissionItem item, out AsvSdrSystemControlAction action)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSystemControlAction)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrSystemControlAction}");
        action = (AsvSdrSystemControlAction)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        
    }

    #endregion

    #region StartMission

    public static void SetArgsForSdrStartMission(CommandLongPayload item,ushort missionIndex)
    {   
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStartMission;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)missionIndex));
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }

    public static void GetArgsForSdrStartMission(CommandLongPayload item,out ushort missionIndex)
    {
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStartMission)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrStartMission}");
        missionIndex = (ushort)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
    }

    #endregion

    #region StopMission

    public static void SetArgsForSdrStopMission(CommandLongPayload item)
    {   
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStopMission;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    public static void GetArgsForSdrStopMission(CommandLongPayload item)
    {   
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrStopMission)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrStopMission}");
    }

    #endregion
    
    #region SdrDelay
    
    public static void SetArgsForSdrDelay(MissionItemIntPayload item,uint delayMs)
    {   
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrDelay;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes(delayMs));
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.X = 0;
        item.Y = 0;
        item.Z = Single.NaN;
    }
    
    public static void GetArgsForSdrDelay(ServerMissionItem item,out uint delayMs)
    {   
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrDelay)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrDelay}");
        delayMs = BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
    }
   
    
    #endregion

    #region SdrWaitVehicleWaypoint

    public static void SetArgsForSdrWaitVehicleWaypoint(MissionItemIntPayload item,ushort index)
    {   
        item.Command = (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrWaitVehicleWaypoint;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)index));
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.X = 0;
        item.Y = 0;
        item.Z = Single.NaN;
    }
    
    public static void GetArgsForSdrWaitVehicleWaypoint(ServerMissionItem item,out ushort index)
    {   
        if (item.Command != (V2.Common.MavCmd)MavCmd.MavCmdAsvSdrWaitVehicleWaypoint)
            throw new ArgumentException($"Command {item.Command} is not {MavCmd.MavCmdAsvSdrWaitVehicleWaypoint}");
        index = (ushort)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
    }

    #endregion

    public static ServerMissionItem Convert(MissionItemIntPacket input)
    {
        return Convert(input.Payload);
    }
    public static ServerMissionItem Convert(MissionItemIntPayload input)
    {
        return new ServerMissionItem
        {
            Param1 = input.Param1,
            Param2 = input.Param2,
            Param3 = input.Param3,
            Param4 = input.Param4,
            X = input.X,
            Y = input.Y,
            Z = input.Z,
            Seq = input.Seq,
            Command = input.Command,
            Frame = input.Frame,
            Autocontinue = input.Autocontinue,
            MissionType = input.MissionType
        };
    }
}