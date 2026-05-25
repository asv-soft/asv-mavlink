# Missions ex-client

`IMissionClientEx` is a higher-level mission API on top of `IMissionClient`.
It adds a local mission cache, sync state, mission distance, upload/download helpers, and start mission command support.

```C#
var missionEx = device.GetMicroservice<IMissionClientEx>()
    ?? throw new Exception("MissionClientEx not found");
```

Download full mission with progress:

```C#
var items = await missionEx.Download(
    cancel,
    p => Console.WriteLine($"Download: {p:P0}")
);
```

Create local mission and upload:

```C#
missionEx.ClearLocal();
missionEx.AddTakeOffMissionItem(new GeoPoint(47.641468, -122.140165, 50));
missionEx.AddNavMissionItem(new GeoPoint(47.642000, -122.141000, 60));
missionEx.AddLandMissionItem(new GeoPoint(47.642500, -122.142000, 0));

await missionEx.Upload(
    cancel,
    p => Console.WriteLine($"Upload: {p:P0}")
);
```

Start mission and change current item:

```C#
await missionEx.StartMission(0, ushort.MaxValue, cancel);
await missionEx.SetCurrent(0, cancel);
```

## [MissionClientExConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Client/Ex/MissionClientEx.cs)

`MissionClientExConfig` extends `MissionClientConfig`.

| Property                | Type  | Default | Description                                          |
|-------------------------|-------|---------|------------------------------------------------------|
| `CommandTimeoutMs`      | `int` | `1000`  | Timeout for request/response mission calls.          |
| `AttemptToCallCount`    | `int` | `3`     | Retry count for request/response calls.              |
| `DeviceUploadTimeoutMs` | `int` | `3000`  | Timeout for waiting next upload request from device. |

## [IMissionClientEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Client/Ex/IMissionClientEx.cs)

Represents an extended interface for interacting with mission clients.

| Property              | Type                                   | Description                                                        |
|-----------------------|----------------------------------------|--------------------------------------------------------------------|
| `Base`                | `IMissionClient`                       | Underlying low-level mission client.                               |
| `MissionItems`        | `IReadOnlyObservableList<MissionItem>` | This property represents an observable collection of MissionItems. |
| `IsSynced`            | `ReadOnlyReactiveProperty<bool>`       | `true` when local and remote missions are in sync.                 |
| `Current`             | `ReadOnlyReactiveProperty<ushort>`     | Current mission item index from remote side.                       |
| `Reached`             | `ReadOnlyReactiveProperty<ushort>`     | Last reached mission item index from remote side.                  |
| `AllMissionsDistance` | `ReadOnlyReactiveProperty<double>`     | Total distance in km of all missions.                              |

| Method                                                                                  | Return Type           | Description                                                    |
|-----------------------------------------------------------------------------------------|-----------------------|----------------------------------------------------------------|
| `StartMission(ushort startIndex, ushort stopIndex, CancellationToken cancel = default)` | `Task`                | Starts the mission (sends MAV_CMD_MISSION_START).              |
| `Download(CancellationToken cancel, Action<double>? progress = null)`                   | `Task<MissionItem[]>` | Downloads mission to local cache and sends ACK.                |
| `Upload(CancellationToken cancel = default, Action<double>? progress = null)`           | `Task`                | Uploads a file to the server.                                  |
| `Create()`                                                                              | `MissionItem`         | Creates and adds new local MissionItem object.                 |
| `Remove(ushort index)`                                                                  | `void`                | Removes an element at the specified index from the collection. |
| `ClearRemote(CancellationToken cancel = default)`                                       | `Task`                | Clears mission on remote side.                                 |
| `ClearLocal()`                                                                          | `void`                | Clears local mission cache only.                               |
| `SetCurrent(ushort index, CancellationToken cancel = default)`                          | `Task`                | Sets current mission item on remote side.                      |

### `IMissionClientEx.StartMission`
| Parameter    | Type                | Description                  |
|--------------|---------------------|------------------------------|
| `startIndex` | `ushort`            | First mission item index.    |
| `stopIndex`  | `ushort`            | Last mission item index.     |
| `cancel`     | `CancellationToken` | Cancel token argument.       |

### `IMissionClientEx.Download`
| Parameter  | Type                | Description                                                    |
|------------|---------------------|----------------------------------------------------------------|
| `cancel`   | `CancellationToken` | Cancel token argument.                                         |
| `progress` | `Action<double>?`   | Optional. The callback to report progress during the download. |

### `IMissionClientEx.Upload`
| Parameter  | Type                | Description                                                                                                                                   |
|------------|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| `cancel`   | `CancellationToken` | Optional cancel token argument.                                                                                                               |
| `progress` | `Action<double>?`   | An optional callback to track the progress of the upload. The callback receives a value between 0 and 1 representing the progress percentage. |

### `IMissionClientEx.Remove`
| Parameter | Type     | Description                         |
|-----------|----------|-------------------------------------|
| `index`   | `ushort` | The index of the element to remove. |

### `IMissionClientEx.ClearRemote`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `IMissionClientEx.SetCurrent`
| Parameter | Type                | Description                                        |
|-----------|---------------------|----------------------------------------------------|
| `index`   | `ushort`            | The index position to set as the current position. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                    |

