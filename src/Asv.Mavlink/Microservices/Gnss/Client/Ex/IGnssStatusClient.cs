using System.Reactive.Linq;
using Asv.Common;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Interface for a GNSS status client.
/// </summary>
public interface IGnssStatusClient
{
    /// <summary>
    /// Gets the ground velocity.
    /// </summary>
    /// <returns>
    /// An <see cref="IRxValue{T}"/> representing the ground velocity.
    /// </returns>
    ReadOnlyReactiveProperty<double> GroundVelocity { get; }

    /// <summary>
    /// Gets the property that represents the GPS information.
    /// </summary>
    /// <remarks>
    /// The GPS information is obtained from an IRxValue object. The GPS information represents
    /// latitude, longitude, altitude, and other related details.
    /// </remarks>
    /// <returns>
    /// An IRxValue object of type GpsInfo that contains the GPS information.
    /// </returns>
    ReadOnlyReactiveProperty<GpsInfo> Info { get; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <returns>The position.</returns>
    ReadOnlyReactiveProperty<GeoPoint> Position { get; }
}