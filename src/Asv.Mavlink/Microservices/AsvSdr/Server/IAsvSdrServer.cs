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
        Task SendSignal(Action<AsvSdrSignalRawPacket> setValueCallback, CancellationToken cancel = default);
        
        #region Calibration
        
        Task SendCalibrationAcc(ushort reqId, AsvSdrRequestAck resultCode , CancellationToken cancel = default);
        IObservable<AsvSdrCalibTableReadPayload> OnCalibrationTableReadRequest { get; }
        Task SendCalibrationTableReadResponse(Action<AsvSdrCalibTablePayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrCalibTableRowReadPayload> OnCalibrationTableRowReadRequest { get; }
        Task SendCalibrationTableRowReadResponse(Action<AsvSdrCalibTableRowPayload> setValueCallback, CancellationToken cancel = default);
        IObservable<AsvSdrCalibTableUploadStartPacket> OnCalibrationTableUploadStart { get; }
        Task<CalibrationTableRow> CallCalibrationTableUploadReadCallback(byte targetSysId, byte targetCompId, ushort reqId, ushort tableIndex, ushort rowIndex , CancellationToken cancel = default);
        
        #endregion
        
        public Task SendRecordResponseFail( AsvSdrRecordRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordResponse(x =>
            {
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        public Task SendRecordResponseSuccess( AsvSdrRecordRequestPayload request,
            ushort recordsCount)
        {
            return SendRecordResponse(x =>
            {
                x.ItemsCount = recordsCount;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        public Task SendRecordTagResponseFail( AsvSdrRecordTagRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordTagResponse(x =>
            {
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        public Task SendRecordTagResponseSuccess( AsvSdrRecordTagRequestPayload request,
            ushort recordsCount)
        {
            return SendRecordTagResponse(x =>
            {
                x.ItemsCount = recordsCount;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }

        public Task SendRecordDeleteResponseFail( AsvSdrRecordDeleteRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        public Task SendRecordDeleteResponseSuccess( AsvSdrRecordDeleteRequestPayload request)
        {
            return SendRecordDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        
        public Task SendRecordTagDeleteResponseFail( AsvSdrRecordTagDeleteRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordTagDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.TagGuid.CopyTo(x.TagGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        
        public Task SendRecordTagDeleteResponseSuccess( AsvSdrRecordTagDeleteRequestPayload request)
        {
           
            return SendRecordTagDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.TagGuid.CopyTo(x.TagGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
        
        public Task SendRecordDataResponseFail( AsvSdrRecordDataRequestPayload request,
            AsvSdrRequestAck resultCode)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordDataResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, CancellationToken.None);
        }
        
        public Task SendRecordDataResponseSuccess( AsvSdrRecordDataRequestPayload request,
            uint count)
        {
            return SendRecordDataResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.ItemsCount = count;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, CancellationToken.None);
        }
    }

}