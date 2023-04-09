using System.Reactive.Linq;
using System.Text;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;
using MavCmd = Asv.Mavlink.V2.AsvSdr.MavCmd;

namespace Asv.Mavlink.Sdr;

public class SdrClientDeviceConfig
{
    public int MaxTimeToWaitForResponseForListMs { get; set; } = 10000;
    public int DefaultCommandAttemptCount { get; set; } = 3;
}
public class SdrClientDevice : DisposableOnceWithCancel, ISdrClientDevice
{
    private readonly IMavlinkClient _client;
    private readonly SdrClientDeviceConfig _config;
    private readonly RxValue<AsvSdrCustomMode> _customMode;
    private readonly SourceCache<SdrClientDeviceRecord,ushort> _records;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly RxValue<ushort> _recordsCount;
    private readonly RxValue<AsvSdrSupportModeFlag> _supportedModes;

    public SdrClientDevice(IMavlinkClient client,SdrClientDeviceConfig config)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _config = config;
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        _customMode = new RxValue<AsvSdrCustomMode>();
        _client.Heartbeat.RawHeartbeat
            .Select(_ => (AsvSdrCustomMode)_.CustomMode)
            .Subscribe(_customMode)
            .DisposeItWith(Disposable);
        
        
        
        _recordsCount = new RxValue<ushort>().DisposeItWith(Disposable);
        _supportedModes = new RxValue<AsvSdrSupportModeFlag>().DisposeItWith(Disposable);
        _client.Sdr.Status.Subscribe(_ =>
        {
            _supportedModes.OnNext(_.SupportedModes);
            _recordsCount.OnNext(_.RecordCount);
        }).DisposeItWith(Disposable);
        
        _records = new SourceCache<SdrClientDeviceRecord, ushort>(x => x.Index).DisposeItWith(Disposable);
        _client.Sdr.OnRecord.Subscribe(_=>_records.Edit(updater =>
            {
                var value = updater.Lookup(_.Index);
                if (value.HasValue)
                {
                    value.Value.UpdateRecord(_);    
                }
                else
                {
                    updater.AddOrUpdate(new SdrClientDeviceRecord(_,_client,config));
                }
            })).DisposeItWith(Disposable);
        Records = _records.Connect().DisposeMany().RefCount();
    }


    public async Task DeleteRecord(ushort recordIndex, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await _client.Sdr.DeleteRecords(recordIndex,recordIndex, cs.Token);
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
            await DeleteRecord(startIndex, cancel);
            return;
        }
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await _client.Sdr.DeleteRecords(startIndex,stopIndex, cs.Token);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");
        _records.RemoveKeys(Enumerable.Range(startIndex,stopIndex-startIndex).Select(_=>(ushort)_));
    }

    public async Task<bool> UploadRecordList(IProgress<double>? progress, CancellationToken cancel)
    {
        var lastUpdate = DateTime.Now;
        _records.Clear();
        using var request = _client.Sdr.OnRecord
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestAck = await _client.Sdr.GetRecordList(cancel);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");

        while (DateTime.Now - lastUpdate > _maxTimeToWaitForResponseForList || _records.Count < requestAck.ItemsCount)
        {
            await Task.Delay(1000, cancel);
            progress?.Report((double)requestAck.ItemsCount/_records.Count);
        }
        return _records.Count == requestAck.ItemsCount;
    }

    public IRxValue<AsvSdrSupportModeFlag> SupportedModes => _supportedModes;
    public IRxValue<AsvSdrCustomMode> CustomMode => _customMode;
    public IRxValue<ushort> RecordsCount => _recordsCount;
    public IObservable<IChangeSet<SdrClientDeviceRecord,ushort>> Records { get; }
    
    public async Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float sendDataRate, CancellationToken cancel)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)),
            BitConverter.ToSingle(freqArray,0),
            BitConverter.ToSingle(freqArray,4),
            sendDataRate,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            _config.DefaultCommandAttemptCount, cs.Token);
        return result.Result;
    }

    public async Task<MavResult> StartRecord(string recordName, CancellationToken cancel)
    {
        if (recordName.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (recordName.Length > WellKnown.RecordNameMaxLength) throw new Exception($"Record name is too long. Max length is {WellKnown.RecordNameMaxLength}");
        var nameArray = new byte[WellKnown.RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,recordName);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(nameArray,16),
            BitConverter.ToSingle(nameArray,20),
            BitConverter.ToSingle(nameArray,27),
            _config.DefaultCommandAttemptCount, cs.Token);
        return result.Result;
    }

    public async Task<MavResult> StopRecord(CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            _config.DefaultCommandAttemptCount, cs.Token);
        return result.Result;
    }

    public async Task<MavResult> CurrentRecordSetTag(AsvSdrRecordTag tag, CancellationToken cancel)
    {
        if (tag.Name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (tag.Name.Length > WellKnown.RecordTagNameMaxLength) throw new Exception($"Record tag name is too long. Max length is {WellKnown.RecordTagNameMaxLength}");
        var nameArray = new byte[WellKnown.RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,tag.Name);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            WellKnown.GetCommandParamValue(0,AsvSdrRecordTagFlag.AsvSdrRecordTagFlagForCurrent, tag.Type),
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(tag.RawValue,0),
            BitConverter.ToSingle(tag.RawValue,4),
            _config.DefaultCommandAttemptCount, cs.Token);
        return result.Result;
    }

    public async Task<MavResult> RecordSetTag(ushort recordIndex, AsvSdrRecordTag tag, CancellationToken cancel)
    {
        if (tag.Name.IsNullOrWhiteSpace()) throw new Exception("Record name is empty");
        if (tag.Name.Length > WellKnown.RecordTagNameMaxLength) throw new Exception($"Record tag name is too long. Max length is {WellKnown.RecordTagNameMaxLength}");
        var nameArray = new byte[WellKnown.RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,tag.Name);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            WellKnown.GetCommandParamValue(recordIndex,AsvSdrRecordTagFlag.AsvSdrRecordTagFlagNone, tag.Type),
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(tag.RawValue,0),
            BitConverter.ToSingle(tag.RawValue,4),
            _config.DefaultCommandAttemptCount, cs.Token);
        return result.Result;
    }
}