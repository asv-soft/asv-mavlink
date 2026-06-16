using System;
using System.Linq;
using Asv.Mavlink.AsvRsga;
using Xunit;

namespace Asv.Mavlink.Test;

public class RsgaChartHelperTest
{
    [Fact]
    public void GeneratedChartTypeEnum_ShouldContainSpectrum()
    {
        // Arrange
        var values = AsvRsgaRttChartTypeHelper.GetValues(x => x).ToArray();

        // Assert
        Assert.Equal(3UL, (ulong)AsvRsgaRttChartType.AsvRsgaRttChartTypeSpectrum);
        Assert.Contains(3UL, values);
    }

    [Fact]
    public void WriteReadChartData_AutoEncoding_ShouldAutoscaleAndDecodeValues()
    {
        // Arrange
        var payload = new AsvRsgaRttChartPayload();
        var values = new[] { -1.0, 0.0, 1.0 };
        var timestamp = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        // Act
        RsgaChartHelper.WriteChartData(payload, values, new RsgaChartSendOptions
        {
            ChartType = AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeXChannel,
            Timestamp = timestamp,
            DataIndex = 42,
        });
        var frame = RsgaChartHelper.ReadChartData(payload);

        // Assert
        Assert.Equal(AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit, frame.Format);
        Assert.Equal(AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeXChannel, frame.ChartType);
        Assert.Equal(42u, frame.DataIndex);
        Assert.Equal(0, frame.XRange.Min);
        Assert.Equal(2, frame.XRange.Max);
        Assert.Equal(-1, frame.YRange.Min, 3);
        Assert.Equal(1, frame.YRange.Max, 3);
        Assert.Equal(values.Length, frame.Values.Length);
        for (var i = 0; i < values.Length; i++)
        {
            Assert.InRange(Math.Abs(values[i] - frame.Values[i]), 0, 0.01);
        }
    }

    [Fact]
    public void WriteReadChartData_FloatEncodingWithMaxSamples_ShouldDownsampleAndClearUnusedPayloadTail()
    {
        // Arrange
        var payload = new AsvRsgaRttChartPayload();
        Array.Fill(payload.Data, byte.MaxValue);
        var values = new[] { 10.0, 20.0, 30.0, 40.0, 50.0 };

        // Act
        RsgaChartHelper.WriteChartData(payload, values, new RsgaChartSendOptions
        {
            Encoding = RsgaChartEncoding.Float,
            Resampling = RsgaChartResampling.Linear,
            MaxSamples = 3,
        });
        var frame = RsgaChartHelper.ReadChartData(payload);

        // Assert
        Assert.Equal(AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat, frame.Format);
        Assert.Equal(new[] { 10.0, 30.0, 50.0 }, frame.Values);

        var usedBytes = RsgaChartHelper.ChartDataHeaderByteSize + (frame.Values.Length * sizeof(float));
        Assert.All(payload.Data.Skip(usedBytes), x => Assert.Equal(0, x));
    }

    [Fact]
    public void WriteReadChartData_RangeFloat16Bit_ShouldDecodeWithHigherPrecision()
    {
        // Arrange
        var payload = new AsvRsgaRttChartPayload();
        var values = new[] { -100.0, -50.0, 0.0, 50.0, 100.0 };

        // Act
        RsgaChartHelper.WriteChartData(payload, values, new RsgaChartSendOptions
        {
            Encoding = RsgaChartEncoding.RangeFloat16Bit,
        });
        var frame = RsgaChartHelper.ReadChartData(payload);

        // Assert
        Assert.Equal(AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat16bit, frame.Format);
        Assert.Equal(values.Length, frame.Values.Length);
        for (var i = 0; i < values.Length; i++)
        {
            Assert.InRange(Math.Abs(values[i] - frame.Values[i]), 0, 0.01);
        }
    }

    [Fact]
    public void WriteReadChartData_AutoResampling_ShouldPreservePeaks()
    {
        // Arrange
        var payload = new AsvRsgaRttChartPayload();
        var values = Enumerable.Repeat(0.0, 1000).ToArray();
        values[500] = 100.0;

        // Act
        RsgaChartHelper.WriteChartData(payload, values, new RsgaChartSendOptions
        {
            MaxSamples = 20,
        });
        var frame = RsgaChartHelper.ReadChartData(payload);

        // Assert
        Assert.Equal(20, frame.Values.Length);
        Assert.Equal(0, frame.XRange.Min);
        Assert.Equal(999, frame.XRange.Max);
        Assert.True(frame.Values.Max() > 90);
    }

    [Fact]
    public void Serialize_SmallChart_ShouldUseMavlinkV2PayloadTruncation()
    {
        // Arrange
        var packet = new AsvRsgaRttChartPacket
        {
            SystemId = 1,
            ComponentId = 1,
            Sequence = 1,
        };
        RsgaChartHelper.WriteChartData(packet.Payload, new[] { 1.0, 2.0, 3.0 }, new RsgaChartSendOptions
        {
            MaxSamples = 3,
        });
        var buffer = new byte[packet.GetMaxByteSize()];
        var span = buffer.AsSpan();

        // Act
        packet.Serialize(ref span);
        var serializedSize = buffer.Length - span.Length;

        // Assert
        Assert.True(serializedSize < packet.GetByteSize());
        Assert.True(serializedSize <= 55);
    }
}
