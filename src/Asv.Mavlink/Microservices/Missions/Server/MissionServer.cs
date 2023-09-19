using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Newtonsoft.Json;

namespace Asv.Mavlink;

public class MissionServer : MavlinkMicroserviceServer, IMissionServer
{
    public MissionServer(string logName, IMavlinkV2Connection connection, MavlinkServerIdentity identity,
        IPacketSequenceCalculator seq, IScheduler rxScheduler) :
        base(logName, connection, identity, seq, rxScheduler)
    {
        OnMissionCount = InternalFilter<MissionCountPacket>(_ => _.Payload.TargetSystem, 
                _ => _.Payload.TargetComponent);
        OnMissionItemInt = InternalFilter<MissionItemIntPacket>(_ => _.Payload.TargetSystem, 
                _ => _.Payload.TargetComponent);
        OnMissionRequestList = InternalFilter<MissionRequestListPacket>(_ => _.Payload.TargetSystem, 
                _ => _.Payload.TargetComponent);
        OnMissionRequestInt = InternalFilter<MissionRequestIntPacket>(_ => _.Payload.TargetSystem, 
                _ => _.Payload.TargetComponent);
        OnMissionClearAll = InternalFilter<MissionClearAllPacket>(_ => _.Payload.TargetSystem, 
                _ => _.Payload.TargetComponent);
        OnMissionSetCurrent = InternalFilter<MissionSetCurrentPacket>(_ => _.Payload.TargetSystem, 
                _ => _.Payload.TargetComponent);
        OnMissionAck = InternalFilter<MissionAckPacket>(_ => _.Payload.TargetSystem, 
            _ => _.Payload.TargetComponent);
    }

    public Task SendMissionAck(Action<MissionAckPacket> changeCallback, CancellationToken cancel = default)
    { 
        return InternalSend<MissionAckPacket>(_ => changeCallback(_), cancel);
    }
    
    public Task SendMissionCurrent(Action<MissionCurrentPacket> changeCallback, CancellationToken cancel = default)
    { 
        return InternalSend<MissionCurrentPacket>(_ => changeCallback(_), cancel);
    }
    
    public Task SendMissionItemInt(Action<MissionItemIntPacket> changeCallback, CancellationToken cancel = default)
    { 
        return InternalSend<MissionItemIntPacket>(_ => changeCallback(_), cancel);
    }
    
    public Task SendMissionCount(Action<MissionCountPacket> changeCallback, CancellationToken cancel = default)
    { 
        return InternalSend<MissionCountPacket>(_ => changeCallback(_), cancel);
    }
    
    public Task SendMissionRequestInt(Action<MissionRequestIntPacket> changeCallback, CancellationToken cancel = default)
    { 
        return InternalSend<MissionRequestIntPacket>(_ => changeCallback(_), cancel);
    }
    
    public MavlinkServerIdentity Identity => base.Identity;

    public IObservable<MissionAckPacket> OnMissionAck { get; }
    public IObservable<MissionCountPacket> OnMissionCount { get; }
    public IObservable<MissionItemIntPacket> OnMissionItemInt { get; }
    public IObservable<MissionRequestListPacket> OnMissionRequestList { get; }
    public IObservable<MissionRequestIntPacket> OnMissionRequestInt { get; }
    public IObservable<MissionClearAllPacket> OnMissionClearAll { get; }
    public IObservable<MissionSetCurrentPacket> OnMissionSetCurrent { get; }
}