using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Newtonsoft.Json;
using NLog;

namespace Asv.Mavlink;

public class MissionServer : MavlinkMicroserviceServer, IMissionServer
{
    private readonly IStatusTextServer _statusLogger;
    public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly SourceCache<MissionItemIntPayload,ushort> _missionSource;
    private double _busy;

    public MissionServer(IStatusTextServer srv, IMavlinkV2Connection connection, MavlinkServerIdentity identity,
        IPacketSequenceCalculator seq, IScheduler rxScheduler) :
        base("MISSION", connection, identity, seq, rxScheduler)
    {
        _statusLogger = srv ?? throw new ArgumentNullException(nameof(srv));
        _missionSource = new SourceCache<MissionItemIntPayload, ushort>(x => x.Seq).DisposeItWith(Disposable);

        Current = new RxValue<ushort>(0).DisposeItWith(Disposable);
        Current.Subscribe(OnCurrentChanged).DisposeItWith(Disposable);
        
        InternalFilter<MissionCountPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
            .Subscribe(UploadMission)
            .DisposeItWith(Disposable);
        InternalFilter<MissionRequestListPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
            .Subscribe(DownloadMission)
            .DisposeItWith(Disposable);
        InternalFilter<MissionRequestIntPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
            .Subscribe(ReadMissionItem)
            .DisposeItWith(Disposable);
        InternalFilter<MissionClearAllPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
            .Subscribe(ClearAll)
            .DisposeItWith(Disposable);
        InternalFilter<MissionSetCurrentPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
            .Subscribe(SetCurrent)
            .DisposeItWith(Disposable);
    }

    private void SetCurrent(MissionSetCurrentPacket req)
    {
        var item = _missionSource.Lookup(req.Payload.Seq);
        if (item.HasValue == false)
        {
            Logger.Warn($"{LogRecv}: '{req.Payload.Seq}' not found");
            return;
        }
        Current.OnNext(req.Payload.Seq);
    }
    private void OnCurrentChanged(ushort current)
    {
        InternalSend<MissionCurrentPacket>(x =>
        {
            x.Payload.Seq = current;
        }, DisposeCancel).ConfigureAwait(false);
    }
    private async void ClearAll(MissionClearAllPacket req)
    {
        if (req.Payload.MissionType != MavMissionType.MavMissionTypeMission)
        {
            await InternalSend<MissionAckPacket>(x =>
            {
                x.Payload.TargetSystem = req.SystemId;
                x.Payload.TargetComponent = req.ComponentId;
                x.Payload.Type = MavMissionResult.MavMissionUnsupported;
            }, DisposeCancel).ConfigureAwait(false);
            return;
        }
        _missionSource.Clear();
        await InternalSend<MissionAckPacket>(x =>
        {
            x.Payload.TargetSystem = req.SystemId;
            x.Payload.TargetComponent = req.ComponentId;
            x.Payload.Type = MavMissionResult.MavMissionAccepted;
        }, DisposeCancel).ConfigureAwait(false);
    }
        
    private async void ReadMissionItem(MissionRequestIntPacket req)
    {
        if (req.Payload.MissionType != MavMissionType.MavMissionTypeMission)
        {
            await InternalSend<MissionAckPacket>(x =>
            {
                x.Payload.TargetSystem = req.SystemId;
                x.Payload.TargetComponent = req.ComponentId;
                x.Payload.Type = MavMissionResult.MavMissionUnsupported;
            }, DisposeCancel).ConfigureAwait(false);
            return;
        }

        var item = _missionSource.Lookup(req.Payload.Seq);
        if (item.HasValue == false)
        {
            await InternalSend<MissionAckPacket>(x =>
            {
                x.Payload.TargetSystem = req.SystemId;
                x.Payload.TargetComponent = req.ComponentId;
                x.Payload.Type = MavMissionResult.MavMissionInvalid;
            }, DisposeCancel).ConfigureAwait(false);
            return;
        }
        await InternalSend<MissionItemIntPacket>(x =>
        {
            x.Payload.TargetSystem = req.SystemId;
            x.Payload.TargetComponent = req.ComponentId;
            x.Payload.MissionType = item.Value.MissionType;
            x.Payload.Seq = item.Value.Seq;
            x.Payload.Current = item.Value.Current;
            x.Payload.Autocontinue = item.Value.Autocontinue;
            x.Payload.Param1 = item.Value.Param1;
            x.Payload.Param2 = item.Value.Param2;
            x.Payload.Param3 = item.Value.Param3;
            x.Payload.Param4 = item.Value.Param4;
            x.Payload.X = item.Value.X;
            x.Payload.Y = item.Value.Y;
            x.Payload.Z = item.Value.Z;
            x.Payload.Command = item.Value.Command;
            x.Payload.TargetSystem = item.Value.TargetSystem;
            x.Payload.TargetComponent = item.Value.TargetComponent;
            x.Payload.Frame = item.Value.Frame;
        }, DisposeCancel).ConfigureAwait(false);
    }
    private async void DownloadMission(MissionRequestListPacket req)
    {
        await InternalSend<MissionCountPacket>(x=>
        {
            x.Payload.TargetSystem = req.SystemId;
            x.Payload.TargetComponent = req.ComponentId;
            x.Payload.Count = (ushort) _missionSource.Count;
        }, DisposeCancel).ConfigureAwait(false);
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
                var item = await InternalCall<MissionItemIntPayload, MissionRequestPacket, MissionItemIntPacket>(
                    x =>
                    {
                        x.Payload.MissionType = req.Payload.MissionType;
                        x.Payload.TargetComponent = req.ComponentId;
                        x.Payload.TargetSystem = req.SystemId;
                        x.Payload.Seq = index;
                    },p=>p.Payload.TargetSystem, p=>p.Payload.TargetComponent,p=>
                    {
                        return p.Payload.Seq == index;
                    }, p=>p.Payload, cancel:DisposeCancel).ConfigureAwait(false);
                if (i % 5 == 0)
                {
                    _statusLogger.Info($"{LogSend}: uploaded '{(i + 1) / count:P0}' items");
                }
                _missionSource.AddOrUpdate(item);
            }
            _statusLogger.Info($"{LogSend}: uploaded '{count}' items");
            await InternalSend<MissionAckPacket>(x =>
            {
                x.Payload.TargetSystem = req.SystemId;
                x.Payload.TargetComponent = req.ComponentId;
                x.Payload.Type = MavMissionResult.MavMissionAccepted;
            }, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Logger.Error(e, $"{LogSend}: upload error");
            await InternalSend<MissionAckPacket>(x =>
            {
                x.Payload.Type = MavMissionResult.MavMissionError;
                x.Payload.MissionType = req.Payload.MissionType;
                x.Payload.TargetSystem = req.SystemId;
                x.Payload.TargetComponent = req.ComponentId;
            }, DisposeCancel).ConfigureAwait(false);
        }
        finally
        {
            Interlocked.Exchange(ref _busy, 0);
        }
    }

    public IRxEditableValue<ushort> Current { get; }
    public IObservable<IChangeSet<MissionItemIntPayload, ushort>> Items => _missionSource.Connect();
    public void SendReached(ushort seq)
    {
        _statusLogger.Info($"Reached {seq}");
        InternalSend<MissionItemReachedPacket>(x =>
        {
            x.Payload.Seq = seq;
        }, DisposeCancel).ConfigureAwait(false);
    }
}

