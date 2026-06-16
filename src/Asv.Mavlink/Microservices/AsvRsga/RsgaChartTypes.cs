using System;
using Asv.Mavlink.AsvRsga;

namespace Asv.Mavlink;

public readonly record struct RsgaChartRange(double Min, double Max);

public enum RsgaChartEncoding
{
    Auto,
    RangeFloat8Bit,
    RangeFloat16Bit,
    Float,
}

public enum RsgaChartResampling
{
    Auto,
    Linear,
    MinMaxEnvelope,
}

public sealed record RsgaChartSendOptions
{
    public AsvRsgaRttChartType ChartType { get; init; } = AsvRsgaRttChartType.AsvRsgaRttChartTypeCustom;
    public RsgaChartEncoding Encoding { get; init; } = RsgaChartEncoding.Auto;
    public RsgaChartResampling Resampling { get; init; } = RsgaChartResampling.Auto;
    public RsgaChartRange? XRange { get; init; }
    public RsgaChartRange? YRange { get; init; }
    public int? MaxSamples { get; init; }
    public uint DataIndex { get; init; }
    public DateTime? Timestamp { get; init; }
    public AsvRsgaDataFlags Flags { get; init; } = AsvRsgaDataFlags.AsvRsgaDataFlagsIsValid;
}

public sealed record RsgaChartFrame(
    DateTime Timestamp,
    uint DataIndex,
    AsvRsgaDataFlags Flags,
    AsvRsgaRttChartType ChartType,
    AsvRsgaRttChartDataFormat Format,
    RsgaChartRange XRange,
    RsgaChartRange YRange,
    double[] Values
);
