using System;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvChart;


namespace Asv.Mavlink;

public static class AsvChartHelper
{
    public const string MavlinkMicroserviceName = "CHART";
    
    public const int SignalNameMaxLength = 16;
    
    public static void CheckSignalName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Signal name is empty");
        if (name.Length > SignalNameMaxLength)
            throw new Exception($"Signal name is too long. Max length is {SignalNameMaxLength}");
    }
    
    public static void CheckSignalAxisName(string name)
    {
        if (name.IsNullOrWhiteSpace()) throw new Exception("Signal axis name is empty");
        if (name.Length > SignalNameMaxLength)
            throw new Exception($"Signal axis name is too long. Max length is {SignalNameMaxLength}");
    }
    
    public static void WriteSignalMeasure(ref Span<byte> span, AsvChartInfo info, float value )
    {
        switch (info.Format)
        {
            case AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit:
                BinSerialize.Write8BitRange(ref span, info.AxisX.Min, info.AxisX.Max, value);
                break;
            case AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit:
                BinSerialize.Write16BitRange(ref span, info.AxisX.Min, info.AxisX.Max, value);
                break;
            case AsvChartDataFormat.AsvChartDataFormatFloat:
                BinSerialize.WriteFloat(ref span,value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static float ReadSignalMeasure(ref ReadOnlySpan<byte> span, AsvChartInfo info)
    {
        return info.Format switch
        {
            AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit => BinSerialize.Read8BitRange(ref span, info.AxisX.Min,
                info.AxisX.Max),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit => BinSerialize.Read16BitRange(ref span, info.AxisX.Min,
                info.AxisX.Max),
            AsvChartDataFormat.AsvChartDataFormatFloat => BinSerialize.ReadFloat(ref span),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static ushort GetHashCode(HashCode hashCode)
    {
        var hash = hashCode.ToHashCode();
        var lowerBits = (ushort)(hash & 0xFFFF);
        var upperBits = (ushort)((hash >> 16) & 0xFFFF);
        return (ushort)(lowerBits ^ upperBits);
    }
    
    public static byte GetByteSizeOneMeasure(AsvChartDataFormat format)
    {   
        return format switch
        {
            AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit => 1,
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit => 2,
            AsvChartDataFormat.AsvChartDataFormatFloat => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterCharts(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder.Register<IAsvChartServer>((identity, context,config) => new AsvChartServer(identity,config.Get<AsvChartServerConfig>(), context));
        return builder;
    }
    public static IMavlinkServerMicroserviceBuilder RegisterCharts(this IMavlinkServerMicroserviceBuilder builder, AsvChartServerConfig config)
    {
        builder.Register<IAsvChartServer>((identity, context,_) => new AsvChartServer(identity,config, context));
        return builder;
    }

    public static IAsvChartServer GetCharts(this IMavlinkServerMicroserviceFactory factory)
    {
        return factory.Get<IAsvChartServer>();
    }
    
    #endregion
}