# Position ex-client

You can also use the higher-level [IPositionClientEx](#ipositionclientex-source),
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

## IPositionClientEx ([source](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Microservices/Position/Client/Ex/IPositionClientEx.cs#L14))

Represents a higher-level wrapper over `IPositionClient` providing convenient reactive properties and additional helper methods.

| Property                                                                             | Type                                  | Description                                                          |
|--------------------------------------------------------------------------------------|---------------------------------------|----------------------------------------------------------------------|
| `Base`                                                                               | `IPositionClient`                     | The underlying base client.                                          |
| `Pitch`                                                                              | `ReadOnlyReactiveProperty<double>`    | Gets the pitch value.                                                |
| `PitchSpeed`                                                                         | `ReadOnlyReactiveProperty<double>`    | Gets the pitch speed property.                                       |
| `Roll`                                                                               | `ReadOnlyReactiveProperty<double>`    | Gets the roll value of the object.                                   |
| `RollSpeed`                                                                          | `ReadOnlyReactiveProperty<double>`    | Gets the roll speed value.                                           |
| `Yaw`                                                                                | `ReadOnlyReactiveProperty<double>`    | Gets the yaw value.                                                  |
| `YawSpeed`                                                                           | `ReadOnlyReactiveProperty<double>`    | Gets the yaw speed.                                                  |
| `Current`                                                                            | `ReadOnlyReactiveProperty<GeoPoint>`  | Gets the current value of type GeoPoint.                             |
| `Target`                                                                             | `ReadOnlyReactiveProperty<GeoPoint?>` | Gets the target value of type `ReadOnlyReactiveProperty<GeoPoint?>`. |
| `Home`                                                                               | `ReadOnlyReactiveProperty<GeoPoint?>` | The property representing the home location.                         |
| `AltitudeAboveHome`                                                                  | `ReadOnlyReactiveProperty<double>`    | Gets the altitude above home.                                        |
| `HomeDistance`                                                                       | `ReadOnlyReactiveProperty<double>`    | Represents the distance from a home location.                        |
| `TargetDistance`                                                                     | `ReadOnlyReactiveProperty<double>`    | Gets the target distance.                                            |
| `IsArmed`                                                                            | `ReadOnlyReactiveProperty<bool>`      | Represents a property that indicates whether the object is armed.    |
| `ArmedTime`                                                                          | `ReadOnlyReactiveProperty<TimeSpan>`  | Gets the armed time as an observable value of type TimeSpan.         |
| `Roi`                                                                                | `ReadOnlyReactiveProperty<GeoPoint?>` | Gets the Roi property.                                               |

| Method                                                                               | Type                                  | Description                                                          |
|--------------------------------------------------------------------------------------|---------------------------------------|----------------------------------------------------------------------|
| `GetHomePosition(CancellationToken cancel)`                                          | `Task`                                | Gets the home position.                                              |
| `ArmDisarm(bool isArm, CancellationToken cancel)`                                    | `Task`                                | Arms or disarms the system.                                          |
| `SetRoi(GeoPoint location, CancellationToken cancel)`                                | `Task`                                | Sets the region of interest (ROI) using the specified location.      |
| `ClearRoi(CancellationToken cancel)`                                                 | `Task`                                | Clears the region of interest (ROI).                                 |
| `SetTarget(GeoPoint point, CancellationToken cancel)`                                | `ValueTask`                           | Sets the target for the application.                                 |
| `TakeOff(double altInMeters, CancellationToken cancel)`                              | `Task`                                | Initiates the takeoff process.                                       |
| `QTakeOff(double altInMeters, CancellationToken cancel)`                             | `Task`                                | Initiates vertical takeoff.                                          |
| `QLand(NavVtolLandOptions landOption, double approachAlt, CancellationToken cancel)` | `Task`                                | VTOL landing procedure.                                              |

#### `IPositionClientEx.GetHomePosition`
| Parameter | Type                | Description                   |
|-----------|---------------------|-------------------------------|
| `cancel`  | `CancellationToken` | Optional cancellation token.  |

#### `IPositionClientEx.ArmDisarm`
| Parameter | Type                | Description                   |
|-----------|---------------------|-------------------------------|
| `isArm`   | `bool`              | True to arm, false to disarm. |
| `cancel`  | `CancellationToken` | Optional cancellation token.  |

#### `IPositionClientEx.SetRoi`
| Parameter  | Type                | Description                                                  |
|------------|---------------------|--------------------------------------------------------------|
| `location` | `GeoPoint`          | The geographical point representing the location of the ROI. |
| `cancel`   | `CancellationToken` | Optional cancellation token.                                 |

#### `IPositionClientEx.ClearRoi`
| Parameter | Type                | Description                   |
|-----------|---------------------|-------------------------------|
| `cancel`  | `CancellationToken` | Optional cancellation token.  |

#### `IPositionClientEx.SetTarget`
| Parameter | Type                | Description                  |
|-----------|---------------------|------------------------------|
| `point`   | `GeoPoint`          | Target position to set.      |
| `none`    | `CancellationToken` | Optional cancellation token. |

#### `IPositionClientEx.TakeOff`
| Parameter     | Type                | Description                  |
|---------------|---------------------|------------------------------|
| `altInMeters` | `double`            | Target altitude in meters.   |
| `cancel`      | `CancellationToken` | Optional cancellation token. |

#### `IPositionClientEx.QTakeOff`
| Parameter     | Type                | Description                  |
|---------------|---------------------|------------------------------|
| `altInMeters` | `double`            | Target altitude in meters.   |
| `cancel`      | `CancellationToken` | Optional cancellation token. |

#### `IPositionClientEx.QLand`
| Parameter     | Type                 | Description                            |
|---------------|----------------------|----------------------------------------|
| `landOption`  | `NavVtolLandOptions` | VTOL landing option.                   |
| `approachAlt` | `double`             | Approach altitude, NaN if unspecified. |
| `cancel`      | `CancellationToken`  | Optional cancellation token.           |
