using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;
/// <summary>
/// Implementation of SDR payload client mavlink interface ()
/// </summary>
public interface IAsvSdrClient
{
    IRxValue<AsvSdrOutStatusPayload> Status { get; }
    IObservable<(RecordId,AsvSdrRecordPayload)> OnRecord { get; }
    Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex, CancellationToken cancel=default);
    IObservable<(TagId,AsvSdrRecordTagPayload)> OnRecordTag { get; }
    IObservable<(RecordId,AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }
    Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(RecordId recordName, CancellationToken cancel = default);
    Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(RecordId recordName, ushort skip, ushort count, CancellationToken cancel = default);
    IObservable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }
    Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tag, CancellationToken cancel = default);
    Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(RecordId recordName, uint skip, uint count, CancellationToken cancel = default);
    IObservable<IPacketV2<IPayload>> OnRecordData { get; }
}

public static class AsvSdrClientExtensions
{
    
}



