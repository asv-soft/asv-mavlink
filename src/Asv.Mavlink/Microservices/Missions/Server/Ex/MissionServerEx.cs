using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

public sealed class MissionServerEx : MavlinkMicroserviceServer, IMissionServerEx
{
    private const int InternalBusyStateIdle = 0;
    private const int InternalBusyStateClearAll = 2;
    
    
    private int _internalBusyState;
    
    
    private readonly IStatusTextServer _statusLogger;
    private readonly ILogger _logger;
    private readonly ObservableList<ServerMissionItem> _missionSource;
    private readonly ReactiveProperty<ushort> _current = new(0);
    private readonly ReactiveProperty<ushort> _reached = new(0);
    private readonly ReactiveProperty<MissionServerState> _state = new(MissionServerState.Idle);
    private CancellationTokenSource? _missionCancel;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;
    private readonly IDisposable _sub5;
    private readonly IDisposable _sub6;
    private readonly IDisposable _sub7;
    private readonly object _sync = new();
    private Thread? _missionThread;
    private readonly ConcurrentDictionary<ushort,MissionTaskDelegate> _registry = new();
    private ushort _nextMissionIndex;
    private CancellationTokenSource? _currentMissionItemCancel;


    public MissionServerEx(IMissionServer baseIfc, IStatusTextServer status) :
        base("MISSION", baseIfc.Identity, baseIfc.Core)
    {
        Base = baseIfc ?? throw new ArgumentNullException(nameof(baseIfc));
        _statusLogger = status ?? throw new ArgumentNullException(nameof(status));
        _logger = baseIfc.Core.LoggerFactory.CreateLogger<MissionServerEx>();
        _missionSource = [];

        _sub1 = Current.Subscribe(Base,(x,b)=>b.SendMissionCurrent(x));
        _sub2 = Reached.Subscribe(Base,(x,b)=>b.SendReached(x));
        
        _sub3 = baseIfc.OnMissionCount.SubscribeAwait(
            async (req, ct) => 
                await UploadMission(req, ct).ConfigureAwait(false)
        );
        _sub4 = baseIfc.OnMissionRequestList.SubscribeAwait(
            async (req, ct) => 
                await DownloadMission(req, ct).ConfigureAwait(false)
        );
        _sub5 = baseIfc.OnMissionRequestInt.SubscribeAwait(
            async (req, ct) => 
                await ReadMissionItem(req, ct).ConfigureAwait(false)
        );
        _sub6 = baseIfc.OnMissionClearAll.SubscribeAwait(
            async (req, ct) => 
                await ClearAll(req, ct).ConfigureAwait(false)
        );
        _sub7 = baseIfc.OnMissionSetCurrent.Subscribe(x=>ChangeCurrentMissionItem(x.Payload.Seq));
    }

    
  
    private async Task ClearAll(MissionClearAllPacket req, CancellationToken token)
    {
        var current = Interlocked.CompareExchange(ref _internalBusyState, InternalBusyStateClearAll,
            InternalBusyStateIdle);
        if (current == InternalBusyStateClearAll)
        {
            _logger.ZLogTrace($"{Id}: Duplicate '{nameof(ClearAll)}' received. Skip it...");
            return;
        }
        if (current != InternalBusyStateIdle)
        {
            _logger.ZLogTrace($"{Id}: Busy state '{current:G}'");
            await Base.SendMissionAck(MavMissionResult.MavMissionDenied, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }
        
        
        if (req.Payload.MissionType is not (MavMissionType.MavMissionTypeMission or MavMissionType.MavMissionTypeAll))
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionUnsupported, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }
        _missionSource.Clear();
        await Base.SendMissionAck(MavMissionResult.MavMissionAccepted, req.SystemId, req.ComponentId).ConfigureAwait(false);
    }
        
    private async Task ReadMissionItem(MissionRequestIntPacket req, CancellationToken token)
    {
        if (req.Payload.MissionType != MavMissionType.MavMissionTypeMission)
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionUnsupported, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }
        
