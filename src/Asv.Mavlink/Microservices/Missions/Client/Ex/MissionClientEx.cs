using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using DynamicData;
using NLog;

namespace Asv.Mavlink;

public class MissionClientExConfig
{
    public int DeviceUploadTimeoutMs { get; set; } = 3000;
}

public class MissionClientEx : DisposableOnceWithCancel, IMissionClientEx
{
    public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IMissionClient _client;
    private readonly MissionClientExConfig _config;
    private readonly SourceCache<MissionItem, ushort> _missionSource;
    private readonly RxValue<bool> _isMissionSynced;
    private readonly RxValue<double> _allMissionDistance;
    private readonly TimeSpan _deviceUploadTimeout;

    public MissionClientEx(IMissionClient client, MissionClientExConfig config = null)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _config = config ?? new MissionClientExConfig();
        _client.DisposeItWith(Disposable);
        _deviceUploadTimeout = TimeSpan.FromMilliseconds(_config.DeviceUploadTimeoutMs);
        _missionSource = new SourceCache<MissionItem, ushort>(_=>_.Index).DisposeItWith(Disposable);
        _isMissionSynced = new RxValue<bool>(false).DisposeItWith(Disposable);
        _allMissionDistance = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        MissionItems = _missionSource.Connect().DisposeMany().Publish().RefCount();
        _isMissionSynced.Subscribe(_ => UpdateMissionsDistance()).DisposeItWith(Disposable);
    }
    
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
        Logger.Info($"Begin download mission");
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
        Logger.Info($"Begin clear mission");
        _missionSource.Clear();
        await _client.ClearAll(MavMissionType.MavMissionTypeMission, cancel).ConfigureAwait(false);
        _isMissionSynced.OnNext(true);
    }

    public async Task Upload(CancellationToken cancel, Action<double> progress = null)
    {
        Logger.Info($"Begin upload mission");
        progress?.Invoke(0);
        await _client.ClearAll(MavMissionType.MavMissionTypeMission ,cancel).ConfigureAwait(false);

        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<Unit>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());
        var current = 0;
        var lastUpdateTime = DateTime.Now;
        using var checkTimer = Observable.Timer(_deviceUploadTimeout, _deviceUploadTimeout)
            .Subscribe(_ =>
            {
                if (DateTime.Now - lastUpdateTime > _deviceUploadTimeout)
                {
                    Logger.Warn($"Mission upload timeout");
                    tcs.TrySetException(new Exception($"Mission upload timeout"));
                }
            });
        using var sub1 = _client.OnMissionRequest.Subscribe(req =>
        {
            Logger.Debug($"UAV request {req.Seq} item");
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

        using var sub2 =_client.OnMissionAck.Subscribe(_ =>
        {
            lastUpdateTime = DateTime.Now;
            if (_.Type == MavMissionResult.MavMissionAccepted)
            {
                tcs.TrySetResult(Unit.Default);
            }
            else
            {
                tcs.TrySetException(new Exception($"Error to upload mission to vehicle:{_.Type:G}"));
            }
                
        });

        await _client.MissionSetCount((ushort)_missionSource.Count, cancel).ConfigureAwait(false);
        await tcs.Task.ConfigureAwait(false);
        _isMissionSynced.OnNext(true);
    }

    public MissionItem Create()
    {
        return AddMissionItem(new MissionItemIntPayload
        {
            Seq = (ushort)_missionSource.Count,
            TargetComponent = _client.Identity.ComponentId,
            TargetSystem = _client.Identity.SystemId,
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
        }).DisposeItWith(Disposable);
        return missionItem;
    }
    
    private void UpdateMissionsDistance()
    {
        var missions = _missionSource.Items.Where(_ =>
                _.Command.Value == MavCmd.MavCmdNavWaypoint || _.Command.Value == MavCmd.MavCmdNavSplineWaypoint)
            .ToArray();
        var dist = 0.0;
        for (var i = 0; i < missions.Length - 1; i++)
        {
            dist += missions[i].Location.Value.DistanceTo(missions[i + 1].Location.Value);
        }
        _allMissionDistance.OnNext(dist / 1000.0);
    }
    
    
}