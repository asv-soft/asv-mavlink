using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink;

public class AsvSdrClient : MavlinkMicroserviceClient, IAsvSdrClient
{
    private readonly MavlinkClientIdentity _identity;
    private uint _requestCounter;
    private readonly ILogger _logger;
    private readonly Subject<(Guid, AsvSdrRecordPayload)> _onRecord;
    private readonly Subject<(TagId, AsvSdrRecordTagPayload)> _onRecordTag;
    private readonly Subject<(Guid, AsvSdrRecordDeleteResponsePayload)> _onDeleteRecord;
    private readonly Subject<(TagId, AsvSdrRecordTagDeleteResponsePayload)> _onDeleteRecordTag;
    private readonly Subject<IPacketV2<IPayload>> _onRecordData;
    private readonly Subject<AsvSdrCalibTablePayload> _onCalibrationTable;
    private readonly Subject<AsvSdrCalibTableUploadReadCallbackPayload> _onCalibrationTableRowUploadCallback;
    private readonly Subject<AsvSdrCalibAccPayload> _onCalibrationAcc;
    private readonly Subject<AsvSdrSignalRawPayload> _onSignal;

    public AsvSdrClient(MavlinkClientIdentity identity,
        ICoreServices core)
        : base("SDR", identity, core)
    {
        _logger = core.Log.CreateLogger<AsvSdrClient>();
        _identity = identity;
        Status = InternalFilter<AsvSdrOutStatusPacket>().Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        
        var dataPacketsHashSet = new HashSet<int>();
        foreach (var item in Enum.GetValues(typeof(AsvSdrCustomMode)).Cast<uint>())
        {
            dataPacketsHashSet.Add((int)item);
        }
        dataPacketsHashSet.Remove((int)AsvSdrCustomMode.AsvSdrCustomModeIdle); // from IDLE we can't get any data

        _onRecord = new Subject<(Guid, AsvSdrRecordPayload)>();
        InternalFilter<AsvSdrRecordPacket>()
            .Select(p => (new Guid(p.Payload.RecordGuid), p.Payload))
            .Subscribe(_onRecord.AsObserver());

        _onRecordTag = new Subject<(TagId, AsvSdrRecordTagPayload)>();
        InternalFilter<AsvSdrRecordTagPacket>().Select(p => (new TagId(p.Payload), p.Payload))
            .Subscribe(_onRecordTag.AsObserver());

        _onDeleteRecordTag = new Subject<(TagId, AsvSdrRecordTagDeleteResponsePayload)>();
        InternalFilter<AsvSdrRecordTagDeleteResponsePacket>()
            .Select(p => (new TagId(p.Payload), p.Payload))
            .Subscribe(_onDeleteRecordTag.AsObserver());

        _onDeleteRecord = new Subject<(Guid, AsvSdrRecordDeleteResponsePayload)>();
        InternalFilter<AsvSdrRecordDeleteResponsePacket>().Select(p => (new Guid(p.Payload.RecordGuid), p.Payload))
            .Subscribe(_onDeleteRecord.AsObserver());

        _onRecordData = new Subject<IPacketV2<IPayload>>();
        InternalFilteredVehiclePackets.Where(x=>dataPacketsHashSet.Contains(x.MessageId))
            .Subscribe(_onRecordData.AsObserver());

        _onCalibrationTableRowUploadCallback = new Subject<AsvSdrCalibTableUploadReadCallbackPayload>();
        InternalFilter<AsvSdrCalibTableUploadReadCallbackPacket>().Select(p => p.Payload)
            .Subscribe(_onCalibrationTableRowUploadCallback.AsObserver());

        _onCalibrationAcc = new Subject<AsvSdrCalibAccPayload>();
        InternalFilter<AsvSdrCalibAccPacket>().Select(p => p.Payload)
            .Subscribe(_onCalibrationAcc.AsObserver());

        _onCalibrationTable = new Subject<AsvSdrCalibTablePayload>();
        InternalFilter<AsvSdrCalibTablePacket>().Select(p => p.Payload)
            .Subscribe(_onCalibrationTable.AsObserver());

        _onSignal = new Subject<AsvSdrSignalRawPayload>();
        InternalFilter<AsvSdrSignalRawPacket>().Select(x => x.Payload)
            .Subscribe(_onSignal.AsObserver());


    }
    public ReadOnlyReactiveProperty<AsvSdrOutStatusPayload?> Status { get; }
    public Observable<(Guid, AsvSdrRecordPayload)> OnRecord => _onRecord;
    public Observable<(TagId, AsvSdrRecordTagPayload)> OnRecordTag => _onRecordTag;
    public Observable<(Guid, AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord => _onDeleteRecord;
    public Observable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag => _onDeleteRecordTag;
    public Observable<IPacketV2<IPayload>> OnRecordData => _onRecordData;
    public Observable<AsvSdrCalibTablePayload> OnCalibrationTable => _onCalibrationTable;
    public Observable<AsvSdrCalibTableUploadReadCallbackPayload> OnCalibrationTableRowUploadCallback => _onCalibrationTableRowUploadCallback;
    public Observable<AsvSdrCalibAccPayload> OnCalibrationAcc => _onCalibrationAcc;
    public Observable<AsvSdrSignalRawPayload> OnSignal => _onSignal;
    public Task<AsvSdrRecordResponsePayload> GetRecordList(ushort skip, ushort count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordResponsePayload, AsvSdrRecordRequestPacket, AsvSdrRecordResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.Target.ComponentId;
            p.Payload.TargetSystem = _identity.Target.SystemId;
            p.Payload.Skip = skip;
            p.Payload.Count = count;
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    public ushort GenerateRequestIndex()
    {
        return (ushort)(Interlocked.Increment(ref _requestCounter)%ushort.MaxValue);
    }
    public Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(Guid recordId, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordDeleteResponsePayload, AsvSdrRecordDeleteRequestPacket, AsvSdrRecordDeleteResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.Target.ComponentId;
            p.Payload.TargetSystem = _identity.Target.SystemId;
            p.Payload.RequestId = id;
            recordId.TryWriteBytes(p.Payload.RecordGuid);
        },p=> p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }
    public Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(Guid recordId, ushort skip, ushort count, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagResponsePayload, AsvSdrRecordTagRequestPacket, AsvSdrRecordTagResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.Target.ComponentId;
            p.Payload.TargetSystem = _identity.Target.SystemId;
            recordId.TryWriteBytes(p.Payload.RecordGuid);
            p.Payload.Skip = skip;
            p.Payload.Count = count;
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId==id,resultGetter:p=>p.Payload,cancel: cancel);
    }
    public Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tagId, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        return InternalCall<AsvSdrRecordTagDeleteResponsePayload, AsvSdrRecordTagDeleteRequestPacket, AsvSdrRecordTagDeleteResponsePacket>(p =>
        {
            p.Payload.TargetComponent = _identity.Target.ComponentId;
            p.Payload.TargetSystem = _identity.Target.SystemId;
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
            p.Payload.TargetComponent = _identity.Target.ComponentId;
            p.Payload.TargetSystem = _identity.Target.SystemId;
            recordId.TryWriteBytes(p.Payload.RecordGuid);
            p.Payload.Skip = skip;
            p.Payload.Count = count;
            p.Payload.RequestId = id;
        },p=>p.Payload.RequestId == id,resultGetter:p=>p.Payload,cancel: cancel);
    }

    

    #region Calibration
    public async Task<AsvSdrCalibTablePayload> ReadCalibrationTable(ushort tableIndex, CancellationToken cancel = default)
    {
        var id = GenerateRequestIndex();
        var result = await InternalCall<(AsvSdrCalibTablePayload,AsvSdrCalibAccPayload), AsvSdrCalibTableReadPacket>(
            arg =>
            {
                arg.Payload.TargetComponent = _identity.Target.ComponentId;
                arg.Payload.TargetSystem = _identity.Target.SystemId;
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
                arg.Payload.TargetComponent = _identity.Target.ComponentId;
                arg.Payload.TargetSystem = _identity.Target.SystemId;
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
            p.Payload.TargetComponent = _identity.Target.ComponentId;
            p.Payload.TargetSystem = _identity.Target.SystemId;
            p.Payload.RequestId = id;
            argsFill(p.Payload);
        },cancel: cancel);
    }

    
    public Task SendCalibrationTableRowUploadItem(Action<AsvSdrCalibTableRowPayload> argsFill, CancellationToken cancel = default)
    {
        return InternalSend<AsvSdrCalibTableRowPacket>(p => { argsFill(p.Payload); },cancel: cancel);
    }
    
    #endregion

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onRecord.Dispose();
            _onRecordTag.Dispose();
            _onDeleteRecord.Dispose();
            _onDeleteRecordTag.Dispose();
            _onRecordData.Dispose();
            _onCalibrationTable.Dispose();
            _onCalibrationTableRowUploadCallback.Dispose();
            _onCalibrationAcc.Dispose();
            _onSignal.Dispose();
            Status.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onRecord).ConfigureAwait(false);
        await CastAndDispose(_onRecordTag).ConfigureAwait(false);
        await CastAndDispose(_onDeleteRecord).ConfigureAwait(false);
        await CastAndDispose(_onDeleteRecordTag).ConfigureAwait(false);
        await CastAndDispose(_onRecordData).ConfigureAwait(false);
        await CastAndDispose(_onCalibrationTable).ConfigureAwait(false);
        await CastAndDispose(_onCalibrationTableRowUploadCallback).ConfigureAwait(false);
        await CastAndDispose(_onCalibrationAcc).ConfigureAwait(false);
        await CastAndDispose(_onSignal).ConfigureAwait(false);
        await CastAndDispose(Status).ConfigureAwait(false);

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