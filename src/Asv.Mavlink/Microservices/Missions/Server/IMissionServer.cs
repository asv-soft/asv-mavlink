using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IMissionServer
{
    Task SendMissionAck(Action<MissionAckPacket> changeCallback, CancellationToken cancel = default);
    Task SendMissionCurrent(Action<MissionCurrentPacket> changeCallback, CancellationToken cancel = default);
    Task SendMissionItemInt(Action<MissionItemIntPacket> changeCallback, CancellationToken cancel = default);
    Task SendMissionCount(Action<MissionCountPacket> changeCallback, CancellationToken cancel = default);
    Task SendMissionRequestInt(Action<MissionRequestIntPacket> changeCallback, CancellationToken cancel = default);
    
    MavlinkServerIdentity Identity { get; }
    IObservable<MissionAckPacket> OnMissionAck { get; }
    IObservable<MissionCountPacket> OnMissionCount { get; }
    IObservable<MissionItemIntPacket> OnMissionItemInt { get; }
    IObservable<MissionRequestListPacket> OnMissionRequestList { get; }
    IObservable<MissionRequestIntPacket> OnMissionRequestInt { get; }
    IObservable<MissionClearAllPacket> OnMissionClearAll { get; }
    IObservable<MissionSetCurrentPacket> OnMissionSetCurrent { get; }
}