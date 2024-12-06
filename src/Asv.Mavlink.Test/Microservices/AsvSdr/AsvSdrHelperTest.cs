using System;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using Xunit;
using MavCmd = Asv.Mavlink.Common.MavCmd;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrHelper))]
public class AsvSdrHelperTest
{
    [Fact]
    public void CheckCalibrationTableName_IncorrectName_Failure()
    {
        // Arrange
        const string incorrectName = "!@#$%^&*()_1234567890";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AsvSdrHelper.CheckCalibrationTableName(incorrectName));
    }

    [Fact]
    public void CheckRecordName_IncorrectName_Failure()
    {
        // Arrange
        const string incorrectName = "!@#$%^&*()_1234567890";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AsvSdrHelper.CheckRecordName(incorrectName));
    }

    #region Tag

    [Fact]
    public void CheckTagName_IncorrectName_Failure()
    {
        // Arrange
        const string incorrectName = "!@#$%^&*()_1234567890";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AsvSdrHelper.CheckRecordName(incorrectName));
    }


    [Fact]
    public void PrintRecordsTags_Success()
    {
        // Arrange
        const string tagName = "TestTag";
        byte[] value = [1, 2, 3, 4, 5, 6, 7, 8];

        // Act
        var s1 = AsvSdrHelper.PrintTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64, value);
        var s2 = AsvSdrHelper.PrintTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, value);
        var s3 = AsvSdrHelper.PrintTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, value);
        var s4 = AsvSdrHelper.PrintTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, value);

        // Assert
        Assert.NotEmpty(s1);
        Assert.NotEmpty(s2);
        Assert.NotEmpty(s3);
        Assert.NotEmpty(s4);
    }


    [Fact]
    public void GetTagValueAsUInt64_Success()
    {
        // Arrange
        var rawValue = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        // Act
        var actual = AsvSdrHelper.GetTagValueAsUInt64(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64);

        // Assert
        Assert.Equal(actual, BitConverter.ToUInt64(rawValue));
    }

    [Fact]
    public void GetTagValueAsInt64_Success()
    {
        // Arrange
        var rawValue = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        // Act
        var actual = AsvSdrHelper.GetTagValueAsInt64(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64);

        // Assert
        Assert.Equal(actual, BitConverter.ToInt64(rawValue));
    }

    [Fact]
    public void GetTagValueAsReal64_Success()
    {
        // Arrange
        var rawValue = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        // Act
        var actual = AsvSdrHelper.GetTagValueAsReal64(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64);

        // Assert
        Assert.Equal(actual, BitConverter.ToDouble(rawValue));
    }

    [Fact]
    public void GetTagValueAsString_Success()
    {
        // Arrange
        var rawValue = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        // Act
        var actual = AsvSdrHelper.GetTagValueAsString(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8);

        // Assert
        Assert.Equal(actual, MavlinkTypesHelper.GetString(rawValue));
    }

    [Fact]
    public void SetTagValueAsUInt64_Success()
    {
        // Arrange
        var rawValue = new Span<byte>(new byte[8]);
        const ulong value = 32ul;

        // Act
        AsvSdrHelper.SetTagValueAsUInt64(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64, value);

        // Assert
        Assert.Equal(value, BitConverter.ToUInt64(rawValue));
    }

    [Fact]
    public void SetTagValueAsInt64_Success()
    {
        // Arrange
        var rawValue = new Span<byte>(new byte[8]);
        const long value = 32L;

        // Act
        AsvSdrHelper.SetTagValueAsInt64(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, value);

        // Assert
        Assert.Equal(value, BitConverter.ToInt64(rawValue));
    }

    [Fact]
    public void SetTagValueAsReal64_Success()
    {
        // Arrange
        var rawValue = new Span<byte>(new byte[8]);
        const double value = 32.0D;

        // Act
        AsvSdrHelper.SetTagValueAsReal64(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, value);

        // Assert
        Assert.Equal(value, BitConverter.ToDouble(rawValue));
    }

    [Fact]
    public void SetTagValueAsString_Success()
    {
        // Arrange
        var rawValue = new byte[8];
        const string value = "32string";

        // Act
        AsvSdrHelper.SetTagValueAsString(rawValue, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, value);

        // Assert
        Assert.Equal(value, MavlinkTypesHelper.GetString(rawValue));
    }

    #endregion

    #region Set mode

    [Fact]
    public void SetArgsForSdrSetMode_CommandLongPayload_Success()
    {
        // Arrange
        CommandLongPayload item = new();
        const AsvSdrCustomMode mode = AsvSdrCustomMode.AsvSdrCustomModeIdle;
        const ulong frequencyHz = 2400ul;
        const float recordRate = 1.2f;
        const uint sendingThinningRatio = 1u;
        const float referencePowerDbm = 9.0f;

        // Act
        AsvSdrHelper.SetArgsForSdrSetMode(
            item,
            mode,
            frequencyHz,
            recordRate,
            sendingThinningRatio,
            referencePowerDbm);

        // Assert
        Assert.Equal(item.Command, (MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode);
        Assert.Equal(item.Param1, BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)));
        Assert.Equal(item.Param2, BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 0));
        Assert.Equal(item.Param3, BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 4));
        Assert.Equal(item.Param4, recordRate);
        Assert.Equal(item.Param5, BitConverter.ToSingle(BitConverter.GetBytes(sendingThinningRatio)));
        Assert.Equal(item.Param6, referencePowerDbm);
        Assert.Equal(item.Param7, float.NaN);
    }

    [Fact]
    public void SetArgsForSdrSetMode_MissionItemIntPayload_Success()
    {
        // Arrange
        MissionItemIntPayload item = new();
        const AsvSdrCustomMode mode = AsvSdrCustomMode.AsvSdrCustomModeIdle;
        const ulong frequencyHz = 2400ul;
        const float recordRate = 1.2f;
        const uint sendingThinningRatio = 1u;
        const float referencePowerDbm = 9.0f;

        // Act
        AsvSdrHelper.SetArgsForSdrSetMode(
            item,
            mode,
            frequencyHz,
            recordRate,
            sendingThinningRatio,
            referencePowerDbm);

        // Assert
        Assert.Equal(item.Command, (MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode);
        Assert.Equal(item.Param1, BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)));
        Assert.Equal(item.Param2, BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 0));
        Assert.Equal(item.Param3, BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 4));
        Assert.Equal(item.Param4, recordRate);
        Assert.Equal(item.X, BitConverter.ToInt32(BitConverter.GetBytes(sendingThinningRatio)));
        Assert.Equal(item.Y, BitConverter.ToInt32(BitConverter.GetBytes(referencePowerDbm)));
        Assert.Equal(item.Z, float.NaN);
    }
    
    [Fact]
    public void GetArgsForSdrSetMode_CommandLongPayload_Success()
    {
        // Arrange
        const AsvSdrCustomMode mode = AsvSdrCustomMode.AsvSdrCustomModeIdle;
        const ulong frequencyHz = 2400ul;
        const float recordRate = 1.2f;
        const uint sendingThinningRatio = 1u;
        const float referencePowerDbm = 9.0f;

        var item = new CommandLongPayload
        {
            Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)),
            Param2 = BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 0),
            Param3 = BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 4),
            Param4 = recordRate,
            Param5 = BitConverter.ToSingle(BitConverter.GetBytes(sendingThinningRatio)),
            Param6 = referencePowerDbm,
            Param7 = float.NaN,
            Command = (MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode
        };

        // Act
        AsvSdrHelper.GetArgsForSdrSetMode(
            item,
            out var modeO, 
            out var frequencyHzO, 
            out var recordRateO,
            out var sendingThinningRatioO, 
            out var referencePowerDbmO);

        // Assert
        Assert.Equal(mode, modeO);
        Assert.Equal(frequencyHz, frequencyHzO);
        Assert.Equal(recordRate, recordRateO);
        Assert.Equal(sendingThinningRatio, sendingThinningRatioO);
        Assert.Equal(referencePowerDbm, referencePowerDbmO);
    }
    
    [Fact]
    public void GetArgsForSdrSetMode_ServerMissionItem_Success()
    {
        // Arrange
        const AsvSdrCustomMode mode = AsvSdrCustomMode.AsvSdrCustomModeIdle;
        const ulong frequencyHz = 2400ul;
        const float recordRate = 1.2f;
        const uint sendingThinningRatio = 1u;
        const float referencePowerDbm = 9.0f;

        var item = new ServerMissionItem
        {
            Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)),
            Param2 = BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 0),
            Param3 = BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz), 4),
            Param4 = recordRate,
            X = BitConverter.ToInt32(BitConverter.GetBytes(sendingThinningRatio)),
            Y = BitConverter.ToInt32(BitConverter.GetBytes(referencePowerDbm)),
            Z = float.NaN,
            Command = (MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSetMode
        };

        // Act
        AsvSdrHelper.GetArgsForSdrSetMode(
            item,
            out var modeO, 
            out var frequencyHzO, 
            out var recordRateO,
            out var sendingThinningRatioO, 
            out var referencePowerDbmO);

        // Assert
        Assert.Equal(mode, modeO);
        Assert.Equal(frequencyHz, frequencyHzO);
        Assert.Equal(recordRate, recordRateO);
        Assert.Equal(sendingThinningRatio, sendingThinningRatioO);
        Assert.Equal(referencePowerDbm, referencePowerDbmO);
    }
    
    #endregion

    #region StartRecords

    [Fact]
    public void Set_Get_ArgsForSdrStartRecord_MissionItemIntPayload_Success()
    {
        // Arrange
        const string recordName = "TestRecordName";
        MissionItemIntPayload payload = new();

        // Act
        AsvSdrHelper.SetArgsForSdrStartRecord(payload, recordName);

        var item = AsvSdrHelper.Convert(payload);
        
        AsvSdrHelper.GetArgsForSdrStartRecord(item, out var recordNameO);

        // Assert
        Assert.Equal(recordName, recordNameO);
    }
    
    [Fact]
    public void Set_Get_ArgsForSdrStartRecord_CommandLongPayload_Success()
    {
        // Arrange
        const string recordName = "TestRecordName";
        CommandLongPayload payload = new();

        // Act
        AsvSdrHelper.SetArgsForSdrStartRecord(payload, recordName);
        AsvSdrHelper.GetArgsForSdrStartRecord(payload, out var recordNameO);

        // Assert
        Assert.Equal(recordName, recordNameO);
    }

    #endregion

    #region StopRecords

    [Fact]
    public void Set_Get_ArgsForSdrStopRecord_Success()
    {
        // Arrange
        CommandLongPayload payload = new(); 

        // Act
        AsvSdrHelper.SetArgsForSdrStopRecord(payload);
        AsvSdrHelper.GetArgsForSdrStopRecord(payload);
        
        // Assert
        Assert.Equal(payload.Command, (MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopRecord);
    }

    #endregion

    #region CurrentRecordSetTag

    [Fact]
    public void Set_Get_ArgsForSdrCurrentRecordSetTag_CommandLongPayload_Success()
    {
        // Arrange
        CommandLongPayload item = new();
        const string tagName = "TestTagName";
        const AsvSdrRecordTagType type = AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64;
        var rawValue = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};

        // Act
        AsvSdrHelper.SetArgsForSdrCurrentRecordSetTag(item, tagName, type, rawValue);
        AsvSdrHelper.GetArgsForSdrCurrentRecordSetTag(item, out var tagNameO, out var typeO, out var rawValueO);
        
        // Assert
        Assert.Equal(tagName, tagNameO);
        Assert.Equal(type, typeO);
        Assert.Equal(rawValue, rawValueO);
    }
    
    [Fact]
    public void Set_Get_ArgsForSdrCurrentRecordSetTag_MissionItemIntPayload_Success()
    {
        // Arrange
        MissionItemIntPayload item = new();
        const string tagName = "TestTagName";
        const AsvSdrRecordTagType type = AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64;
        var rawValue = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};

        // Act
        AsvSdrHelper.SetArgsForSdrCurrentRecordSetTag(item, tagName, type, rawValue);

        var itemO = AsvSdrHelper.Convert(item);

        AsvSdrHelper.GetArgsForSdrCurrentRecordSetTag(itemO, out var tagNameO, out var typeO, out var rawValueO);
        
        // Assert
        Assert.Equal(tagName, tagNameO);
        Assert.Equal(type, typeO);
        Assert.Equal(rawValue, rawValueO);
    }

    #endregion

    #region SystemControlAction

    [Fact]
    public void Set_Get_ArgsForSdrSystemControlAction_CommandLongPayload_Success()
    {
        // Arrange
        CommandLongPayload item = new();
        const AsvSdrSystemControlAction action = AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown;

        // Act
        AsvSdrHelper.SetArgsForSdrSystemControlAction(item, action);
        AsvSdrHelper.GetArgsForSdrSystemControlAction(item, out var actionO);
        
        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction, item.Command);
        Assert.Equal(action, actionO);
    }
    
    [Fact]
    public void Set_Get_ArgsForSdrSystemControlAction_MissionItemIntPayload_Success()
    {
        // Arrange
        MissionItemIntPayload item = new();
        const AsvSdrSystemControlAction action = AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown;

        // Act
        AsvSdrHelper.SetArgsForSdrSystemControlAction(item, action);
        
        var itemO = AsvSdrHelper.Convert(item);
        
        AsvSdrHelper.GetArgsForSdrSystemControlAction(itemO, out var actionO);
        
        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction, item.Command);
        Assert.Equal(action, actionO);
    }

    #endregion

    #region StartMission

    [Fact]
    public void Set_Get_ArgsForSdrStartMission_Success()
    {
        // Arrange
        CommandLongPayload item = new();
        const ushort missionIndex = 1;

        // Act
        AsvSdrHelper.SetArgsForSdrStartMission(item, missionIndex);
        AsvSdrHelper.GetArgsForSdrStartMission(item, out var missionIndexO);
        
        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartMission, item.Command);
        Assert.Equal(missionIndex, missionIndexO);
    }

    #endregion

    #region StopMission

    [Fact]
    public void Set_Get_ArgsForSdrStopMission_Success()
    {
        // Arrange
        CommandLongPayload item = new();

        // Act
        AsvSdrHelper.SetArgsForSdrStopMission(item);
        AsvSdrHelper.GetArgsForSdrStopMission(item);
        
        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopMission, item.Command);
    }

    #endregion

    #region SdrDelay

    [Fact]
    public void SetArgsForSdrDelay_Success()
    {
        // Arrange
        MissionItemIntPayload payload = new();
        const uint delayMs = 1000;

        // Act
        AsvSdrHelper.SetArgsForSdrDelay(payload, delayMs);
        
        var item = AsvSdrHelper.Convert(payload);
        
        AsvSdrHelper.GetArgsForSdrDelay(item, out var delayMsO);

        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrDelay, payload.Command);
        Assert.Equal(delayMs, delayMsO);
    } 

    #endregion

    #region SdrWaitVehicleWaypoint

    [Fact]
    public void Set_Get_ArgsForSdrWaitVehicleWaypoint_MissionItemIntPayload_Success()
    {
        // Arrange
        MissionItemIntPayload payload = new();
        const ushort index = 1000;
        
        // Act
        AsvSdrHelper.SetArgsForSdrWaitVehicleWaypoint(payload, index);

        var item = AsvSdrHelper.Convert(payload);
        
        AsvSdrHelper.GetArgsForSdrWaitVehicleWaypoint(item, out var indexO);

        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrWaitVehicleWaypoint, payload.Command);
        Assert.Equal(index, indexO);
    }

    #endregion

    #region Calibration

    [Fact]
    public void Set_Get_ArgsForSdrStartCalibration_Success()
    {
        // Arrange
        CommandLongPayload payload = new();
        
        // Act
        AsvSdrHelper.SetArgsForSdrStartCalibration(payload);
        AsvSdrHelper.GetArgsForSdrStartCalibration(payload);

        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStartCalibration, payload.Command);
    }
    
    [Fact]
    public void Set_Get_ArgsForSdrStopCalibration_Success()
    {
        // Arrange
        CommandLongPayload payload = new();
        
        // Act
        AsvSdrHelper.SetArgsForSdrStopCalibration(payload);
        AsvSdrHelper.GetArgsForSdrStopCalibration(payload);

        // Assert
        Assert.Equal((MavCmd)AsvSdr.MavCmd.MavCmdAsvSdrStopCalibration, payload.Command);
    }

    #endregion
}