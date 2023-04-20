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
    IRxValue<AsvSdrSupportModeFlag> SupportedModes { get; }
    IRxValue<AsvSdrCustomMode> CustomMode { get; }
    IRxValue<ushort> RecordsCount { get; }
    IObservable<IChangeSet<AsvSdrClientRecord,ushort>> Records { get; }
    Task DeleteRecord(ushort recordIndex, CancellationToken cancel = default);
    Task DeleteRecords(ushort startIndex,ushort stopIndex, CancellationToken cancel = default);
    Task<bool> DownloadRecordList(IProgress<double> progress = null, CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, CancellationToken cancel);
    Task<MavResult> StartRecord(string recordName, CancellationToken cancel);
    Task<MavResult> StopRecord(CancellationToken cancel);
    Task<MavResult> CurrentRecordSetTag(AsvSdrClientRecordTag tag, CancellationToken cancel);
    Task<MavResult> RecordSetTag(ushort recordIndex,AsvSdrClientRecordTag tag, CancellationToken cancel);
}