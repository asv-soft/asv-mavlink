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

        public IObservable<AsvSdrRecordListRequestPayload> OnGetRecordList =>
            _connection.Filter<AsvSdrRecordListRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordList(Action<AsvSdrRecordListResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordListResponsePacket,AsvSdrRecordListResponsePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordReadRequestPayload> OnGetRecord =>
            _connection.Filter<AsvSdrRecordReadRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordPacket,AsvSdrRecordPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordDeleteRequestPayload> OnRecordDelete =>
            _connection.Filter<AsvSdrRecordDeleteRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordDelete(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordDeleteResponsePacket,AsvSdrRecordDeleteResponsePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordTagListRequestPayload> OnGetRecordTagList =>
            _connection.Filter<AsvSdrRecordTagListRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordTagList(Action<AsvSdrRecordTagListResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordTagListResponsePacket,AsvSdrRecordTagListResponsePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordTagReadRequestPayload> OnGetRecordTag =>
            _connection.Filter<AsvSdrRecordTagReadRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordTagPacket,AsvSdrRecordTagPayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDelete =>
            _connection.Filter<AsvSdrRecordTagDeleteRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);

        public Task SendRecordTagDelete(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordTagDeleteResponsePacket,AsvSdrRecordTagDeleteResponsePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordDataListRequestPayload> OnGetRecordDataList =>
            _connection.Filter<AsvSdrRecordDataListRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordDataList(Action<AsvSdrRecordDataListResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordDataListResponsePacket,AsvSdrRecordDataListResponsePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }

        public IObservable<AsvSdrRecordDataReadRequestPayload> OnGetRecordDataIls =>
            _connection.Filter<AsvSdrRecordDataReadRequestPacket>()
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

        public IObservable<AsvSdrRecordDataDeleteRequestPayload> OnRecordDataDelete  =>
            _connection.Filter<AsvSdrRecordDataDeleteRequestPacket>()
                .Where(_ => _.Payload.TargetComponent == _identity.ComponentId && _.Payload.TargetSystem == _identity.SystemId)
                .Select(_ => _.Payload);
        public Task SendRecordDataDelete(Action<AsvSdrRecordDataDeleteResponsePayload> setValueCallback, CancellationToken cancel = default)
        {
            return _connection.Send<AsvSdrRecordDataDeleteResponsePacket,AsvSdrRecordDataDeleteResponsePayload>(setValueCallback,_identity.SystemId,_identity.ComponentId,_seq,cancel);
        }
    }
}