using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a client for interacting with missions.
    /// </summary>
    public interface IMissionClient: IMavlinkMicroserviceClient
    {
        /// <summary>
        /// Gets the current mission value.
        /// </summary>
        /// <value>The current mission value.</value>
        ReadOnlyReactiveProperty<ushort> MissionCurrent { get; }

        /// <summary>
        /// Drone receives message and attempts to update the current mission sequence number.
        /// </summary>
        /// <param name="missionItemsIndex">Index of the mission item within the mission sequence.</param>
        /// <param name="cancel">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task MissionSetCurrent(ushort missionItemsIndex, CancellationToken cancel = default);

        /// <summary>
        /// Gets the interface representing the mission reached property.
        /// </summary>
        /// <remarks>
        /// The MissionReached property provides access to the current mission reached value.
        /// </remarks>
        /// <returns>
        /// The interface representing the mission reached property.
        /// </returns>
        ReadOnlyReactiveProperty<ushort> MissionReached { get; }

        /// <summary>
        /// Gets an observable stream of mission request payloads.
        /// </summary>
        /// <returns>An Observable stream of MissionRequestPayload objects.</returns>
        Observable<MissionRequestPayload> OnMissionRequest { get; }

        /// <summary>
        /// Gets the observable sequence that emits MissionAckPayload when a mission acknowledgement is received.
        /// </summary>
        /// <value>
        /// The observable sequence that emits MissionAckPayload when a mission acknowledgement is received.
        /// </value>
        Observable<MissionAckPayload> OnMissionAck { get; }

        /// <summary>
        /// Requests a mission item of the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the mission item to request.</param>
        /// <param name="cancel">Optional cancellation token to cancel the request.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task will complete with
        /// a MissionItemIntPayload containing the mission item data.
        /// </returns>
        Task<MissionItemIntPayload> MissionRequestItem(ushort index, CancellationToken cancel = default);

        /// <summary>
        /// Initiate mission download from a system by requesting the list of mission items.
        /// </summary>
        /// <param name="attemptCount">The number of attempts to request the mission count from the system.</param>
        /// <param name="cancel">A cancellation token that can be used to cancel the mission request.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result is the count of mission items requested from the system.
        /// </returns>
        Task<int> MissionRequestCount(CancellationToken cancel = default);

        /// <summary>
        /// Sets the count for the mission and returns a task that represents the asynchronous operation.
        /// </summary>
        /// <param name="count">The count to set for the mission.</param>
        /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task completes when the count is set successfully.
        /// </returns>
        ValueTask MissionSetCount(ushort count, CancellationToken cancel = default);

        /// <summary>
        /// Writes a mission item to the vehicle's mission list.
        /// </summary>
        /// <param name="seq">The sequence number of the mission item.</param>
        /// <param name="frame">The coordinate system in which the mission item's position is specified.</param>
        /// <param name="cmd">The scheduled action for the mission item.</param>
        /// <param name="current">True if this mission item is the currently active one.</param>
        /// <param name="autoContinue">True to automatically continue to the next waypoint after this mission item.</param>
        /// <param name="param1">PARAM1 for the scheduled action (refer to MAV_CMD enum for details).</param>
        /// <param name="param2">PARAM2 for the scheduled action (refer to MAV_CMD enum for details).</param>
        /// <param name="param3">PARAM3 for the scheduled action (refer to MAV_CMD enum for details).</param>
        /// <param name="param4">PARAM4 for the scheduled action (refer to MAV_CMD enum for details).</param>
        /// <param name="x">The x-coordinate of the mission item's position.</param>
        /// <param name="y">The y-coordinate of the mission item's position.</param>
        /// <param name="z">The z-coordinate of the mission item's position.</param>
        /// <param name="missionType">The type of mission item.</param>
        /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        ValueTask WriteMissionItem(
            ushort seq, 
            MavFrame frame, 
            MavCmd cmd,
            bool current,
            bool autoContinue,
            float param1,
            float param2, 
            float param3,
            float param4, 
            float x, 
            float y,
            float z, 
            MavMissionType missionType,
            CancellationToken cancel = default
        );

        /// <summary>
        /// Clears all mission items of the specified type.
        /// </summary>
        /// <param name="type">The type of mission items to clear. Default value is MavMissionType.MavMissionTypeAll.</param>
        /// <param name="cancel">A cancellation token to cancel the operation. Default value is default(CancellationToken).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ClearAll(MavMissionType type = MavMissionType.MavMissionTypeAll, CancellationToken cancel = default);

        /// <summary>
        /// Writes a mission item to a target.
        /// </summary>
        /// <param name="missionItem">The mission item to write.</param>
        /// <param name="cancel">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        ValueTask WriteMissionItem(MissionItem missionItem, CancellationToken cancel = default);

        /// <summary>
        /// Writes a mission item with an integer payload.
        /// </summary>
        /// <param name="fillCallback">A callback function that fills the mission item payload.</param>
        /// <param name="cancel">Optional cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        ValueTask WriteMissionIntItem(Action<MissionItemIntPayload> fillCallback, CancellationToken cancel = default);

        /// <summary>
        /// Sends a mission acknowledgment message to the specified target system and component IDs.
        /// </summary>
        /// <param name="result">The result of the mission.</param>
        /// <param name="targetSystemId">The target system ID. Default value is 0.</param>
        /// <param name="targetComponentId">The target component ID. Default value is 0.</param>
        /// <param name="cancel">Optional cancellation token to cancel the operation.</param>
        /// <param name="type">The mission type. Default value is null.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        ValueTask SendMissionAck
        (
            MavMissionResult result,
            byte targetSystemId = 0,
            byte targetComponentId = 0,
            MavMissionType? type = null,
            CancellationToken cancel = default
        );
    }
}
