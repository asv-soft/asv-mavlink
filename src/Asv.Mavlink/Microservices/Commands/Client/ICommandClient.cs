using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a command client that is capable of sending commands to an external system.
    /// </summary>
    public interface ICommandClient:IMavlinkMicroserviceClient
    {
        /// <summary>
        /// Gets the observable sequence for receiving command acknowledgement payloads.
        /// </summary>
        /// <value>
        /// The observable sequence for receiving command acknowledgement payloads.
        /// </value>
        Observable<CommandAckPayload> OnCommandAck { get; }

        /// <summary>
        /// Sends a command with long parameter to the vehicle.
        /// </summary>
        /// <param name="edit">The delegate used to edit the command payload.</param>
        /// <param name="cancel">The cancellation token to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the acknowledgment payload received from the vehicle.</returns>
        Task<CommandAckPayload> CommandLong(Action<CommandLongPayload> edit, CancellationToken cancel = default);

        /// <summary>
        /// Executes a long-duration command with specified parameters.
        /// </summary>
        /// <param name="command">The MAVLink command to execute.</param>
        /// <param name="param1">The first parameter value.</param>
        /// <param name="param2">The second parameter value.</param>
        /// <param name="param3">The third parameter value.</param>
        /// <param name="param4">The fourth parameter value.</param>
        /// <param name="param5">The fifth parameter value.</param>
        /// <param name="param6">The sixth parameter value.</param>
        /// <param name="param7">The seventh parameter value.</param>
        /// <param name="cancel">A CancellationToken to cancel the command execution.</param>
        /// <returns>
        /// A Task object representing the asynchronous operation with a CommandAckPayload
        /// containing the acknowledgment result of the command execution.
        /// </returns>
        /// <remarks>
        /// This method executes a long-duration command with the specified parameters on
        /// the MAVLink system. A long-duration command represents a command that may take some
        /// time to complete and needs periodic checking for progress or completion.
        /// </remarks>
        Task<CommandAckPayload> CommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default);

        /// <summary>
        /// Executes a long command and waits for the response packet.
        /// </summary>
        /// <typeparam name="TAnswerPacket">The type of the response packet.</typeparam>
        /// <param name="command">The command to be executed.</param>
        /// <param name="param1">The first command parameter.</param>
        /// <param name="param2">The second command parameter.</param>
        /// <param name="param3">The third command parameter.</param>
        /// <param name="param4">The fourth command parameter.</param>
        /// <param name="param5">The fifth command parameter.</param>
        /// <param name="param6">The sixth command parameter.</param>
        /// <param name="param7">The seventh command parameter.</param>
        /// <param name="cancel">The cancellation token to cancel the command execution.</param>
        /// <returns>A task that represents the asynchronous operation and contains the response packet.</returns>
        /// <exception cref="ArgumentException">Thrown when the TAnswerPacket is not a valid packet type.</exception>
        /// ```
        /// Example usage:
        /// Task<TAnswerPacket> commandResponse = CommandLongAndWaitPacket<SomePacketType>(MavCmd.SomeCommand, 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f);
        /// ```
        /// </returns>
        Task<TAnswerPacket> CommandLongAndWaitPacket<TAnswerPacket>(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default)
            where TAnswerPacket : MavlinkMessage, new();

        /// <summary>
        /// Sends a long command to the specified MavCmd with the provided parameters.
        /// </summary>
        /// <param name="command">The MavCmd command to send.</param>
        /// <param name="param1">The first parameter of the command.</param>
        /// <param name="param2">The second parameter of the command.</param>
        /// <param name="param3">The third parameter of the command.</param>
        /// <param name="param4">The fourth parameter of the command.</param>
        /// <param name="param5">The fifth parameter of the command.</param>
        /// <param name="param6">The sixth parameter of the command.</param>
        /// <param name="param7">The seventh parameter of the command.</param>
        /// <param name="cancel">The cancellation token to cancel the command (optional).</param>
        /// <returns>A task representing the asynchronous operation of sending the command.</returns>
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
        ValueTask SendCommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue,
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
