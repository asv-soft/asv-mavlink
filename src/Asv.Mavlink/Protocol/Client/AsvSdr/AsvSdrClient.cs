using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrClient : MavlinkMicroserviceClient, IAsvSdrClient
{
    private readonly MavlinkClientIdentity _identity;
    private readonly RxValue<AsvSdrOutStatusPayload> _status;

    public AsvSdrClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, IScheduler scheduler)
        : base(connection, identity, seq, "SDR", scheduler)
    {
        _identity = identity;
        _status = new RxValue<AsvSdrOutStatusPayload>().DisposeItWith(Disposable);
        Filter<AsvSdrOutStatusPacket>().Select(_ => _.Payload).Subscribe(_status).DisposeItWith(Disposable);
    }

    public IRxValue<AsvSdrOutStatusPayload> Status => _status;

    public IObservable<AsvSdrStoragePayload> OnStorage => Filter<AsvSdrStoragePacket>().Select(_ => _.Payload);

    public Task<AsvSdrStoragePayload> GetStorage(CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrStoragePayload, AsvSdrStorageRequestReadPacket, AsvSdrStoragePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordPayload> OnRecord => Filter<AsvSdrRecordPacket>().Select(_ => _.Payload);


    public Task<AsvSdrRecordResponseListPayload> GetRecordList(CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordResponseListPayload, AsvSdrRecordRequestListPacket, AsvSdrRecordResponseListPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordPayload> GetRecord(ushort recordIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordPayload, AsvSdrRecordRequestReadPacket, AsvSdrRecordPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
        },_=>_.Payload.Index == recordIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordTagPayload> OnRecordTag => Filter<AsvSdrRecordTagPacket>().Select(_ => _.Payload);

    public Task<AsvSdrRecordTagResponseListPayload> GetRecordTagList(ushort recordIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordTagResponseListPayload, AsvSdrRecordTagRequestListPacket, AsvSdrRecordTagResponseListPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagPayload> GetRecordTag(ushort recordIndex, ushort tagIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordTagPayload, AsvSdrRecordTagRequestReadPacket, AsvSdrRecordTagPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.TagIndex = tagIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.TagIndex == tagIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordDataResponseListPayload> GetRecordDataList(ushort recordIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataResponseListPayload, AsvSdrRecordDataRequestListPacket, AsvSdrRecordDataResponseListPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordDataIlsPayload> OnRecordDataIls => Filter<AsvSdrRecordDataIlsPacket>().Select(_ => _.Payload);

    public Task<AsvSdrRecordDataIlsPayload> GetRecordDataIls(ushort recordIndex, uint dataIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataIlsPayload, AsvSdrRecordDataRequestReadPacket, AsvSdrRecordDataIlsPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.DataIndex = dataIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.DataIndex == dataIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordDataVorPayload> OnRecordDataVor => Filter<AsvSdrRecordDataVorPacket>().Select(_ => _.Payload);
    public Task<AsvSdrRecordDataVorPayload> GetRecordDataVor(ushort recordIndex, uint dataIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataVorPayload, AsvSdrRecordDataRequestReadPacket, AsvSdrRecordDataVorPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.DataIndex = dataIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.DataIndex == dataIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }
}