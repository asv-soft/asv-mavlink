namespace Asv.Mavlink;

/// <summary>
/// Represents a client device that can communicate with an ADS-B (Automatic Dependent Surveillance-Broadcast) server.
/// </summary>
public interface IAdsbClientDevice : IClientDevice
{
    /// <summary>
    /// Property to access the Adsb vehicle client.
    /// </summary>
    /// <value>
    /// An object of type <see cref="IAdsbVehicleClient"/>.
    /// </value>
    IAdsbVehicleClient Adsb { get; }
}