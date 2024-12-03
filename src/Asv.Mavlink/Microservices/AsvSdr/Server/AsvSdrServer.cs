using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;

using Microsoft.Extensions.Logging;
using R3;

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
        private readonly MavlinkPacketTransponder<AsvSdrOutStatusPacket> _transponder;
        private readonly Subject<AsvSdrRecordRequestPayload> _onRecordRequest;
        private readonly Subject<AsvSdrRecordDeleteRequestPayload> _onRecordDeleteRequest;
        private readonly Subject<AsvSdrRecordTagRequestPayload> _onRecordTagRequest;
        private readonly Subject<AsvSdrRecordTagRequestPayload> _onGetRecordTag;
        private readonly Subject<AsvSdrRecordTagDeleteRequestPayload> _onRecordTagDeleteRequest;
        private readonly Subject<AsvSdrRecordDataRequestPayload> _onRecordDataRequest;
        private readonly Subject<AsvSdrCalibTableReadPayload> _onCalibrationTableReadRequest;
        private readonly Subject<AsvSdrCalibTableRowReadPayload> _onCalibrationTableRowReadRequest;
        private readonly Subject<AsvSdrCalibTableUploadStartPacket> _onCalibrationTableUploadStart;
        private readonly IDisposable _sub1;
        private readonly IDisposable _sub2;
        private readonly IDisposable _sub3;
        private readonly IDisposable _sub4;
        private readonly IDisposable _sub5;
        private readonly IDisposable _sub6;
        private readonly IDisposable _sub7;
        private readonly IDisposable _sub8;
        private readonly IDisposable _sub9;

        public AsvSdrServer(MavlinkIdentity identity, AsvSdrServerConfig config, ICoreServices core) 
            : base("SDR", identity, core)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = core.LoggerFactory.CreateLogger<AsvSdrServer>();
            _transponder =
                new MavlinkPacketTransponder<AsvSdrOutStatusPacket>(identity, core);
                    
            _transponder.Set(x =>
            {
                x.Payload.SignalOverflow = float.NaN;
                x.Payload.RefPower = float.NaN;
            });

            _onRecordRequest = new Subject<AsvSdrRecordRequestPayload>();   
            _sub1 = InternalFilter<AsvSdrRecordRequestPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onRecordRequest.AsObserver());

            _onRecordDeleteRequest = new Subject<AsvSdrRecordDeleteRequestPayload>();
            _sub2 = InternalFilter<AsvSdrRecordDeleteRequestPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onRecordDeleteRequest.AsObserver());

            _onRecordTagRequest = new Subject<AsvSdrRecordTagRequestPayload>();
            _sub3 = InternalFilter<AsvSdrRecordTagRequestPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onRecordTagRequest.AsObserver());

            _onGetRecordTag = new Subject<AsvSdrRecordTagRequestPayload>();
            _sub4 = InternalFilter<AsvSdrRecordTagRequestPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onGetRecordTag.AsObserver());

            _onRecordTagDeleteRequest = new Subject<AsvSdrRecordTagDeleteRequestPayload>();
            _sub5 = InternalFilter<AsvSdrRecordTagDeleteRequestPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onRecordTagDeleteRequest.AsObserver());

            _onRecordDataRequest = new Subject<AsvSdrRecordDataRequestPayload>();
            _sub6 = InternalFilter<AsvSdrRecordDataRequestPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onRecordDataRequest.AsObserver());

            _onCalibrationTableReadRequest = new Subject<AsvSdrCalibTableReadPayload>();
            _sub7 = InternalFilter<AsvSdrCalibTableReadPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Select(p => p.Payload)
                .Subscribe(_onCalibrationTableReadRequest.AsObserver());
    
            _onCalibrationTableRowReadRequest = new Subject<AsvSdrCalibTableRowReadPayload>();
            _sub8 = InternalFilter<AsvSdrCalibTableRowReadPacket>(p => p.Payload.TargetSystem,
                        p => p.Payload.TargetComponent)
                    .Select(p => p.Payload).Subscribe(_onCalibrationTableRowReadRequest.AsObserver());
            _onCalibrationTableUploadStart = new Subject<AsvSdrCalibTableUploadStartPacket>();
            _sub9 = InternalFilter<AsvSdrCalibTableUploadStartPacket>(p => p.Payload.TargetSystem,
                    p => p.Payload.TargetComponent)
                .Subscribe(_onCalibrationTableUploadStart.AsObserver());


        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(_config.StatusRateMs),TimeSpan.FromMilliseconds(_config.StatusRateMs));
        }

        public void Set(Action<AsvSdrOutStatusPayload> changeCallback)
        {
            ArgumentNullException.ThrowIfNull(changeCallback);
            _transponder.Set(x=>changeCallback(x.Payload));
        }

        public Observable<AsvSdrRecordRequestPayload> OnRecordRequest => _onRecordRequest;

        public ValueTask SendRecordResponse(Action<AsvSdrRecordResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public ValueTask SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordPacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Observable<AsvSdrRecordDeleteRequestPayload> OnRecordDeleteRequest => _onRecordDeleteRequest;

        public ValueTask SendRecordDeleteResponse(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordDeleteResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Observable<AsvSdrRecordTagRequestPayload> OnRecordTagRequest => _onRecordTagRequest;

        public ValueTask SendRecordTagResponse(Action<AsvSdrRecordTagResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordTagResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Observable<AsvSdrRecordTagRequestPayload> OnGetRecordTag => _onGetRecordTag;

        public ValueTask SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordTagPacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Observable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDeleteRequest => _onRecordTagDeleteRequest;

        public ValueTask SendRecordTagDeleteResponse(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordTagDeleteResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public Observable<AsvSdrRecordDataRequestPayload> OnRecordDataRequest => _onRecordDataRequest;

        public ValueTask SendRecordDataResponse(Action<AsvSdrRecordDataResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrRecordDataResponsePacket>(p =>{ setValueCallback(p.Payload); }, cancel);
        }

        public ValueTask SendRecordData(AsvSdrCustomMode mode, Action<IPayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            if (mode == AsvSdrCustomMode.AsvSdrCustomModeIdle)
                throw new ArgumentException("Can't create message for IDLE mode", nameof(mode));
            return InternalSend((int)mode,v=>setValueCallback(v.Payload), cancel);
        }

        public MavlinkMessage? CreateRecordData(AsvSdrCustomMode mode)
        {
            if (mode == AsvSdrCustomMode.AsvSdrCustomModeIdle)
                throw new ArgumentException("Can't create message for IDLE mode", nameof(mode));
            return MavlinkV2MessageFactory.Instance.Create((ushort)mode);
        }

        #region Calibration

        public ValueTask SendSignal(Action<AsvSdrSignalRawPacket> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend(setValueCallback,cancel);
        }

        public ValueTask SendCalibrationAcc(ushort reqId, AsvSdrRequestAck resultCode,
            CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrCalibAccPacket>(args =>
            {
                args.Payload.Result = resultCode;
                args.Payload.RequestId = reqId;
            }, cancel);
        }


        public Observable<AsvSdrCalibTableReadPayload> OnCalibrationTableReadRequest => _onCalibrationTableReadRequest;

        public ValueTask SendCalibrationTableReadResponse(Action<AsvSdrCalibTablePayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrCalibTablePacket>(args => setValueCallback(args.Payload), cancel);
        }

        public Observable<AsvSdrCalibTableRowReadPayload> OnCalibrationTableRowReadRequest => _onCalibrationTableRowReadRequest;

        public ValueTask SendCalibrationTableRowReadResponse(Action<AsvSdrCalibTableRowPayload> setValueCallback, CancellationToken cancel = default)
        {
            ArgumentNullException.ThrowIfNull(setValueCallback);
            return InternalSend<AsvSdrCalibTableRowPacket>(args =>
            {
                setValueCallback(args.Payload);
            }, cancel);
        }

        public Observable<AsvSdrCalibTableUploadStartPacket> OnCalibrationTableUploadStart => _onCalibrationTableUploadStart;

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

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transponder.Dispose();
                _onRecordRequest.Dispose();
                _onRecordDeleteRequest.Dispose();
                _onRecordTagRequest.Dispose();
                _onGetRecordTag.Dispose();
                _onRecordTagDeleteRequest.Dispose();
                _onRecordDataRequest.Dispose();
                _onCalibrationTableReadRequest.Dispose();
                _onCalibrationTableRowReadRequest.Dispose();
                _onCalibrationTableUploadStart.Dispose();
                _sub1.Dispose();
                _sub2.Dispose();
                _sub3.Dispose();
                _sub4.Dispose();
                _sub5.Dispose();
                _sub6.Dispose();
                _sub7.Dispose();
                _sub8.Dispose();
                _sub9.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_transponder).ConfigureAwait(false);
            await CastAndDispose(_onRecordRequest).ConfigureAwait(false);
            await CastAndDispose(_onRecordDeleteRequest).ConfigureAwait(false);
            await CastAndDispose(_onRecordTagRequest).ConfigureAwait(false);
            await CastAndDispose(_onGetRecordTag).ConfigureAwait(false);
            await CastAndDispose(_onRecordTagDeleteRequest).ConfigureAwait(false);
            await CastAndDispose(_onRecordDataRequest).ConfigureAwait(false);
            await CastAndDispose(_onCalibrationTableReadRequest).ConfigureAwait(false);
            await CastAndDispose(_onCalibrationTableRowReadRequest).ConfigureAwait(false);
            await CastAndDispose(_onCalibrationTableUploadStart).ConfigureAwait(false);
            await CastAndDispose(_sub1).ConfigureAwait(false);
            await CastAndDispose(_sub2).ConfigureAwait(false);
            await CastAndDispose(_sub3).ConfigureAwait(false);
            await CastAndDispose(_sub4).ConfigureAwait(false);
            await CastAndDispose(_sub5).ConfigureAwait(false);
            await CastAndDispose(_sub6).ConfigureAwait(false);
            await CastAndDispose(_sub7).ConfigureAwait(false);
            await CastAndDispose(_sub8).ConfigureAwait(false);
            await CastAndDispose(_sub9).ConfigureAwait(false);

            await base.DisposeAsyncCore().ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }

        #endregion

    }
}