using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client for receiving GNSS data.
/// </summary>
public interface IGnssClient:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets the main property of the class.
    /// This property represents an ReadOnlyReactiveProperty object that contains a GpsRawIntPayload value.
    /// </summary>
    /// <returns>
    /// The ReadOnlyReactiveProperty object that contains a GpsRawIntPayload value.
    /// </returns>
    ReadOnlyReactiveProperty<GpsRawIntPayload?> Main { get; }

    /// <summary>
    /// Gets the additional information related to GPS raw payload.
    /// </summary>
    /// <value>
    /// The additional information related to GPS raw payload.
    /// </value>
    ReadOnlyReactiveProperty<Gps2RawPayload?> Additional { get; }
}