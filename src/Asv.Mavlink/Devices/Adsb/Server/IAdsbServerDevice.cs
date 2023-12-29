namespace Asv.Mavlink;

/// <summary>
/// Represents a server device that supports ADS-B (Automatic Dependent Surveillance-Broadcast) functionality.
/// </summary>
public interface IAdsbServerDevice : IServerDevice
{
    /// <summary>
    /// Gets the Adsb vehicle server.
    /// </summary>
    /// <value>
    /// The Adsb vehicle server.
    /// </value>
    IAdsbVehicleServer Adsb { get; }
}