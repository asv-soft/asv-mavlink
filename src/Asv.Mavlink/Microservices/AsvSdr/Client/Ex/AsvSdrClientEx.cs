#nullable enable
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public class AsvSdrClientExConfig
{
    public int MaxTimeToWaitForResponseForListMs { get; set; } = 2000;
}

public class AsvSdrClientEx : IAsvSdrClientEx, IDisposable, IAsyncDisposable
{
    private readonly ICommandClient _commandClient;
    private readonly AsvSdrClientExConfig _config;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly ObservableDictionary<Guid,IAsvSdrClientRecord> _records;
    private readonly ObservableDictionary<string,AsvSdrClientCalibrationTable> _calibrationTables;
    private readonly ILogger _logger;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly CancellationTokenSource _disposeCancel;

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
        _disposeCancel = new CancellationTokenSource();
        
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        CustomMode = heartbeatClient.RawHeartbeat
            .Select(p => (AsvSdrCustomMode)(p?.CustomMode ?? 0))
            .ToReadOnlyReactiveProperty();

        RecordsCount =  client.Status
            .Select(x=>x?.RecordCount ?? 0)
            .ToReadOnlyReactiveProperty();
            
        SupportedModes = client.Status
            .Select(x=>x?.SupportedModes ?? 0)
            .ToReadOnlyReactiveProperty();

        CurrentRecord = client.Status
            .Select(x => new Guid(x?.CurrentRecordGuid ?? Guid.Empty.ToByteArray()))
            .ToReadOnlyReactiveProperty();

        IsRecordStarted = CurrentRecord.Select(x => Guid.Empty.Equals(x))
            .ToReadOnlyReactiveProperty();
        CalibrationTableRemoteCount = client.Status
            .Select(x => x?.CalibTableCount)
            .ToReadOnlyReactiveProperty();

        CalibrationState = client.Status
            .Select(x => x?.CalibState)
            .ToReadOnlyReactiveProperty();
        
        _records = new ObservableDictionary<Guid,IAsvSdrClientRecord>();
        _sub1 = client.OnRecord.Subscribe(t => _records[t.Item1] = new AsvSdrClientRecord(t.Item1, t.Item2, client, config));
        _sub2 = client.OnDeleteRecord
            .Where(t => t.Item2.Result == AsvSdrRequestAck.AsvSdrRequestAckOk)
            .Subscribe(t => _records.Remove(t.Item1));

        _calibrationTables = new ObservableDictionary<string, AsvSdrClientCalibrationTable>();
        Base.OnCalibrationTable.Subscribe(payload=>
        {
            var name = MavlinkTypesHelper.GetString(payload.TableName);
            if (_calibrationTables.TryGetValue(name, out var updater))
            {
                updater.Update(payload);
            }
            else
            {
                _calibrationTables.Add(name,new AsvSdrClientCalibrationTable(payload, Base, _maxTimeToWaitForResponseForList));
            }
        });
    }
    public string Name => $"{Base.Name}Ex";
    public IAsvSdrClient Base { get; }
    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
    public ReadOnlyReactiveProperty<Guid> CurrentRecord { get; }
    public ReadOnlyReactiveProperty<bool> IsRecordStarted { get; }
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
    
    public ReadOnlyReactiveProperty<AsvSdrCustomModeFlag> SupportedModes { get; }

    public ReadOnlyReactiveProperty<AsvSdrCustomMode> CustomMode { get; }

    public ReadOnlyReactiveProperty<ushort> RecordsCount { get; }

    public IReadOnlyObservableDictionary<Guid, IAsvSdrClientRecord> Records => _records;
    
    public async Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float referencePowerDbm,
        CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => AsvSdrHelper.SetArgsForSdrSetMode(item, mode,frequencyHz,recordRate,sendingThinningRatio,referencePowerDbm),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    private CancellationToken DisposeCancel => _disposeCancel.Token;

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
    public ReadOnlyReactiveProperty<ushort?> CalibrationTableRemoteCount { get; }
    public ReadOnlyReactiveProperty<AsvSdrCalibState?> CalibrationState { get; }
    public async Task ReadCalibrationTableList(IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        var count = CalibrationTableRemoteCount.CurrentValue;
        for (ushort i = 0; i < count; i++)
        {
            progress.Report(i);
            await Base.ReadCalibrationTable(i,cancel).ConfigureAwait(false);
            // no need to add result to cache, it will be added by OnCalibrationTable subscription in ctor
        }
    }

    public async Task<AsvSdrClientCalibrationTable?> GetCalibrationTable(string name, CancellationToken cancel = default)
    {
        if (_calibrationTables.TryGetValue(name, out var table)) return null;
        await ReadCalibrationTableList(null, cancel).ConfigureAwait(false);
        return table;
    }
    public IReadOnlyObservableDictionary<string, AsvSdrClientCalibrationTable> CalibrationTables => _calibrationTables;


    #region Dispose

    public void Dispose()
    {
        _sub1.Dispose();
        _sub2.Dispose();
        _disposeCancel.Cancel(false);
        _disposeCancel.Dispose();
        CurrentRecord.Dispose();
        IsRecordStarted.Dispose();
        SupportedModes.Dispose();
        CustomMode.Dispose();
        RecordsCount.Dispose();
        CalibrationTableRemoteCount.Dispose();
        CalibrationState.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        _disposeCancel.Cancel(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);
        await CastAndDispose(CurrentRecord).ConfigureAwait(false);
        await CastAndDispose(IsRecordStarted).ConfigureAwait(false);
        await CastAndDispose(SupportedModes).ConfigureAwait(false);
        await CastAndDispose(CustomMode).ConfigureAwait(false);
        await CastAndDispose(RecordsCount).ConfigureAwait(false);
        await CastAndDispose(CalibrationTableRemoteCount).ConfigureAwait(false);
        await CastAndDispose(CalibrationState).ConfigureAwait(false);

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