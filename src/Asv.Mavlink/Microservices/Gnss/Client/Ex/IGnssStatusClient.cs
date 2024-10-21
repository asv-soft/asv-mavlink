using System;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;
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
    IRxValue<double> GroundVelocity { get; }

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
    IRxValue<GpsInfo?> Info { get; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <returns>The position.</returns>
    IRxValue<GeoPoint> Position { get; }
}

/// <summary>
/// This class represents a GNSS status client that receives GNSS data and provides access to ground velocity, GPS info, and position.
/// </summary>
public class GnssStatusClient : IGnssStatusClient, IDisposable
{
    /// <summary>
    /// Represents the current ground velocity.
    /// </summary>
    private readonly RxValueBehaviour<double> _groundVelocity;

    /// <summary>
    /// Represents a reactive value of type GpsInfo.
    /// </summary>
    private readonly RxValueBehaviour<GpsInfo?> _info;

    /// <summary>
    /// Represents the current position as a GeoPoint.
    /// </summary>
    private readonly RxValueBehaviour<GeoPoint> _position;

    private readonly IDisposable _disposeIt;

    /// Initializes a new instance of the GnssStatusClient class.
    /// @param pipe The observable pipe that provides GpsRawIntPayload objects.
    /// /
    public GnssStatusClient(IObservable<GpsRawIntPayload> pipe)
    {
        _info = new RxValueBehaviour<GpsInfo?>(null);
        var d1 = pipe.Select(p => new GpsInfo(p)).Subscribe(_info);
        _groundVelocity = new RxValueBehaviour<double>(double.NaN);
        var d2 = pipe.Select(p => p.Vel / 100D).Subscribe(_groundVelocity);
        _position = new RxValueBehaviour<GeoPoint>(GeoPoint.Zero);
        var d3 = pipe.Select(p => new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat),
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon),
                MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .Subscribe(_position);
        _disposeIt =  Disposable.Combine(_info, _groundVelocity, _position, d1, d2, d3);
    }

    /// <summary>
    /// Represents a client for handling GNSS status.
    /// </summary>
    /// <param name="pipe">The observable pipe to receive GPS2RawPayload data.</param>
    public GnssStatusClient(IObservable<Gps2RawPayload> pipe)
    {
        _info = new RxValueBehaviour<GpsInfo>(null);
        var d1 = pipe.Select(p => new GpsInfo(p)).Subscribe(_info);
        _groundVelocity = new RxValueBehaviour<double>(Double.NaN);
        var d2 = pipe.Select(p => p.Vel / 100D).Subscribe(_groundVelocity);
        _position = new RxValueBehaviour<GeoPoint>(GeoPoint.Zero);
        var d3 = pipe.Select(p => new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat),
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon),
                MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .Subscribe(_position);
        _disposeIt =  Disposable.Combine(_info, _groundVelocity, _position, d1, d2, d3);
    }


    /// <summary>
    /// Returns the ground velocity as an observable value.
    /// </summary>
    /// <remarks>
    /// The ground velocity is represented by an <see cref="IRxValue{T}"/> object,
    /// which provides a reactive way of accessing the current value and receiving
    /// notifications about any changes in the ground velocity.
    /// </remarks>
    /// <returns>
    /// An <see cref="IRxValue{T}"/> object representing the ground velocity.
    /// </returns>
    public IRxValue<double> GroundVelocity => _groundVelocity;

    /// <summary>
    /// Gets the GPS information.
    /// </summary>
    /// <value>
    /// The GPS information.
    /// </value>
    /// <remarks>
    /// This property provides access to the GPS information represented by an <see cref="IRxValue{T}"/> of type <see cref="GpsInfo"/>.
    /// </remarks>
    public IRxValue<GpsInfo?> Info => _info;

    /// <summary>
    /// Gets the position value as an instance of <see cref="IRxValue{GeoPoint}"/>.
    /// </summary>
    /// <value>
    /// The position value.
    /// </value>
    public IRxValue<GeoPoint> Position => _position;

    public void Dispose()
    {
        _disposeIt.Dispose();
    }
}