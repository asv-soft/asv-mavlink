using System;
using System.Collections.Generic;
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
    private uint _requestCounter;

    public AsvSdrClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, IScheduler scheduler)
        : base("SDR", connection, identity, seq, scheduler)
    {
        _identity = identity;
        _status = new RxValue<AsvSdrOutStatusPayload>().DisposeItWith(Disposable);
        InternalFilter<AsvSdrOutStatusPacket>().Select(_ => _.Payload).Subscribe(_status).DisposeItWith(Disposable);
        var dataPacketsHashSet = new HashSet<int>();
        foreach (var item in Enum.GetValues(typeof(AsvSdrCustomMode)).Cast<uint>())
        {
            dataPacketsHashSet.Add((int)item);
        }
        dataPacketsHashSet.Remove((int)AsvSdrCustomMode.AsvSdrCustomModeIdle); // from IDLE we can't get any data
        
        OnRecord = InternalFilter<AsvSdrRecordPacket>()
            .Select(_ => (new RecordId(_.Payload), _.Payload))
            .Publish().RefCount();
        OnRecordTag = InternalFilter<AsvSdrRecordTagPacket>().Select(_ => (new TagId(_.Payload),_.Payload))
            .Publish().RefCount();
        
        OnDeleteRecordTag = InternalFilter<AsvSdrRecordTagDeleteResponsePacket>()
            .Select(_ => (new TagId(_.Payload), _.Payload))
            .Publish().RefCount();
        OnDeleteRecord = InternalFilter<AsvSdrRecordDeleteResponsePacket>().Select(_ => (new RecordId(_.Payload),_.Payload))
            .Publish().RefCount();
        
        OnRecordData = InternalFilteredVehiclePackets.Where(_=>dataPacketsHashSet.Contains(_.MessageId));
    }

    public IRxValue<AsvSdrOutStatusPayload> Status => _status;

    public IObservable<(RecordId, AsvSdrRecordPayload)> OnRecord { get; }

    public Task<AsvSdrRecordResponsePayload> GetRecordList(ushort skip, ushort count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordResponsePayload, AsvSdrRecordRequestPacket, AsvSdrRecordResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.Skip = skip;
            _.Payload.Count = count;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    private ushort GenerateRequestIndex()
    {
        return (ushort)(Interlocked.Increment(ref _requestCounter)%ushort.MaxValue);
    }

    public IObservable<(TagId, AsvSdrRecordTagPayload)> OnRecordTag { get; } 
    public IObservable<(RecordId, AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }
    
    public Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(RecordId recordName, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDeleteResponsePayload, AsvSdrRecordDeleteRequestPacket, AsvSdrRecordDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            _.Payload.RequestId = id;
            MavlinkTypesHelper.SetString(_.Payload.RecordName, recordName.Name);
        },_=> _.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(RecordId recordName, ushort skip, ushort count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagResponsePayload, AsvSdrRecordTagRequestPacket, AsvSdrRecordTagResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            MavlinkTypesHelper.SetString(_.Payload.RecordName, recordName.Name);
            _.Payload.Skip = skip;
            _.Payload.Count = count;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId==id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }

    public Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tagId, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagDeleteResponsePayload, AsvSdrRecordTagDeleteRequestPacket, AsvSdrRecordTagDeleteResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            MavlinkTypesHelper.SetString(_.Payload.RecordName, tagId.RecordName);
            MavlinkTypesHelper.SetString(_.Payload.TagName, tagId.TagName);
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(RecordId recordId, uint skip, uint count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDataResponsePayload, AsvSdrRecordDataRequestPacket, AsvSdrRecordDataResponsePacket>(_ =>
        {
            _.Payload.TargetComponent = _identity.TargetComponentId;
            _.Payload.TargetSystem = _identity.TargetSystemId;
            MavlinkTypesHelper.SetString(_.Payload.RecordName, recordId.Name);
            _.Payload.Skip = skip;
            _.Payload.Count = count;
            _.Payload.RequestId = id;
        },_=>_.Payload.RequestId == id,resultGetter:_=>_.Payload,cancel: cancel);
    }

    public IObservable<IPacketV2<IPayload>> OnRecordData { get; }
}