using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public interface IAsvSdrServer
    {
        void Start(TimeSpan statusRate);
        void Set(Action<AsvSdrOutStatusPayload> changeCallback);
        IObservable<AsvSdrRecordListRequestPayload> OnGetRecordList { get; }
        Task SendRecordList(Action<AsvSdrRecordListResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordReadRequestPayload> OnGetRecord { get; }
        Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDeleteRequestPayload> OnRecordDelete { get; }
        Task SendRecordDelete(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagListRequestPayload> OnGetRecordTagList { get; }
        Task SendRecordTagList(Action<AsvSdrRecordTagListResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagReadRequestPayload> OnGetRecordTag { get; }
        Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDelete { get; }
        Task SendRecordTagDelete(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataListRequestPayload> OnGetRecordDataList { get; }
        Task SendRecordDataList(Action<AsvSdrRecordDataListResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataReadRequestPayload> OnGetRecordDataIls { get; }
        Task SendRecordDataIls(Action<AsvSdrRecordDataIlsPayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordDataVor(Action<AsvSdrRecordDataVorPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataDeleteRequestPayload> OnRecordDataDelete { get; }
        Task SendRecordDataDelete(Action<AsvSdrRecordDataDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        
    }
}