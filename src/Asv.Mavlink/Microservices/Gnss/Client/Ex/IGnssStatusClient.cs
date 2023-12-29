using System;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;

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
    IRxValue<GpsInfo> Info { get; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <returns>The position.</returns>
    IRxValue<GeoPoint> Position { get; }
}

/// <summary>
/// This class represents a GNSS status client that receives GNSS data and provides access to ground velocity, GPS info, and position.
/// </summary>
public class GnssStatusClient : DisposableOnceWithCancel, IGnssStatusClient
{
    /// <summary>
    /// Represents the current ground velocity.
    /// </summary>
    private readonly RxValue<double> _groundVelocity;

    /// <summary>
    /// Represents a reactive value of type GpsInfo.
    /// </summary>
    private readonly RxValue<GpsInfo> _info;

    /// <summary>
    /// Represents the current position as a GeoPoint.
    /// </summary>
    private readonly RxValue<GeoPoint> _position;

    /// Initializes a new instance of the GnssStatusClient class.
    /// @param pipe The observable pipe that provides GpsRawIntPayload objects.
    /// /
    public GnssStatusClient(IObservable<GpsRawIntPayload> pipe)
    {
        _info = new RxValue<GpsInfo>(null).DisposeItWith(Disposable);
        pipe.Select(_ => new GpsInfo(_)).Subscribe(_info).DisposeItWith(Disposable);
        _groundVelocity = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        pipe.Select(_ => _.Vel / 100D).Subscribe(_groundVelocity).DisposeItWith(Disposable);
        _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        pipe.Select(_ => new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(_.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(_.Lon), MavlinkTypesHelper.AltFromMmToDoubleMeter(_.Alt))).Subscribe(_position).DisposeItWith(Disposable);
    }

    /// <summary>
    /// Represents a client for handling GNSS status.
    /// </summary>
    /// <param name="pipe">The observable pipe to receive GPS2RawPayload data.</param>
    public GnssStatusClient(IObservable<Gps2RawPayload> pipe)
    {
        _info = new RxValue<GpsInfo>(null).DisposeItWith(Disposable);
        pipe.Select(_ => new GpsInfo(_)).Subscribe(_info).DisposeItWith(Disposable);
        _groundVelocity = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        pipe.Select(_ => _.Vel / 100D).Subscribe(_groundVelocity).DisposeItWith(Disposable);
        _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        pipe.Select(_ => new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(_.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(_.Lon), MavlinkTypesHelper.AltFromMmToDoubleMeter(_.Alt))).Subscribe(_position).DisposeItWith(Disposable);
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
    public IRxValue<GpsInfo> Info => _info;

    /// <summary>
    /// Gets the position value as an instance of <see cref="IRxValue{GeoPoint}"/>.
    /// </summary>
    /// <value>
    /// The position value.
    /// </value>
    public IRxValue<GeoPoint> Position => _position;
}