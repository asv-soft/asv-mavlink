using System;
using System.Reactive.Concurrency;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using NLog;

namespace Asv.Mavlink;

public class MissionServerEx : MavlinkMicroserviceServer, IMissionServerEx
{
    private readonly IStatusTextServer _statusLogger;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly SourceCache<ServerMissionItem, ushort> _missionSource;
    private double _busy;

    public MissionServerEx(IMissionServer baseIfc, IStatusTextServer status, IMavlinkV2Connection connection, MavlinkServerIdentity identity,
        IPacketSequenceCalculator seq, IScheduler rxScheduler) :
        base("MISSION", connection, identity, seq, rxScheduler)
    {
        Base = baseIfc;
        _statusLogger = status ?? throw new ArgumentNullException(nameof(status));
        _missionSource = new SourceCache<ServerMissionItem, ushort>(x => x.Seq).DisposeItWith(Disposable);

        Current = new RxValue<ushort>(0).DisposeItWith(Disposable);
        Current.Subscribe(x=>Base.SendMissionCurrent(x)).DisposeItWith(Disposable);

        Reached = new RxValue<ushort>().DisposeItWith(Disposable);
        Reached.Subscribe(x=>Base.SendReached(x)).DisposeItWith(Disposable);
        
        baseIfc.OnMissionCount.Subscribe(UploadMission).DisposeItWith(Disposable);
        baseIfc.OnMissionRequestList.Subscribe(DownloadMission).DisposeItWith(Disposable);
        baseIfc.OnMissionRequestInt.Subscribe(ReadMissionItem).DisposeItWith(Disposable);
        baseIfc.OnMissionClearAll.Subscribe(ClearAll).DisposeItWith(Disposable);
        baseIfc.OnMissionSetCurrent.Subscribe(SetCurrent).DisposeItWith(Disposable);
    }

    private void SetCurrent(MissionSetCurrentPacket req)
    {
        var item = _missionSource.Lookup(req.Payload.Seq);
        if (item.HasValue == false)
        {
            Logger.Warn($"{LogRecv}: '{req.Payload.Seq}' not found");
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
            Logger.Trace($"{LogSend}: Duplicate '{nameof(MissionCountPacket)}' received. Skip it...");
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
            Logger.Error(e, $"{LogSend}: upload error");
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
}

