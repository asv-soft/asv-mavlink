using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive.Concurrency;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class MissionServerEx : MavlinkMicroserviceServer, IMissionServerEx
{
    private readonly IStatusTextServer _statusLogger;
    private readonly ILogger _logger;
    private readonly SourceCache<ServerMissionItem, ushort> _missionSource;
    private double _busy;
    private readonly IDisposable _disposeIt;

    public MissionServerEx(IMissionServer baseIfc, IStatusTextServer status) :
        base("MISSION", baseIfc.Identity, baseIfc.Core)
    {
        _logger = baseIfc.Core.Log.CreateLogger<MissionServerEx>();
        Base = baseIfc;
        var builder = Disposable.CreateBuilder();
        _statusLogger = status ?? throw new ArgumentNullException(nameof(status));
        _missionSource = new SourceCache<ServerMissionItem, ushort>(x => x.Seq).AddTo(ref builder);

        Current = new RxValueBehaviour<ushort>(0).AddTo(ref builder);
        Current.Subscribe(x=>Base.SendMissionCurrent(x)).AddTo(ref builder);

        Reached = new RxValueBehaviour<ushort>(0).AddTo(ref builder);
        Reached.Subscribe(x=>Base.SendReached(x)).AddTo(ref builder);
        
        baseIfc.OnMissionCount.Subscribe(UploadMission).AddTo(ref builder);
        baseIfc.OnMissionRequestList.Subscribe(DownloadMission).AddTo(ref builder);
        baseIfc.OnMissionRequestInt.Subscribe(ReadMissionItem).AddTo(ref builder);
        baseIfc.OnMissionClearAll.Subscribe(ClearAll).AddTo(ref builder);
        baseIfc.OnMissionSetCurrent.Subscribe(SetCurrent).AddTo(ref builder);
        _disposeIt = builder.Build();
    }

    private void SetCurrent(MissionSetCurrentPacket req)
    {
        var item = _missionSource.Lookup(req.Payload.Seq);
        if (item.HasValue == false)
        {
            _logger.ZLogWarning($"{LogRecv}: '{req.Payload.Seq}' not found");
            _statusLogger.Info($"{LogSend}: item '{req.Payload.Seq}' not found");
            return;
        }
        Current.OnNext(req.Payload.Seq);
    }
  
    private async void ClearAll(MissionClearAllPacket req)
    {
        if (req.Payload.MissionType is not (MavMissionType.MavMissionTypeMission or MavMissionType.MavMissionTypeAll))
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionUnsupported, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }
        _missionSource.Clear();
        await Base.SendMissionAck(MavMissionResult.MavMissionAccepted, req.SystemId, req.ComponentId).ConfigureAwait(false);
    }
        
    private async void ReadMissionItem(MissionRequestIntPacket req)
    {
        if (req.Payload.MissionType != MavMissionType.MavMissionTypeMission)
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionUnsupported, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }

        var item = _missionSource.Lookup(req.Payload.Seq);
        if (item.HasValue == false)
        {
            await Base.SendMissionAck(MavMissionResult.MavMissionInvalid, req.SystemId, req.ComponentId).ConfigureAwait(false);
            return;
        }

        await Base.SendMissionItemInt(item.Value).ConfigureAwait(false);
    }
    private async void DownloadMission(MissionRequestListPacket req)
    {
        await Base.SendMissionCount((ushort)_missionSource.Count, req.SystemId, req.ComponentId).ConfigureAwait(false);
    }

    private async void UploadMission(MissionCountPacket req)
    {
        if (Interlocked.CompareExchange(ref _busy, 1, 0) != 0)
        {
            _logger.ZLogTrace($"{LogSend}: Duplicate '{nameof(MissionCountPacket)}' received. Skip it...");
            return;
        }
        _missionSource.Clear();
        _statusLogger.Info($"{LogSend}: begin upload '{req.Payload.Count}' items");
        try
        {
            var count = req.Payload.Count;
            for (ushort i = 0; i < count; i++)
            {
                var index = i;
                var item = await Base.RequestMissionItem(index, req.Payload.MissionType, req.SystemId, req.ComponentId, DisposeCancel).ConfigureAwait(false);
                if (i % 5 == 0)
                {
                    _statusLogger.Info($"{LogSend}: uploaded '{(i + 1) / count:P0}' items");
                }
                _missionSource.AddOrUpdate(item);
            }
            _statusLogger.Info($"{LogSend}: uploaded '{count}' items");
            await Base.SendMissionAck(MavMissionResult.MavMissionAccepted, req.SystemId, req.ComponentId).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError($"{LogSend}: upload error");
            await Base.SendMissionAck(MavMissionResult.MavMissionError, req.SystemId, req.ComponentId).ConfigureAwait(false);
        }
        finally
        {
            Interlocked.Exchange(ref _busy, 0);
        }
    }

    public IMissionServer Base { get; }
    public IRxEditableValue<ushort> Current { get; }
    public IRxEditableValue<ushort> Reached { get; }
    public IObservable<IChangeSet<ServerMissionItem, ushort>> Items => _missionSource.Connect();
    public void AddItems(IEnumerable<ServerMissionItem> items)
    {
        _missionSource.AddOrUpdate(items);
    }

    public void RemoveItems(IEnumerable<ServerMissionItem> items)
    {
        _missionSource.Remove(items);
    }

    public ImmutableArray<ServerMissionItem> GetItemsSnapshot()
    {
        return  [.._missionSource.Items];
    }

    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}

