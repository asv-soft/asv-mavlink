using System;
using Asv.Mavlink.V2.AsvRfsa;

namespace Asv.Mavlink;

public class SignalInfo
{
    public SignalInfo(ushort id, float maxX, float minX, float maxY, float minY, int axisXSize, int axisYSize, 
        AsvRfsaSignalFormat format,string signalName,string axesXName,string axesYName)
    {
        RfsaHelper.CheckSignalName(signalName);
        SignalName = signalName;
        RfsaHelper.CheckSignalAxisName(axesXName);
        AxesXName = axesXName;
        RfsaHelper.CheckSignalAxisName(axesYName);
        AxesYName = axesYName;
        Id = id;
        MaxX = maxX;
        MinX = minX;
        if (minX > maxX)
        {
            throw new ArgumentOutOfRangeException(nameof(maxX));
        }
        
        MaxY = maxY;
        MinY = minY;
        if (minY > maxY)
        {
            throw new ArgumentOutOfRangeException(nameof(maxY));
        }
        
        AxisXSize = axisXSize;
        AxisYSize = axisYSize;
        if (AxisXSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(axisXSize));
        }
        if (AxisYSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(axisYSize));
        }
        Format = format;
        
        OneMeasureByteSize = RfsaHelper.GetByteSizeOneMeasure(Format);
        OneFrameByteSize = (byte)(AxisXSize * AxisYSize * OneMeasureByteSize);
        OneFrameMeasureSize = AxisYSize * AxisXSize;
    }

    public SignalInfo(AsvRfsaSignalInfoPayload p)
        :this(p.SignalId, p.AxesXMax, p.AxesXMin, p.AxesYMax,
            p.AxesYMin, p.AxesXCount, p.AxesYCount, p.Format,
            MavlinkTypesHelper.GetString(p.SignalName),MavlinkTypesHelper.GetString(p.AxesXName),MavlinkTypesHelper.GetString(p.AxesYName))
    {
       
    }
    public ushort Id { get; }
    public string AxesYName { get; set; }
    public string AxesXName { get; set; }
    public string SignalName { get; set; }
    public float MaxX { get; }
    public float MinX { get; }
    public float MaxY { get; }
    public float MinY { get; }
    public int AxisXSize { get; }
    public int AxisYSize { get; }
    public AsvRfsaSignalFormat Format { get; }
    public int OneFrameByteSize { get; }
    public byte OneMeasureByteSize { get; }
    public int OneFrameMeasureSize { get; }
    public void Fill(AsvRfsaSignalInfoPayload payload)
    {
        payload.SignalId = Id;
        payload.AxesXMax = MaxX;
        payload.AxesXMin = MinX;
        payload.AxesYMax = MaxY;
        payload.AxesYMin = MinY;
        payload.AxesXCount = (ushort)AxisXSize;
        payload.AxesYCount = (ushort)AxisYSize;
        payload.Format = Format;
        MavlinkTypesHelper.SetString(payload.SignalName,SignalName);
        MavlinkTypesHelper.SetString(payload.AxesXName,AxesXName);
        MavlinkTypesHelper.SetString(payload.AxesYName,AxesYName);
    }
}