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
        InternalFilter<AsvSdrOutStatusPacket>().Select(_ => _.Payload).Subscribe(_status).DisposeItWith(Disposable);
    }

    public IRxValue<AsvSdrOutStatusPayload> Status => _status;

    public IObservable<AsvSdrRecordPayload> OnRecord => InternalFilter<AsvSdrRecordPacket>().Select(_ => _.Payload);

    public Task<AsvSdrRecordListResponsePayload> GetRecordList(CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordListResponsePayload, AsvSdrRecordListRequestPacket, AsvSdrRecordListResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }
    public Task<AsvSdrRecordPayload> GetRecord(ushort recordIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordPayload, AsvSdrRecordReadRequestPacket, AsvSdrRecordPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
        },_=>_.Payload.Index == recordIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }
    public IObservable<AsvSdrRecordTagPayload> OnRecordTag => InternalFilter<AsvSdrRecordTagPacket>().Select(_ => _.Payload);
    public Task<AsvSdrRecordDeleteResponsePayload> DeleteRecords(ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDeleteResponsePayload, AsvSdrRecordDeleteRequestPacket, AsvSdrRecordDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordStartIndex = startIndex;
            _.Payload.RecordStopIndex = stopIndex;
        },_=> _.Payload.RecordStartIndex == startIndex && _.Payload.RecordStopIndex == stopIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }
    public Task<AsvSdrRecordTagListResponsePayload> GetRecordTagList(ushort recordIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordTagListResponsePayload, AsvSdrRecordTagListRequestPacket, AsvSdrRecordTagListResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagPayload> GetRecordTag(ushort recordIndex, ushort tagIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordTagPayload, AsvSdrRecordTagReadRequestPacket, AsvSdrRecordTagPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.TagIndex = tagIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.TagIndex == tagIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordDataListResponsePayload> GetRecordDataList(ushort recordIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataListResponsePayload, AsvSdrRecordDataListRequestPacket, AsvSdrRecordDataListResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
        },_=>true,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTags(ushort recordIndex, ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordTagDeleteResponsePayload, AsvSdrRecordTagDeleteRequestPacket, AsvSdrRecordTagDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.TagStartIndex = startIndex;
            _.Payload.TagStopIndex = stopIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.TagStartIndex == startIndex && _.Payload.TagStopIndex == stopIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordDataIlsPayload> OnRecordDataIls => InternalFilter<AsvSdrRecordDataIlsPacket>().Select(_ => _.Payload);

    public Task<AsvSdrRecordDataIlsPayload> GetRecordDataIls(ushort recordIndex, uint dataIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataIlsPayload, AsvSdrRecordDataReadRequestPacket, AsvSdrRecordDataIlsPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.DataIndex = dataIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.DataIndex == dataIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordDataVorPayload> OnRecordDataVor => InternalFilter<AsvSdrRecordDataVorPacket>().Select(_ => _.Payload);
    public Task<AsvSdrRecordDataVorPayload> GetRecordDataVor(ushort recordIndex, uint dataIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataVorPayload, AsvSdrRecordDataReadRequestPacket, AsvSdrRecordDataVorPacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.DataIndex = dataIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.DataIndex == dataIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordDataDeleteResponsePayload> DeleteRecordData(ushort recordIndex, ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        return InternalCall<AsvSdrRecordDataDeleteResponsePayload, AsvSdrRecordDataDeleteRequestPacket, AsvSdrRecordDataDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.StartDataIndex = startIndex;
            _.Payload.StopDataIndex = stopIndex;
        },_=>_.Payload.RecordIndex == recordIndex && _.Payload.StartDataIndex == startIndex && _.Payload.StopDataIndex == stopIndex,resultGetter:_=>_.Payload,cancel: cancel);
    }
}