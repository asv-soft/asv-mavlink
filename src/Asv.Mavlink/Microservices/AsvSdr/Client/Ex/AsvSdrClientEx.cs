using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
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
    private readonly RxValue<AsvSdrCustomModeFlag> _supportedModes;
    private readonly SourceCache<AsvSdrClientRecord,RecordId> _records;

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

        _recordsCount = new RxValue<ushort>()
            .DisposeItWith(Disposable);
        _supportedModes = new RxValue<AsvSdrCustomModeFlag>()
            .DisposeItWith(Disposable);
        client.Status.Subscribe(_ =>
        {
            _supportedModes.OnNext(_.SupportedModes);
            _recordsCount.OnNext(_.RecordCount);
        }).DisposeItWith(Disposable);
        
        _records = new SourceCache<AsvSdrClientRecord, RecordId>(x => x.Id)
            .DisposeItWith(Disposable);
        client.OnRecord.Subscribe(_=>_records.Edit(updater =>
        {
            var value = updater.Lookup(_.Item1);
            if (value.HasValue)
            {
                updater.AddOrUpdate(new AsvSdrClientRecord(_.Item1,_.Item2,client,config));
            }
        })).DisposeItWith(Disposable);
        client.OnDeleteRecord.Subscribe(_=>_records.Edit(updater =>
        {
            var value = updater.Lookup(_.Item1);
            if (!value.HasValue) return;
            updater.RemoveKey(_.Item1);
            value.Value.Dispose();
        })).DisposeItWith(Disposable);
     
        
        Records = _records.Connect().Transform(_=>(IAsvSdrClientRecord)_).RefCount();
    }
    
    public async Task DeleteRecord(RecordId recordName, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await Base.DeleteRecord(recordName, cs.Token).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");
    }

 

    public async Task<bool> DownloadRecordList(IProgress<double> progress, CancellationToken cancel)
    {
        var lastUpdate = DateTime.Now;
        _records.Clear();
        using var request = Base.OnRecord
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestAck = await Base.GetRecordList(0,ushort.MaxValue,cancel).ConfigureAwait(false);
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
    public IRxValue<AsvSdrCustomModeFlag> SupportedModes => _supportedModes;
    public IRxValue<AsvSdrCustomMode> CustomMode => _customMode;
    public IRxValue<ushort> RecordsCount => _recordsCount;
    public IObservable<IChangeSet<IAsvSdrClientRecord,RecordId>> Records { get; }
    
    public async Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio,
        CancellationToken cancel)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(BitConverter.GetBytes((uint)mode)),
            BitConverter.ToSingle(freqArray,0),
            BitConverter.ToSingle(freqArray,4),
            recordRate,
            BitConverter.ToSingle(BitConverter.GetBytes(sendingThinningRatio)),
            Single.NaN,
            Single.NaN,
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StartRecord(RecordId recordName, CancellationToken cancel)
    {
        var nameArray = new byte[SdrWellKnown.RecordNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,recordName.Name);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(nameArray,16),
            BitConverter.ToSingle(nameArray,20),
            BitConverter.ToSingle(nameArray,24),
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

    public async Task<MavResult> CurrentRecordSetTag(string tagName, AsvSdrRecordTagType type, byte[] rawValue , CancellationToken cancel)
    {
        if (rawValue.Length != 8) 
            throw new ArgumentException("Raw value must be 8 bytes long");
        SdrWellKnown.CheckTagName(tagName);
        var nameArray = new byte[SdrWellKnown.RecordTagNameMaxLength];
        MavlinkTypesHelper.SetString(nameArray,tagName);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong((V2.Common.MavCmd)MavCmd.MavCmdAsvSdrSetMode,
            BitConverter.ToSingle(BitConverter.GetBytes((uint)type)),
            BitConverter.ToSingle(nameArray,0),
            BitConverter.ToSingle(nameArray,4),
            BitConverter.ToSingle(nameArray,8),
            BitConverter.ToSingle(nameArray,12),
            BitConverter.ToSingle(rawValue,0),
            BitConverter.ToSingle(rawValue,4),
            cs.Token).ConfigureAwait(false);
        return result.Result;
    }

}