## [MissionClientExHelper](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Client/Ex/IMissionClientEx.cs)

Helper extension methods to quickly create common mission items.

| Method                                                                                                                                                                                             | Return Type   | Description                                                                                                  |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|--------------------------------------------------------------------------------------------------------------|
| `AddSplineMissionItem(this IMissionClientEx vehicle, GeoPoint point)`                                                                                                                              | `MissionItem` | Adds a spline mission item to a vehicle's mission.                                                           |
| `SetVehicleSpeed(this IMissionClientEx vehicle, float speed, float speedType = 1, float throttle = -1)`                                                                                            | `MissionItem` | Change speed and/or throttle set points. The value persists until it is overridden or there is a mode change |
| `AddNavMissionItem(this IMissionClientEx vehicle, GeoPoint point, float holdTime = 0, float acceptRadius = 0, float passRadius = 0, float yawAngle = float.NaN)`                                   | `MissionItem` | Adds a nav mission item to a vehicle's mission.                                                              |
| `AddTakeOffMissionItem(this IMissionClientEx vehicle, GeoPoint point, float pitch = 0, float yawAngle = float.NaN)`                                                                                | `MissionItem` | Adds a takeoff mission item to the mission.                                                                  |
| `AddLandMissionItem(this IMissionClientEx vehicle, GeoPoint point, float abortAltitude = 0, PrecisionLandMode landMode = PrecisionLandMode.PrecisionLandModeDisabled, float yawAngle = float.NaN)` | `MissionItem` | Adds a land mission item to the mission list.                                                                |
| `AddRoiMissionItem(this IMissionClientEx vehicle, GeoPoint point, MavRoi roiMode = MavRoi.MavRoiLocation, float wpIndex = 0, float roiIndex = 0)`                                                  | `MissionItem` | Adds a Region of Interest (ROI) mission item to the mission.                                                 |

### `IMissionClientEx.AddSplineMissionItem`
| Parameter | Type               | Description                               |
|-----------|--------------------|-------------------------------------------|
| `vehicle` | `IMissionClientEx` | The vehicle's mission client              |
| `point`   | `GeoPoint`         | The geographic point for the mission item |

### `IMissionClientEx.SetVehicleSpeed`
| Parameter   | Type               | Description                                                                              |
|-------------|--------------------|------------------------------------------------------------------------------------------|
| `vehicle`   | `IMissionClientEx` | The vehicle's mission client                                                             |
| `speed`     | `float`            | Speed (-1 indicates no change, -2 indicates return to default vehicle speed)             |
| `speedType` | `float`            | Speed type of value set in param2 (such as airspeed, ground speed, and so on)            |
| `throttle`  | `float`            | Throttle (-1 indicates no change, -2 indicates return to default vehicle throttle value) |

### `IMissionClientEx.AddNavMissionItem`
| Parameter      | Type               | Description                               |
|----------------|--------------------|-------------------------------------------|
| `vehicle`      | `IMissionClientEx` | The mission client                        |
| `point`        | `GeoPoint`         | The geographic point for the mission item |
| `holdTime`     | `float`            | Hold time value written to Param1         |
| `acceptRadius` | `float`            | Accept radius value written to Param2     |
| `passRadius`   | `float`            | Pass radius value written to Param3       |
| `yawAngle`     | `float`            | Yaw angle value written to Param4         |

### `IMissionClientEx.AddTakeOffMissionItem`
| Parameter  | Type               | Description                              |
|------------|--------------------|------------------------------------------|
| `vehicle`  | `IMissionClientEx` | The mission client.                      |
| `point`    | `GeoPoint`         | The coordinates of the takeoff point.    |
| `pitch`    | `float`            | The pitch angle (optional, default = 0). |
| `yawAngle` | `float`            | The yaw angle (optional, default = NaN). |

### `IMissionClientEx.AddLandMissionItem`
| Parameter       | Type                | Description                                                               |
|-----------------|---------------------|---------------------------------------------------------------------------|
| `vehicle`       | `IMissionClientEx`  | The vehicle that the mission item will be added to.                       |
| `point`         | `GeoPoint`          | The geographical point of the land mission item.                          |
| `abortAltitude` | `float`             | The altitude at which the land mission item can be aborted, default is 0. |
| `landMode`      | `PrecisionLandMode` | The mode of precision landing, default is PrecisionLandModeDisabled.      |
| `yawAngle`      | `float`             | The yaw angle for the land mission item, default is NaN.                  |

### `IMissionClientEx.AddRoiMissionItem`
| Parameter  | Type               | Description                                                         |
|------------|--------------------|---------------------------------------------------------------------|
| `vehicle`  | `IMissionClientEx` | The mission client extension.                                       |
| `point`    | `GeoPoint`         | The geographical point target for the ROI.                          |
| `roiMode`  | `MavRoi`           | The mode for the ROI (optional, defaults to MavRoi.MavRoiLocation). |
| `wpIndex`  | `float`            | The waypoint index of the mission item (optional, defaults to 0).   |
| `roiIndex` | `float`            | The ROI index (optional, defaults to 0).                            |
