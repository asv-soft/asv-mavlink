using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IMavlinkOffboardMode:IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeBootMs">Timestamp (time since system boot).</param>
        /// <param name="coordinateFrame"> Valid options are: MAV_FRAME_LOCAL_NED = 1, MAV_FRAME_LOCAL_OFFSET_NED = 7, MAV_FRAME_BODY_NED = 8, MAV_FRAME_BODY_OFFSET_NED = 9</param>
        /// <param name="typeMask">Bitmap to indicate which dimensions should be ignored by the vehicle.</param>
        /// <param name="x">X Position in NED frame</param>
        /// <param name="y">Y Position in NED frame</param>
        /// <param name="z">Z Position in NED frame (note, altitude is negative in NED)</param>
        /// <param name="vx">X velocity in NED frame</param>
        /// <param name="vy">Y velocity in NED frame</param>
        /// <param name="vz">Z velocity in NED frame</param>
        /// <param name="afx">X acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N</param>
        /// <param name="afy">Y acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N</param>
        /// <param name="afz">Z acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N</param>
        /// <param name="yaw">yaw setpoint</param>
        /// <param name="yawRate">yaw rate setpoint</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task SetPositionTargetLocalNed(uint timeBootMs, MavFrame coordinateFrame, PositionTargetTypemask typeMask, float x,
            float y, float z, float vx, float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate,
            CancellationToken cancel);

    }

    public static class OffboardModeHelper
    {
        public static Task SetPositionTargetLocalNed(this IMavlinkOffboardMode src, uint timeBootMs, MavFrame coordinateFrame, float? x,
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

    }

   
}
