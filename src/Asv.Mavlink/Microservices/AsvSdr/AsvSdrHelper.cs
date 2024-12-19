using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public static class AsvSdrHelper
{
    public const string AsvSdrMicroserviceName = "SDR";
    public const string AsvSdrMicroserviceExName = $"{AsvSdrMicroserviceName}EX";
    
    public const int RecordTagValueLength = 8;

    public const int RecordNameMaxLength = 28;
    private const string RecordNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,28}$";
    private static readonly Regex RecordNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);


    public const int RecordTagNameMaxLength = 16;
    private const string RecordTagNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,16}$";
    private static readonly Regex RecordTagNameRegex = new(RecordTagNameRegexString, RegexOptions.Compiled);

    
    public const int CalibrationTableNameMaxLength = 28;
    private const string CalibrationTableNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,28}$";
    private static readonly Regex CalibrationTableNameRegex = new(CalibrationTableNameRegexString, RegexOptions.Compiled);
    
    public static void CheckCalibrationTableName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (name.Length > CalibrationTableNameMaxLength)
            throw new Exception($"Record name is too long. Max length is {CalibrationTableNameMaxLength}");
        if (CalibrationTableNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{CalibrationTableNameRegexString}')");
    }

    
    public static void CheckRecordName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (name.Length > RecordNameMaxLength)
            throw new Exception($"Record name is too long. Max length is {RecordNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{RecordNameRegexString}')");
    }

    #region Tags

    public static void CheckTagName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Tag name is empty");
        if (name.Length > RecordTagNameMaxLength)
            throw new Exception($"Tag name is too long. Max length is {RecordTagNameMaxLength}");
        if (RecordTagNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Tag name '{name}' not match regex '{RecordTagNameRegexString}')");
    }

    public static string PrintTag(string tagName,AsvSdrRecordTagType type, byte[] value)
    {
        return $"{tagName}:{PrintTagValue(type, value)}";
    }
    public static string PrintTagValue(AsvSdrRecordTagType type, byte[] rawValue)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException(nameof(rawValue), $"Tag value array must be {RecordTagValueLength} bytes length");
        return type switch
        {
            AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64 => $"{BitConverter.ToUInt64(rawValue, 0)}",
            AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64 => $"{BitConverter.ToInt64(rawValue, 0)}",
            AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64 => $"{BitConverter.ToDouble(rawValue, 0)}",
            AsvSdrRecordTagType.AsvSdrRecordTagTypeString8 => $"{MavlinkTypesHelper.GetString(rawValue)}",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static ulong GetTagValueAsUInt64(ReadOnlySpan<byte> rawValue, AsvSdrRecordTagType type)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64}", nameof(type));
        return BitConverter.ToUInt64(rawValue);
    }
    public static void SetTagValueAsUInt64(Span<byte> rawValue, AsvSdrRecordTagType type, ulong value)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64}", nameof(type));
        if (BitConverter.TryWriteBytes(rawValue, value) == false)
            throw new ArgumentException($"Can't write value {value} to tag value array", nameof(value));
        
    }
    public static long GetTagValueAsInt64(ReadOnlySpan<byte> rawValue, AsvSdrRecordTagType type)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64}", nameof(type));
        return BitConverter.ToInt64(rawValue);
    }

    public static void SetTagValueAsInt64(Span<byte> rawValue, AsvSdrRecordTagType type, long value)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64}", nameof(type));
        if (BitConverter.TryWriteBytes(rawValue, value) == false)
            throw new ArgumentException($"Can't write value {value} to tag value array", nameof(value));
    }
    public static double GetTagValueAsReal64(ReadOnlySpan<byte> rawValue, AsvSdrRecordTagType type)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64}", nameof(type));
        return BitConverter.ToDouble(rawValue);
    }

    public static void SetTagValueAsReal64(Span<byte> rawValue, AsvSdrRecordTagType type, double value)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64}", nameof(type));
        if (BitConverter.TryWriteBytes(rawValue, value) == false)
            throw new ArgumentException($"Can't write value {value} to tag value array", nameof(value));
    }

    public static string GetTagValueAsString(byte[] rawValue, AsvSdrRecordTagType type)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeString8)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64}", nameof(type));
        return MavlinkTypesHelper.GetString(rawValue);
    }
    
    public static void SetTagValueAsString(byte[] rawValue, AsvSdrRecordTagType type, string value)
    {
        if (rawValue.Length != RecordTagValueLength)
            throw new ArgumentException($"Tag value array must be {RecordTagValueLength} bytes length", nameof(rawValue));
        if (type != AsvSdrRecordTagType.AsvSdrRecordTagTypeString8)
            throw new ArgumentException($"Tag type must be {AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64}", nameof(type));
        MavlinkTypesHelper.SetString(rawValue, value);
    }
    
    #endregion

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
        float recordRate, uint sendingThinningRatio, float referencePowerDbm)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        item.Param2 = BitConverter.ToSingle(freqArray, 0);
        item.Param3 = BitConverter.ToSingle(freqArray, 4);
        item.Param4 = recordRate;
        item.Param5 = BitConverter.ToSingle(BitConverter.GetBytes(sendingThinningRatio));
        item.Param6 = referencePowerDbm;
        item.Param7 = Single.NaN;
    }

    public static void SetArgsForSdrSetMode(MissionItemIntPayload payload, AsvSdrCustomMode mode, ulong frequencyHz,
        float recordRate, uint sendingThinningRatio, float referencePowerDbm)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        payload.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode;
        payload.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        payload.Param2 = BitConverter.ToSingle(freqArray, 0);
        payload.Param3 = BitConverter.ToSingle(freqArray, 4);
        payload.Param4 = recordRate;
        payload.X = BitConverter.ToInt32(BitConverter.GetBytes(sendingThinningRatio));
        payload.Y = BitConverter.ToInt32(BitConverter.GetBytes(referencePowerDbm));
        payload.Z = Single.NaN;
    }
    public static void GetArgsForSdrSetMode(CommandLongPayload item, out AsvSdrCustomMode mode, out ulong frequencyHz,
        out float recordRate, out uint sendingThinningRatio, out float referencePowerDbm)
    {
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrSetMode}");
        mode = (AsvSdrCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        var freqArray = new byte[8];
        BitConverter.GetBytes(item.Param2).CopyTo(freqArray,0);
        BitConverter.GetBytes(item.Param3).CopyTo(freqArray,4);
        frequencyHz = BitConverter.ToUInt64(freqArray,0);
        recordRate = item.Param4;
        sendingThinningRatio = BitConverter.ToUInt32(BitConverter.GetBytes(item.Param5));
        referencePowerDbm = item.Param6;
    }

    public static void GetArgsForSdrSetMode(ServerMissionItem item, out AsvSdrCustomMode mode,  out ulong frequencyHz,
        out float recordRate, out uint sendingThinningRatio, out float referencePowerDbm)
    {
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrSetMode}");
        mode = (AsvSdrCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        var freqArray = new byte[8];
        BitConverter.GetBytes(item.Param2).CopyTo(freqArray,0);
        BitConverter.GetBytes(item.Param3).CopyTo(freqArray,4);
        frequencyHz = BitConverter.ToUInt64(freqArray,0);
        recordRate = item.Param4;
        sendingThinningRatio = BitConverter.ToUInt32(BitConverter.GetBytes(item.X));
        referencePowerDbm = BitConverter.ToSingle(BitConverter.GetBytes(item.Y));
    }

    #endregion

    #region StartRecord

    public static void SetArgsForSdrStartRecord(MissionItemIntPayload payload, string recordName)
    {
        CheckRecordName(recordName);
        var nameArray = new byte[RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray, recordName);
        payload.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartRecord;
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
        if (payload.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartRecord)
            throw new ArgumentException($"Command {payload.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStartRecord}");
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
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartRecord;
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
        if (payload.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartRecord)
            throw new ArgumentException($"Command {payload.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStartRecord}");
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
        payload.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopRecord;
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
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopRecord;
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
        if (payload.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopRecord)
            throw new ArgumentException($"Command {payload.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStopRecord}");
    }
   
    public static void GetArgsForSdrStopRecord(CommandLongPayload payload)
    {
        if (payload.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopRecord)
            throw new ArgumentException($"Command {payload.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStopRecord}");
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
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag;
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
        payload.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag;
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag}");
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag}");
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
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction;
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
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction;
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction}");
        action = (AsvSdrSystemControlAction)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        
    }
    
    public static void GetArgsForSdrSystemControlAction(ServerMissionItem item, out AsvSdrSystemControlAction action)
    {
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction}");
        action = (AsvSdrSystemControlAction)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        
    }

    #endregion

    #region StartMission

    public static void SetArgsForSdrStartMission(CommandLongPayload item,ushort missionIndex)
    {   
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartMission;
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartMission)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStartMission}");
        missionIndex = (ushort)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
    }

    #endregion

    #region StopMission

    public static void SetArgsForSdrStopMission(CommandLongPayload item)
    {   
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopMission;
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopMission)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStopMission}");
    }

    #endregion
    
    #region SdrDelay
    
    public static void SetArgsForSdrDelay(MissionItemIntPayload item,uint delayMs)
    {   
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrDelay;
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrDelay)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrDelay}");
        delayMs = BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
    }
   
    
    #endregion

    #region SdrWaitVehicleWaypoint

    public static void SetArgsForSdrWaitVehicleWaypoint(MissionItemIntPayload item,ushort index)
    {   
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrWaitVehicleWaypoint;
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
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrWaitVehicleWaypoint)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrWaitVehicleWaypoint}");
        index = (ushort)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
    }

    public static void SetArgsForSdrStartCalibration(CommandLongPayload item)
    {
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartCalibration;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    
    public static void GetArgsForSdrStartCalibration(CommandLongPayload item)
    {   
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartCalibration)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStartCalibration}");
    }

    public static void SetArgsForSdrStopCalibration(CommandLongPayload item)
    {
        item.Command = (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopCalibration;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    public static void GetArgsForSdrStopCalibration(CommandLongPayload item)
    {   
        if (item.Command != (Common.MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopCalibration)
            throw new ArgumentException($"Command {item.Command} is not {AsvSdr.MavCmd.MavCmdAsvSdrStopCalibration}");
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

    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterSdr(this IServerDeviceBuilder builder)
    {
        builder.Register<IAsvSdrServer>((identity, context,config) => new AsvSdrServer(identity,config.Get<AsvSdrServerConfig>(), context));
        return builder;
    }
    public static IServerDeviceBuilder RegisterSdr(this IServerDeviceBuilder builder, AsvSdrServerConfig config)
    {
        builder.Register<IAsvSdrServer>((identity, context,_) => new AsvSdrServer(identity,config, context));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterSdrEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IAsvSdrServerEx, IAsvSdrServer, IStatusTextServer, IHeartbeatServer,
                ICommandServerEx<CommandLongPacket>>((_, _, _, @base, status, hb, cmd) =>
                new AsvSdrServerEx(@base, status, hb, cmd));
        return builder;
    }
   

    public static IAsvSdrServer GetSdr(this IServerDevice factory) 
        => factory.Get<IAsvSdrServer>();

    public static IAsvSdrServerEx GetSdrEx(this IServerDevice factory) 
        => factory.Get<IAsvSdrServerEx>();

    #endregion
   
}