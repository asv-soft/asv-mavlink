using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public interface IMavlinkCommon:IDisposable
    {

        Task SetMode(uint baseMode, uint customMode, CancellationToken cancel);
        /// <summary>
        /// Request a data stream.
        /// DEPRECATED: Replaced by SET_MESSAGE_INTERVAL (2015-08).
        /// 
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="rateHz"></param>
        /// <param name="startStop"></param>
        /// <returns></returns>
        Task RequestDataStream(int streamId,int rateHz,bool startStop, CancellationToken cancel);


        /// <summary>
        /// Sets a desired vehicle position, velocity, and/or acceleration in a global coordinate system (WGS84). Used by an external controller to command the vehicle (manual controller or other system).
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
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task SetPositionTargetGlobalInt(uint timeBootMs, MavFrame coordFrame, int latInt, int lonInt, float alt, float vx,
            float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate,
            PositionTargetTypemask typeMask, CancellationToken cancel);
    }

    public static class MavlinkCommonHelper
    {
        public static Task SetPositionTargetGlobalInt(this IMavlinkCommon src,uint timeBootMs, MavFrame coordFrame, CancellationToken cancel, 
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

            return src.SetPositionTargetGlobalInt(timeBootMs, coordFrame, latInt ?? 0, lonInt ?? 0, alt ?? 0, vx ?? 0,
                vy ?? 0, vz ?? 0, afx ?? 0, afy ?? 0, afz ?? 0, yaw ?? 0, yawRate ?? 0, mask, cancel);

        }
    }
}