using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using DynamicData;

namespace Asv.Mavlink.Sdr;

public interface ISdrClientDeviceRecord
{
    ushort DataMessageId { get; }
    ushort Index { get; }
    string Name { get; }
    IRxValue<AsvSdrCustomMode> RecordMode { get; }
    IRxValue<ulong> Frequency { get; }
    IRxValue<AsvSdrRecordStateFlag> State { get; }
    IRxValue<DateTime> Created { get; }
    IRxValue<ushort> TagsCount { get; }
    IRxValue<uint> DataCount { get; }
    IRxValue<uint> ByteSize { get; }
    IRxValue<TimeSpan> Duration { get; }
    IObservable<IChangeSet<SdrClientDeviceRecordTag, ushort>> Tags { get; }
    Task<bool> UploadTagList(IProgress<double>? progress = null, CancellationToken cancel = default);
    Task DeleteTags(ushort startIndex, ushort stopIndex, CancellationToken cancel = default);
    Task DeleteTag(ushort recordIndex, CancellationToken cancel);
}