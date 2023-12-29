using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// Represents a command server that receives and sends commands.
    /// /
    public interface ICommandServer
    {
        /// <summary>
        /// Gets the event stream for receiving CommandLong packets.
        /// </summary>
        /// <remarks>
        /// CommandLong packets are sent from the vehicle to the ground station as responses to MAVLink command messages.
        /// This event stream allows subscribing to received CommandLong packets.
        /// </remarks>
        /// <returns>
        /// An <see cref="IObservable{T}"/> of <see cref="CommandLongPacket"/> representing the event stream for receiving CommandLong packets.
        /// </returns>
        IObservable<CommandLongPacket> OnCommandLong { get; }

        /// <summary>
        /// Gets an observable sequence of CommandIntPacket events.
        /// </summary>
        /// <remarks>
        /// This property returns an IObservable<CommandIntPacket> that can be subscribed to in order to receive CommandIntPacket events.
        /// </remarks>
        IObservable<CommandIntPacket> OnCommandInt { get; }

        /// <summary>
        /// Sends a command acknowledgement with the specified parameters.
        /// </summary>
        /// <param name="cmd">The command being acknowledged.</param>
        /// <param name="responseTarget">The target device identity for the acknowledgement.</param>
        /// <param name="result">The result of the command execution.</param>
        /// <param name="cancel">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method sends a command acknowledgement to the specified target device identity.
        /// The acknowledgement includes information about the command being acknowledged and the result of its execution.
        /// It is an asynchronous operation that returns a task representing the completion of the send operation.
        /// </remarks>
        Task SendCommandAck(MavCmd cmd, DeviceIdentity responseTarget, CommandResult result, CancellationToken cancel = default);
    }

    /// <summary>
    /// Helper class containing extension methods for ICommandServer interface.
    /// </summary>
    public static class CommandServerHelper
    {
        /// <summary>
        /// Sends command acknowledge accepted to the command server.
        /// </summary>
        /// <param name="server">The command server.</param>
        /// <param name="req">The command request packet.</param>
        /// <param name="result">The mav result.</param>
        /// <param name="cancel">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task SendCommandAckAccepted(this ICommandServer server, CommandIntPacket req, MavResult result, CancellationToken cancel = default)
        {
            return server.SendCommandAck(req.Payload.Command, new DeviceIdentity(req.SystemId,req.ComponentId), new CommandResult(result), cancel);
        }

        /// <summary>
        /// Sends a command acknowledgment indicating that the command has been accepted.
        /// </summary>
        /// <param name="server">The command server.</param>
        /// <param name="req">The command long packet.</param>
        /// <param name="result">The result of the command.</param>
        /// <param name="cancel">The cancellation token (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public static Task SendCommandAckAccepted(this ICommandServer server, CommandLongPacket req, MavResult result, CancellationToken cancel = default)
        {
            return server.SendCommandAck(req.Payload.Command, new DeviceIdentity(req.SystemId,req.ComponentId), new CommandResult(result), cancel);
        }
    }

    /// Represents the result of a command execution.
    /// /
    public class CommandResult
    {
        /// <summary>
        /// Represents the result of a command.
        /// </summary>
        /// <param name="resultCode">The result code of the command.</param>
        /// <param name="result">The result value.</param>
        /// <param name="progress">The progress value.</param>
        /// <param name="resultParam2">An optional second result parameter.</param>
        public CommandResult(MavResult resultCode, int result = 0,byte? progress = null,int? resultParam2 = null)
        {
            ResultCode = resultCode;
            Progress = progress;
            Result = result;
            ResultParam2 = resultParam2;
        }

        /// <summary>
        /// Gets or sets the second result parameter.
        /// </summary>
        /// <value>
        /// The second result parameter. Can be null.
        /// </value>
        public int? ResultParam2 { get; set; }

        /// <summary>
        /// Gets the result code of a MAV operation.
        /// </summary>
        /// <value>
        /// The result code indicates the outcome of a MAV operation.
        /// </value>
        public MavResult ResultCode { get; }

        /// <summary>
        /// Gets the result value.
        /// </summary>
        /// <value>
        /// The result value.
        /// </value>
        public int Result { get; }

        /// <summary>
        /// Gets the progress of the property. </summary>
        /// <remarks>
        /// This property represents the progress value as a byte. It can range from 0 to 100.
        /// If the progress is not available, it returns null. </remarks>
        /// <value>
        /// The progress value as a byte, or null if the progress is not available. </value>
        /// /
        public byte? Progress { get; }
        
    }

    /// <summary>
    /// Represents the identity of a device.
    /// </summary>
    public class DeviceIdentity
    {
        /// <summary>
        /// Represents the broadcast device identity.
        /// </summary>
        public static readonly DeviceIdentity Broadcast = new() { ComponentId = 0, SystemId = 0 };

        /// <summary>
        /// Represents a device identity.
        /// </summary>
        public DeviceIdentity()
        {
            
        }

        /// Initializes a new instance of the DeviceIdentity class.
        /// @param systemId The system ID.
        /// @param componentId The component ID.
        /// /
        public DeviceIdentity(byte systemId,byte componentId)
        {
            SystemId = systemId;
            ComponentId = componentId;
        }

        /// <summary>
        /// Gets or sets the system ID.
        /// </summary>
        /// <value>
        /// The system ID.
        /// </value>
        public byte SystemId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the component.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public byte ComponentId { get; set; }
    }


   

    

   
}
