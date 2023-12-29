using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client for receiving GNSS data.
/// </summary>
public interface IGnssClient
{
    /// <summary>
    /// Gets the main property of the class.
    /// This property represents an IRxValue object that contains a GpsRawIntPayload value.
    /// </summary>
    /// <returns>
    /// The IRxValue object that contains a GpsRawIntPayload value.
    /// </returns>
    IRxValue<GpsRawIntPayload> Main { get; }

    /// <summary>
    /// Gets the additional information related to GPS raw payload.
    /// </summary>
    /// <value>
    /// The additional information related to GPS raw payload.
    /// </value>
    IRxValue<Gps2RawPayload> Additional { get; }
}