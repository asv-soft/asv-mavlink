using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public class AsvSdrServerConfig
    {
        public int StatusRateMs { get; set; } = 1000;
    }
    public class AsvSdrServer: MavlinkMicroserviceServer, IAsvSdrServer
    {
        private readonly AsvSdrServerConfig _config;
        private readonly MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload> _transponder;

        public AsvSdrServer(IMavlinkV2Connection connection,
            MavlinkServerIdentity identity,AsvSdrServerConfig config, IPacketSequenceCalculator seq,
            IScheduler rxScheduler) 
            : base("SDR", connection, identity, seq, rxScheduler)
        {
            _config = config;
            _transponder =
                new MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload>(connection, identity, seq)
                    .DisposeItWith(Disposable);
            OnGetRecordList = InternalFilter<AsvSdrRecordListRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnGetRecord = InternalFilter<AsvSdrRecordReadRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnRecordDelete = InternalFilter<AsvSdrRecordDeleteRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnGetRecordTagList = InternalFilter<AsvSdrRecordTagListRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnGetRecordTag = InternalFilter<AsvSdrRecordTagReadRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnRecordTagDelete = InternalFilter<AsvSdrRecordTagDeleteRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnGetRecordDataList = InternalFilter<AsvSdrRecordDataListRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnGetRecordDataIls = InternalFilter<AsvSdrRecordDataReadRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
            OnRecordDataDelete = InternalFilter<AsvSdrRecordDataDeleteRequestPacket>(_=>_.Payload.TargetSystem,_=>_.Payload.TargetComponent)
                .Select(_ => _.Payload).Publish().RefCount();
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(_config.StatusRateMs));
        }

        public void Set(Action<AsvSdrOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

        public IObservable<AsvSdrRecordListRequestPayload> OnGetRecordList { get; }
        public Task SendRecordList(Action<AsvSdrRecordListResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordListResponsePacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordReadRequestPayload> OnGetRecord { get; }
        public Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordPacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordDeleteRequestPayload> OnRecordDelete { get; }
    
        public Task SendRecordDelete(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordDeleteResponsePacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordTagListRequestPayload> OnGetRecordTagList { get; }
        public Task SendRecordTagList(Action<AsvSdrRecordTagListResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordTagListResponsePacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordTagReadRequestPayload> OnGetRecordTag { get; }
        public Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordTagPacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDelete { get; }
        public Task SendRecordTagDelete(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordTagDeleteResponsePacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordDataListRequestPayload> OnGetRecordDataList { get; }
        public Task SendRecordDataList(Action<AsvSdrRecordDataListResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordDataListResponsePacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordDataReadRequestPayload> OnGetRecordDataIls { get; }
        public Task SendRecordDataIls(Action<AsvSdrRecordDataIlsPayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordDataIlsPacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public Task SendRecordDataVor(Action<AsvSdrRecordDataVorPayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordDataVorPacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }

        public IObservable<AsvSdrRecordDataDeleteRequestPayload> OnRecordDataDelete { get; }
        public Task SendRecordDataDelete(Action<AsvSdrRecordDataDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return InternalSend<AsvSdrRecordDataDeleteResponsePacket>(_ =>{ setValueCallback(_.Payload); }, cancel);
        }
    }
}