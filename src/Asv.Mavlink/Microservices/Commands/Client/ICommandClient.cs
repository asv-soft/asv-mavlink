using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface ICommandClient
    {
        IObservable<CommandAckPayload> OnCommandAck { get; }
        Task<CommandAckPayload> CommandLong(Action<CommandLongPayload> edit, CancellationToken cancel = default);
        Task<CommandAckPayload> CommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default);

        Task<TAnswerPacket> CommandLongAndWaitPacket<TAnswerPacket>(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default)
            where TAnswerPacket : IPacketV2<IPayload>, new();
        Task SendCommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7,  CancellationToken cancel = default);
        /// <summary>
        /// Message encoding a command with parameters as scaled integers. Scaling depends on the actual command value.
        ///  Don't wait answer
        /// </summary>
        /// <param name="command">The scheduled action for the mission item.</param>
        /// <param name="frame">The coordinate system of the COMMAND.</param>
        /// <param name="current"></param>
        /// <param name="autocontinue">autocontinue to next wp</param>
        /// <param name="param1">PARAM1, see MAV_CMD enum</param>
        /// <param name="param2">PARAM2, see MAV_CMD enum</param>
        /// <param name="param3">PARAM3, see MAV_CMD enum</param>
        /// <param name="param4">PARAM4, see MAV_CMD enum</param>
        /// <param name="x">PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7</param>
        /// <param name="y">PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7</param>
        /// <param name="z">PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame).</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task SendCommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue,
            float param1, float param2,
            float param3, float param4, int x, int y, float z, CancellationToken cancel = default);
        /// <summary>
        /// Message encoding a command with parameters as scaled integers. Scaling depends on the actual command value.
        /// </summary>
        /// <param name="command">The scheduled action for the mission item.</param>
        /// <param name="frame">The coordinate system of the COMMAND.</param>
        /// <param name="current"></param>
        /// <param name="autoContinue">autocontinue to next wp</param>
        /// <param name="param1">PARAM1, see MAV_CMD enum</param>
        /// <param name="param2">PARAM2, see MAV_CMD enum</param>
        /// <param name="param3">PARAM3, see MAV_CMD enum</param>
        /// <param name="param4">PARAM4, see MAV_CMD enum</param>
        /// <param name="x">PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7</param>
        /// <param name="y">PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7</param>
        /// <param name="z">PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame).</param>
        /// <param name="attemptCount"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<CommandAckPayload> CommandInt(MavCmd command, MavFrame frame, bool current, bool autoContinue, float param1, float param2, float param3, float param4, int x, int y, float z, CancellationToken cancel = default);
    }
}
