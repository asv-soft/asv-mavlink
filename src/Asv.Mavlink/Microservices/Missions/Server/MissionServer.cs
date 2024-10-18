using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class MissionServer : MavlinkMicroserviceServer, IMissionServer
{
    private ushort _currentMissionIndex;

    public MissionServer(
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq, 
        TimeProvider? timeProvider = null,
        IScheduler? rxScheduler = null,
        ILoggerFactory? logFactory = null)
        : base("MISSION", connection, identity, seq, timeProvider, rxScheduler,logFactory)
    {
       
    }


    public IObservable<MissionCountPacket> OnMissionCount =>
        InternalFilter<MissionCountPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

    public IObservable<MissionRequestListPacket> OnMissionRequestList =>
        InternalFilter<MissionRequestListPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

    public IObservable<MissionRequestIntPacket> OnMissionRequestInt =>
        InternalFilter<MissionRequestIntPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

    public IObservable<MissionClearAllPacket> OnMissionClearAll =>
        InternalFilter<MissionClearAllPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

    public IObservable<MissionSetCurrentPacket> OnMissionSetCurrent =>
        InternalFilter<MissionSetCurrentPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

    public Task SendMissionAck(MavMissionResult result, byte targetSystemId = 0, byte targetComponentId = 0,
        MavMissionType? type = null)
    {
        return InternalSend<MissionAckPacket>(x =>
        {
            x.Payload.TargetSystem = targetSystemId;
            x.Payload.TargetComponent = targetComponentId;
            x.Payload.Type = result;
            if (type.HasValue)
            {
                x.Payload.MissionType = type.Value;
            }
        }, DisposeCancel);
    }

    public Task SendMissionCount(ushort count, byte targetSystemId = 0, byte targetComponentId = 0)
    {
        return InternalSend<MissionCountPacket>(x =>
        {
            x.Payload.TargetSystem = targetSystemId;
            x.Payload.TargetComponent = targetComponentId;
            x.Payload.Count = count;
        }, DisposeCancel);
    }

    public Task SendReached(ushort seq)
    {
        return InternalSend<MissionItemReachedPacket>(x =>
        {
            x.Payload.Seq = seq;
        },DisposeCancel);
    }

    public Task SendMissionCurrent(ushort current)
    {
        _currentMissionIndex = current;
        return InternalSend<MissionCurrentPacket>(x =>
        {
            x.Payload.Seq = current;
        }, DisposeCancel);
    }

    public Task SendMissionItemInt(ServerMissionItem item,byte targetSystemId = 0, byte targetComponentId = 0)
    {
        return InternalSend<MissionItemIntPacket>(x =>
        {
            x.Payload.Frame = item.Frame;
            x.Payload.TargetSystem = targetSystemId;
            x.Payload.TargetComponent = targetComponentId;
            x.Payload.MissionType = item.MissionType;
            x.Payload.Seq = item.Seq;
            x.Payload.Current = (byte)(_currentMissionIndex == item.Seq ? 1:0);
            x.Payload.Autocontinue = item.Autocontinue;
            x.Payload.Param1 = item.Param1;
            x.Payload.Param2 = item.Param2;
            x.Payload.Param3 = item.Param3;
            x.Payload.Param4 = item.Param4;
            x.Payload.X = item.X;
            x.Payload.Y = item.Y;
            x.Payload.Z = item.Z;
            x.Payload.Command = item.Command;
            x.Payload.Frame = item.Frame;
        }, DisposeCancel);
    }

    public Task<ServerMissionItem> RequestMissionItem(ushort index, MavMissionType type,byte targetSystemId = 0, byte targetComponentId = 0, CancellationToken cancel = default)
    {
        return InternalCall<ServerMissionItem, MissionRequestPacket, MissionItemIntPacket>(
            x =>
            {
                x.Payload.MissionType = type;
                x.Payload.TargetComponent = targetComponentId;
                x.Payload.TargetSystem = targetSystemId;
                x.Payload.Seq = index;
            },p=>p.Payload.TargetSystem, p=>p.Payload.TargetComponent,p=> p.Payload.Seq == index, AsvSdrHelper.Convert , cancel:DisposeCancel);
    }
}