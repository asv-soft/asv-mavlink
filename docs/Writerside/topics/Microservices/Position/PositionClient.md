# Position client

To work with the Position microservice from a client, request the [IPositionClient](#ipositionclient-source) from the device instance:

```C#
var position = device.GetMicroservice<IPositionClient>()
?? throw new Exception("No position client found");
```

After that, you can subscribe to various position-related observables. For example, current global position:

```C#
var subscription = position.GlobalPosition.Subscribe(payload =>
{
    if (payload == null) return;
    Console.WriteLine($"Lat: {payload.Lat}, Lon: {payload.Lon}, Alt: {payload.Alt}");
});
```

Or the current altitude:

```C#
var subscription = position.Altitude.Subscribe(alt =>
{
    if (alt == null) return;
    Console.WriteLine($"Altitude: {alt.Value} m");
});
```

>Don't forget to dispose subscriptions when they are no longer needed.
{style="warning"}

## IPositionClient ([source](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Microservices/Position/Client/IPositionClient.cs#L12))

Represents a client that accesses position-related data.

| Property         | Type                                                        | Description                                                                                                                                                                                        |
|------------------|-------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `GlobalPosition` | `ReadOnlyReactiveProperty<GlobalPositionIntPayload?>`       | Gets the RX value for GlobalPosition of type GlobalPositionIntPayload.                                                                                                                             |
| `Home`           | `ReadOnlyReactiveProperty<HomePositionPayload?>`            | Gets the home position of the property Home.                                                                                                                                                       |
| `Target`         | `ReadOnlyReactiveProperty<PositionTargetGlobalIntPayload?>` | Gets the target position value.                                                                                                                                                                    |
| `Altitude`       | `ReadOnlyReactiveProperty<AltitudePayload?>`                | Gets the observable altitude value.                                                                                                                                                                |
| `VfrHud`         | `ReadOnlyReactiveProperty<VfrHudPayload?>`                  | The VfrHud property provides access to an ReadOnlyReactiveProperty interface for VfrHudPayload, which represents the information received from a VFR (Visual Flight Rules) Heads-Up Display (HUD). |
| `Imu`            | `ReadOnlyReactiveProperty<HighresImuPayload?>`              | Gets the reactive value representing the high resolution IMU payload.                                                                                                                              |
| `Attitude`       | `ReadOnlyReactiveProperty<AttitudePayload?>`                | Gets the RX value of the current Attitude payload.                                                                                                                                                 |

| Method                                                                                                                                                                                                                                                 | Return Type | Description                                                                                                                                                                                                                                                    |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `SetTargetGlobalInt(uint timeBootMs, MavFrame coordinateFrame, int latInt, int lonInt, float alt, float vx, float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate, PositionTargetTypemask typeMask, CancellationToken cancel)` | `ValueTask` | Sets a desired vehicle position, velocity, and/or acceleration in a global coordinate system ([WGS 84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84)). Used by an external controller to command the vehicle (manual controller or other system). |
| `SetPositionTargetLocalNed(uint timeBootMs, MavFrame coordinateFrame, PositionTargetTypemask typeMask, float x, float y, float z, float vx, float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate, CancellationToken cancel)`  | `ValueTask` | Set vehicle target in local NED coordinates.                                                                                                                                                                                                                   |

### `IPositionClient.SetTargetGlobalInt`
| Parameter         | Type                     | Description                                                                             |
|-------------------|--------------------------|-----------------------------------------------------------------------------------------|
| `timeBootMs`      | `uint`                   | Timestamp (time since system boot).                                                     |
| `coordinateFrame` | `MavFrame`               | Frame: global, relative alt, or terrain alt.                                            |
| `latInt`          | `int`                    | Latitude in [WGS 84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84) frame.  |
| `lonInt`          | `int`                    | Longitude in [WGS 84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84) frame. |
| `alt`             | `float`                  | Altitude (MSL, relative, or AGL).                                                       |
| `vx`              | `float`                  | X velocity in NED frame.                                                                |
| `vy`              | `float`                  | Y velocity in NED frame.                                                                |
| `vz`              | `float`                  | Z velocity in NED frame.                                                                |
| `afx`             | `float`                  | X acceleration/force in NED frame.                                                      |
| `afy`             | `float`                  | Y acceleration/force in NED frame.                                                      |
| `afz`             | `float`                  | Z acceleration/force in NED frame.                                                      |
| `yaw`             | `float`                  | Yaw setpoint.                                                                           |
| `yawRate`         | `float`                  | Yaw rate setpoint.                                                                      |
| `typeMask`        | `PositionTargetTypemask` | Bitmap indicating which dimensions to ignore.                                           |
| `cancel`          | `CancellationToken`      | Optional cancellation token.                                                            |

### `IPositionClient.SetPositionTargetLocalNed`
| Parameter         | Type                     | Description                                   |
|-------------------|--------------------------|-----------------------------------------------|
| `timeBootMs`      | `uint`                   | Timestamp (time since system boot).           |
| `coordinateFrame` | `MavFrame`               | Frame: local NED, body, or offset.            |
| `typeMask`        | `PositionTargetTypemask` | Bitmap indicating which dimensions to ignore. |
| `x`               | `float`                  | X position in NED frame.                      |
| `y`               | `float`                  | Y position in NED frame.                      |
| `z`               | `float`                  | Z position in NED frame (altitude negative).  |
| `vx`              | `float`                  | X velocity in NED frame.                      |
| `vy`              | `float`                  | Y velocity in NED frame.                      |
| `vz`              | `float`                  | Z velocity in NED frame.                      |
| `afx`             | `float`                  | X acceleration/force in NED frame.            |
| `afy`             | `float`                  | Y acceleration/force in NED frame.            |
| `afz`             | `float`                  | Z acceleration/force in NED frame.            |
| `yaw`             | `float`                  | Yaw setpoint.                                 |
| `yawRate`         | `float`                  | Yaw rate setpoint.                            |
| `cancel`          | `CancellationToken`      | Optional cancellation token.                  |
