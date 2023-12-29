using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client for retrieving ADS-B vehicle data.
/// </summary>
public interface IAdsbVehicleClient : IDisposable
{
    /// <summary>
    /// Gets an observable sequence of AdsbVehiclePayload, representing the events when a target is detected.
    /// </summary>
    /// <value>The observable sequence of AdsbVehiclePayload.</value>
    IObservable<AdsbVehiclePayload> OnTarget { get; }

    /// <summary>
    /// Gets the property representing the collection of aircraft targets.
    /// </summary>
    /// <remarks>
    /// The collection contains <see cref="IChangeSet{TObject, TKey}"/> objects that represent changes to the aircraft targets.
    /// Each change set contains <see cref="IAdsbVehicle"/> objects which represent the aircraft targets.
    /// </remarks>
    /// <returns>
    /// An <see cref="IObservable{T}"/> representing the property that emits changes to the aircraft targets.
    /// </returns>
    IObservable<IChangeSet<IAdsbVehicle, uint>> Targets { get; }

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
    IRxEditableValue<TimeSpan> TargetTimeout { get; }
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
    /// An <see cref="IRxValue{T}"/> representing the call sign.
    /// </returns>
    IRxValue<string> CallSign { get; }

    /// <summary>
    /// The property Location represents the current location as a geographical point.
    /// </summary>
    /// <returns>
    /// An IRxValue object containing a GeoPoint value.
    /// </returns>
    IRxValue<GeoPoint> Location { get; }

    /// <summary>
    /// Represents the type of altitude for ADS-B data.
    /// </summary>
    /// <value>
    /// An object that provides an observable stream of <see cref="AdsbAltitudeType"/> values.
    /// </value>
    IRxValue<AdsbAltitudeType> AltitudeType { get; }

    /// <summary>
    /// Gets the heading value.
    /// </summary>
    /// <remarks>
    /// The heading value represents the direction in which an object is pointing.
    /// </remarks>
    /// <returns>The heading value.</returns>
    IRxValue<double> Heading { get; }

    /// <summary>
    /// Represents the emitter type of an ADS-B message.
    /// </summary>
    /// <returns>The emitter type as an observable value.</returns>
    IRxValue<AdsbEmitterType> EmitterType { get; }

    /// <summary>
    /// Gets the time since the last communication.
    /// </summary>
    /// <value>
    /// The time since the last communication as an <see cref="IRxValue{T}"/> instance
    /// representing a <see cref="TimeSpan"/>.
    /// </value>
    IRxValue<TimeSpan> Tslc { get; }

    /// <summary>
    /// The Flags property represents the flags associated with an Adsb message.
    /// </summary>
    /// <remarks>
    /// The value of this property is of type IRxValue<AdsbFlags>, which is an interface
    /// representing a reactive value that can be observed for changes. The AdsbFlags
    /// enumeration contains various flags that can be set or unset.
    /// </remarks>
    IRxValue<AdsbFlags> Flags { get; }

    /// <summary>
    /// Gets the horizontal velocity.
    /// </summary>
    /// <remarks>
    /// This property provides access to the horizontal velocity of an object.
    /// The value is of type double and can be retrieved using an IRxValue wrapper.
    /// </remarks>
    /// <returns>
    /// An instance of IRxValue<double> that represents the horizontal velocity.
    /// </returns>
    IRxValue<double> HorVelocity { get; }

    /// <summary>
    /// Gets the vertical velocity of an object.
    /// </summary>
    /// <returns>
    /// An IRxValue<double> representing the vertical velocity of the object.
    /// </returns>
    IRxValue<double> VerVelocity { get; }

    /// <summary>
    /// Gets the value of the Squawk property.
    /// </summary>
    /// <returns>
    /// The value of the Squawk property.
    /// </returns>
    IRxValue<ushort> Squawk { get; }
}

