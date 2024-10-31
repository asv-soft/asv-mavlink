using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class AsvSdrServerConfig
    {
        public int StatusRateMs { get; set; } = 1000;
    }
    public class AsvSdrServer: MavlinkMicroserviceServer, IAsvSdrServer
    {
        private readonly AsvSdrServerConfig _config;
        private readonly ILogger _logger;
        private readonly MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload> _transponder;

        public AsvSdrServer(MavlinkIdentity identity, AsvSdrServerConfig config, ICoreServices core) 
            : base("SDR", identity, core)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = core.Log.CreateLogger<AsvSdrServer>();
            _transponder =
                new MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload>(identity, core);
                    
            _transponder.Set(x =>
            {
                x.SignalOverflow = float.NaN;
                x.RefPower = float.NaN;
            });
            
            OnRecordRequest = InternalFilter<AsvSdrRecordRequestPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnRecordDeleteRequest = InternalFilter<AsvSdrRecordDeleteRequestPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnRecordTagRequest = InternalFilter<AsvSdrRecordTagRequestPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnGetRecordTag = InternalFilter<AsvSdrRecordTagRequestPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnRecordTagDeleteRequest = InternalFilter<AsvSdrRecordTagDeleteRequestPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnRecordDataRequest = InternalFilter<AsvSdrRecordDataRequestPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            
            OnCalibrationTableReadRequest = InternalFilter<AsvSdrCalibTableReadPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnCalibrationTableRowReadRequest = InternalFilter<AsvSdrCalibTableRowReadPacket>(p=>p.Payload.TargetSystem,p=>p.Payload.TargetComponent)
                .Select(p => p.Payload).Publish().RefCount();
            OnCalibrationTableUploadStart =
                InternalFilter<AsvSdrCalibTableUploadStartPacket>(p => p.Payload.TargetSystem,
                        p => p.Payload.TargetComponent)
                    .Publish().RefCount();
            
            
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(600),TimeSpan.FromMilliseconds(_config.StatusRateMs));
        }

        public void Set(Action<AsvSdrOutStatusPayload> changeCallback)
        {
            if (changeCallback == null) throw new ArgumentNullException(nameof(changeCallback));
            _transponder.Set(changeCallback);
        }

        public IObservable<AsvSdrRecordRequestPayload> OnRecordRequest { get; }
        public Task SendRecordResponse(Action<AsvSdrRecordResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
            return InternalSend<AsvSdrRecordResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default)
        {
            if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
            return InternalSend<AsvSdrRecordPacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordDeleteRequestPayload> OnRecordDeleteRequest { get; }
    
        public Task SendRecordDeleteResponse(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
            return InternalSend<AsvSdrRecordDeleteResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordTagRequestPayload> OnRecordTagRequest { get; }
        public Task SendRecordTagResponse(Action<AsvSdrRecordTagResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
            return InternalSend<AsvSdrRecordTagResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordTagRequestPayload> OnGetRecordTag { get; }
        public Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordTagPacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDeleteRequest { get; }
        public Task SendRecordTagDeleteResponse(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordTagDeleteResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordDataRequestPayload> OnRecordDataRequest { get; }
        public Task SendRecordDataResponse(Action<AsvSdrRecordDataResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordDataResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Task SendRecordData(AsvSdrCustomMode mode, Action<IPayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            if (mode == AsvSdrCustomMode.AsvSdrCustomModeIdle)
                throw new ArgumentException("Can't create message for IDLE mode", nameof(mode));
            return InternalSend((int)mode,v=>setValueCallback(v.Payload), cancel);
        }

        public IPacketV2<IPayload> CreateRecordData(AsvSdrCustomMode mode)
        {
            if (mode == AsvSdrCustomMode.AsvSdrCustomModeIdle)
                throw new ArgumentException("Can't create message for IDLE mode", nameof(mode));
            return Core.Connection.CreatePacketByMessageId((int)mode);
        }

        #region Calibration

        public Task SendSignal(Action<AsvSdrSignalRawPacket> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend(setValueCallback,cancel);
        }

        public Task SendCalibrationAcc(ushort reqId, AsvSdrRequestAck resultCode,
            CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrCalibAccPacket>(args =>
            {
                args.Payload.Result = resultCode;
                args.Payload.RequestId = reqId;
            }, cancel);
        }


        public IObservable<AsvSdrCalibTableReadPayload> OnCalibrationTableReadRequest { get; }
        public Task SendCalibrationTableReadResponse(Action<AsvSdrCalibTablePayload> setValueCallback, CancellationToken cancel = default)
        {
            if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
            return InternalSend<AsvSdrCalibTablePacket>(args => setValueCallback(args.Payload), cancel);
        }

        public IObservable<AsvSdrCalibTableRowReadPayload> OnCalibrationTableRowReadRequest { get; }
        public Task SendCalibrationTableRowReadResponse(Action<AsvSdrCalibTableRowPayload> setValueCallback, CancellationToken cancel = default)
        {
            if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
            return InternalSend<AsvSdrCalibTableRowPacket>(args =>
            {
                setValueCallback(args.Payload);
            }, cancel);
        }

        public IObservable<AsvSdrCalibTableUploadStartPacket> OnCalibrationTableUploadStart { get; }
        public Task<CalibrationTableRow> CallCalibrationTableUploadReadCallback(byte targetSysId, byte targetCompId, ushort reqId, ushort tableIndex, ushort rowIndex , CancellationToken cancel = default)
        {
            return InternalCall<CalibrationTableRow, AsvSdrCalibTableUploadReadCallbackPacket, AsvSdrCalibTableRowPacket>(
                x =>
                {
                    x.Payload.TableIndex = tableIndex;
                    x.Payload.RequestId = reqId;
                    x.Payload.TargetComponent = targetCompId;
                    x.Payload.TargetSystem = targetSysId;
                    x.Payload.RowIndex = rowIndex;
                },p=>p.Payload.TargetSystem, p=>p.Payload.TargetComponent,p=> p.Payload.RowIndex == rowIndex && p.Payload.TableIndex == tableIndex, p=>new CalibrationTableRow(p.Payload) , cancel:DisposeCancel);
            
        }

        #endregion

        public override void Dispose()
        {
            _transponder.Dispose();
            base.Dispose();
        }
    }
}