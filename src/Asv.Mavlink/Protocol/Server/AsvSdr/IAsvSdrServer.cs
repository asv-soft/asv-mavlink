using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public interface IAsvSdrServer
    {
        void Start(TimeSpan statusRate);
        void Set(Action<AsvSdrOutStatusPayload> changeCallback);
        IObservable<AsvSdrStorageRequestReadPayload> OnGetStorage { get; }
        Task SendStorage(Action<AsvSdrStoragePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordRequestListPayload> OnGetRecordList { get; }
        Task SendRecordList(Action<AsvSdrRecordResponseListPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordRequestReadPayload> OnGetRecord { get; }
        Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagRequestListPayload> OnGetRecordTagList { get; }
        Task SendRecordTagList(Action<AsvSdrRecordTagResponseListPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagRequestReadPayload> OnGetRecordTag { get; }
        Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataRequestListPayload> OnGetRecordDataList { get; }
        Task SendRecordDataList(Action<AsvSdrRecordDataResponseListPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataRequestReadPayload> OnGetRecordDataIls { get; }
        Task SendRecordDataIls(Action<AsvSdrRecordDataIlsPayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordDataVor(Action<AsvSdrRecordDataVorPayload> setValueCallback, CancellationToken cancel = default);
    }
}