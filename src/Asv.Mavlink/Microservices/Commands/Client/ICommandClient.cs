using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface ICommandClient
    {
        IObservable<CommandAckPayload> OnCommandAck { get; }
        Task<CommandAckPayload> CommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel);
        Task SendCommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7,  CancellationToken cancel);

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
            float param3, float param4, int x, int y, float z, CancellationToken cancel);
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
        Task<CommandAckPayload> CommandInt(MavCmd command, MavFrame frame, bool current, bool autoContinue, float param1, float param2, float param3, float param4, int x, int y, float z, CancellationToken cancel);
    }
    
    public static class CommandClientHelper
    {
        public static async Task CommandLongAndCheckResult(this ICommandClient client, MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel)
        {
            var result = await client.CommandLong(command, param1, param2, param3, param4, param5, param6, param7, cancel).ConfigureAwait(false);
            switch (result.Result)
            {
                case MavResult.MavResultTemporarilyRejected:
                case MavResult.MavResultDenied:
                case MavResult.MavResultUnsupported:
                case MavResult.MavResultFailed:
                    throw new CommandException(result);
            }
        }
    }
}
