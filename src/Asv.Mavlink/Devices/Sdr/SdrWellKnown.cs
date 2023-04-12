using System;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink.Sdr;

public static class SdrWellKnown
{
    public const int RecordNameMaxLength = 27;
    public const int RecordTagNameMaxLength = 16;

    public static float GetCommandParamValue(ushort recordIndex, AsvSdrRecordTagFlag asvSdrRecordTagFlagForCurrent, AsvSdrRecordTagType tagType)
    {
        var array = new byte[4];
        BitConverter.TryWriteBytes(array, recordIndex);
        array[2] = (byte)asvSdrRecordTagFlagForCurrent;
        array[3] = (byte)tagType;
        return BitConverter.ToSingle(array);
    }
    public static void ParseCommandParamValue(float paramValue, out ushort recordIndex, out AsvSdrRecordTagFlag asvSdrRecordTagFlagForCurrent, out AsvSdrRecordTagType tagType)
    {
        var value = BitConverter.GetBytes(paramValue);
        recordIndex = BitConverter.ToUInt16(value, 0);
        asvSdrRecordTagFlagForCurrent = (AsvSdrRecordTagFlag)value[2];
        tagType = (AsvSdrRecordTagType)value[3];
    }
}