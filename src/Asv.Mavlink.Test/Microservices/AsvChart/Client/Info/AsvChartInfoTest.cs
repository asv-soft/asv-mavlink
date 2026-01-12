using System;
using Asv.Mavlink.AsvChart;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartInfo))]
public class AsvChartInfoTest
{
    [Theory]
    [InlineData(20, "test", AsvChartDataFormat.AsvChartDataFormatFloat, 192)]
    [InlineData(ushort.MinValue, "otherTestName", AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit, 324)]
    [InlineData(ushort.MaxValue, "otherTestName16", AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit, 3421)]
    public void CtorWithManyArguments_PerfectArguments_Success(ushort id, string signalName, AsvChartDataFormat format, ushort hash)
    {
        // Arrange 
        var axisInfoX = new AsvChartAxisInfo("x", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 100, 10);
        var axisInfoY = new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 10, 30);
        var expectedOneMeasureByteSize = AsvChartHelper.GetByteSizeOneMeasure(format);
        var expectedOneFrameByteSize = (ushort)(axisInfoX.Size * axisInfoY.Size * expectedOneMeasureByteSize);
        var expectedOneFrameMeasureSize = axisInfoX.Size * axisInfoY.Size;
        
        // Act
        var info = new AsvChartInfo(id, signalName, axisInfoX, axisInfoY, format, hash);
        
