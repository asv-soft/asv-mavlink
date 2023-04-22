using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using DynamicData;

namespace Asv.Mavlink;

public interface IAsvSdrClientRecord
{
    RecordId Id { get; }
    AsvSdrCustomMode RecordMode { get; }
    IRxValue<ulong> Frequency { get; }
    IRxValue<DateTime> Created { get; }
    IRxValue<ushort> TagsCount { get; }
    IRxValue<uint> DataCount { get; }
    IRxValue<uint> ByteSize { get; }
    IRxValue<TimeSpan> Duration { get; }
    IObservable<IChangeSet<AsvSdrClientRecordTag, TagId>> Tags { get; }
    Task<bool> UploadTagList(IProgress<double> progress = null, CancellationToken cancel = default);
    Task DeleteTag(TagId id, CancellationToken cancel);
}