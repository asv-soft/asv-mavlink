using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IAsvSdrClientEx
{
    IAsvSdrClient Base { get; }
    IRxValue<AsvSdrCustomModeFlag> SupportedModes { get; }
    IRxValue<AsvSdrCustomMode> CustomMode { get; }
    IRxValue<ushort> RecordsCount { get; }
    IObservable<IChangeSet<IAsvSdrClientRecord,RecordId>> Records { get; }
    Task DeleteRecord(RecordId recordName, CancellationToken cancel = default);
    Task<bool> DownloadRecordList(IProgress<double> progress = null, CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, CancellationToken cancel);
    Task<MavResult> StartRecord(RecordId recordName, CancellationToken cancel);
    Task<MavResult> StopRecord(CancellationToken cancel);
    Task<MavResult> CurrentRecordSetTag(string tagName, AsvSdrRecordTagType type, byte[] rawValue , CancellationToken cancel);
}

public static class AsvSdrClientExHelper
{
    public static Task<MavResult> CurrentRecordSetTag(this IAsvSdrClientEx src, string tagName, string value, CancellationToken cancel)
    {
        if (value.Length > SdrWellKnown.RecordTagValueMaxLength) 
            throw new Exception($"Tag string value is too long. Max length is {SdrWellKnown.RecordTagValueMaxLength}");
        var nameArray = new byte[SdrWellKnown.RecordTagValueMaxLength];
        MavlinkTypesHelper.SetString(nameArray,value);
        return src.CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, cancel);
    }
    public static Task<MavResult> CurrentRecordSetTag(this IAsvSdrClientEx src, string tagName, ulong value, CancellationToken cancel)
    {
        return src.CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64, BitConverter.GetBytes(value), cancel);
    }
    public static Task<MavResult> CurrentRecordSetTag(this IAsvSdrClientEx src, string tagName, long value, CancellationToken cancel)
    {
        return src.CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, BitConverter.GetBytes(value), cancel);
    }
    
    public static Task<MavResult> CurrentRecordSetTag(this IAsvSdrClientEx src, string tagName, double value, CancellationToken cancel)
    {
        return src.CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, BitConverter.GetBytes(value), cancel);
    }
    
}