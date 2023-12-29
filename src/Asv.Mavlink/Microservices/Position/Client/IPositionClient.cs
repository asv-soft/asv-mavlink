using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client for accessing position-related data and commands for a vehicle.
/// </summary>
public interface IPositionClient
{
    /// <summary>
    /// Gets the RX value for GlobalPosition of type GlobalPositionIntPayload.
    /// </summary>
    /// <remarks>
    /// This property represents the current global position.
    /// </remarks>
    IRxValue<GlobalPositionIntPayload> GlobalPosition { get; }

    /// <summary>
    /// Gets the home position of the property Home.
    /// </summary>
    /// <value>
    /// The home position represented by an IRxValue of HomePositionPayload.
    /// </value>
    IRxValue<HomePositionPayload> Home { get; }

    /// <summary>
    /// Gets the target position value.
    /// </summary>
    /// <returns>
    /// The RxValue object that represents the target position value.
    /// </returns>
    IRxValue<PositionTargetGlobalIntPayload> Target { get; }

    /// <summary>
    /// Gets the observable altitude value.
    /// </summary>
    /// <value>
    /// The observable altitude value.
    /// </value>
    IRxValue<AltitudePayload> Altitude { get; }

    /// <summary>
    /// Gets the RxValue for VfrHud.
    /// </summary>
    /// <remarks>
    /// The VfrHud property provides access to an IRxValue interface for VfrHudPayload, which represents the
    /// information received from a VFR (Visual Flight Rules) Heads-Up Display (HUD).
    /// </remarks>
    /// <returns>The IRxValue interface for VfrHudPayload.</returns>
    IRxValue<VfrHudPayload> VfrHud { get; }

    /// <summary>
    /// Gets the reactive value representing the high resolution IMU payload.
    /// </summary>
    /// <value>
    /// The reactive value holding the high resolution IMU payload.
    /// </value>
    IRxValue<HighresImuPayload> Imu { get; }

    /// <summary>
    /// Gets the RX value of the current Attitude payload.
    /// </summary>
    /// <returns>The RX value of the Attitude payload.</returns>
    IRxValue<AttitudePayload> Attitude { get; }

    /// <summary>
    /// Sets a desired vehicle position, velocity, and/or acceleration in a global coordinate system (WGS84). Used by an external controller to command the vehicle (manual controller or
    /// other system).
    /// </summary>
    /// <param name="timeBootMs">Timestamp (time since system boot). The rationale for the timestamp in the setpoint is to allow the system to compensate for the transport delay of the setpoint. This allows the system to compensate processing latency.</param>
    /// <param name="coordFrame">Valid options are: MAV_FRAME_GLOBAL_INT = 5, MAV_FRAME_GLOBAL_RELATIVE_ALT_INT = 6, MAV_FRAME_GLOBAL_TERRAIN_ALT_INT = 11</param>
    /// <param name="latInt">X Position in WGS84 frame</param>
    /// <param name="lonInt">Y Position in WGS84 frame</param>
    /// <param name="alt">Altitude (MSL, Relative to home, or AGL - depending on frame)</param>
    /// <param name="vx">X velocity in NED frame</param>
    /// <param name="vy">Y velocity in NED frame</param>
    /// <param name="vz">Z velocity in NED frame</param>
    /// <param name="afx">X acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N</param>
    /// <param name="afy">Y acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N</param>
    /// <param name="afz">Z acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N</param>
    /// <param name="yaw">yaw setpoint</param>
    /// <param name="yawRate">yaw rate setpoint</param>
    /// <param name="typeMask">Bitmap to indicate which dimensions should be ignored by the vehicle.</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>Returns a Task representing the asynchronous operation</returns>
    Task SetTargetGlobalInt(uint timeBootMs, MavFrame coordinateFrame, int latInt, int lonInt, float alt,
        float vx, float vy, float vz, float afx, float afy, float afz, float yaw,
        float yawRate, PositionTargetTypemask typeMask, CancellationToken cancel = default);
}

