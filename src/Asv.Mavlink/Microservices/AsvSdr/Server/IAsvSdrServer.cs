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
        Task SendRecordData(AsvSdrCustomMode mode, Action<IPayload> setValueCallback, CancellationToken cancel = default);
        IPacketV2<IPayload> CreateRecordData(AsvSdrCustomMode mode);
        
    }

    public static class AsvSdrServerHelper
    {
        public static Task SendRecordResponseFail(this IAsvSdrServer src, AsvSdrRecordRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return src.SendRecordResponse(x =>
            {
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        public static Task SendRecordResponseSuccess(this IAsvSdrServer src, AsvSdrRecordRequestPayload request,
            ushort recordsCount)
        {
            return src.SendRecordResponse(x =>
            {
                x.ItemsCount = recordsCount;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        public static Task SendRecordTagResponseFail(this IAsvSdrServer src, AsvSdrRecordTagRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return src.SendRecordTagResponse(x =>
            {
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        public static Task SendRecordTagResponseSuccess(this IAsvSdrServer src, AsvSdrRecordTagRequestPayload request,
            ushort recordsCount)
        {
            return src.SendRecordTagResponse(x =>
            {
                x.ItemsCount = recordsCount;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }

        public static Task SendRecordDeleteResponseFail(this IAsvSdrServer src, AsvSdrRecordDeleteRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return src.SendRecordDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        public static Task SendRecordDeleteResponseSuccess(this IAsvSdrServer src, AsvSdrRecordDeleteRequestPayload request)
        {
            return src.SendRecordDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        
        public static Task SendRecordTagDeleteResponseFail(this IAsvSdrServer src, AsvSdrRecordTagDeleteRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return src.SendRecordTagDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.TagGuid.CopyTo(x.TagGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        
        public static Task SendRecordTagDeleteResponseSuccess(this IAsvSdrServer src, AsvSdrRecordTagDeleteRequestPayload request)
        {
           
            return src.SendRecordTagDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.TagGuid.CopyTo(x.TagGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        
        public static Task SendRecordDataResponseFail(this IAsvSdrServer src, AsvSdrRecordDataRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return src.SendRecordDataResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.Count = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        
        public static Task SendRecordDataResponseSuccess(this IAsvSdrServer src, AsvSdrRecordDataRequestPayload request,
            uint count)
        {
            return src.SendRecordDataResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.Count = count;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        
          
        
    }
}