using System;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class CalibrationTableInfo
{
    public string Name { get; set; }
    public DateTime Updated { get; set; }
    public ushort Size { get; set; }
}

public class CalibrationTableRow
{
    public CalibrationTableRow(AsvSdrCalibTableRowPayload result)
    {
        FrequencyHz = result.RefFreq;
        RefPower = result.RefPower;
        RefValue = result.RefValue;
        MeasuredValue = result.MeasuredValue;
    }

    public CalibrationTableRow(ulong frequencyHz, float refPower, float refValue, float measuredValue)
    {
        FrequencyHz = frequencyHz;
        RefPower = refPower;
        RefValue = refValue;
        MeasuredValue = measuredValue;
    }
    public ulong FrequencyHz { get; }
    public float RefPower { get; }
    public float RefValue { get; }
    public float MeasuredValue { get; }

    public void Fill(AsvSdrCalibTableRowPayload args)
    {
        args.RefFreq = FrequencyHz;
        args.RefPower = RefPower;
        args.RefValue = RefValue;
        args.MeasuredValue = MeasuredValue;
    }
}