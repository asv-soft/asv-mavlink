# Position ex-client

You can also use the higher-level [IPositionClientEx](#ipositionclientex),
which provides convenient reactive properties and helper methods for common operations such as
takeoff, landing, setting targets, and ROI management.

```C#
var positionEx = device.GetMicroservice<IPositionClientEx>()
?? throw new Exception("No position client Ex found");

// Subscribe to current altitude above home
var subscription = positionEx.AltitudeAboveHome.Subscribe(alt =>
{
    Console.WriteLine($"Altitude above home: {alt} m");
});

// Arm the vehicle and take off
await positionEx.ArmDisarm(true);
await positionEx.TakeOff(altInMeters: 10);
```

>Don't forget to dispose subscriptions when they are no longer needed.
{style="warning"}

>If you want to control the device, check out the [Control microservice](Control.md) — it might be a better fit for your needs.
>It also acts as a higher-level abstraction over the Position and other microservices.

## [IPositionClientEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Position/Client/Ex/IPositionClientEx.cs#L14)

Represents a higher-level wrapper over `IPositionClient` providing convenient reactive properties and additional helper methods.

| Property            | Type                                  | Description                                                       |
|---------------------|---------------------------------------|-------------------------------------------------------------------|
| `Base`              | `IPositionClient`                     | The underlying base client.                                       |
| `Pitch`             | `ReadOnlyReactiveProperty<double>`    | Pitch angle in degrees.                                           |
| `PitchSpeed`        | `ReadOnlyReactiveProperty<double>`    | Pitch angular speed in radians per second.                        |
| `Roll`              | `ReadOnlyReactiveProperty<double>`    | Roll angle in degrees.                                            |
| `RollSpeed`         | `ReadOnlyReactiveProperty<double>`    | Roll angular speed in radians per second.                         |
| `Yaw`               | `ReadOnlyReactiveProperty<double>`    | Yaw angle in degrees.                                             |
| `YawSpeed`          | `ReadOnlyReactiveProperty<double>`    | Yaw angular speed in radians per second.                          |
| `Current`           | `ReadOnlyReactiveProperty<GeoPoint>`  | Current global position in degrees with MSL altitude in meters.   |
| `Target`            | `ReadOnlyReactiveProperty<GeoPoint?>` | Current global target in degrees with altitude in meters.         |
| `Home`              | `ReadOnlyReactiveProperty<GeoPoint?>` | Home position in degrees with MSL altitude in meters.             |
| `AltitudeAboveHome` | `ReadOnlyReactiveProperty<double>`    | Altitude above home in meters.                                    |
| `HomeDistance`      | `ReadOnlyReactiveProperty<double>`    | Distance from the current position to home.                       |
| `TargetDistance`    | `ReadOnlyReactiveProperty<double>`    | Distance from the current position to the target.                 |
| `IsArmed`           | `ReadOnlyReactiveProperty<bool>`      | Represents a property that indicates whether the object is armed. |
| `ArmedTime`         | `ReadOnlyReactiveProperty<TimeSpan>`  | Gets the armed time as an observable value of type TimeSpan.      |
| `Roi`               | `ReadOnlyReactiveProperty<GeoPoint?>` | Gets the Roi property.                                            |

| Method                                                                               | Return Type | Description                                                          |
|--------------------------------------------------------------------------------------|-------------|----------------------------------------------------------------------|
| `GetHomePosition(CancellationToken cancel)`                                          | `Task`      | Requests the vehicle to publish its home position.                   |
| `SetHomePosition(GeoPoint location, CancellationToken cancel)`                       | `Task`      | Sets the vehicle home position.                                      |
| `ArmDisarm(bool isArm, CancellationToken cancel)`                                    | `Task`      | Arms or disarms the system.                                          |
| `SetRoi(GeoPoint location, CancellationToken cancel)`                                | `Task`      | Sets the region of interest (ROI) using the specified location.      |
| `ClearRoi(CancellationToken cancel)`                                                 | `Task`      | Clears the region of interest (ROI).                                 |
| `SetTarget(GeoPoint point, CancellationToken none)`                                  | `ValueTask` | Sends a global position target to the vehicle.                       |
| `TakeOff(double altInMeters, CancellationToken cancel)`                              | `Task`      | Initiates the takeoff process.                                       |
| `QTakeOff(double altInMeters, CancellationToken cancel)`                             | `Task`      | Initiates vertical takeoff.                                          |
| `QLand(NavVtolLandOptions landOption, double approachAlt, CancellationToken cancel)` | `Task`      | VTOL landing procedure.                                              |

### `IPositionClientEx.GetHomePosition`
| Parameter | Type                | Description                   |
|-----------|---------------------|-------------------------------|
| `cancel`  | `CancellationToken` | Optional cancellation token.  |

The command response confirms that the request was accepted. The position itself is received through the `Home` property.

### `IPositionClientEx.SetHomePosition`
| Parameter  | Type                | Description                                      |
|------------|---------------------|--------------------------------------------------|
| `location` | `GeoPoint`          | New home latitude, longitude, and altitude.      |
| `cancel`   | `CancellationToken` | Optional cancellation token.                     |

### `IPositionClientEx.ArmDisarm`
| Parameter | Type                | Description                   |
|-----------|---------------------|-------------------------------|
| `isArm`   | `bool`              | True to arm, false to disarm. |
| `cancel`  | `CancellationToken` | Optional cancellation token.  |

### `IPositionClientEx.SetRoi`
| Parameter  | Type                | Description                                                  |
|------------|---------------------|--------------------------------------------------------------|
| `location` | `GeoPoint`          | The geographical point representing the location of the ROI. |
| `cancel`   | `CancellationToken` | Optional cancellation token.                                 |

### `IPositionClientEx.ClearRoi`
| Parameter | Type                | Description                   |
|-----------|---------------------|-------------------------------|
| `cancel`  | `CancellationToken` | Optional cancellation token.  |

### `IPositionClientEx.SetTarget`
| Parameter | Type                | Description                  |
|-----------|---------------------|------------------------------|
| `point`   | `GeoPoint`          | Target position to set.      |
| `none`    | `CancellationToken` | Cancellation token.          |

### `IPositionClientEx.TakeOff`
| Parameter     | Type                | Description                  |
|---------------|---------------------|------------------------------|
| `altInMeters` | `double`            | Target altitude in meters.   |
| `cancel`      | `CancellationToken` | Optional cancellation token. |

### `IPositionClientEx.QTakeOff`
| Parameter     | Type                | Description                  |
|---------------|---------------------|------------------------------|
| `altInMeters` | `double`            | Target altitude in meters.   |
| `cancel`      | `CancellationToken` | Optional cancellation token. |

### `IPositionClientEx.QLand`
| Parameter     | Type                 | Description                            |
|---------------|----------------------|----------------------------------------|
| `landOption`  | `NavVtolLandOptions` | VTOL landing option.                   |
| `approachAlt` | `double`             | Approach altitude, NaN if unspecified. |
| `cancel`      | `CancellationToken`  | Optional cancellation token.           |
