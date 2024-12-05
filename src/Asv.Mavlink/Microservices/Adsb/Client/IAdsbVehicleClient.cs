using System;
using Asv.Common;
using Asv.Mavlink.Common;

using ObservableCollections;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client for retrieving ADS-B vehicle data.
/// </summary>
public interface IAdsbVehicleClient : IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets an observable sequence of AdsbVehiclePayload, representing the events when a target is detected.
    /// </summary>
    /// <value>The observable sequence of AdsbVehiclePayload.</value>
    Observable<AdsbVehiclePayload> OnTarget { get; }

    /// <summary>
    /// Gets the property representing the collection of aircraft targets.
    /// </summary>
    IReadOnlyObservableDictionary<uint,IAdsbVehicle> Targets { get; }

    /// <summary>
    /// Gets the target timeout for the editable value.
    /// </summary>
    /// <remarks>
    /// The target timeout specifies the maximum amount of time that is allowed for the value to be edited.
    /// Once the target timeout is reached, any ongoing editing process will be automatically canceled.
    /// </remarks>
    /// <returns>
    /// The target timeout for the editable value, specified as a TimeSpan.
    /// </returns>
    ReactiveProperty<TimeSpan> TargetTimeout { get; }
}

/// <summary>
/// Represents an ADS-B vehicle.
/// </summary>
public interface IAdsbVehicle
{
    /// <summary>
    /// Gets the ICAO (International Civil Aviation Organization) address of the entity.
    /// </summary>
    /// <value>
    /// The ICAO address.
    /// </value>
    uint IcaoAddress { get; }

    /// <summary>
    /// Gets the call sign of the object.
    /// </summary>
    /// <returns>
    /// An <see cref="ReadOnlyReactiveProperty{T}"/> representing the call sign.
    /// </returns>
    ReadOnlyReactiveProperty<string> CallSign { get; }

    /// <summary>
    /// The property Location represents the current location as a geographical point.
    /// </summary>
    /// <returns>
    /// An ReadOnlyReactiveProperty object containing a GeoPoint value.
    /// </returns>
    ReadOnlyReactiveProperty<GeoPoint> Location { get; }

    /// <summary>
    /// Represents the type of altitude for ADS-B data.
    /// </summary>
    /// <value>
    /// An object that provides an observable stream of <see cref="AdsbAltitudeType"/> values.
    /// </value>
    ReadOnlyReactiveProperty<AdsbAltitudeType> AltitudeType { get; }

    /// <summary>
    /// Gets the heading value.
    /// </summary>
    /// <remarks>
    /// The heading value represents the direction in which an object is pointing.
    /// </remarks>
    /// <returns>The heading value.</returns>
    ReadOnlyReactiveProperty<double> Heading { get; }

    /// <summary>
    /// Represents the emitter type of an ADS-B message.
    /// </summary>
    /// <returns>The emitter type as an observable value.</returns>
    ReadOnlyReactiveProperty<AdsbEmitterType> EmitterType { get; }

    /// <summary>
    /// Gets the time since the last communication.
    /// </summary>
    /// <value>
    /// The time since the last communication as an <see cref="ReadOnlyReactiveProperty{T}"/> instance
    /// representing a <see cref="TimeSpan"/>.
    /// </value>
    ReadOnlyReactiveProperty<TimeSpan> Tslc { get; }

    /// <summary>
    /// The Flags property represents the flags associated with an Adsb message.
    /// </summary>
    /// <remarks>
    /// The value of this property is of type ReadOnlyReactiveProperty<AdsbFlags>, which is an interface
    /// representing a reactive value that can be observed for changes. The AdsbFlags
    /// enumeration contains various flags that can be set or unset.
    /// </remarks>
    ReadOnlyReactiveProperty<AdsbFlags> Flags { get; }

    /// <summary>
    /// Gets the horizontal velocity.
    /// </summary>
    /// <remarks>
    /// This property provides access to the horizontal velocity of an object.
    /// The value is of type double and can be retrieved using an ReadOnlyReactiveProperty wrapper.
    /// </remarks>
    /// <returns>
    /// An instance of ReadOnlyReactiveProperty<double> that represents the horizontal velocity.
    /// </returns>
    ReadOnlyReactiveProperty<double> HorVelocity { get; }

    /// <summary>
    /// Gets the vertical velocity of an object.
    /// </summary>
    /// <returns>
    /// An ReadOnlyReactiveProperty<double> representing the vertical velocity of the object.
    /// </returns>
    ReadOnlyReactiveProperty<double> VerVelocity { get; }

    /// <summary>
    /// Gets the value of the Squawk property.
    /// </summary>
    /// <returns>
    /// The value of the Squawk property.
    /// </returns>
    ReadOnlyReactiveProperty<ushort> Squawk { get; }
}

