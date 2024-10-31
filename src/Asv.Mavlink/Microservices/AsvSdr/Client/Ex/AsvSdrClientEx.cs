#nullable enable
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;

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
    private readonly ReactiveProperty<AsvSdrCustomMode> _customMode;
    private readonly ReactiveProperty<ushort> _recordsCount;
    private readonly ReactiveProperty<AsvSdrCustomModeFlag> _supportedModes;
    private readonly SourceCache<AsvSdrClientRecord,Guid> _records;
    private readonly ReactiveProperty<bool> _isRecordStarted;
    private readonly ReactiveProperty<Guid> _currentRecord;
    private readonly ReactiveProperty<ushort> _calibrationTableRemoteCount;
    private readonly ReactiveProperty<AsvSdrCalibState> _calibrationState;
    private readonly ISourceCache<AsvSdrClientCalibrationTable,string> _calibrationTables;
    private readonly ILogger _logger;

    public AsvSdrClientEx(
        IAsvSdrClient client, 
        IHeartbeatClient heartbeatClient, 
        ICommandClient commandClient, 
        AsvSdrClientExConfig config)
    {
        Base = client;
        _logger = client.Core.Log.CreateLogger<AsvSdrClientEx>();
        _commandClient = commandClient;
        _config = config;
        
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        _customMode = new ReactiveProperty<AsvSdrCustomMode>();
        heartbeatClient.RawHeartbeat
            .Select(p => (AsvSdrCustomMode)p.CustomMode)
            .Subscribe(_customMode)
            .DisposeItWith(Disposable);

        _recordsCount = new ReactiveProperty<ushort>()
            .DisposeItWith(Disposable);
        _supportedModes = new ReactiveProperty<AsvSdrCustomModeFlag>()
            .DisposeItWith(Disposable);
        _currentRecord = new ReactiveProperty<Guid>()
            .DisposeItWith(Disposable);
        _isRecordStarted = new ReactiveProperty<bool>(false)
            .DisposeItWith(Disposable);
        _calibrationTableRemoteCount = new ReactiveProperty<ushort>()
            .DisposeItWith(Disposable);
        _calibrationState = new ReactiveProperty<AsvSdrCalibState>()
            .DisposeItWith(Disposable);
        
        client.Status.Subscribe(p =>
        {
            _supportedModes.OnNext(p.SupportedModes);
            _recordsCount.OnNext(p.RecordCount);
            var guid = new Guid(p.CurrentRecordGuid);
            _currentRecord.OnNext(guid);
            _isRecordStarted.OnNext(guid != Guid.Empty);
            _calibrationTableRemoteCount.OnNext(p.CalibTableCount);
            _calibrationState.OnNext(p.CalibState);
        }).DisposeItWith(Disposable);
        
        _records = new SourceCache<AsvSdrClientRecord, Guid>(x => x.Id)
            .DisposeItWith(Disposable);
        client.OnRecord.Subscribe(t=>_records.Edit(updater =>
        {
            var value = updater.Lookup(t.Item1);
            if (value.HasValue == false)
            {
                updater.AddOrUpdate(new AsvSdrClientRecord(t.Item1,t.Item2,client,config));
            }
        })).DisposeItWith(Disposable);
        client.OnDeleteRecord.Where(t=>t.Item2.Result == AsvSdrRequestAck.AsvSdrRequestAckOk).Subscribe(t=>_records.Edit(updater =>
        {
            var value = updater.Lookup(t.Item1);
            if (!value.HasValue) return;
            updater.RemoveKey(t.Item1);
            value.Value.Dispose();
        })).DisposeItWith(Disposable);
        
        Records = _records.Connect().Transform(r=>(IAsvSdrClientRecord)r).RefCount();
        
        _calibrationTables = new SourceCache<AsvSdrClientCalibrationTable,string>(t=>t.Name)
            .DisposeItWith(Disposable);
        CalibrationTables = _calibrationTables.Connect().DisposeMany().RefCount();
        Base.OnCalibrationTable.Subscribe(payload=>_calibrationTables.Edit(updater =>
        {
            var name = MavlinkTypesHelper.GetString(payload.TableName);
            var value = updater.Lookup(name);
            if (value.HasValue == false)
            {
                updater.AddOrUpdate(new AsvSdrClientCalibrationTable(payload, Base, _maxTimeToWaitForResponseForList));
            }
            else
            {
                value.Value.Update(payload);
            }
        })).DisposeItWith(Disposable);
    }
    public string Name => $"{Base.Name}Ex";
    public ReadOnlyReactiveProperty<Guid> CurrentRecord => _currentRecord;

    public ReadOnlyReactiveProperty<bool> IsRecordStarted => _isRecordStarted;

    public async Task DeleteRecord(Guid recordName, CancellationToken cancel)
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

        while (DateTime.Now - lastUpdate < _maxTimeToWaitForResponseForList && _records.Count < requestAck.ItemsCount)
        {
            await Task.Delay(1000, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_records.Count);
        }
        return _records.Count == requestAck.ItemsCount;
    }
    public IAsvSdrClient Base { get; }
    public ReadOnlyReactiveProperty<AsvSdrCustomModeFlag> SupportedModes => _supportedModes;
    public ReadOnlyReactiveProperty<AsvSdrCustomMode> CustomMode => _customMode;
    
    public ReadOnlyReactiveProperty<ushort> RecordsCount => _recordsCount;
    public IObservable<IChangeSet<IAsvSdrClientRecord,Guid>> Records { get; }
    
    public async Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float referencePowerDbm,
        CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => AsvSdrHelper.SetArgsForSdrSetMode(item, mode,frequencyHz,recordRate,sendingThinningRatio,referencePowerDbm),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StartRecord(string recordName, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => AsvSdrHelper.SetArgsForSdrStartRecord(item, recordName), cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StopRecord(CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(AsvSdrHelper.SetArgsForSdrStopRecord, cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> CurrentRecordSetTag(string tagName, AsvSdrRecordTagType type, byte[] rawValue , CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        
        var result = await _commandClient.CommandLong(item=>AsvSdrHelper.SetArgsForSdrCurrentRecordSetTag(item,tagName,type,rawValue), cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> SystemControlAction(AsvSdrSystemControlAction action, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item=>AsvSdrHelper.SetArgsForSdrSystemControlAction(item, action), cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StartMission(ushort missionIndex = 0, CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item=>AsvSdrHelper.SetArgsForSdrStartMission(item,missionIndex), cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StopMission(CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(AsvSdrHelper.SetArgsForSdrStopMission, cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StartCalibration(CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(AsvSdrHelper.SetArgsForSdrStartCalibration, cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StopCalibration(CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(AsvSdrHelper.SetArgsForSdrStopCalibration, cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public ReadOnlyReactiveProperty<ushort> CalibrationTableRemoteCount => _calibrationTableRemoteCount;
    public ReadOnlyReactiveProperty<AsvSdrCalibState> CalibrationState => _calibrationState;
    public async Task ReadCalibrationTableList(IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        var count = CalibrationTableRemoteCount.Value;
        for (ushort i = 0; i < count; i++)
        {
            progress.Report(i);
            await Base.ReadCalibrationTable(i,cancel).ConfigureAwait(false);
            // no need to add result to cache, it will be added by OnCalibrationTable subscription in ctor
        }
    }

    public async Task<AsvSdrClientCalibrationTable?> GetCalibrationTable(string name, CancellationToken cancel = default)
    {
        var value = _calibrationTables.Lookup(name);
        if (value.HasValue) return value.Value;
        await ReadCalibrationTableList(null, cancel).ConfigureAwait(false);
        value = _calibrationTables.Lookup(name);
        return value.HasValue ? value.Value : null;
    }


    public IObservable<IChangeSet<AsvSdrClientCalibrationTable,string>> CalibrationTables { get; }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}