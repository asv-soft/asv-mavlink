using System;
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
    private readonly IStatusTextServer _statusLogger;
    private readonly ILogger _logger;
    private readonly ObservableList<ServerMissionItem> _missionSource;
    private double _busy;

    public MissionServerEx(IMissionServer baseIfc, IStatusTextServer status) :
        base("MISSION", baseIfc.Identity, baseIfc.Core)
    {
        Base = baseIfc ?? throw new ArgumentNullException(nameof(baseIfc));
        _statusLogger = status ?? throw new ArgumentNullException(nameof(status));
        _logger = baseIfc.Core.LoggerFactory.CreateLogger<MissionServerEx>();
        _missionSource = new ObservableList<ServerMissionItem>();

        Current = new ReactiveProperty<ushort>(0);
        _sub1 = Current.Subscribe(Base,(x,b)=>b.SendMissionCurrent(x));

        Reached = new ReactiveProperty<ushort>(0);
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
        _sub7 = baseIfc.OnMissionSetCurrent.Subscribe(SetCurrent);
    }

    private void SetCurrent(MissionSetCurrentPacket req)
    {
        if (req.Payload.Seq >= _missionSource.Count)
        {
            _logger.ZLogWarning($"{Id}: '{req.Payload.Seq}' not found");
            _statusLogger.Info($"{Id}: item '{req.Payload.Seq}' not found");
            return;
        }
        Current.OnNext(req.Payload.Seq);
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
        if (Interlocked.CompareExchange(ref _busy, 1, 0) != 0)
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
            Interlocked.Exchange(ref _busy, 0);
        }
    }

    public IMissionServer Base { get; }
    public ReactiveProperty<ushort> Current { get; }
    public ReactiveProperty<ushort> Reached { get; }
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
    
    #region Dispose
    
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;
    private readonly IDisposable _sub5;
    private readonly IDisposable _sub6;
    private readonly IDisposable _sub7;
    
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
            Current.Dispose();
            Reached.Dispose();
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
        await CastAndDispose(Current).ConfigureAwait(false);
        await CastAndDispose(Reached).ConfigureAwait(false);

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