        if (req.Payload.Seq >= _missionSource.Count)
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionInvalid, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }
        var item = _missionSource[req.Payload.Seq];
        await Base.SendMissionItemInt(item).ConfigureAwait(false);
    }
    private async Task DownloadMission(MissionRequestListPacket req, CancellationToken token)
    {
        await Base.SendMissionCount((ushort)_missionSource.Count, req.SystemId, req.ComponentId).ConfigureAwait(false);
    }

    private async Task UploadMission(MissionCountPacket req, CancellationToken token)
    {
        if (Interlocked.CompareExchange(ref _internalBusyState, 1, 0) != 0)
        {
            _logger.ZLogTrace($"{Id}: Duplicate '{nameof(MissionCountPacket)}' received. Skip it...");
            return;
        }
        _missionSource.Clear();
        _statusLogger.Info($"{Id}: begin upload '{req.Payload.Count}' items");
        try
        {
            var count = req.Payload.Count;
            for (int i = 0; i < count; i++)
            {
                var index = i;
                var item = await Base.RequestMissionItem((ushort) index, req.Payload.MissionType, req.SystemId, req.ComponentId, DisposeCancel).ConfigureAwait(false);
                if (i % 5 == 0)
                {
                    _statusLogger.Info($"{Id}: uploaded '{(i + 1) / count:P0}' items");
                }
                _missionSource.Add(item);
            }
            _statusLogger.Info($"{Id}: uploaded '{count}' items");
            await Base.SendMissionAck(MavMissionResult.MavMissionAccepted, req.SystemId, req.ComponentId).ConfigureAwait(false);
        }
        catch (Exception)
        {
            _logger.ZLogError($"{Id}: upload error");
            await Base.SendMissionAck(MavMissionResult.MavMissionError, req.SystemId, req.ComponentId).ConfigureAwait(false);
        }
        finally
        {
            Interlocked.Exchange(ref _internalBusyState, 0);
        }
    }

    public IMissionServer Base { get; }

    public ReadOnlyReactiveProperty<ushort> Current => _current;

    public ReadOnlyReactiveProperty<ushort> Reached => _reached;

    public IReadOnlyObservableList<ServerMissionItem> Items => _missionSource;
    public void AddItems(IEnumerable<ServerMissionItem> items)
    {
        var serverMissionItems = items as ServerMissionItem[] ?? items.ToArray();
        if (_missionSource.Count + serverMissionItems.Length > ushort.MaxValue + 1)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        _missionSource.AddRange(serverMissionItems);
        EnsureIndexCorrected();
    }

    public void RemoveItems(IEnumerable<ServerMissionItem> items)
    {
        foreach (var item in items)
        {
            _missionSource.Remove(item);    
        }
        EnsureIndexCorrected();
    }

    public void ClearItems()
    {
        
        _missionSource.Clear();
    }

    private void EnsureIndexCorrected()
    {
        for (var i = 0; i < _missionSource.Count; i++)
        {
            _missionSource[i].Seq = (ushort)i;
        }
    }

    public ImmutableArray<ServerMissionItem> GetItemsSnapshot()
    {
        EnsureIndexCorrected();
        return [.._missionSource];
    }

    public ReadOnlyReactiveProperty<MissionServerState> State => _state;

    public void StartMission(ushort missionIndex = 0)
    {
        try
        {
            Interlocked.CompareExchange(ref _state, MissionServerState.Idle, MissionServerState.CompleteSuccess);
        }
        finally
        {
            
        }
        
        _statusLogger.Info($"Start mission {missionIndex} of {_missionSource.Count}");
        if (_state.Value == MissionServerState.Running)
        {
            _statusLogger.Info($"Mission already running");
            return;
        }
        if (_missionSource.Count == 0)
        {
            _statusLogger.Info($"Mission is empty");
            return;
        }
        lock (_sync)
        {
            _nextMissionIndex = missionIndex;
            _missionCancel?.Cancel(false);
            _missionCancel = new CancellationTokenSource();
            _missionThread = new Thread(MissionTick);
            _missionThread.Start(_missionCancel.Token);
        }
    }

    public void StopMission(CancellationToken cancel = default)
    {
        _statusLogger.Info($"Stop mission");
        lock (_sync)
        {
            _missionCancel?.Cancel(false);
            _missionThread = null;
        }
        _state.OnNext(MissionServerState.Idle);
    }

    public MissionTaskDelegate? this[MavCmd cmd]
    {
        set
        {
            if (value == null)
            {
                _registry.TryRemove((ushort)cmd, out _);
                return;
            }
            _registry.AddOrUpdate((ushort)cmd, value, (_, _) => value);
        }
    }

    public IEnumerable<MavCmd> SupportedCommands => _registry.Keys.Select(x => (MavCmd)x);


    public void ChangeCurrentMissionItem(ushort index)
    {
        _nextMissionIndex = index;
        _currentMissionItemCancel?.Cancel(false);
        _current.OnNext(index);
    }
    private async void MissionTick(object? obj)
    {
        try
        {
            Interlocked.CompareExchange(ref _internalBusyState, 1);
            var cancel = (CancellationToken)(obj ?? throw new ArgumentNullException(nameof(obj)));
            var items = GetItemsSnapshot();
            if (items.Length == 0)
            {
                _statusLogger.Info($"Mission is empty");
                _state.OnNext(MissionServerState.CompleteSuccess);
                return;
            }
            _state.OnNext(MissionServerState.Running);
            while (true)
            {
                var missionIndex = _nextMissionIndex;
                ++_nextMissionIndex; // ! important here
                if (missionIndex >= items.Length)
                {
                    _statusLogger.Info($"Mission already completed");
                    _state.OnNext(MissionServerState.CompleteSuccess);
                    return;
                }

                _current.OnNext(missionIndex);
                if (cancel.IsCancellationRequested)
                {
                    _statusLogger.Info($"Mission canceled");
                    _state.OnNext(MissionServerState.Canceled);
                    return;
                }

                var item = items[missionIndex];

                if (_registry.TryGetValue((ushort)item.Command, out var task))
                {
                    _statusLogger.Info($"Run '{item.Command:G}'");
                    _currentMissionItemCancel = new CancellationTokenSource();
                    using var linked =
                        CancellationTokenSource.CreateLinkedTokenSource(cancel, _currentMissionItemCancel.Token,
                            DisposeCancel);
                    try
                    {
                        await task(item, linked.Token).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        _logger.ZLogError(e, $"Error at mission tick");
                        _statusLogger.Error($"Mission error: {e.Message}");
                    }
                }
                else
                {
                    _statusLogger.Info($"Skip unsupported command '{item.Command}'");
                }
                _reached.OnNext(missionIndex);
                
            }
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error at mission tick");
            _statusLogger.Error($"Error at mission tick: {e.Message}");
            _state.OnNext(MissionServerState.CompleteError);
        }
        
    }

    

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
            _sub4.Dispose();
            _sub5.Dispose();
            _sub6.Dispose();
            _sub7.Dispose();
            _current.Dispose();
            _reached.Dispose();
            _state.Dispose();
            _currentMissionItemCancel?.Cancel(false);
            _currentMissionItemCancel?.Dispose();
            _missionCancel?.Cancel(false);
            _missionCancel?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);
        await CastAndDispose(_sub4).ConfigureAwait(false);
        await CastAndDispose(_sub5).ConfigureAwait(false);
        await CastAndDispose(_sub6).ConfigureAwait(false);
        await CastAndDispose(_sub7).ConfigureAwait(false);
        await CastAndDispose(_current).ConfigureAwait(false);
        await CastAndDispose(_reached).ConfigureAwait(false);
        await CastAndDispose(_state).ConfigureAwait(false);
        
        _currentMissionItemCancel?.Cancel(false);
        _currentMissionItemCancel?.Dispose();
        _missionCancel?.Cancel(false);
        _missionCancel?.Dispose();

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

