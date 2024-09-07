using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class AsvSdrClient : MavlinkMicroserviceClient, IAsvSdrClient
{
    private readonly MavlinkClientIdentity _identity;
    private readonly RxValue<AsvSdrOutStatusPayload> _status;
    private uint _requestCounter;
    private readonly ILogger _logger;

    public AsvSdrClient(
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null)
        : base("SDR", connection, identity, seq,scheduler,logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _identity = identity;
        _status = new RxValue<AsvSdrOutStatusPayload>().DisposeItWith(Disposable);
        InternalFilter<AsvSdrOutStatusPacket>().Select(p => p.Payload).Subscribe(_status).DisposeItWith(Disposable);
        var dataPacketsHashSet = new HashSet<int>();
        foreach (var item in Enum.GetValues(typeof(AsvSdrCustomMode)).Cast<uint>())
        {
            dataPacketsHashSet.Add((int)item);
        }
        dataPacketsHashSet.Remove((int)AsvSdrCustomMode.AsvSdrCustomModeIdle); // from IDLE we can't get any data
        
        OnRecord = InternalFilter<AsvSdrRecordPacket>()
            .Select(p => (new Guid(p.Payload.RecordGuid), p.Payload))
            .Publish().RefCount();
        OnRecordTag = InternalFilter<AsvSdrRecordTagPacket>().Select(p => (new TagId(p.Payload),p.Payload))
            .Publish().RefCount();
        
        OnDeleteRecordTag = InternalFilter<AsvSdrRecordTagDeleteResponsePacket>()
            .Select(p => (new TagId(p.Payload), p.Payload))
            .Publish().RefCount();
        OnDeleteRecord = InternalFilter<AsvSdrRecordDeleteResponsePacket>().Select(p => (new Guid(p.Payload.RecordGuid),p.Payload))
            .Publish().RefCount();
        
        
        
        OnRecordData = InternalFilteredVehiclePackets.Where(x=>dataPacketsHashSet.Contains(x.MessageId));
        
        OnCalibrationTableRowUploadCallback = InternalFilter<AsvSdrCalibTableUploadReadCallbackPacket>().Select(p => p.Payload)
            .Publish().RefCount();
        OnCalibrationAcc = InternalFilter<AsvSdrCalibAccPacket>().Select(p => p.Payload)
            .Publish().RefCount();
        
        OnCalibrationTable = InternalFilter<AsvSdrCalibTablePacket>().Select(p => p.Payload)
            .Publish().RefCount();
        
        OnSignal = InternalFilter<AsvSdrSignalRawPacket>().Select(x=>x.Payload).Publish().RefCount();
        
        
    }

    public IRxValue<AsvSdrOutStatusPayload> Status => _status;

    public IObservable<(Guid, AsvSdrRecordPayload)> OnRecord { get; }

    public Task<AsvSdrRecordResponsePayload> GetRecordList(ushort skip, ushort count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordResponsePayload, AsvSdrRecordRequestPacket, AsvSdrRecordResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.TargetComponentId;
            p.Payload.TargetSystem = _identity.TargetSystemId;
            p.Payload.Skip = skip;
            p.Payload.Count = count;
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    public ushort GenerateRequestIndex()
    {
        return (ushort)(Interlocked.Increment(ref _requestCounter)%ushort.MaxValue);
    }

    public IObservable<(TagId, AsvSdrRecordTagPayload)> OnRecordTag { get; } 
    public IObservable<(Guid, AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }
    
    public Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(Guid recordId, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDeleteResponsePayload, AsvSdrRecordDeleteRequestPacket, AsvSdrRecordDeleteResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.TargetComponentId;
            p.Payload.TargetSystem = _identity.TargetSystemId;
            p.Payload.RequestId = id;
            recordId.TryWriteBytes(p.Payload.RecordGuid);
        },p=> p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(Guid recordId, ushort skip, ushort count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagResponsePayload, AsvSdrRecordTagRequestPacket, AsvSdrRecordTagResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.TargetComponentId;
            p.Payload.TargetSystem = _identity.TargetSystemId;
            recordId.TryWriteBytes(p.Payload.RecordGuid);
            p.Payload.Skip = skip;
            p.Payload.Count = count;
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId==id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    public IObservable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }

    public Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tagId, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagDeleteResponsePayload, AsvSdrRecordTagDeleteRequestPacket, AsvSdrRecordTagDeleteResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.TargetComponentId;
            p.Payload.TargetSystem = _identity.TargetSystemId;
            tagId.RecordGuid.TryWriteBytes(p.Payload.RecordGuid);
            tagId.TagGuid.TryWriteBytes(p.Payload.TagGuid);
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    public Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(Guid recordId, uint skip, uint count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDataResponsePayload, AsvSdrRecordDataRequestPacket, AsvSdrRecordDataResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.TargetComponentId;
            p.Payload.TargetSystem = _identity.TargetSystemId;
            recordId.TryWriteBytes(p.Payload.RecordGuid);
            p.Payload.Skip = skip;
            p.Payload.Count = count;
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    public IObservable<IPacketV2<IPayload>> OnRecordData { get; }
    public IObservable<AsvSdrCalibTablePayload> OnCalibrationTable { get; }

    #region Calibration
    public async Task<AsvSdrCalibTablePayload> ReadCalibrationTable(ushort tableIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        var result = await InternalCall<(AsvSdrCalibTablePayload,AsvSdrCalibAccPayload), AsvSdrCalibTableReadPacket>(
            arg =>
            {
                arg.Payload.TargetComponent = _identity.TargetComponentId;
                arg.Payload.TargetSystem = _identity.TargetSystemId;
                arg.Payload.RequestId = id;
                arg.Payload.TableIndex = tableIndex;
            }, (IPacketV2<IPayload> input, out (AsvSdrCalibTablePayload, AsvSdrCalibAccPayload) tuple) =>
            {
                switch (input.MessageId)
                {
                    case AsvSdrCalibTablePacket.PacketMessageId when input is AsvSdrCalibTablePacket packet2 && packet2.Payload.TableIndex == tableIndex:
                        tuple = (packet2.Payload, default);
                        return true;
                    case AsvSdrCalibAccPacket.PacketMessageId when input is AsvSdrCalibAccPacket packet && packet.Payload.RequestId == id:
                        tuple = (default, packet.Payload);
                        return true;
                    default:
                        tuple = default;
                        return false;
                }
            }, cancel:cancel).ConfigureAwait(false);
        if (result.Item2 != null)
        {
            throw new AsvSdrException($"Error to read calibration table {tableIndex}: {result.Item2.Result:G}");
        }
        if (result.Item1 == null)
        {
            throw new AsvSdrException($"Error to read calibration table {tableIndex}: no response");
        }
        return result.Item1;
    }

    public async Task<AsvSdrCalibTableRowPayload> ReadCalibrationTableRow(ushort tableIndex, ushort rowIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        var result = await InternalCall<(AsvSdrCalibTableRowPayload,AsvSdrCalibAccPayload), AsvSdrCalibTableRowReadPacket>(
            arg =>
            {
                arg.Payload.TargetComponent = _identity.TargetComponentId;
                arg.Payload.TargetSystem = _identity.TargetSystemId;
                arg.Payload.RequestId = id;
                arg.Payload.TableIndex = tableIndex;
                arg.Payload.RowIndex = rowIndex;
            }, (IPacketV2<IPayload> input, out (AsvSdrCalibTableRowPayload, AsvSdrCalibAccPayload) tuple) =>
            {
                switch (input.MessageId)
                {
                    case AsvSdrCalibTableRowPacket.PacketMessageId when input is AsvSdrCalibTableRowPacket packet2 
                                                                     && packet2.Payload.TableIndex == tableIndex
                                                                     && packet2.Payload.RowIndex == rowIndex:
                        tuple = (packet2.Payload, default);
                        return true;
                    case AsvSdrCalibAccPacket.PacketMessageId when input is AsvSdrCalibAccPacket packet && packet.Payload.RequestId == id:
                        tuple = (default, packet.Payload);
                        return true;
                    default:
                        tuple = default;
                        return false;
                }
            }, cancel:cancel).ConfigureAwait(false);
        if (result.Item2 != null)
        {
            throw new AsvSdrException($"Error to read calibration table {tableIndex}: {result.Item2.Result:G}");
        }
        if (result.Item1 == null)
        {
            throw new AsvSdrException($"Error to read calibration table {tableIndex}: no response");
        }
        return result.Item1;
    }

    public Task SendCalibrationTableRowUploadStart(Action<AsvSdrCalibTableUploadStartPayload> argsFill, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalSend<AsvSdrCalibTableUploadStartPacket>(p =>
        {
            p.Payload.TargetComponent = _identity.TargetComponentId;
            p.Payload.TargetSystem = _identity.TargetSystemId;
            p.Payload.RequestId = id;
            argsFill(p.Payload);
        },cancel: cancel);
    }

    public IObservable<AsvSdrCalibTableUploadReadCallbackPayload> OnCalibrationTableRowUploadCallback { get; }
    public IObservable<AsvSdrCalibAccPayload> OnCalibrationAcc { get; }
    public Task SendCalibrationTableRowUploadItem(Action<AsvSdrCalibTableRowPayload> argsFill, CancellationToken cancel = default)
    {
        return InternalSend<AsvSdrCalibTableRowPacket>(p => { argsFill(p.Payload); },cancel: cancel);
    }
    
    #endregion

    public IObservable<AsvSdrSignalRawPayload> OnSignal { get; }
}