        // Assert
        Assert.NotNull(info);
        Assert.Equal(id, info.Id);
        Assert.Equal(signalName, info.SignalName);
        Assert.Equal(format, info.Format);
        Assert.True(axisInfoX.IsDeepEqual(info.AxisX));
        Assert.True(axisInfoY.IsDeepEqual(info.AxisY));
        Assert.Equal(hash, info.InfoHash);
        Assert.Equal(expectedOneMeasureByteSize, info.OneMeasureByteSize);
        Assert.Equal(expectedOneFrameByteSize, info.OneFrameByteSize);
        Assert.Equal(expectedOneFrameMeasureSize, info.OneFrameMeasureSize);
    }
    
    [Theory]
    [InlineData(20, "test", AsvChartType.AsvChartTypeHeatmap, AsvChartDataFormat.AsvChartDataFormatFloat, 192)]
    [InlineData(ushort.MinValue, "otherTestName", AsvChartType.AsvChartTypeSimple, AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit, 324)]
    [InlineData(ushort.MaxValue, "otherTestName16", AsvChartType.AsvChartTypeHeatmap, AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit, 3421)]
    public void CtorWithPayload_PerfectArguments_Success(ushort id, string signalName, AsvChartType type, AsvChartDataFormat format, ushort hash)
    {
        // Arrange 
        var axisInfoX = new AsvChartAxisInfo("x", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 100, 10);
        var axisInfoY = new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 10, 30);
        var expectedOneMeasureByteSize = AsvChartHelper.GetByteSizeOneMeasure(format);
        var expectedOneFrameByteSize = (ushort)(axisInfoX.Size * axisInfoY.Size * expectedOneMeasureByteSize);
        var expectedOneFrameMeasureSize = axisInfoX.Size * axisInfoY.Size;
            
        var payload = CreatePayload(id, signalName, hash, type, format, axisInfoX, axisInfoY);
        
        // Act
        var info = new AsvChartInfo(payload);
        
        // Assert
        Assert.NotNull(info);
        Assert.Equal(id, info.Id);
        Assert.Equal(signalName, info.SignalName);
        Assert.Equal(format, info.Format);
        Assert.True(axisInfoX.IsDeepEqual(info.AxisX));
        Assert.True(axisInfoY.IsDeepEqual(info.AxisY));
        Assert.Equal(hash, info.InfoHash);
        Assert.Equal(expectedOneMeasureByteSize, info.OneMeasureByteSize);
        Assert.Equal(expectedOneFrameByteSize, info.OneFrameByteSize);
        Assert.Equal(expectedOneFrameMeasureSize, info.OneFrameMeasureSize);
    }
    
    [Fact]
    public void Ctor_NullArguments_Throws()
    {
        // Arrange
        AsvChartInfoPayload? payload = null;
        AsvChartAxisInfo? axis = null;
        
        // Act + Assert
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<NullReferenceException>(() => new AsvChartInfo(payload));
        Assert.Throws<ArgumentNullException>(() => new AsvChartInfo(1, "testChart", axis, axis, AsvChartDataFormat.AsvChartDataFormatFloat));
#pragma warning restore CS8604 // Possible null reference argument.
    }
    
    [Theory]
    [InlineData(20, "test", AsvChartDataFormat.AsvChartDataFormatFloat)]
    [InlineData(ushort.MinValue, "otherTestName", AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit)]
    [InlineData(ushort.MaxValue, "otherTestName16", AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit)]
    public void Ctor_InternalHashCalculation_Success(ushort id, string signalName, AsvChartDataFormat format)
    {
        // Arrange 
        var axisInfoX = new AsvChartAxisInfo("x", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 100, 10);
        var axisInfoY = new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 10, 30);
        var hash = new HashCode();
        hash.Add(id);
        hash.Add(signalName);
        hash.Add(axisInfoX.Name);
        hash.Add(axisInfoX.Unit);
        hash.Add(axisInfoX.Min);
        hash.Add(axisInfoX.Max);
        hash.Add(axisInfoX.Size);
        hash.Add(axisInfoY.Name);
        hash.Add(axisInfoY.Unit);
        hash.Add(axisInfoY.Min);
        hash.Add(axisInfoY.Max);
        hash.Add(axisInfoY.Size);
        hash.Add(format);
        var manualHash = AsvChartHelper.GetHashCode(hash);
        
        // Act
        var info = new AsvChartInfo(id, signalName, axisInfoX, axisInfoY, format);
        
        // Assert
        Assert.NotNull(info);
        Assert.Equal(manualHash, info.InfoHash);
    }

    [Theory(Skip = "AsvChartInfo has no field for AsvChartType")]
    [InlineData(AsvChartType.AsvChartTypeSimple)]
    [InlineData(AsvChartType.AsvChartTypeHeatmap)]
    public void Fill_ProperInput_Success(AsvChartType chartType)
    {
        // Arrange
        var axisInfoX = new AsvChartAxisInfo("x", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 100, 10);
        var axisInfoY = new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom, 0, 10, 30);
        var initialPayload = CreatePayload(
            1, 
            "name", 
            123, 
            chartType, 
            AsvChartDataFormat.AsvChartDataFormatFloat, 
            axisInfoX, 
            axisInfoY
        );
        
        var info = new AsvChartInfo(initialPayload);
        var payloadToFill = new AsvChartInfoPayload();
        
        // Act
        info.Fill(payloadToFill);
        
        // Assert
        Assert.NotNull(payloadToFill);
        Assert.True(initialPayload.IsDeepEqual(payloadToFill));
    }

    private AsvChartInfoPayload CreatePayload(
        ushort id, 
        string chartName,
        ushort hash,
        AsvChartType type,
        AsvChartDataFormat format, 
        AsvChartAxisInfo axisX,
        AsvChartAxisInfo axisY
    )
    {
        var payload = new AsvChartInfoPayload
        {
            ChartId = id,
            ChartInfoHash = hash,
            ChartType = type,
            Format = format,
            
            AxesXMin = axisX.Min,
            AxesXMax = axisX.Max,
            AxesXCount = (ushort) axisX.Size,
            AxesXUnit = axisX.Unit,
            
            AxesYMin = axisY.Min,
            AxesYMax = axisY.Max,
            AxesYCount = (ushort) axisY.Size,
            AxesYUnit = axisY.Unit,
        };
        
        MavlinkTypesHelper.SetString(payload.ChartName, chartName);
        MavlinkTypesHelper.SetString(payload.AxesXName, axisX.Name);
        MavlinkTypesHelper.SetString(payload.AxesYName, axisY.Name);
        
        return payload;
    }
}