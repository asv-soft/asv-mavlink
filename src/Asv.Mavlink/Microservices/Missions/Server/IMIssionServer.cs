using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class ServerMissionItem
{
    /// <summary>
    /// PARAM1, see MAV_CMD enum
    /// OriginName: param1, Units: , IsExtended: false
    /// </summary>
    public float Param1 { get; set; }
    /// <summary>
    /// PARAM2, see MAV_CMD enum
    /// OriginName: param2, Units: , IsExtended: false
    /// </summary>
    public float Param2 { get; set; }
    /// <summary>
    /// PARAM3, see MAV_CMD enum
    /// OriginName: param3, Units: , IsExtended: false
    /// </summary>
    public float Param3 { get; set; }
    /// <summary>
    /// PARAM4, see MAV_CMD enum
    /// OriginName: param4, Units: , IsExtended: false
    /// </summary>
    public float Param4 { get; set; }
    /// <summary>
    /// PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7
    /// OriginName: x, Units: , IsExtended: false
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// PARAM6 / y position: local: x position in meters * 1e4, global: longitude in degrees *10^7
    /// OriginName: y, Units: , IsExtended: false
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame.
    /// OriginName: z, Units: , IsExtended: false
    /// </summary>
    public float Z { get; set; }
    /// <summary>
    /// Waypoint ID (sequence number). Starts at zero. Increases monotonically for each waypoint, no gaps in the sequence (0,1,2,3,4).
    /// OriginName: seq, Units: , IsExtended: false
    /// </summary>
    public ushort Seq { get; set; }
    /// <summary>
    /// The scheduled action for the waypoint.
    /// OriginName: command, Units: , IsExtended: false
    /// </summary>
    public MavCmd Command { get; set; }
    /// <summary>
    /// The coordinate system of the waypoint.
    /// OriginName: frame, Units: , IsExtended: false
    /// </summary>
    public MavFrame Frame { get; set; }
    /// <summary>
    /// Autocontinue to next waypoint
    /// OriginName: autocontinue, Units: , IsExtended: false
    /// </summary>
    public byte Autocontinue { get; set; }
    /// <summary>
    /// Mission type.
    /// OriginName: mission_type, Units: , IsExtended: true
    /// </summary>
    public MavMissionType MissionType { get; set; }
}


public interface IMissionServer
{
    IObservable<MissionCountPacket> OnMissionCount { get; }
    IObservable<MissionRequestListPacket> OnMissionRequestList { get; }
    IObservable<MissionRequestIntPacket> OnMissionRequestInt { get; }
    IObservable<MissionClearAllPacket> OnMissionClearAll { get; }
    IObservable<MissionSetCurrentPacket> OnMissionSetCurrent { get; }
    Task SendMissionAck(MavMissionResult result, byte targetSystemId = 0, byte targetComponentId = 0,
        MavMissionType? type = null);
    Task SendMissionCount(ushort count, byte targetSystemId = 0, byte targetComponentId = 0);
    Task SendReached(ushort seq);
    Task SendMissionCurrent(ushort current);
    Task SendMissionItemInt(ServerMissionItem item,byte targetSystemId = 0, byte targetComponentId = 0);

    Task<ServerMissionItem> RequestMissionItem(ushort index, MavMissionType type, byte targetSystemId = 0,
        byte targetComponentId = 0, CancellationToken cancel = default);
}