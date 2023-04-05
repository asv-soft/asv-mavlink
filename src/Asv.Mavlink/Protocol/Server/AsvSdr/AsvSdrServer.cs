using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public class AsvSdrServer:DisposableOnceWithCancel, IAsvSdrServer
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkServerIdentity _identity;
        private readonly MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload> _transponder;

        public AsvSdrServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,MavlinkServerIdentity identity)
        {
            _connection = connection;
            
            _seq = seq;
            _identity = identity;
            _transponder =
                new MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload>(connection, identity, seq)
                    .DisposeItWith(Disposable);
        }

        public void Start(TimeSpan statusRate)
        {
            _transponder.Start(statusRate);
        }

        public void Set(Action<AsvSdrOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

        public IObservable<AsvSdrStorageRequestReadPayload> OnGetStorage =>
            _connection.Filter<AsvSdrStorageRequestReadPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendStorage(Action<AsvSdrStoragePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrStoragePacket,AsvSdrStoragePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordRequestListPayload> OnGetRecordList =>
            _connection.Filter<AsvSdrRecordRequestListPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordList(Action<AsvSdrRecordResponseListPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordResponseListPacket,AsvSdrRecordResponseListPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordRequestReadPayload> OnGetRecord =>
            _connection.Filter<AsvSdrRecordRequestReadPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordPacket,AsvSdrRecordPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordTagRequestListPayload> OnGetRecordTagList =>
            _connection.Filter<AsvSdrRecordTagRequestListPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordTagList(Action<AsvSdrRecordTagResponseListPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordTagResponseListPacket,AsvSdrRecordTagResponseListPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordTagRequestReadPayload> OnGetRecordTag =>
            _connection.Filter<AsvSdrRecordTagRequestReadPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordTagPacket,AsvSdrRecordTagPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordDataRequestListPayload> OnGetRecordDataList =>
            _connection.Filter<AsvSdrRecordDataRequestListPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordDataList(Action<AsvSdrRecordDataResponseListPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordDataResponseListPacket,AsvSdrRecordDataResponseListPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordDataRequestReadPayload> OnGetRecordDataIls =>
            _connection.Filter<AsvSdrRecordDataRequestReadPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordDataIls(Action<AsvSdrRecordDataIlsPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordDataIlsPacket,AsvSdrRecordDataIlsPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public Task SendRecordDataVor(Action<AsvSdrRecordDataVorPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordDataVorPacket,AsvSdrRecordDataVorPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }
    }
}