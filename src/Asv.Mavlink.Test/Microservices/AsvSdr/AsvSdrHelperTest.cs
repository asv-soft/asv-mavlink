using System;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class AsvSdrHelperTest
{

    [Theory]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeLlz, 0, 0, 0)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeGp, 330000001, 5, 1)]
    public void Check_mission_conversion_for_set_mode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio)
    {
        var input = new MissionItemIntPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        
        AsvSdrHelper.SetArgsForSdrSetMode(input, mode, frequencyHz, recordRate, sendingThinningRatio);
        
        input.Serialize(ref inputSpan);
        
        var output = new MissionItemIntPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        
        
        var serverItem = AsvSdrHelper.Convert(output);
        AsvSdrHelper.GetArgsForSdrSetMode(serverItem, out var modeOut, out var frequencyHzOut, out var recordRateOut, out var sendingThinningRatioOut);
        Assert.Equal(mode, modeOut);
        Assert.Equal(frequencyHz, frequencyHzOut);
        Assert.Equal(recordRate, recordRateOut);
        Assert.Equal(sendingThinningRatio, sendingThinningRatioOut);
    }
    
    [Theory]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeLlz, 0, 0, 0)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeGp, 330000001, 5, 1)]
    public void Check_command_conversion_for_set_mode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio)
    {
        var input = new CommandLongPayload();
        var buffer = new byte[input.GetByteSize()];
        var inputSpan = new Span<byte>(buffer);
        
        AsvSdrHelper.SetArgsForSdrSetMode(input, mode, frequencyHz, recordRate, sendingThinningRatio);
        input.Serialize(ref inputSpan);
        
        var output = new CommandLongPayload();
        var outputSpan = new ReadOnlySpan<byte>(buffer);
        output.Deserialize(ref outputSpan);
        
        AsvSdrHelper.GetArgsForSdrSetMode(output, out var modeOut, out var frequencyHzOut, out var recordRateOut, out var sendingThinningRatioOut);
        Assert.Equal(mode, modeOut);
        Assert.Equal(frequencyHz, frequencyHzOut);
        Assert.Equal(recordRate, recordRateOut);
        Assert.Equal(sendingThinningRatio, sendingThinningRatioOut);
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
    
    
    
  
    
}