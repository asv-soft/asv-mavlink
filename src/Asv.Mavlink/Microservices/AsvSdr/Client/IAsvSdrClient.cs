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
    IObservable<(Guid,AsvSdrRecordPayload)> OnRecord { get; }
    Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex, CancellationToken cancel=default);
    IObservable<(TagId,AsvSdrRecordTagPayload)> OnRecordTag { get; }
    IObservable<(Guid,AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }
    Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(Guid recordId, CancellationToken cancel = default);
    Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(Guid recordId, ushort skip, ushort count, CancellationToken cancel = default);
    IObservable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }
    Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tag, CancellationToken cancel = default);
    Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(Guid recordId, uint skip, uint count, CancellationToken cancel = default);
    IObservable<IPacketV2<IPayload>> OnRecordData { get; }
    IObservable<AsvSdrSignalRawPayload> OnSignal { get; }
}

public static class AsvSdrClientExtensions
{
    
}



