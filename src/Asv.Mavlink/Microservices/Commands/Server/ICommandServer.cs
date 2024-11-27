using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    /// Represents a command server that receives and sends commands.
    /// /
    public interface ICommandServer:IMavlinkMicroserviceServer
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
        Observable<CommandLongPacket> OnCommandLong { get; }

        /// <summary>
        /// Gets an observable sequence of CommandIntPacket events.
        /// </summary>
        /// <remarks>
        /// This property returns an IObservable<CommandIntPacket> that can be subscribed to in order to receive CommandIntPacket events.
        /// </remarks>
        Observable<CommandIntPacket> OnCommandInt { get; }

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
        ValueTask SendCommandAck(MavCmd cmd, DeviceIdentity responseTarget, CommandResult result,
            CancellationToken cancel = default);
    }

    /// Represents the result of a command execution.
    /// /
    public class CommandResult
    {
        #region Static

        private static CommandResult? _accepted;
        public static CommandResult Accepted => _accepted ??= new(MavResult.MavResultAccepted);
       
        private static CommandResult? _temporarilyRejected;
        public static CommandResult TemporarilyRejected => _temporarilyRejected ??= new(MavResult.MavResultTemporarilyRejected);
        
       
        
        private static CommandResult? _denied;
        public static CommandResult Denied => _denied ??= new(MavResult.MavResultDenied);
        
        private static CommandResult? _unsupported;
        public static CommandResult Unsupported => _unsupported ??= new(MavResult.MavResultUnsupported);
        
        private static CommandResult? _failed;
        public static CommandResult Failed => _failed ??= new(MavResult.MavResultFailed);
        
        private static CommandResult? _inProgress;
        public static CommandResult InProgress => _inProgress ??= new(MavResult.MavResultInProgress);
        
        private static CommandResult? _cancelled;
        public static CommandResult Cancelled => _cancelled ??= new(MavResult.MavResultCancelled);
        
        private static CommandResult? _commandLongOnly;
        public static CommandResult CommandLongOnly => _commandLongOnly ??= new(MavResult.MavResultCommandLongOnly);
        
        private static CommandResult? _commandIntOnly;
        public static CommandResult CommandIntOnly => _commandIntOnly ??= new(MavResult.MavResultCommandIntOnly);
        
        private static CommandResult? _commandUnsupportedMavFrame;
        
        public static CommandResult CommandUnsupportedMavFrame => _commandUnsupportedMavFrame ??= new(MavResult.MavResultCommandUnsupportedMavFrame);
        
        private static Task<CommandResult>? _acceptedTask;
        public static Task<CommandResult> AcceptedTask => _acceptedTask ??= Task.FromResult(Accepted);
        
        private static Task<CommandResult>? _temporarilyRejectedTask;
        public static Task<CommandResult> TemporarilyRejectedTask => _temporarilyRejectedTask ??= Task.FromResult(TemporarilyRejected);
        
        private static Task<CommandResult>? _deniedTask;
        public static Task<CommandResult> DeniedTask => _deniedTask ??= Task.FromResult(Denied);
        
        private static Task<CommandResult>? _unsupportedTask;
        public static Task<CommandResult> UnsupportedTask => _unsupportedTask ??= Task.FromResult(Unsupported);
        
        private static Task<CommandResult>? _failedTask;
        public static Task<CommandResult> FailedTask => _failedTask ??= Task.FromResult(Failed);
        
        private static Task<CommandResult>? _inProgressTask;
        public static Task<CommandResult> InProgressTask => _inProgressTask ??= Task.FromResult(InProgress);
        
        private static Task<CommandResult>? _cancelledTask;
        public static Task<CommandResult> CancelledTask => _cancelledTask ??= Task.FromResult(Cancelled);
        
        private static Task<CommandResult>? _commandLongOnlyTask;
        public static Task<CommandResult> CommandLongOnlyTask => _commandLongOnlyTask ??= Task.FromResult(CommandLongOnly);
        
        private static Task<CommandResult>? _commandIntOnlyTask;
        public static Task<CommandResult> CommandIntOnlyTask => _commandIntOnlyTask ??= Task.FromResult(CommandIntOnly);
        
        private static Task<CommandResult>? _commandUnsupportedMavFrameTask;
        public static Task<CommandResult> CommandUnsupportedMavFrameTask => _commandUnsupportedMavFrameTask ??= Task.FromResult(CommandUnsupportedMavFrame);
        
        

        
        public static CommandResult FromResult(MavResult result)
        {
            return result switch
            {
                MavResult.MavResultAccepted => Accepted,
                MavResult.MavResultTemporarilyRejected => TemporarilyRejected,
                MavResult.MavResultDenied => Denied,
                MavResult.MavResultUnsupported => Unsupported,
                MavResult.MavResultFailed => Failed,
                MavResult.MavResultInProgress => InProgress,
                MavResult.MavResultCancelled => Cancelled,
                MavResult.MavResultCommandLongOnly => CommandLongOnly,
                MavResult.MavResultCommandIntOnly => CommandIntOnly,
                MavResult.MavResultCommandUnsupportedMavFrame => CommandUnsupportedMavFrame,
                _ => CommandResult.FromResult(result)
            };
        }
        public static Task<CommandResult> AsTask(MavResult result)
        {
            return result switch
            {
                MavResult.MavResultAccepted => AcceptedTask,
                MavResult.MavResultTemporarilyRejected => TemporarilyRejectedTask,
                MavResult.MavResultDenied => DeniedTask,
                MavResult.MavResultUnsupported => UnsupportedTask,
                MavResult.MavResultFailed => FailedTask,
                MavResult.MavResultInProgress => InProgressTask,
                MavResult.MavResultCancelled => CancelledTask,
                MavResult.MavResultCommandLongOnly => CommandLongOnlyTask,
                MavResult.MavResultCommandIntOnly => CommandIntOnlyTask,
                MavResult.MavResultCommandUnsupportedMavFrame => CommandUnsupportedMavFrameTask,
                _ => Task.FromResult(CommandResult.FromResult(result))
            };
        }
        

        #endregion
        
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
