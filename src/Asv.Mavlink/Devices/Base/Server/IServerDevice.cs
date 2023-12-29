using System.Reactive.Concurrency;

namespace Asv.Mavlink;

/// <summary>
/// Interface for a server device.
/// </summary>
public interface IServerDevice
{
    /// <summary>
    /// Starts the process.
    /// </summary>
    /// <remarks>
    /// This method is used to initiate the process and begin the execution of the code.
    /// </remarks>
    void Start();

    /// <summary>
    /// Gets the MAVLink V2 connection used for communication.
    /// </summary>
    /// <remarks>
    /// This property should be used to access the MAVLink V2 connection instance.
    /// The connection provides methods to send and receive MAVLink messages.
    /// </remarks>
    /// <returns>
    /// The MAVLink V2 connection used for communication.
    /// </returns>
    IMavlinkV2Connection Connection { get; }

    /// <summary>
    /// Represents a property named Seq that returns an object of type IPacketSequenceCalculator. </summary>
    /// <value>
    /// An object implementing the IPacketSequenceCalculator interface. </value>
    /// /
    IPacketSequenceCalculator Seq { get; }

    /// <summary>
    /// Gets the instance of the scheduler.
    /// </summary>
    /// <value>
    /// The instance of the scheduler.
    /// </value>
    IScheduler Scheduler { get; }

    /// <summary>
    /// Gets the identity of the Mavlink server.
    /// </summary>
    /// <returns>
    /// Returns the MavlinkServerIdentity object representing the identity of the Mavlink server.
    /// </returns>
    MavlinkServerIdentity Identity { get; }

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