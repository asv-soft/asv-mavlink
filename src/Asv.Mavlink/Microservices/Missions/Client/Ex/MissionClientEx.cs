using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using R3;
using ZLogger;
using ObservableExtensions = System.ObservableExtensions;
using Unit = System.Reactive.Unit;

namespace Asv.Mavlink;

public class MissionClientExConfig:MissionClientConfig
{
    public int DeviceUploadTimeoutMs { get; set; } = 3000;
}

public class MissionClientEx : IMissionClientEx, IDisposable
{
    private readonly ILogger _logger;
    private readonly IMissionClient _client;
    private readonly MissionClientExConfig _config;
    private readonly SourceCache<MissionItem, ushort> _missionSource;
    private readonly RxValue<bool> _isMissionSynced;
    private readonly RxValue<double> _allMissionDistance;
    private readonly TimeSpan _deviceUploadTimeout;
    private readonly IDisposable _disposeIt;
    private CancellationTokenSource _disposeCancel;

    public MissionClientEx(
        IMissionClient client, 
        MissionClientExConfig config)
    {
        _logger = client.Core.Log.CreateLogger<MissionClientEx>();
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _disposeCancel = new CancellationTokenSource();
        _deviceUploadTimeout = TimeSpan.FromMilliseconds(_config.DeviceUploadTimeoutMs);
        _missionSource = new SourceCache<MissionItem, ushort>(i=>i.Index);
        _isMissionSynced = new RxValue<bool>(false);
        _allMissionDistance = new RxValue<double>(double.NaN);
        MissionItems = _missionSource.Connect().DisposeMany().Publish().RefCount();
        _isMissionSynced.Subscribe(_ => UpdateMissionsDistance());
        _disposeIt = Disposable.Combine(_missionSource, _isMissionSynced, _allMissionDistance, _isMissionSynced,_disposeCancel);
    }
    
    public string Name => $"{Base.Name}Ex";
    public IMissionClient Base => _client;
    
    public IObservable<IChangeSet<MissionItem, ushort>> MissionItems { get; }
    public IRxValue<bool> IsSynced => _isMissionSynced;
    public IRxValue<ushort> Current => _client.MissionCurrent;
    public IRxValue<ushort> Reached => _client.MissionReached;
    public IRxValue<double> AllMissionsDistance => _allMissionDistance;
    
    public Task SetCurrent(ushort index, CancellationToken cancel = default)
    {
        return _client.MissionSetCurrent(index, cancel);
    }

    public async Task<MissionItem[]> Download(CancellationToken cancel, Action<double> progress = null)
    {
        _logger.ZLogInformation($"Begin download mission");
        progress?.Invoke(0);
        var count = await _client.MissionRequestCount(cancel).ConfigureAwait(false);
        var result = new MissionItem[count];
        _missionSource.Clear();
        var current = 0;
        for (ushort i = 0; i < count; i++)
        {
            var item = await _client.MissionRequestItem(i,cancel).ConfigureAwait(false);
            result[i] = AddMissionItem(item);
            current++;
            progress?.Invoke((double)current / count);
        }
        _isMissionSynced.OnNext(true);
        return result;
    }

    public async Task ClearRemote( CancellationToken cancel)
    {
        _logger.ZLogInformation($"Begin clear mission");
        _missionSource.Clear();
        await _client.ClearAll(MavMissionType.MavMissionTypeMission, cancel).ConfigureAwait(false);
        _isMissionSynced.OnNext(true);
    }

    public async Task Upload(CancellationToken cancel, Action<double> progress = null)
    {
        _logger.ZLogInformation($"Begin upload mission");
        progress?.Invoke(0);
        await _client.ClearAll(MavMissionType.MavMissionTypeMission ,cancel).ConfigureAwait(false);

        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<Unit>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var current = 0;
        var lastUpdateTime = DateTime.Now;
        using var checkTimer = Base.Core.TimeProvider.CreateTimer(x =>
        {
            if (DateTime.Now - lastUpdateTime > _deviceUploadTimeout)
            {
                _logger.ZLogWarning($"Mission upload timeout");
                tcs.TrySetException(new Exception($"Mission upload timeout"));
            }
        }, null, _deviceUploadTimeout, _deviceUploadTimeout); 
        using var sub1 = _client.OnMissionRequest.Subscribe(req =>
        {
            _logger.ZLogDebug($"UAV request {req.Seq} item");
            lastUpdateTime = DateTime.Now;
            current++;
            progress?.Invoke((double)(current) / _missionSource.Count);
            var item = _missionSource.Lookup(req.Seq);
            if (item.HasValue == false)
            {
                tcs.TrySetException(new Exception($"Requested mission item with index '{req.Seq}' not found in local store"));
                return;
            }
            _client.WriteMissionItem(item.Value, cancel);

        } );

        using var sub2 =_client.OnMissionAck.Subscribe(p =>
        {
            lastUpdateTime = DateTime.Now;
            if (p.Type == MavMissionResult.MavMissionAccepted)
            {
                tcs.TrySetResult(Unit.Default);
            }
            else
            {
                tcs.TrySetException(new Exception($"Error to upload mission to vehicle:{p.Type:G}"));
            }
                
        });

        await _client.MissionSetCount((ushort)_missionSource.Count, cancel).ConfigureAwait(false);
        await tcs.Task.ConfigureAwait(false);
        _isMissionSynced.OnNext(true);
    }

    private CancellationToken DisposeCancel => _disposeCancel.Token;

    public MissionItem Create()
    {
        return AddMissionItem(new MissionItemIntPayload
        {
            Seq = (ushort)_missionSource.Count,
            TargetComponent = _client.Identity.Self.ComponentId,
            TargetSystem = _client.Identity.Self.SystemId,
        });
    }

    public void Remove(ushort index)
    {
        _isMissionSynced.OnNext(false);
        _missionSource.RemoveKey(index);
    }

    public void ClearLocal()
    {
        _isMissionSynced.OnNext(false);
        _missionSource.Clear();
    }

    private MissionItem AddMissionItem(MissionItemIntPayload item)
    {
        var missionItem = new MissionItem(item);
        _missionSource.AddOrUpdate(missionItem);
        missionItem.OnChanged.Subscribe(_ =>
        {
            _isMissionSynced.OnNext(false);
        });// subscribe will be disposed by MissionItem 
        return missionItem;
    }
    
    private void UpdateMissionsDistance()
    {
        var missions = _missionSource.Items.Where(i =>
                i.Command.Value == MavCmd.MavCmdNavWaypoint || i.Command.Value == MavCmd.MavCmdNavSplineWaypoint)
            .ToArray();
        var dist = 0.0;
        for (var i = 0; i < missions.Length - 1; i++)
        {
            dist += missions[i].Location.Value.DistanceTo(missions[i + 1].Location.Value);
        }
        _allMissionDistance.OnNext(dist / 1000.0);
    }

    public void Dispose()
    {
        _disposeCancel.Cancel(false);
        _disposeIt.Dispose();
    }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;

    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}