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
    private const int InternalBusyStateClearAll = 1;
    private const int InternalBusyStateUploadMission = 2;
    private const int InternalBusyStateStartMission = 3;
    
    
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

        _sub1 = Current.SubscribeAwait((x,c)=>Base.SendMissionCurrent(x));
        _sub2 = Reached.Subscribe(Base,(x,b)=>b.SendReached(x));
        
        _sub3 = baseIfc.OnMissionCount.SubscribeAwait(
            async (req, ct) => 
                await UploadMission(req, ct).ConfigureAwait(false)
        );
        _sub4 = baseIfc.OnMissionRequestList.SubscribeAwait(OnDownloadMission, AwaitOperation.Drop);
        _sub5 = baseIfc.OnMissionRequestInt.SubscribeAwait(OnReadMissionItem, AwaitOperation.Drop);
        _sub6 = baseIfc.OnMissionClearAll.SubscribeAwait(OnMissionClearAll, AwaitOperation.Drop);
        _sub7 = baseIfc.OnMissionSetCurrent.Subscribe(x=>ChangeCurrentMissionItem(x.Payload.Seq));
    }

    #region ClearItems

    public void ClearItems()
    {
        var current = Interlocked.CompareExchange(ref _internalBusyState, InternalBusyStateClearAll,
            InternalBusyStateIdle);
        if (current == InternalBusyStateClearAll)
        {
            _logger.ZLogWarning($"{Id}: Duplicate '{nameof(OnMissionClearAll)}' received. Skip it...");
            return;
        }
        if (current != InternalBusyStateIdle)
        {
            throw new InvalidOperationException($"{Id}: Receive {nameof(OnMissionClearAll)}, but now state '{current:G}. Response with error'");
        }

        try
        {
            _logger.ZLogInformation($"{Id}: Clear all mission items (local call)");
            _missionSource.Clear();
        }
        finally
        {
            Interlocked.Exchange(ref _internalBusyState, InternalBusyStateIdle);            
        }
    }
  
    private async Task ClearAll(MissionClearAllPacket req, CancellationToken token)
    {
        if (req.Payload.MissionType is not (MavMissionType.MavMissionTypeMission or MavMissionType.MavMissionTypeAll))
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionUnsupported, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }
        _missionSource.Clear();
        await Base.SendMissionAck(MavMissionResult.MavMissionAccepted, req.SystemId, req.ComponentId).ConfigureAwait(false);
    }
        
    }

    #endregion
        
    private ValueTask OnReadMissionItem(MissionRequestIntPacket req, CancellationToken cancel)
    {
        if (cancel.IsCancellationRequested)
        {
            return ValueTask.FromCanceled(cancel);
        }
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
        _logger.ZLogTrace($"{Id}: Send mission item '{item}'");
        return Base.SendMissionItemInt(item, cancel: cancel);
    }
    
    private ValueTask OnDownloadMission(MissionRequestListPacket req, CancellationToken token)
    {
        await Base.SendMissionCount((ushort)_missionSource.Count, req.SystemId, req.ComponentId).ConfigureAwait(false);
    }

    private async Task UploadMission(MissionCountPacket req, CancellationToken cancel)
    {
        cancel.ThrowIfCancellationRequested();
        
        var lastState = Interlocked.CompareExchange(ref _internalBusyState, InternalBusyStateUploadMission,
            InternalBusyStateIdle);
        if (lastState == InternalBusyStateUploadMission)
        {
            _logger.ZLogWarning($"{Id}: Duplicate '{nameof(UploadMission)}' received. Skip it...");
            return;
        }
        if (lastState != InternalBusyStateIdle)
        {
            _logger.ZLogWarning($"{Id}: Receive {nameof(UploadMission)}, but now state '{lastState:G}. Response with error'");
            await Base.SendMissionAck(MavMissionResult.MavMissionUnsupported, req.SystemId, req.ComponentId)
                .ConfigureAwait(false);
            return;
        }
        try
        {
            var builder = ImmutableArray.CreateBuilder<ServerMissionItem>(req.Payload.Count);
            _statusLogger.Info($"{Id}: begin upload '{req.Payload.Count}' items");
            var count = req.Payload.Count;
            for (int i = 0; i < count; i++)
            {
                var index = i;
                var item = await Base.RequestMissionItem((ushort) index, req.Payload.MissionType, req.SystemId, req.ComponentId, DisposeCancel).ConfigureAwait(false);
                if (i % 5 == 0)
                {
                    _statusLogger.Info($"{Id}: uploaded '{(i + 1) / count:P0}' items");
                }
                builder.Add(item);
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
            Interlocked.Exchange(ref _internalBusyState, InternalBusyStateIdle);
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
        var lastState = Interlocked.CompareExchange(ref _internalBusyState, InternalBusyStateStartMission,
            InternalBusyStateIdle);
        if (lastState == InternalBusyStateStartMission)
        {
            _logger.ZLogWarning($"{Id}: Duplicate '{nameof(StartMission)}' received. Skip it...");
            return;
        }
        if (lastState != InternalBusyStateIdle)
        {
            _logger.ZLogWarning($"{Id}: Try {nameof(StartMission)}, but now state '{lastState:G}. Response with error'");
            return;
        }

        try
        {
            if (_missionSource.Count == 0)
            {
                _statusLogger.Info($"Mission is empty");
                return;
            }

            _statusLogger.Info($"Start mission {missionIndex} of {_missionSource.Count}");
            _state.OnNext(MissionServerState.Running);
            _nextMissionIndex = missionIndex;
            _missionCancel?.Cancel(false);
            _missionCancel = new CancellationTokenSource();
            _missionThread = new Thread(MissionTick);
            _missionThread.Start(_missionCancel.Token);
        }
        catch (Exception)
        {
            _state.OnNext(MissionServerState.CompleteError);
            Interlocked.Exchange(ref _internalBusyState, InternalBusyStateIdle);            
        }
    }

    public void StopMission(CancellationToken cancel = default)
    {
        _statusLogger.Info($"Stop mission");
        _missionCancel?.Cancel(false);
        _missionCancel?.Dispose();
        _currentMissionItemCancel?.Cancel(false);
        _currentMissionItemCancel?.Dispose();
        _missionThread = null;
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

    public ValueTask ChangeCurrentMissionItem(ushort index)
    {
        _nextMissionIndex = index;
        _currentMissionItemCancel?.Cancel(false);
        _current.OnNext(index);
        return ValueTask.CompletedTask;
    }
    private async void MissionTick(object? obj)
    {
        try
        {
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
                    _statusLogger.Info($"Mission completed");
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
        finally
        {
            Interlocked.Exchange(ref _internalBusyState, InternalBusyStateIdle);
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