/// Helper class for common Mavlink operations.
/// /
public static class MavlinkCommonHelper
{
    /// <summary>
    /// Sets the target global position for control commands.
    /// </summary>
    /// <param name="src">The IPositionClient instance.</param>
    /// <param name="timeBootMs">Time (in milliseconds) since system boot.</param>
    /// <param name="coordFrame">Coordinate frame used for the target position.</param>
    /// <param name="cancel">Cancellation token to cancel the operation.</param>
    /// <param name="latInt">Latitude (integer degrees) of the target position. Set to null to ignore.</param>
    /// <param name="lonInt">Longitude (integer degrees) of the target position. Set to null to ignore.</param>
    /// <param name="alt">Altitude (meters) of the target position. Set to null to ignore.</param>
    /// <param name="vx">Velocity in the x-axis (m/s) of the target position. Set to null to ignore.</param>
    /// <param name="vy">Velocity in the y-axis (m/s) of the target position. Set to null to ignore.</param>
    /// <param name="vz">Velocity in the z-axis (m/s) of the target position. Set to null to ignore.</param>
    /// <param name="afx">Acceleration in the x-axis (m/s^2) of the target position. Set to null to ignore.</param>
    /// <param name="afy">Acceleration in the y-axis (m/s^2) of the target position. Set to null to ignore.</param>
    /// <param name="afz">Acceleration in the z-axis (m/s^2) of the target position. Set to null to ignore.</param>
    /// <param name="yaw">Yaw angle (radians) of the target position. Set to null to ignore.</param>
    /// <param name="yawRate">Yaw rate (rad/s) of the target position. Set to null to ignore.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static Task SetTargetGlobalInt(this IPositionClient src, uint timeBootMs, MavFrame coordFrame,
        CancellationToken cancel,
        int? latInt = null,
        int? lonInt = null,
        float? alt = null,
        float? vx = null,
        float? vy = null,
        float? vz = null,
        float? afx = null,
        float? afy = null,
        float? afz = null,
        float? yaw = null,
        float? yawRate = null)
    {
        var mask = default(PositionTargetTypemask);
        if (latInt.HasValue)
        {
            mask |= PositionTargetTypemask.PositionTargetTypemaskXIgnore;
        }

        if (lonInt.HasValue)
        {
            mask |= PositionTargetTypemask.PositionTargetTypemaskYIgnore;
        }

        if (alt.HasValue)
        {
            mask |= PositionTargetTypemask.PositionTargetTypemaskZIgnore;
        }

        if (vx.HasValue)
        {
            mask |= PositionTargetTypemask.PositionTargetTypemaskVxIgnore;
        }

        if (vy.HasValue)
        {
            mask |= PositionTargetTypemask.PositionTargetTypemaskVyIgnore;
        }

        if (vz.HasValue)
        {
            mask |= PositionTargetTypemask.PositionTargetTypemaskVzIgnore;
            }
            if (afx.HasValue)
            {
                mask |= PositionTargetTypemask.PositionTargetTypemaskAxIgnore;
            }
            if (afy.HasValue)
            {
                mask |= PositionTargetTypemask.PositionTargetTypemaskAyIgnore;
            }
            if (afz.HasValue)
            {
                mask |= PositionTargetTypemask.PositionTargetTypemaskAzIgnore;
            }
            if (yaw.HasValue)
            {
                mask |= PositionTargetTypemask.PositionTargetTypemaskYawIgnore;
            }
            if (yawRate.HasValue)
            {
                mask |= PositionTargetTypemask.PositionTargetTypemaskYawRateIgnore;
            }

            mask = ~mask;

            return src.SetTargetGlobalInt(timeBootMs, coordFrame, latInt ?? 0, lonInt ?? 0, alt ?? 0, vx ?? 0,
                vy ?? 0, vz ?? 0, afx ?? 0, afy ?? 0, afz ?? 0, yaw ?? 0, yawRate ?? 0, mask, cancel);

        }
    }