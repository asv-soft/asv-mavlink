using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public interface IAsvSdrServer
    {
        void Start();
        void Set(Action<AsvSdrOutStatusPayload> changeCallback);
        IObservable<AsvSdrRecordRequestPayload> OnGetRecordList { get; }
        Task SendRecordList(Action<AsvSdrRecordResponsePayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDeleteRequestPayload> OnRecordDelete { get; }
        Task SendRecordDelete(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagRequestPayload> OnGetRecordTagList { get; }
        Task SendRecordTagList(Action<AsvSdrRecordTagResponsePayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDelete { get; }
        Task SendRecordTagDelete(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataRequestPayload> OnGetRecordDataList { get; }
        Task SendRecordDataList(Action<AsvSdrRecordDataResponsePayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordDataIls(Action<AsvSdrRecordDataIlsPayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordDataVor(Action<AsvSdrRecordDataVorPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataDeleteRequestPayload> OnRecordDataDelete { get; }
        Task SendRecordDataDelete(Action<AsvSdrRecordDataDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        
    }
}