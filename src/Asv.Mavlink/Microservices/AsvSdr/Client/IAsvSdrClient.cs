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
    Task<AsvSdrRecordListResponsePayload> GetRecordList(CancellationToken cancel=default);
    Task<AsvSdrRecordPayload> GetRecord(ushort recordIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordTagPayload> OnRecordTag { get; }
    Task<AsvSdrRecordDeleteResponsePayload> DeleteRecords(ushort startIndex,ushort stopIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordTagListResponsePayload> GetRecordTagList(ushort recordIndex,CancellationToken cancel=default);
    Task<AsvSdrRecordTagPayload> GetRecordTag(ushort recordIndex,ushort tagIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordDataListResponsePayload> GetRecordDataList(ushort recordIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTags(ushort recordIndex,ushort startIndex,ushort stopIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordDataIlsPayload> OnRecordDataIls { get; }
    Task<AsvSdrRecordDataIlsPayload> GetRecordDataIls(ushort recordIndex,uint dataIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordDataVorPayload> OnRecordDataVor { get; }
    Task<AsvSdrRecordDataVorPayload> GetRecordDataVor(ushort recordIndex,uint dataIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordDataDeleteResponsePayload> DeleteRecordData(ushort recordIndex,ushort startIndex,ushort stopIndex, CancellationToken cancel=default);
    
}

