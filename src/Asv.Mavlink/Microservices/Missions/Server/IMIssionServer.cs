using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

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

    public override string ToString()
    {
        return $"Seq:{Seq} Command:{Command:G} Frame:{Frame:G} X:{X} Y:{Y} Z:{Z} Param1:{Param1} Param2:{Param2} Param3:{Param3} Param4:{Param4} Autocontinue:{Autocontinue} MissionType:{MissionType:G}"; 
    }
}

/// <summary>
/// Interface for a mission server that handles mission related communication.
/// </summary>
public interface IMissionServer:IMavlinkMicroserviceServer
{
    /// <summary>
    /// Event that is raised whenever the mission count is updated.
    /// </summary>
    /// <remarks>
    /// This event provides an observable stream of <see cref="MissionCountPacket"/> objects,
    /// which contain information about the current mission count.
    /// </remarks>
    Observable<MissionCountPacket> OnMissionCount { get; }

    /// <summary>
    /// Gets an observable sequence of MissionRequestListPacket that represents the event raised when a mission request list is received.
    /// </summary>
    /// <remarks>
    /// Subscribing to this event allows you to listen for incoming mission request list packets.
    /// </remarks>
    Observable<MissionRequestListPacket> OnMissionRequestList { get; }

    /// <summary>
    /// Gets an observable sequence of <see cref="MissionRequestIntPacket"/> for mission requests of type int.
    /// </summary>
    /// <remarks>
    /// This property provides an event-driven mechanism to receive mission request packets of type int.
    /// The returned observable sequence can be subscribed to in order to receive notifications whenever
    /// a mission request packet of type int is received.
    /// </remarks>
    /// <value>
    /// An <see cref="Observable{T}"/> of <see cref="MissionRequestIntPacket"/>, representing the observable
    /// sequence of mission request packets of type int.
    /// </value>
    Observable<MissionRequestIntPacket> OnMissionRequestInt { get; }

    /// <summary>
    /// Gets an observable sequence of MissionClearAllPacket events.
    /// </summary>
    /// <remarks>
    /// This property represents the event stream for when a mission clear all event occurs.
    /// </remarks>
    /// <returns>
    /// An Observable<MissionClearAllPacket> that can be subscribed to receive MissionClearAllPacket events.
    /// </returns>
    Observable<MissionClearAllPacket> OnMissionClearAll { get; }

    /// <summary>
    /// Represents an event that is raised when a mission is set as the current mission.
    /// </summary>
    /// <value>
    /// An <see cref="Observable{T}"/> of type <see cref="MissionSetCurrentPacket"/> that can be subscribed to receive notifications when the event is raised.
    /// </value>
    Observable<MissionSetCurrentPacket> OnMissionSetCurrent { get; }
    
    /// <summary>
    /// Gets the observable sequence that emits MissionAckPayload when a mission acknowledgement is received.
    /// </summary>
    /// <value>
    /// The observable sequence that emits MissionAckPayload when a mission acknowledgement is received.
    /// </value>
    public Observable<MissionAckPayload> OnMissionAck { get; }
    
    /// <summary>
    /// Sends a mission acknowledgment message to the specified target system and component IDs.
    /// </summary>
    /// <param name="result">The result of the mission.</param>
    /// <param name="targetSystemId">The target system ID. Default value is 0.</param>
    /// <param name="targetComponentId">The target component ID. Default value is 0.</param>
    /// <param name="type">The mission type. Default value is null.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask SendMissionAck(
        MavMissionResult result, 
        byte targetSystemId = 0, 
        byte targetComponentId = 0,
        MavMissionType? type = null
    );

    /// <summary>
    /// Sends the mission count to the specified target system and component IDs.
    /// </summary>
    /// <param name="count">The number of missions to be sent.</param>
    /// <param name="targetSystemId">The ID of the target system. (optional, default value is 0)</param>
    /// <param name="targetComponentId">The ID of the target component. (optional, default value is 0)</param>
    /// <returns>
    /// A task that represents the asynchronous operation of sending the mission count.
    /// </returns>
    ValueTask SendMissionCount(
        ushort count, 
        byte targetSystemId = 0, 
        byte targetComponentId = 0
    );

    /// <summary>
    /// Sends the reached value of a sequence.
    /// </summary>
    /// <param name="seq">The sequence number.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask SendReached(ushort seq);

    /// <summary>
    /// Sends the current mission index to the system.
    /// </summary>
    /// <param name="current">The index of the current mission.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask SendMissionCurrent(ushort current);

    /// <summary>
    /// Sends a mission item to the server.
    /// </summary>
    /// <param name="item">The mission item to be sent.</param>
    /// <param name="targetSystemId">The target system ID. Default value is 0.</param>
    /// <param name="targetComponentId">The target component ID. Default value is 0.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will complete once the mission item
    /// has been successfully sent to the server.
    /// </returns>
    ValueTask SendMissionItemInt(
        ServerMissionItem item, 
        byte targetSystemId = 0,
        byte targetComponentId = 0
    );

    /// <summary>
    /// Requests a mission item from the server.
    /// </summary>
    /// <param name="index">The index of the mission item to request.</param>
    /// <param name="type">The type of the mission item.</param>
    /// <param name="targetSystemId">The target system ID. Default value is 0.</param>
    /// <param name="targetComponentId">The target component ID. Default value is 0.</param>
    /// <param name="cancel">Cancellation token to cancel the request. Default value is default (no cancellation).</param>
    /// <returns>A task that represents the asynchronous request operation. The task result is a ServerMissionItem object.</returns>
    Task<ServerMissionItem> RequestMissionItem(
        ushort index, 
        MavMissionType type, 
        byte targetSystemId = 0,
        byte targetComponentId = 0, 
        CancellationToken cancel = default
    );
}