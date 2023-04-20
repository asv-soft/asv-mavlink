using System;
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
    private uint _requestCounter;

    public AsvSdrClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, IScheduler scheduler)
        : base("SDR", connection, identity, seq, scheduler)
    {
        _identity = identity;
        _status = new RxValue<AsvSdrOutStatusPayload>().DisposeItWith(Disposable);
        InternalFilter<AsvSdrOutStatusPacket>().Select(_ => _.Payload).Subscribe(_status).DisposeItWith(Disposable);
    }

    public IRxValue<AsvSdrOutStatusPayload> Status => _status;

    public IObservable<AsvSdrRecordPayload> OnRecord => InternalFilter<AsvSdrRecordPacket>().Select(_ => _.Payload);

    public Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordResponsePayload, AsvSdrRecordRequestPacket, AsvSdrRecordResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordStartIndex = startIndex;
            _.Payload.RecordStopIndex = stopIndex;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    private ushort GenerateRequestIndex()
    {
        return (ushort)(Interlocked.Increment(ref _requestCounter)%ushort.MaxValue);
    }

    public IObservable<AsvSdrRecordTagPayload> OnRecordTag => InternalFilter<AsvSdrRecordTagPacket>().Select(_ => _.Payload);
    public Task<AsvSdrRecordDeleteResponsePayload> DeleteRecords(ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDeleteResponsePayload, AsvSdrRecordDeleteRequestPacket, AsvSdrRecordDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordStartIndex = startIndex;
            _.Payload.RecordStopIndex = stopIndex;
            _.Payload.RequestId = id;
        },_=> _.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(ushort recordIndex, ushort startTagIndex, ushort stopTagIndex,
        CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagResponsePayload, AsvSdrRecordTagRequestPacket, AsvSdrRecordTagResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.TagStartIndex = startTagIndex;
            _.Payload.TagStopIndex = stopTagIndex;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId==id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTags(ushort recordIndex, ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagDeleteResponsePayload, AsvSdrRecordTagDeleteRequestPacket, AsvSdrRecordTagDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.TagStartIndex = startIndex;
            _.Payload.TagStopIndex = stopIndex;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(ushort recordIndex, ushort startDataIndex, ushort stopDataIndex,
        CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDataResponsePayload, AsvSdrRecordDataRequestPacket, AsvSdrRecordDataResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.DataStartIndex = startDataIndex;
            _.Payload.DataStopIndex = stopDataIndex;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<AsvSdrRecordDataIlsPayload> OnRecordDataIls => InternalFilter<AsvSdrRecordDataIlsPacket>().Select(_ => _.Payload);
    public IObservable<AsvSdrRecordDataVorPayload> OnRecordDataVor => InternalFilter<AsvSdrRecordDataVorPacket>().Select(_ => _.Payload);

    public Task<AsvSdrRecordDataDeleteResponsePayload> DeleteRecordData(ushort recordIndex, ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDataDeleteResponsePayload, AsvSdrRecordDataDeleteRequestPacket, AsvSdrRecordDataDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RecordIndex = recordIndex;
            _.Payload.StartDataIndex = startIndex;
            _.Payload.StopDataIndex = stopIndex;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }
}