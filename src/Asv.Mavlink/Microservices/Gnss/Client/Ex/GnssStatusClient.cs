using System;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// This class represents a GNSS status client that receives GNSS data and provides access to ground velocity, GPS info, and position.
/// </summary>
public sealed class GnssStatusClient : IGnssStatusClient, IDisposable,IAsyncDisposable
{
    

    /// Initializes a new instance of the GnssStatusClient class.
    /// @param pipe The observable pipe that provides GpsRawIntPayload objects.
    /// /
    public GnssStatusClient(Observable<GpsRawIntPayload?> pipe)
    {
        Info =  pipe.WhereNotNull().Select(p => new GpsInfo(p) ).ToReadOnlyReactiveProperty(GpsInfo.NoGps);
        GroundVelocity = pipe.WhereNotNull().Select(p => p.Vel / 100D).ToReadOnlyReactiveProperty();
        Position = new ReactiveProperty<GeoPoint>(GeoPoint.Zero);
        Position = pipe.WhereNotNull().Select(p => new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat),
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon),
                MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .ToReadOnlyReactiveProperty();
    }

    /// <summary>
    /// Represents a client for handling GNSS status.
    /// </summary>
    /// <param name="pipe">The observable pipe to receive GPS2RawPayload data.</param>
    public GnssStatusClient(Observable<Gps2RawPayload?> pipe)
    {
        Info =  pipe.WhereNotNull().Select(p => new GpsInfo(p) ).ToReadOnlyReactiveProperty(GpsInfo.NoGps);
        GroundVelocity = pipe.WhereNotNull().Select(p => p.Vel / 100D).ToReadOnlyReactiveProperty();
        Position = new ReactiveProperty<GeoPoint>(GeoPoint.Zero);
        Position = pipe.WhereNotNull().Select(p => new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat),
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon),
                MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .ToReadOnlyReactiveProperty();
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
    public ReadOnlyReactiveProperty<double> GroundVelocity { get; }

    /// <summary>
    /// Gets the GPS information.
    /// </summary>
    /// <value>
    /// The GPS information.
    /// </value>
    /// <remarks>
    /// This property provides access to the GPS information represented by an <see cref="IRxValue{T}"/> of type <see cref="GpsInfo"/>.
    /// </remarks>
    public ReadOnlyReactiveProperty<GpsInfo> Info { get; }

    /// <summary>
    /// Gets the position value as an instance of <see cref="IRxValue{GeoPoint}"/>.
    /// </summary>
    /// <value>
    /// The position value.
    /// </value>
    public ReadOnlyReactiveProperty<GeoPoint> Position { get; }

    #region Dispose

    public void Dispose()
    {
        GroundVelocity.Dispose();
        Info.Dispose();
        Position.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(GroundVelocity).ConfigureAwait(false);
        await CastAndDispose(Info).ConfigureAwait(false);
        await CastAndDispose(Position).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}