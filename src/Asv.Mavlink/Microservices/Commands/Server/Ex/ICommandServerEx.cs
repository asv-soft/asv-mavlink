using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

/// <summary>
/// Represents an extended command server.
/// </summary>
/// <typeparam name="TArgPacket">The type of the argument packet.</typeparam>
public interface ICommandServerEx<out TArgPacket>
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
    CommandDelegate<TArgPacket> this[MavCmd cmd] { set; }
}

/// Represents a delegate that defines a command handler.
/// @typeparam TArgPacket The type of argument packet for the command handler.
/// @param from The identity of the device that sent the command.
/// @param args The argument packet for the command handler.
/// @param cancel The cancellation token that can be used to cancel the command execution.
/// @returns A Task representing the asynchronous command result.
/// /
public delegate Task<CommandResult> CommandDelegate<in TArgPacket>(DeviceIdentity from, TArgPacket args, CancellationToken cancel);