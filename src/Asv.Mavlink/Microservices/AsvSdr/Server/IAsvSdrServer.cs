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
        IObservable<AsvSdrRecordRequestPayload> OnRecordRequest { get; }
        Task SendRecordResponse(Action<AsvSdrRecordResponsePayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDeleteRequestPayload> OnRecordDeleteRequest { get; }
        Task SendRecordDeleteResponse(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagRequestPayload> OnRecordTagRequest { get; }
        Task SendRecordTagResponse(Action<AsvSdrRecordTagResponsePayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDeleteRequest { get; }
        Task SendRecordTagDeleteResponse(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrRecordDataRequestPayload> OnRecordDataRequest { get; }
        Task SendRecordDataResponse(Action<AsvSdrRecordDataResponsePayload> setValueCallback, CancellationToken cancel = default);
        Task SendRecordData<TPacket>(Action<TPacket> setValueCallback, CancellationToken cancel = default)
            where TPacket :IPacketV2<IPayload>, new();
 
    }
}