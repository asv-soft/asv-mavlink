using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink
{
    public interface IVehiclePx4:IMavlinkClient
    {
        IRxValue<Px4VehicleMode> Mode { get; }

        /// <summary>
        /// Maximum horizontal velocity in mission (MPC_XY_CRUISE)
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> WriteXYCruise(float velocity, CancellationToken cancel);

        /// <summary>
        /// Maximum horizontal velocity in mission (MPC_XY_CRUISE)
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> ReadXYCruise(CancellationToken cancel);

        /// <summary>
        /// Manual-Position control sub-mode (MPC_XY_VEL_MAX)
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> WriteXYVelMax(float velocity, CancellationToken cancel);

        /// <summary>
        /// Manual-Position control sub-mode (MPC_XY_VEL_MAX)
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> ReadXYVelMax(CancellationToken cancel);

        /// <summary>
        /// Maximum vertical descent velocity (MPC_Z_VEL_MAX_DN)
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> WriteZVelMaxDn(float velocity, CancellationToken cancel);

        /// <summary>
        /// Maximum vertical descent velocity (MPC_Z_VEL_MAX_DN)
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> ReadZVelMaxDn(CancellationToken cancel);

        /// <summary>
        /// Maximum vertical ascent velocity (MPC_Z_VEL_MAX_UP)
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> WriteZVelMaxUp(float velocity, CancellationToken cancel);

        /// <summary>
        /// Maximum vertical ascent velocity (MPC_Z_VEL_MAX_UP)
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> ReadZVelMaxUp(CancellationToken cancel);

        /// <summary>
        /// Take-off altitude (MIS_TAKEOFF_ALT)
        /// </summary>
        /// <param name="alt"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> WriteMissionTakeOffAltitude(float alt, CancellationToken cancel);

        /// <summary>
        /// Take-off altitude (MIS_TAKEOFF_ALT)
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<float> ReadMissionTakeOffAltitude(CancellationToken cancel);



    }
}
