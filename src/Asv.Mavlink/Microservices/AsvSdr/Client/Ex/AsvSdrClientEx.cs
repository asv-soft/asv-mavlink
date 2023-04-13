using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Sdr;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;
using MavCmd = Asv.Mavlink.V2.AsvSdr.MavCmd;

namespace Asv.Mavlink;

public class AsvSdrClientExConfig
{
    public int MaxTimeToWaitForResponseForListMs { get; set; } = 2000;
}

public class AsvSdrClientEx : DisposableOnceWithCancel, IAsvSdrClientEx
{
    private readonly ICommandClient _commandClient;
    private readonly AsvSdrClientExConfig _config;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly RxValue<AsvSdrCustomMode> _customMode;
    private readonly RxValue<ushort> _recordsCount;
    private readonly RxValue<AsvSdrSupportModeFlag> _supportedModes;
    private readonly SourceCache<AsvSdrClientRecord,ushort> _records;

    public AsvSdrClientEx(IAsvSdrClient client, IHeartbeatClient heartbeatClient, ICommandClient commandClient, AsvSdrClientExConfig config)
    {
        _commandClient = commandClient;
        _config = config;
        Base = client;
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        _customMode = new RxValue<AsvSdrCustomMode>();
        heartbeatClient.RawHeartbeat
            .Select(_ => (AsvSdrCustomMode)_.CustomMode)
            .Subscribe(_customMode)
            .DisposeItWith(Disposable);

        _recordsCount = new RxValue<ushort>().DisposeItWith(Disposable);
        _supportedModes = new RxValue<AsvSdrSupportModeFlag>().DisposeItWith(Disposable);
        client.Status.Subscribe(_ =>
        {
            _supportedModes.OnNext(_.SupportedModes);
            _recordsCount.OnNext(_.RecordCount);
        }).DisposeItWith(Disposable);
        
        _records = new SourceCache<AsvSdrClientRecord, ushort>(x => x.Index).DisposeItWith(Disposable);
        client.OnRecord.Subscribe(_=>_records.Edit(updater =>
        {
            var value = updater.Lookup(_.Index);
            if (value.HasValue)
            {
                value.Value.UpdateRecord(_);    
            }
            else
            {
                updater.AddOrUpdate(new AsvSdrClientRecord(_,client,config));
            }
        })).DisposeItWith(Disposable);
        Records = _records.Connect().DisposeMany().RefCount();
    }
    
    public async Task DeleteRecord(ushort recordIndex, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await Base.DeleteRecords(recordIndex,recordIndex, cs.Token).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");
        _records.RemoveKey(recordIndex);
    }

    public async Task DeleteRecords(ushort startIndex, ushort stopIndex, CancellationToken cancel = default)
    {
        if (startIndex>stopIndex) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (startIndex == stopIndex)
        {
            await DeleteRecord(startIndex, cancel).ConfigureAwait(false);
            return;
        }
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await Base.DeleteRecords(startIndex,stopIndex, cs.Token).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");
        _records.RemoveKeys(Enumerable.Range(startIndex,stopIndex-startIndex).Select(_=>(ushort)_));
    }

    public async Task<bool> UploadRecordList(IProgress<double> progress, CancellationToken cancel)
    {
        var lastUpdate = DateTime.Now;
        _records.Clear();
        using var request = Base.OnRecord
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestAck = await Base.GetRecordList(cancel).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");

        while (DateTime.Now - lastUpdate > _maxTimeToWaitForResponseForList || _records.Count < requestAck.ItemsCount)
        {
            await Task.Delay(1000, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_records.Count);
        }
        return _records.Count == requestAck.ItemsCount;
    }

    public IAsvSdrClient Base { get; }
    public IRxValue<AsvSdrSupportModeFlag> SupportedModes => _supportedModes;
    public IRxValue<AsvSdrCustomMode> CustomMode => _customMode;
    public IRxValue<ushort> RecordsCount => _recordsCount;
    public IObservable<IChangeSet<AsvSdrClientRecord,ushort>> Records { get; }
    
    public async Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float sendDataRate, CancellationToken cancel)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)),
            BitConverter.ToSingle(freqArray,0),
            BitConverter.ToSingle(freqArray,4),
            sendDataRate,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StartRecord(string recordName, CancellationToken cancel)
    {
        if (recordName.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (recordName.Length > SdrWellKnown.RecordNameMaxLength) throw new Exception($"Record name is too long. Max length is {SdrWellKnown.RecordNameMaxLength}");
        var nameArray = new byte[SdrWellKnown.RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,recordName);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(nameArray,16),
            BitConverter.ToSingle(nameArray,20),
            BitConverter.ToSingle(nameArray,27),
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StopRecord(CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> CurrentRecordSetTag(AsvSdrRecordTag tag, CancellationToken cancel)
    {
        if (tag.Name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (tag.Name.Length > SdrWellKnown.RecordTagNameMaxLength) throw new Exception($"Record tag name is too long. Max length is {SdrWellKnown.RecordTagNameMaxLength}");
        var nameArray = new byte[SdrWellKnown.RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,tag.Name);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            SdrWellKnown.GetCommandParamValue(0,AsvSdrRecordTagFlag.AsvSdrRecordTagFlagForCurrent, tag.Type),
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(tag.RawValue,0),
            BitConverter.ToSingle(tag.RawValue,4),
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> RecordSetTag(ushort recordIndex, AsvSdrRecordTag tag, CancellationToken cancel)
    {
        if (tag.Name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (tag.Name.Length > SdrWellKnown.RecordTagNameMaxLength) throw new Exception($"Record tag name is too long. Max length is {SdrWellKnown.RecordTagNameMaxLength}");
        var nameArray = new byte[SdrWellKnown.RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,tag.Name);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            SdrWellKnown.GetCommandParamValue(recordIndex,AsvSdrRecordTagFlag.AsvSdrRecordTagFlagNone, tag.Type),
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(tag.RawValue,0),
            BitConverter.ToSingle(tag.RawValue,4),
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }
}