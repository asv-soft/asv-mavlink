using System;
using Asv.Mavlink.AsvChart;


namespace Asv.Mavlink;

public class AsvChartAxisInfo
{
    public AsvChartAxisInfo(string name, AsvChartUnitType unit, float min, float max, int size)
    {
        AsvChartHelper.CheckSignalAxisName(name);
        Name = name;
        Unit = unit;
        Min = min;
        Max = max;
        if (min >= max)
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }
        Size = size;
        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }
    }
    public string Name { get; }
    public AsvChartUnitType Unit { get; }
    public float Min { get; }
    public float Max { get; }
    public int Size { get; }

}

public class AsvChartInfo
{
    public AsvChartInfo(ushort id, string signalName, AsvChartAxisInfo axisX, AsvChartAxisInfo axisY, AsvChartDataFormat format, ushort? hash = null)
    {
        ArgumentNullException.ThrowIfNull(axisX);
        ArgumentNullException.ThrowIfNull(axisY);
        AsvChartHelper.CheckSignalName(signalName);
        SignalName = signalName;
        AxisX = axisX;
        AxisY = axisY;
        Id = id;
        Format = format;
        OneMeasureByteSize = AsvChartHelper.GetByteSizeOneMeasure(Format);
        OneFrameByteSize = (ushort)(AxisX.Size * AxisY.Size * OneMeasureByteSize);
        OneFrameMeasureSize = AxisX.Size * AxisY.Size;
        if (hash.HasValue)
        {
            InfoHash = hash.Value;
        }
        else
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(signalName);
            hashCode.Add(AxisX.Name);
            hashCode.Add(AxisX.Unit);
            hashCode.Add(AxisX.Min);
            hashCode.Add(AxisX.Max);
            hashCode.Add(AxisX.Size);
            hashCode.Add(AxisY.Name);
            hashCode.Add(AxisY.Unit);
            hashCode.Add(AxisY.Min);
            hashCode.Add(AxisY.Max);
            hashCode.Add(AxisY.Size);
            hashCode.Add(Format);
            InfoHash = AsvChartHelper.GetHashCode(hashCode);
        }
    }

    

    public AsvChartInfo(AsvChartInfoPayload p)
        :this(p.ChartId,MavlinkTypesHelper.GetString(p.ChartName), new AsvChartAxisInfo(MavlinkTypesHelper.GetString(p.AxesXName),p.AxesXUnit,p.AxesXMin,p.AxesXMax,p.AxesXCount), 
            new AsvChartAxisInfo(MavlinkTypesHelper.GetString(p.AxesYName),p.AxesYUnit,p.AxesYMin,p.AxesYMax,p.AxesYCount), p.Format,p.ChartInfoHash)
    {
       
    }
    
    public string SignalName { get; }

    public AsvChartAxisInfo AxisY { get; }

    public AsvChartAxisInfo AxisX { get;  }
    
    public ushort Id { get; }
    public AsvChartDataFormat Format { get; }
    public ushort InfoHash { get; }
    public int OneFrameByteSize { get; }
    public byte OneMeasureByteSize { get; }
    public int OneFrameMeasureSize { get; }
    
    public void Fill(AsvChartInfoPayload payload)
    {
        payload.ChartId = Id;
        payload.AxesXMax = AxisX.Max;
        payload.AxesXMin = AxisX.Min;
        payload.AxesYMax = AxisY.Max;
        payload.AxesYMin = AxisY.Min;
        payload.AxesXCount = (ushort)AxisX.Size;
        payload.AxesYCount = (ushort)AxisY.Size;
        payload.Format = Format;
        MavlinkTypesHelper.SetString(payload.ChartName,SignalName);
        MavlinkTypesHelper.SetString(payload.AxesXName,AxisX.Name);
        MavlinkTypesHelper.SetString(payload.AxesYName,AxisY.Name);
        payload.AxesXUnit = AxisX.Unit;
        payload.AxesYUnit = AxisY.Unit;
        payload.ChartInfoHash = InfoHash;
    }

}