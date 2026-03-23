using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended command server.
/// </summary>
/// <typeparam name="TArgPacket">The type of the argument packet.</typeparam>
public interface ICommandServerEx<out TArgPacket> : IMavlinkMicroserviceServer
    where TArgPacket : MavlinkMessage
{
    /// <summary>
    /// Gets the ICommandServer Base property.
    /// </summary>
    /// <remarks>
    /// This property represents the ICommandServer object that serves as the base for a particular functionality.
    /// </remarks>
    /// <returns>The ICommandServer Base property.</returns>
    ICommandServer Base { get; }

    /// <summary>
    /// Sets the command delegate for the specified MAVLink command.
    /// </summary>
    /// <typeparam name="TArgPacket">The type of argument packet expected by the command.</typeparam>
    /// <param name="cmd">The MAVLink command.</param>
    CommandDelegate<TArgPacket>? this[MavCmd cmd] { set; }
    
    /// <summary>
    /// Commands supported by the server.
    /// </summary>
    IEnumerable<MavCmd> SupportedCommands { get; }
}

/// <summary>
/// Represents a delegate that defines a command handler.
/// </summary>
/// <typeparam name="TArgPacket">
/// The type of argument packet for the command handler.
/// </typeparam>
/// <param name="from">
/// The identity of the device that sent the command.
/// </param>
/// <param name="args">
/// The argument packet for the command handler.
/// </param>
/// <param name="cancel">
/// The cancellation token that can be used to cancel the command execution.
/// </param>
/// <returns>
/// A task representing the asynchronous command result.
/// </returns>
public delegate Task<CommandResult> CommandDelegate<in TArgPacket>(
    DeviceIdentity from,
    TArgPacket args,
    CancellationToken cancel
);