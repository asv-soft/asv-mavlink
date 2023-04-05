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
    IObservable<AsvSdrStoragePayload> OnStorage { get; }
    Task<AsvSdrStoragePayload> GetStorage(CancellationToken cancel=default);
    IObservable<AsvSdrRecordPayload> OnRecord { get; }
    Task<AsvSdrRecordResponseListPayload> GetRecordList(CancellationToken cancel=default);
    Task<AsvSdrRecordPayload> GetRecord(ushort recordIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordTagPayload> OnRecordTag { get; }
    Task<AsvSdrRecordTagResponseListPayload> GetRecordTagList(ushort recordIndex,CancellationToken cancel=default);
    Task<AsvSdrRecordTagPayload> GetRecordTag(ushort recordIndex,ushort tagIndex, CancellationToken cancel=default);
    Task<AsvSdrRecordDataResponseListPayload> GetRecordDataList(ushort recordIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordDataIlsPayload> OnRecordDataIls { get; }
    Task<AsvSdrRecordDataIlsPayload> GetRecordDataIls(ushort recordIndex,uint dataIndex, CancellationToken cancel=default);
    IObservable<AsvSdrRecordDataVorPayload> OnRecordDataVor { get; }
    Task<AsvSdrRecordDataVorPayload> GetRecordDataVor(ushort recordIndex,uint dataIndex, CancellationToken cancel=default);
    
}

