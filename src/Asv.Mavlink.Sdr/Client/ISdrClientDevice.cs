using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink.Sdr;

public interface ISdrClientDevice
{
    IRxValue<AsvSdrSupportModeFlag> SupportedModes { get; }
    IRxValue<AsvSdrCustomMode> CustomMode { get; }
    IRxValue<ushort> RecordsCount { get; }
    IObservable<IChangeSet<SdrClientDeviceRecord,ushort>> Records { get; }
    Task DeleteRecord(ushort recordIndex, CancellationToken cancel = default);
    Task DeleteRecords(ushort startIndex,ushort stopIndex, CancellationToken cancel = default);
    Task<bool> UploadRecordList(IProgress<double>? progress = null, CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float sendDataRate, CancellationToken cancel);
    Task<MavResult> StartRecord(string recordName, CancellationToken cancel);
    Task<MavResult> StopRecord(CancellationToken cancel);
    Task<MavResult> CurrentRecordSetTag(AsvSdrRecordTag tag, CancellationToken cancel);
    Task<MavResult> RecordSetTag(ushort recordIndex,AsvSdrRecordTag tag, CancellationToken cancel);
    
    
}