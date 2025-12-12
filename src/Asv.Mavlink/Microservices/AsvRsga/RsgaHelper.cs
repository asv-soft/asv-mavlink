using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using R3;


namespace Asv.Mavlink;

public static class RsgaHelper
{
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    public const string MicroserviceName = "RSGA";
    
    public const int RecordNameMaxLength = 28;
    private const string RecordNameRegexString = "^[A-Za-z][A-Za-z0-9_\\- +]{2,28}$";
    private static readonly Regex RecordNameRegex = new(RecordNameRegexString, RegexOptions.Compiled);
    
    
    public static void CheckRecordName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new MavlinkException("Record name is empty");
        if (name.Length > RecordNameMaxLength)
            throw new MavlinkException($"Record name is too long. Max length is {RecordNameMaxLength}");
        if (RecordNameRegex.IsMatch(name) == false)
            throw new ArgumentException(
                $"Record name '{name}' not match regex '{RecordNameRegexString}')");
    }
    
    #region StartRecord

    public static Observable<bool> IsRecording(this IAsvRsgaClientEx src)
    {
        return src.CurrentSubMode.Select(x => x.HasFlag(AsvRsgaCustomSubMode.AsvRsgaCustomSubModeRecord));
    }
    
    public static Observable<bool> IsMissionExecuting(this IAsvRsgaClientEx src)
    {
        return src.CurrentSubMode.Select(x => x.HasFlag(AsvRsgaCustomSubMode.AsvRsgaCustomSubModeMission));
    }
    
    public static void SetArgsForStartRecord(MissionItemIntPayload payload, string recordName)
    {
        CheckRecordName(recordName);
        var nameArray = new byte[RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray, recordName);
        payload.Command = (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStartRecord;
        payload.Param1 = BitConverter.ToSingle(nameArray, 0);
        payload.Param2 = BitConverter.ToSingle(nameArray, 4);
        payload.Param3 = BitConverter.ToSingle(nameArray, 8);
        payload.Param4 = BitConverter.ToSingle(nameArray, 12);
        payload.X = BitConverter.ToInt32(nameArray, 16);
        payload.Y = BitConverter.ToInt32(nameArray, 20);
        payload.Z = BitConverter.ToSingle(nameArray, 24);
    }
    
    public static void GetArgsForStartRecord(ServerMissionItem payload, out string recordName)
    {
        if (payload.Command != (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStartRecord)
            throw new ArgumentException($"Invalid command: {payload.Command}");

        const int len = AsvSdrHelper.RecordNameMaxLength;
        Span<byte> buffer = stackalloc byte[len];

        var span = buffer;
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(0,  4), payload.Param1);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(4,  4), payload.Param2);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(8,  4), payload.Param3);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(12, 4), payload.Param4);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(16, 4), payload.X);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(20, 4), payload.Y);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(24, 4), payload.Z);


        recordName = MavlinkTypesHelper.GetString(buffer);

        if (!string.IsNullOrWhiteSpace(recordName))
            CheckRecordName(recordName);
    }

    public static void SetArgsForStartRecord(CommandLongPayload item, string recordName)
    {
        const int recordNameMaxLength = AsvSdrHelper.RecordNameMaxLength;

        Span<byte> nameBuffer = stackalloc byte[recordNameMaxLength];
        if (!string.IsNullOrWhiteSpace(recordName))
        {
            CheckRecordName(recordName);
            MavlinkTypesHelper.SetString(nameBuffer, recordName);
        }
        item.Command = (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStartRecord;
        item.Param1 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(0,  4));
        item.Param2 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(4,  4));
        item.Param3 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(8,  4));
        item.Param4 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(12, 4));
        item.Param5 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(16, 4));
        item.Param6 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(20, 4));
        item.Param7 = BinaryPrimitives.ReadSingleLittleEndian(nameBuffer.Slice(24, 4));
    }

    public static void GetArgsForStartRecord(CommandLongPayload payload, out string recordName)
    {
        if (payload.Command != (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStartRecord)
            throw new ArgumentException(
                $"Expected command {AsvRsga.MavCmd.MavCmdAsvRsgaStartRecord}, got {payload.Command}");

        Span<byte> nameBuffer = stackalloc byte[RecordNameMaxLength];

        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(0,  4), payload.Param1);
        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(4,  4), payload.Param2);
        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(8,  4), payload.Param3);
        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(12, 4), payload.Param4);
        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(16, 4), payload.Param5);
        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(20, 4), payload.Param6);
        BinaryPrimitives.WriteSingleLittleEndian(nameBuffer.Slice(24, 4), payload.Param7);

        recordName = MavlinkTypesHelper.GetString(nameBuffer);

        if (!string.IsNullOrWhiteSpace(recordName))
        {
            CheckRecordName(recordName);
        }
    }

    #endregion
    
    #region StopRecord

    public static void SetArgsForStopRecord(MissionItemIntPayload payload)
    {
        payload.Command = (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord;;
        payload.Param1 = Single.NaN;
        payload.Param2 = Single.NaN;
        payload.Param3 = Single.NaN;
        payload.Param4 = Single.NaN;
        payload.X = 0;
        payload.Y = 0;
        payload.Z = Single.NaN;
    }
    
    public static void SetArgsForStopRecord(CommandLongPayload item)
    {
        item.Command = (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord;;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    
    public static void GetArgsForStopRecord(ServerMissionItem payload)
    {
        if (payload.Command != (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord)
            throw new ArgumentException($"Command {payload.Command} is not {AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord}");
    }
   
    public static void GetArgsForStopRecord(CommandLongPayload payload)
    {
        if (payload.Command != (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord)
            throw new ArgumentException($"Command {payload.Command} is not {AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord}");
    }
    
    #endregion

    #region SetMode
    
    public static void SetArgsForSetMode(CommandLongPayload item, AsvRsgaCustomMode mode, float param2 = float.NaN, float param3 = float.NaN, float param4 = float.NaN, float param5 = float.NaN, float param6 = float.NaN, float param7 = float.NaN)
    {
        item.Command = (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaSetMode;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        item.Param2 = float.NaN;
        item.Param3 = float.NaN;
        item.Param4 = float.NaN;
        item.Param5 = float.NaN;
        item.Param6 = float.NaN;
        item.Param7 = float.NaN;
    }

    public static void GetArgsForSetMode(CommandLongPayload item, out AsvRsgaCustomMode mode, out float param2, out float param3, out float param4, out float param5, out float param6, out float param7)
    {
        if (item.Command != (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaSetMode)
            throw new ArgumentException($"Command {item.Command:G} is not {AsvRsga.MavCmd.MavCmdAsvRsgaSetMode:G}");
        mode = (AsvRsgaCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
       
        param2 = item.Param2;
        param3 = item.Param3;
        param4 = item.Param4;
        param5 = item.Param5;
        param6 = item.Param6;
        param7 = item.Param7;
        
    }

    #endregion

    #region SupportedModes

    public static void SetSupportedModes(AsvRsgaCompatibilityResponsePayload payload, IEnumerable<AsvRsgaCustomMode> modes)
    {
        var arr = new BitArray(payload.SupportedModes.Length * 8);
        foreach (var mode in modes)
        {
            var index = (int)mode;
            if (index < 0 || index >= arr.Length * sizeof(byte))
                throw new ArgumentOutOfRangeException(nameof(modes));
            arr[index] = true;
        }
        arr.CopyTo(payload.SupportedModes, 0);
    }
    public static IEnumerable<AsvRsgaCustomMode> GetSupportedModes(AsvRsgaCompatibilityResponsePayload payload)
    {
        var arr = new BitArray(payload.SupportedModes);
        for (var i = 0; i < arr.Length; i++)
        {
            if (arr[i])
                yield return (AsvRsgaCustomMode)i;
        }
    }

    #endregion

    #region Custom mode

    public static void SetCustomMode(HeartbeatPayload payload, AsvRsgaCustomMode mode)
    {
       payload.SetCustomMode(0,8,(byte)mode);
    }
    
    public static AsvRsgaCustomMode GetCustomMode(HeartbeatPayload? payload)
    {
        if (payload == null)
        {
            return AsvRsgaCustomMode.AsvRsgaCustomModeIdle;
        }

        return (AsvRsgaCustomMode)payload.GetCustomMode(0,8);
    }
    
    public static AsvRsgaCustomSubMode GetCustomSubMode(HeartbeatPayload? payload)
    {
        if (payload == null)
        {
            return 0;
        }

        return (AsvRsgaCustomSubMode)payload.GetCustomMode(8,8);
    }
    
    public static void SetCustomSubMode(HeartbeatPayload payload, AsvRsgaCustomSubMode subMode)
    {
        payload.SetCustomMode(8,8,(uint)subMode);
    }
    

    #endregion
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterRsga(this IServerDeviceBuilder builder)
    {
        builder.Register<IAsvRsgaServer>((identity, context,_) => new AsvRsgaServer(identity,context));
        return builder;
    }
    public static IServerDeviceBuilder RegisterRsgaEx(this IServerDeviceBuilder builder, AsvRadioCapabilities capabilities, IReadOnlySet<AsvAudioCodec> codecs)
    {
        builder
            .Register<IAsvRsgaServerEx, IAsvRsgaServer, ICommandServerEx<CommandLongPacket>,
                IStatusTextServer>((_, _, _, @base, cmd, status) =>
                new AsvRsgaServerEx(@base,cmd));
        return builder;
    }
   

    #endregion

    
    
}
