using System.Reactive.Concurrency;

namespace Asv.Mavlink;

/// <summary>
/// Interface for a server device.
/// </summary>
public interface IServerDevice
{
    ICoreServices Core { get; }
    /// <summary>
    /// Starts the process.
    /// </summary>
    /// <remarks>
    /// This method is used to initiate the process and begin the execution of the code.
    /// </remarks>
    void Start();

    /// <summary>
    /// Gets the identity of the Mavlink server.
    /// </summary>
    /// <returns>
    /// Returns the MavlinkServerIdentity object representing the identity of the Mavlink server.
    /// </returns>
    MavlinkIdentity Identity { get; }

    /// <summary>
    /// Gets the server status text.
    /// </summary>
    /// <value>
    /// The server status text.
    /// </value>
    IStatusTextServer StatusText { get; }

    /// <summary>
    /// Gets the heartbeat server interface.
    /// </summary>
    /// <value>
    /// The heartbeat server interface.
    /// </value>
    IHeartbeatServer Heartbeat { get; }
}