#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client for controlling a vehicle.
/// </summary>
public interface IVehicleClient:IClientDevice
{
    /// <summary>
    /// Gets the ICommandClient interface property.
    /// </summary>
    /// <returns>The ICommandClient property.</returns>
    ICommandClient Commands { get; }

    /// <summary>
    /// Gets the debug client for the current software.
    /// </summary>
    /// <remarks>
    /// The debug client provides debugging functionality and allows interaction with the debugger.
    /// </remarks>
    /// <value>
    /// The debug client interface.
    /// </value>
    IDebugClient Debug { get; }

    /// <summary>
    /// Gets or sets the Dgps client. </summary>
    /// /
    IDgpsClient Dgps { get; }

    /// <summary>
    /// Gets the FTP client interface.
    /// </summary>
    /// <remarks>
    /// The FTP client interface provides methods for managing FTP connections
    /// and performing various FTP operations such as uploading, downloading,
    /// and deleting files on an FTP server.
    /// </remarks>
    IFtpClient Ftp { get; }

    /// <summary>
    /// Gets the GNSS client.
    /// </summary>
    /// <value>
    /// The GNSS client.
    /// </value>
    IGnssClientEx Gnss { get; }

    /// <summary>
    /// Gets the logging client interface.
    /// </summary>
    /// <value>
    /// The logging client interface.
    /// </value>
    ILoggingClient Logging { get; }

    /// <summary>
    /// Gets the IMissionClientEx interface for accessing mission-related functionality.
    /// </summary>
    /// <value>
    /// The IMissionClientEx interface.
    /// </value>
    IMissionClientEx Missions { get; }

    /// <summary>
    /// Represents the offboard client property.
    /// </summary>
    /// <value>
    /// An object implementing the IOffboardClient interface.
    /// </value>
    IOffboardClient Offboard { get; }

    /// <summary>
    /// Gets the Params property.
    /// </summary>
    /// <value>
    /// The Params property.
    /// </value>
    IParamsClientEx Params { get; }

    /// <summary>
    /// Gets the Position client interface.
    /// </summary>
    /// <value>
    /// The Position client interface.
    /// </value>
    IPositionClientEx Position { get; }

    /// Get the real-time telemetry client.
    /// @returns The real-time telemetry client.
    /// /
    ITelemetryClientEx Rtt { get; }

    /// <summary>
    /// Gets or sets the IV2ExtensionClient associated with this object.
    /// </summary>
    /// <value>
    /// The IV2ExtensionClient associated with this object.
    /// </value>
    IV2ExtensionClient V2Extension { get; }

    /// <summary>
    /// Ensures that the application is in guided mode.
    /// </summary>
    /// <param name="cancel">
    /// A cancellation token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    Task EnsureInGuidedMode(CancellationToken cancel);

    /// <summary>
    /// Checks the status of the guided mode.
    /// </summary>
    /// <param name="cancel">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating if the guided mode is enabled or not.</returns>
    Task<bool> CheckGuidedMode(CancellationToken cancel);

    /// <summary>
    /// Navigates to the specified GeoPoint. </summary> <param name="point">The GeoPoint to navigate to.</param> <param name="cancel">The cancellation token to cancel the navigation (optional).</param> <returns>
    /// A Task representing the asynchronous operation.
    /// The task will complete when the navigation to the specified GeoPoint is finished. </returns>
    /// /
    Task GoTo(GeoPoint point, CancellationToken cancel = default);

    /// <summary>
    /// Land method.
    /// </summary>
    /// <param name="cancel">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method is used to initiate the landing process.
    /// The cancel token can be used to cancel the landing operation.
    /// </remarks>
    Task DoLand(CancellationToken cancel = default);

    /// <summary>
    /// Performs a right-to-left operation asynchronously.
    /// </summary>
    /// <param name="cancel">An optional cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DoRtl(CancellationToken cancel = default);

    /// <summary>
    /// Sets the auto mode.
    /// </summary>
    /// <param name="cancel">The cancellation token (optional).</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task SetAutoMode(CancellationToken cancel = default);

    /// <summary>
    /// Initiates the takeoff process and ascends to the specified altitude in meters.
    /// </summary>
    /// <param name="altInMeters">The target altitude in meters to ascend to.</param>
    /// <param name="cancel">The cancellation token (optional) to cancel the takeoff.</param>
    /// <returns>A task representing the asynchronous takeoff operation.</returns>
    Task TakeOff(double altInMeters, CancellationToken cancel = default);

    /// <summary>
    /// Gets the available modes for the vehicle.
    /// </summary>
    /// <returns>
    /// An IEnumerable of IVehicleMode representing the available modes for the vehicle.
    /// </returns>
    IEnumerable<IVehicleMode> AvailableModes { get; }

    /// <summary>
    /// Gets the current mode of the vehicle.
    /// </summary>
    /// <value>
    /// The observable value representing the current mode of the vehicle.
    /// </value>
    IRxValue<IVehicleMode> CurrentMode { get; }

    /// <summary>
    /// Sets the mode of the vehicle asynchronously.
    /// </summary>
    /// <param name="mode">The vehicle mode to set.</param>
    /// <param name="cancel">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default);
}