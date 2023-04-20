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
    IObservable<AsvSdrRecordPayload> OnRecord { get; }
    Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordTagPayload> OnRecordTag { get; }
    Task<AsvSdrRecordDeleteResponsePayload> DeleteRecords(ushort startIndex,ushort stopIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(ushort recordIndex,ushort startTagIndex, ushort stopTagIndex,CancellationToken cancel=default);
    Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTags(ushort recordIndex,ushort startIndex,ushort stopIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(ushort recordIndex,ushort startDataIndex, ushort stopDataIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordDataIlsPayload> OnRecordDataIls { get; }
    IObservable<AsvSdrRecordDataVorPayload> OnRecordDataVor { get; }
    Task<AsvSdrRecordDataDeleteResponsePayload> DeleteRecordData(ushort recordIndex,ushort startDataIndex,ushort stopDataIndex, CancellationToken cancel=default);
    
}

