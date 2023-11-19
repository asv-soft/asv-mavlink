using System;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class AsvSdrHelperTest
{

    [Theory]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeLlz, 0, 0, 0, -10)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeGp, 330000001, 5, 1,-60)]
    public void Check_mission_conversion_for_set_mode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        
        AsvSdrHelper.SetArgsForSdrSetMode(input, mode, frequencyHz, recordRate, sendingThinningRatio,refPower);
        
        input.Serialize(ref inputSpan);
        
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        
        
        var serverItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrSetMode(serverItem, out var modeOut, out var frequencyHzOut, out var recordRateOut, out var sendingThinningRatioOut, out var outRefPower);
        Assert.Equal(mode, modeOut);
        Assert.Equal(frequencyHz, frequencyHzOut);
        Assert.Equal(recordRate, recordRateOut);
        Assert.Equal(sendingThinningRatio, sendingThinningRatioOut);
        Assert.Equal(refPower, outRefPower);
    }
    
    [Theory]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeLlz, 0, 0, 0, -10)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeGp, 330000001, 5, 1,-60)]
    public void Check_command_conversion_for_set_mode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower)
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        
        AsvSdrHelper.SetArgsForSdrSetMode(input, mode, frequencyHz, recordRate, sendingThinningRatio, refPower);
        input.Serialize(ref inputSpan);
        
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        
        AsvSdrHelper.GetArgsForSdrSetMode(output, out var modeOut, out var frequencyHzOut, out var recordRateOut, out var sendingThinningRatioOut, out var outRefPower);
        Assert.Equal(mode, modeOut);
        Assert.Equal(frequencyHz, frequencyHzOut);
        Assert.Equal(recordRate, recordRateOut);
        Assert.Equal(sendingThinningRatio, sendingThinningRatioOut);
        Assert.Equal(refPower, outRefPower);
    }

    [Theory]
    [InlineData("Data1")]
    [InlineData("Record1")]
    public void Check_mission_conversion_for_start_record(string inputRecordName)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrStartRecord(input, inputRecordName);
        input.Serialize(ref inputSpan);
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        var outputServerItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrStartRecord(outputServerItem, out var recordNameOut);
        Assert.Equal(inputRecordName, recordNameOut);
    }
    
    [Theory]
    [InlineData("Data1")]
    [InlineData("Record1")]
    public void Check_command_conversion_for_start_record(string inputRecordName)
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrStartRecord(input, inputRecordName);
        input.Serialize(ref inputSpan);
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        AsvSdrHelper.GetArgsForSdrStartRecord(output, out var recordNameOut);
        Assert.Equal(inputRecordName, recordNameOut);
    }

    [Fact]
    public void Check_mission_conversion_for_stop_record()
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrStopRecord(input);
        input.Serialize(ref inputSpan);
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        var outputServerItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrStopRecord(outputServerItem);
        
    }
    [Fact]
    public void Check_command_conversion_for_stop_record()
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrStopRecord(input);
        input.Serialize(ref inputSpan);
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        AsvSdrHelper.GetArgsForSdrStopRecord(output);
    }

    [Theory]
    [InlineData("Tag1", AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, new byte[]{0,2,3,4,0,6,7,0})]
    [InlineData("Tag23",AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, new byte[]{1,2,3,4,5,6,7,8})]
    public void Check_mission_conversion_for_set_tag(string nameIn, AsvSdrRecordTagType typeIn, byte[] rawValueIn)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrCurrentRecordSetTag(input, nameIn, typeIn,rawValueIn);
        input.Serialize(ref inputSpan);
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        var outputServerItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrCurrentRecordSetTag(outputServerItem, out var nameOut, out var typeOut, out var rawValueOut);
        Assert.Equal(nameIn, nameOut);
        Assert.Equal(typeIn, typeOut);
        Assert.Equal(rawValueIn, rawValueOut);
        
    }
    [Theory]
    [InlineData("Tag1", AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, new byte[]{0,2,3,4,0,6,7,0})]
    [InlineData("Tag23",AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, new byte[]{1,2,3,4,5,6,7,8})]
    public void Check_command_conversion_for_set_tag(string nameIn, AsvSdrRecordTagType typeIn, byte[] rawValueIn)
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrCurrentRecordSetTag(input, nameIn, typeIn,rawValueIn);
        input.Serialize(ref inputSpan);
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        AsvSdrHelper.GetArgsForSdrCurrentRecordSetTag(output, out var nameOut, out var typeOut, out var rawValueOut);
        Assert.Equal(nameIn, nameOut);
        Assert.Equal(typeIn, typeOut);
        Assert.Equal(rawValueIn, rawValueOut);
    }
    
    [Theory]
    [InlineData(AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot)]
    [InlineData(AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown)]
    public void Check_mission_conversion_for_system_control_action(AsvSdrSystemControlAction action)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrSystemControlAction(input, action);
        input.Serialize(ref inputSpan);
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        var outputServerItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrSystemControlAction(outputServerItem, out var actionOut);
        Assert.Equal(action,actionOut);
    }
    [Theory]
    [InlineData(AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot)]
    [InlineData(AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown)]
    public void Check_command_conversion_for_system_control_action(AsvSdrSystemControlAction action)
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrSystemControlAction(input, action);
        input.Serialize(ref inputSpan);
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        AsvSdrHelper.GetArgsForSdrSystemControlAction(output, out var actionOut);
        Assert.Equal(action,actionOut);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void Check_command_conversion_for_start_mission(ushort missionIndex)
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrStartMission(input, missionIndex);
        input.Serialize(ref inputSpan);
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        AsvSdrHelper.GetArgsForSdrStartMission(output, out var missionIndexOut);
        Assert.Equal(missionIndex,missionIndexOut);
    }
    
    [Fact]
    public void Check_command_conversion_for_stop_mission()
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrStopMission(input);
        input.Serialize(ref inputSpan);
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        AsvSdrHelper.GetArgsForSdrStopMission(output);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(uint.MaxValue)]
    public void Check_mission_conversion_for_sdr_delay(uint delayMs)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrDelay(input, delayMs);
        input.Serialize(ref inputSpan);
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        var outputServerItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrDelay(outputServerItem, out var delayMsOut);
        Assert.Equal(delayMs,delayMsOut);
    }
    
    
    [Theory]
    [InlineData(0)]
    [InlineData(ushort.MaxValue)]
    public void Check_mission_conversion_for_wait_vehicle(ushort index)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        AsvSdrHelper.SetArgsForSdrWaitVehicleWaypoint(input, index);
        input.Serialize(ref inputSpan);
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        var outputServerItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrWaitVehicleWaypoint(outputServerItem, out var indexOut);
        Assert.Equal(index,indexOut);
    }
    [Theory]
    [InlineData(ulong.MaxValue)]
    [InlineData(ulong.MinValue)]
    [InlineData(1234567789)]
    public void Check_set_and_get_value_from_byte_array_uint64(ulong value)
    {
        var data = new byte[AsvSdrHelper.RecordTagValueLength];
        AsvSdrHelper.SetTagValueAsUInt64(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64, value);
        var valueOut = AsvSdrHelper.GetTagValueAsUInt64(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64);
        Assert.Equal(value,valueOut);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(123456789)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void Check_set_and_get_value_from_byte_array_int64(long value)
    {
        var data = new byte[AsvSdrHelper.RecordTagValueLength];
        AsvSdrHelper.SetTagValueAsInt64(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, value);
        var valueOut = AsvSdrHelper.GetTagValueAsInt64(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64);
        Assert.Equal(value,valueOut);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(double.Epsilon)]
    [InlineData(double.NaN)]
    [InlineData(12345.45)]
    public void Check_set_and_get_value_from_byte_array_double(double value)
    {
        var data = new byte[AsvSdrHelper.RecordTagValueLength];
        AsvSdrHelper.SetTagValueAsReal64(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, value);
        var valueOut = AsvSdrHelper.GetTagValueAsReal64(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64);
        Assert.Equal(value,valueOut);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("1234")]
    [InlineData("A")]
    public void Check_set_and_get_value_from_byte_array_string8(string value)
    {
        var data = new byte[AsvSdrHelper.RecordTagValueLength];
        AsvSdrHelper.SetTagValueAsString(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, value);
        var valueOut = AsvSdrHelper.GetTagValueAsString(data, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8);
        Assert.Equal(value,valueOut);
    }
    
    
    
    
  
    
}