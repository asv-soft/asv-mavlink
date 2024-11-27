using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

/// Helper class for common Mavlink operations.
/// /
public static class MavlinkCommonHelper
{
    public static ValueTask SetPositionTargetLocalNed(this IPositionClient src, uint timeBootMs, MavFrame coordinateFrame, float? x,
        float? y, float? z, float? vx, float? vy, float? vz, float? afx, float? afy, float? afz, float? yaw, float? yawRate,
        CancellationToken cancel)
    {
        PositionTargetTypemask mask = 0;
        if (!x.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskXIgnore;
        if (!y.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskYIgnore;
        if (!z.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskZIgnore;

        if (!vx.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskVxIgnore;
        if (!vy.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskVyIgnore;
        if (!vz.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskVzIgnore;

        if (!afx.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskAxIgnore;
        if (!afy.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskVyIgnore;
        if (!afz.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskVzIgnore;

        if (!yaw.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskYawIgnore;
        if (!yawRate.HasValue) mask |= PositionTargetTypemask.PositionTargetTypemaskYawRateIgnore;

        return src.SetPositionTargetLocalNed(timeBootMs, coordinateFrame, mask, x ?? 0, y ?? 0, z ?? 0, vx ?? 0, vy ?? 0, vz ?? 0, afx ?? 0, afy ?? 0, afz ?? 0, yaw ?? 0, yawRate ?? 0, cancel);
    }
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
    public static ValueTask SetTargetGlobalInt(this IPositionClient src, uint timeBootMs, MavFrame coordFrame,
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