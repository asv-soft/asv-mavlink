# Control client

To control a device from a client, request the [IControlClient](#icontrolclient-source) from the device instance.
[Implementations](#implementations) of this interface typically delegate control operations to existing MAVLink microservices.

The Control microservice is only available for supported vehicle types and will be registered automatically when a 
matching [implementation](#implementations) exists.

You can get the control client from a device:

```C#
var control = device.GetMicroservice<IControlClient>()
    ?? throw new Exception("No control client found");
```

Basic usage:

```C#
// Take off to 3 meters
await control.TakeOff(3.0);

// Go to a target point
await control.GoTo(new GeoPoint(lat, lon, alt));

// Return to launch
await control.DoRtl();

// Land
await control.DoLand();
```

>Most methods complete the `Task` after the command is sent, not when the action is physically finished.
For example, `GoTo` returns once the target is set, not when the point is reached.
{style="info"}

## IControlClient ([source](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Microservices/Control/Client/IControlClient.cs#L7C1-L7C7))

Represents a client that can control a vehicle.

| Method                                                                 | Return Type       | Description                                                                    |
|------------------------------------------------------------------------|-------------------|--------------------------------------------------------------------------------|
| `ValueTask<bool> IsAutoMode(CancellationToken cancel = default)`       | `ValueTask<bool>` | Returns true if the vehicle is currently in Auto mode.                         |
| `Task SetAutoMode(CancellationToken cancel = default)`                 | `Task`            | Switches the vehicle to Auto mode.                                             |
| `ValueTask<bool> IsGuidedMode(CancellationToken cancel = default)`     | `ValueTask<bool>` | Returns true if the vehicle is in Guided mode.                                 |
| `Task SetGuidedMode(CancellationToken cancel = default)`               | `Task`            | Switches the vehicle to Guided mode.                                           |
| `Task GoTo(GeoPoint point, CancellationToken cancel = default)`        | `Task`            | Sets a navigation target to the given point.                                   |
| `Task DoLand(CancellationToken cancel = default)`                      | `Task`            | Initiates the landing sequence.                                                |
| `Task DoRtl(CancellationToken cancel = default)`                       | `Task`            | Performs a return to launch operation asynchronously.                          |
| `Task TakeOff(double altInMeters, CancellationToken cancel = default)` | `Task`            | Initiates the takeoff process and ascends to the specified altitude in meters. |

### Parameters:
`GeoPoint` represents a location on Earth using [WGS 84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84) coordinates. It consists of latitude, longitude, and altitude in meters. 

The `CancellationToken` parameter allows cancelling the operation before it completes, for example if the navigation or takeoff should be aborted.

## Implementations

Different vehicle types have their own implementations of [`IControlClient`](#icontrolclient-source):

- [ArduCopterControlClient](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/ArduCopterControlClient.cs#L13) — commands mapped to [mode](Mode.md), [position](Position.md) and [heartbeat](Heartbeat.md) microservices.
- [ArduPlaneControlClient](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Plane/Simple/ArduPlaneControlClient.cs#L14) and [ArduQuadPlaneControlClient](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Plane/Quad/ArduQuadPlaneControlClient.cs#L13) — similar but adapted for planes.
