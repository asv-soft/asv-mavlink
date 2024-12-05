using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended interface for controlling the position of a client.
/// </summary>
public interface IPositionClientEx : IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets the client for interacting with Position data.
    /// </summary>
    IPositionClient Base { get; }

    /// <summary>
    /// Gets the pitch value.
    /// </summary>
    /// <remarks>
    /// The pitch value represents the frequency of a sound or musical note.
    /// </remarks>
    /// <returns>
    /// An object implementing the <see cref="ReadOnlyReactiveProperty{T}"/> interface with a generic argument of type <see cref="double"/>.
    /// The value represents the pitch of the sound or musical note.
    /// </returns>
    ReadOnlyReactiveProperty<double> Pitch { get; }

    /// <summary>
    /// Gets the pitch speed property.
    /// </summary>
    /// <remarks>
    /// The pitch speed property represents the speed at which the pitch changes in a certain context.
    /// </remarks>
    /// <returns>
    /// An <see cref="ReadOnlyReactiveProperty{T}"/> instance that represents the pitch speed property.
    /// </returns>
    ReadOnlyReactiveProperty<double> PitchSpeed { get; }

    /// <summary>
    /// Gets the roll value of the object.
    /// </summary>
    /// <value>
    /// The roll value represented as an <see cref="ReadOnlyReactiveProperty{T}"/> interface, where T is <see cref="double"/>.
    /// </value>
    ReadOnlyReactiveProperty<double> Roll { get; }

    /// <summary>
    /// Gets the roll speed value.
    /// </summary>
    /// <remarks>
    /// The roll speed value represents the rotational speed of an object around its longitudinal axis.
    /// </remarks>
    /// <returns>An instance of ReadOnlyReactiveProperty&lt;double&gt; representing the roll speed.</returns>
    ReadOnlyReactiveProperty<double> RollSpeed { get; }

    /// <summary>
    /// Gets the yaw value.
    /// </summary>
    /// <returns>
    /// The yaw value as an Rx value of type double.
    /// </returns>
    ReadOnlyReactiveProperty<double> Yaw { get; }

    /// <summary>
    /// Gets the yaw speed.
    /// </summary>
    /// <remarks>
    /// The yaw speed represents the rate of change of the yaw angle in a particular scenario.
    /// It is expressed in degrees per second.
    /// </remarks>
    /// <returns>An <see cref="ReadOnlyReactiveProperty{T}"/> object representing the yaw speed.</returns>
    ReadOnlyReactiveProperty<double> YawSpeed { get; }

    /// <summary>
    /// Gets the current value of type GeoPoint.
    /// </summary>
    /// <value>
    /// The current value.
    /// </value>
    ReadOnlyReactiveProperty<GeoPoint> Current { get; }

    /// <summary>
    /// Gets the target value of type ReadOnlyReactiveProperty<GeoPoint?>.
    /// </summary>
    /// <remarks>
    /// The Target property represents a nullable GeoPoint value that can be observed and subscribed to.
    /// It is an interface that provides methods and properties for working with the target value.
    /// </remarks>
    /// <returns>
    /// The target value of type ReadOnlyReactiveProperty<GeoPoint?>.
    /// </returns>
    ReadOnlyReactiveProperty<GeoPoint?> Target { get; }

    /// <summary>
    /// The property representing the home location.
    /// </summary>
    /// <remarks>
    /// Use this property to get or set the home location, which is a <see cref="GeoPoint"/> object.
    /// The home location is represented as an <see cref="ReadOnlyReactiveProperty{GeoPoint?}"/> object,
    /// allowing for asynchronous and reactive operations on the value.
    /// </remarks>
    ReadOnlyReactiveProperty<GeoPoint?> Home { get; }

    /// <summary>
    /// Gets the altitude above home.
    /// </summary>
    /// <returns>The altitude above home.</returns>
    ReadOnlyReactiveProperty<double> AltitudeAboveHome { get; }

    /// <summary>
    /// Gets the home position.
    /// </summary>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns the home position.</returns>
    Task GetHomePosition(CancellationToken cancel = default);

    /// <summary>
    /// Represents the distance from a home location.
    /// </summary>
    /// <remarks>
    /// This property provides the distance from a home location.
    /// The distance is measured in units such as miles or kilometers.
    /// </remarks>
    /// <returns>
    /// An <see cref="ReadOnlyReactiveProperty{T}"/> representing the home distance.
    /// </returns>
    ReadOnlyReactiveProperty<double> HomeDistance { get; }

    /// <summary>
    /// Gets the target distance.
    /// </summary>
    /// <value>
    /// The target distance.
    /// </value>
    /// <remarks>
    /// This property represents the target distance value. The value is of type <see cref="ReadOnlyReactiveProperty{T}"/> with double as the generic type parameter.
    /// </remarks>
    ReadOnlyReactiveProperty<double> TargetDistance { get; }

    /// <summary>
    /// Represents a property that indicates whether the object is armed.
    /// </summary>
    /// <remarks>
    /// The IsArmed property provides read-only access to a boolean value indicating
    /// whether the object is armed or not. An armed object typically means that it is
    /// ready to perform certain actions or operations.
    /// </remarks>
    /// <returns>
    /// An instance of ReadOnlyReactiveProperty&lt;bool&gt; representing the IsArmed property.
    /// </returns>
    ReadOnlyReactiveProperty<bool> IsArmed { get; }

    /// <summary>
    /// Gets the armed time as an observable value of type TimeSpan.
    /// </summary>
    /// <returns>
    /// An interface representing an observable value of type TimeSpan that holds the armed time.
    /// </returns>
    ReadOnlyReactiveProperty<TimeSpan> ArmedTime { get; }

    /// <summary>
    /// Arms or disarms the system.
    /// </summary>
    /// <param name="isArm">A boolean value indicating whether to arm or disarm the system. True to arm, false to disarm.</param>
    /// <param name="cancel">A cancellation token to cancel the operation. Defaults to the default cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method arms or disarms the system based on the value of the <paramref name="isArm"/> parameter.
    /// A cancellation token can be provided to cancel the operation if needed.
    /// </remarks>
    Task ArmDisarm(bool isArm, CancellationToken cancel = default);

    /// <summary>
    /// Gets the Roi property.
    /// </summary>
    /// <returns>The Roi property of type <see cref="ReadOnlyReactiveProperty{T}"/> with a generic argument of <see cref="GeoPoint?"/>.</returns>
    ReadOnlyReactiveProperty<GeoPoint?> Roi { get; }

    /// <summary>
    /// Sets the region of interest (ROI) using the specified location.
    /// </summary>
    /// <param name="location">The geographical point representing the location of the ROI.</param>
    /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task SetRoi(GeoPoint location, CancellationToken cancel = default);

    /// <summary>
    /// Clears the Region of Interest (ROI).
    /// </summary>
    /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ClearRoi(CancellationToken cancel = default);

    /// <summary>
    /// Sets the target for the application.
    /// </summary>
    /// <param name="point">The target GeoPoint to set.</param>
    /// <param name="none">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method sets the target for the application. The target GeoPoint
    /// specifies the point to which the application will navigate.
    /// </remarks>
    ValueTask SetTarget(GeoPoint point, CancellationToken none);

    /// <summary>
    /// Initiates the takeoff process.
    /// </summary>
    /// <param name="altInMeters">The altitude to reach in meters.</param>
    /// <param name="cancel">Optional cancellation token to cancel the takeoff process.</param>
    /// <returns>A task representing the asynchronous takeoff process.</returns>
    Task TakeOff(double altInMeters, CancellationToken cancel=default);
    
    /// <summary>
    /// Initiates vertical takeoff
    /// </summary>
    /// <param name="altInMeters">The altitude to reach in meters.</param>
    /// <param name="cancel">Optional cancellation token to cancel the takeoff process.</param>
    /// <returns>A task representing the asynchronous takeoff process for quad planes.</returns>
    Task QTakeOff(double altInMeters, CancellationToken cancel=default);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="landOption">VTOL land option</param>
    /// <param name="approachAlt">Approach altitude (with the same reference as the Altitude field). NaN if unspecified.</param>
    /// <param name="cancel">Optional cancellation token to cancel the takeoff process.</param>
    /// <returns></returns>
    Task QLand(NavVtolLandOptions landOption, double approachAlt, CancellationToken cancel=default);
}