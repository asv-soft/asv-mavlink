using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;


namespace Asv.Mavlink;

public class MissionClientExConfig:MissionClientConfig
{
    private int _deviceUploadTimeoutMs = 3000;

    public int DeviceUploadTimeoutMs
    {
        get => _deviceUploadTimeoutMs;
        set => _deviceUploadTimeoutMs = value >= 0 
            ? value 
            : throw new ArgumentOutOfRangeException(nameof(DeviceUploadTimeoutMs));
    }
}

public sealed class MissionClientEx : IMissionClientEx, IDisposable, IAsyncDisposable
{
    private readonly ILogger _logger;
    private readonly IMissionClient _client;
    private readonly ObservableList<MissionItem> _missionSource;
    private readonly ReactiveProperty<bool> _isMissionSynced;
    private readonly ReactiveProperty<double> _allMissionDistance;
    private readonly TimeSpan _deviceUploadTimeout;
    private readonly CancellationTokenSource _disposeCancel;

    public MissionClientEx(
        IMissionClient client, 
        MissionClientExConfig config)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = client.Core.LoggerFactory.CreateLogger<MissionClientEx>();
        var config1 = config ?? throw new ArgumentNullException(nameof(config));
        _disposeCancel = new CancellationTokenSource();
        _deviceUploadTimeout = TimeSpan.FromMilliseconds(config1.DeviceUploadTimeoutMs);
        _missionSource = new ObservableList<MissionItem>();
        _isMissionSynced = new ReactiveProperty<bool>(false);
        _allMissionDistance = new ReactiveProperty<double>(double.NaN);
        _obs1 = _isMissionSynced.Subscribe(_ => UpdateMissionsDistance());
    }
    
    public string TypeName => $"{Base.TypeName}Ex";
    public IMissionClient Base => _client;
    public IReadOnlyObservableList<MissionItem> MissionItems => _missionSource;
    public ReadOnlyReactiveProperty<bool> IsSynced => _isMissionSynced;
    public ReadOnlyReactiveProperty<ushort> Current => _client.MissionCurrent;
    public ReadOnlyReactiveProperty<ushort> Reached => _client.MissionReached;
    public ReadOnlyReactiveProperty<double> AllMissionsDistance => _allMissionDistance;
    
    public Task SetCurrent(ushort index, CancellationToken cancel = default)
    {
        return _client.MissionSetCurrent(index, cancel);
    }

    public async Task<MissionItem[]> Download(CancellationToken cancel, Action<double>? progress = null)
    {
        _logger.ZLogInformation($"Begin download mission");
        progress?.Invoke(0);
        var count = await _client.MissionRequestCount(cancel).ConfigureAwait(false);
        var result = new MissionItem[count];
        _missionSource.Clear();
        var current = 0;
        for (int i = 0; i < count; i++)
        {
            var item = await _client.MissionRequestItem((ushort) i,cancel).ConfigureAwait(false);
            result[i] = AddMissionItem(item);
            current++;
            progress?.Invoke((double)current / count);
        }

        if (result.Length != count)
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionError, cancel: cancel).ConfigureAwait(false);
            return [];
        }

        await Base.SendMissionAck(MavMissionResult.MavMissionAccepted, cancel: cancel).ConfigureAwait(false);
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

    public async Task Upload(CancellationToken cancel, Action<double>? progress = null)
    {
        _logger.ZLogInformation($"Begin upload mission");
        progress?.Invoke(0);
        await _client.ClearAll(MavMissionType.MavMissionTypeMission ,cancel).ConfigureAwait(false);

        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<Unit>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var current = 0;
        var lastUpdateTime = DateTime.Now;
        await using var checkTimer = Base.Core.TimeProvider.CreateTimer(x =>
        {
            if (DateTime.Now - lastUpdateTime <= _deviceUploadTimeout)
            {
                return;
            }
            
            _logger.ZLogWarning($"Mission upload timeout");
            tcs.TrySetException(new Exception("Mission upload timeout"));
        }, null, _deviceUploadTimeout, _deviceUploadTimeout); 
        using var sub1 = _client.OnMissionRequest.SubscribeAwait(async (req, ct) =>
        {
            _logger.ZLogDebug($"UAV request {req.Seq} item");
            lastUpdateTime = DateTime.Now;
            current++;
            progress?.Invoke((double)(current) / _missionSource.Count);
            var item = _missionSource.FirstOrDefault(i => i.Index == req.Seq);
            if (item == null)
            {
                tcs.TrySetException(new Exception($"Requested mission item with index '{req.Seq}' not found in local store"));
                return;
            }
            
            await _client.WriteMissionItem(item, cancel).ConfigureAwait(false);
        } );

        using var sub2 = _client.OnMissionAck.Subscribe(p =>
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

        await _client.MissionSetCount((ushort) _missionSource.Count, cancel).ConfigureAwait(false);
        await tcs.Task.ConfigureAwait(false);
        _isMissionSynced.OnNext(true);
    }

    private CancellationToken DisposeCancel => _disposeCancel.Token;

    public MissionItem Create()
    {
        if (_missionSource.Count > ushort.MaxValue)
        {
            throw new ArgumentOutOfRangeException();
        } 
        
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
        _missionSource.RemoveAt(index);
        EnsureIndex();
    }

    private void EnsureIndex()
    {
        for (var i = 0; i < _missionSource.Count; i++)
        {
            var i1 = i;
            _missionSource[i].Edit(x=>x.Seq = (ushort)i1);
        }
    }

    public void ClearLocal()
    {
        _isMissionSynced.OnNext(false);
        _missionSource.Clear();
        EnsureIndex();
    }

    private MissionItem AddMissionItem(MissionItemIntPayload item)
    {
        var missionItem = new MissionItem(item);
        _missionSource.Add(missionItem);
        EnsureIndex();
        missionItem.OnChanged.Subscribe(_ =>
        {
            _isMissionSynced.OnNext(false);
        });// subscribe will be disposed by MissionItem 
        return missionItem;
    }
    
    private void  UpdateMissionsDistance()
    {
        var missions = _missionSource.Where(i =>
                i.Command.Value == MavCmd.MavCmdNavWaypoint || i.Command.Value == MavCmd.MavCmdNavSplineWaypoint)
            .ToArray();
        var dist = 0.0;
        for (var i = 0; i < missions.Length - 1; i++)
        {
            dist += missions[i].Location.Value.DistanceTo(missions[i + 1].Location.Value);
        }
        _allMissionDistance.OnNext(dist / 1000.0);
    }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;

    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
    
    #region Dispose
    
    private readonly IDisposable _obs1;
    
    public void Dispose()
    {
        _isMissionSynced.Dispose();
        _allMissionDistance.Dispose();
        _disposeCancel.Dispose();
        _obs1.Dispose();
        foreach (var item in _missionSource)
        {
            item.Dispose();
        }
    }
    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_isMissionSynced).ConfigureAwait(false);
        await CastAndDispose(_allMissionDistance).ConfigureAwait(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);
        await CastAndDispose(_obs1).ConfigureAwait(false);

        var cached = _missionSource.ToImmutableArray();
        _missionSource.Clear();
        foreach (var item in cached)
        {
            await item.DisposeAsync().ConfigureAwait(false);
        }
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