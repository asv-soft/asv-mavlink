namespace Asv.Mavlink;

/// <summary>
/// Represents a GNSS client with extended functionalities.
/// </summary>
public interface IGnssClientEx:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets the main object implementing the IGnssStatusClient interface.
    /// </summary>
    /// <remarks>
    /// The IGnssStatusClient interface provides methods and properties
    /// to interact with the GNSS (Global Navigation Satellite System) status.
    /// It allows the user to retrieve information about the status of
    /// satellite signals, including the number of satellites visible,
    /// their signal strength, and their signal-to-noise ratio.
    /// </remarks>
    /// <value>
    /// The main object implementing the IGnssStatusClient interface.
    /// </value>
    IGnssStatusClient Main { get; }

    /// <summary>
    /// Gets the additional GNSS status client.
    /// </summary>
    /// <value>
    /// The additional GNSS status client.
    /// </value>
    IGnssStatusClient Additional { get; }
}