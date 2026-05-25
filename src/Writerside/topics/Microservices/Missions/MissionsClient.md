# Missions client

Use `IMissionClient` when you need direct mission protocol operations from a client device.

```C#
var mission = device.GetMicroservice<IMissionClient>()
    ?? throw new Exception("Mission client not found");
```

Basic download flow (same idea as [`show-mission`](download-mission-items-command.md) CLI command):

```C#
var count = await mission.MissionRequestCount(cancel);
for (ushort i = 0; i < count; i++)
{
    var item = await mission.MissionRequestItem(i, cancel);
    Console.WriteLine($"#{item.Seq}: {item.Command}");
}
```

Set current item:

```C#
await mission.MissionSetCurrent(0, cancel);
```

Clear remote mission:

```C#
await mission.ClearAll(MavMissionType.MavMissionTypeMission, cancel);
```

## [MissionClientConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Client/MissionClient.cs)

| Property             | Type  | Default | Description                                 |
|----------------------|-------|---------|---------------------------------------------|
| `CommandTimeoutMs`   | `int` | `1000`  | Timeout for request/response mission calls. |
| `AttemptToCallCount` | `int` | `3`     | Retry count for request/response calls.     |

## [IMissionClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Client/IMissionClient.cs)

Represents a client for interacting with missions.

| Property           | Type                                | Description                                                                                           |
|--------------------|-------------------------------------|-------------------------------------------------------------------------------------------------------|
| `MissionCurrent`   | `ReadOnlyReactiveProperty<ushort>`  | Current mission item index from remote side.                                                          |
| `MissionReached`   | `ReadOnlyReactiveProperty<ushort>`  | Last reached mission item index.                                                                      |
| `OnMissionRequest` | `Observable<MissionRequestPayload>` | Gets an observable stream of mission request payloads.                                                |
| `OnMissionAck`     | `Observable<MissionAckPayload>`     | Gets the observable sequence that emits MissionAckPayload when a mission acknowledgement is received. |

| Method                                                                                                                                                                                                                                         | Return Type                   | Description                                                                                   |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------|-----------------------------------------------------------------------------------------------|
| `MissionSetCurrent(ushort missionItemsIndex, CancellationToken cancel = default)`                                                                                                                                                              | `Task`                        | Drone receives message and attempts to update the current mission sequence number.            |
| `MissionRequestItem(ushort index, CancellationToken cancel = default)`                                                                                                                                                                         | `Task<MissionItemIntPayload>` | Requests a mission item of the specified index.                                               |
| `MissionRequestCount(CancellationToken cancel = default)`                                                                                                                                                                                      | `Task<int>`                   | Initiate mission download from a system by requesting the list of mission items.              |
| `MissionSetCount(ushort count, CancellationToken cancel = default)`                                                                                                                                                                            | `ValueTask`                   | Sets the count for the mission and returns a task that represents the asynchronous operation. |
| `WriteMissionItem(ushort seq, MavFrame frame, MavCmd cmd, bool current, bool autoContinue, float param1, float param2, float param3, float param4, float x, float y, float z, MavMissionType missionType, CancellationToken cancel = default)` | `ValueTask`                   | Writes a mission item to the vehicle's mission list.                                          |
| `WriteMissionItem(MissionItem missionItem, CancellationToken cancel = default)`                                                                                                                                                                | `ValueTask`                   | Writes a mission item to a target.                                                            |
| `WriteMissionIntItem(Action<MissionItemIntPayload> fillCallback, CancellationToken cancel = default)`                                                                                                                                          | `ValueTask`                   | Writes a mission item with an integer payload.                                                |
| `ClearAll(MavMissionType type = MavMissionType.MavMissionTypeAll, CancellationToken cancel = default)`                                                                                                                                         | `Task`                        | Clears all mission items of the specified type.                                               |
| `SendMissionAck(MavMissionResult result, byte targetSystemId = 0, byte targetComponentId = 0, MavMissionType? type = null, CancellationToken cancel = default)`                                                                                | `ValueTask`                   | Sends a mission acknowledgment message to the specified target system and component IDs.      |

### `IMissionClient.MissionSetCurrent`
| Parameter           | Type                | Description                                            |
|---------------------|---------------------|--------------------------------------------------------|
| `missionItemsIndex` | `ushort`            | Index of the mission item within the mission sequence. |
| `cancel`            | `CancellationToken` | Optional cancel token argument.                        |

### `IMissionClient.MissionRequestItem`
| Parameter | Type                | Description                               |
|-----------|---------------------|-------------------------------------------|
| `index`   | `ushort`            | The index of the mission item to request. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.           |

### `IMissionClient.MissionRequestCount`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `IMissionClient.MissionSetCount`
| Parameter | Type                | Description                       |
|-----------|---------------------|-----------------------------------|
| `count`   | `ushort`            | The count to set for the mission. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.   |

### `IMissionClient.WriteMissionItem(...)`
| Parameter      | Type                | Description                                                                  |
|----------------|---------------------|------------------------------------------------------------------------------|
| `seq`          | `ushort`            | The sequence number of the mission item.                                     |
| `frame`        | `MavFrame`          | The coordinate system in which the mission item's position is specified.     |
| `cmd`          | `MavCmd`            | The scheduled action for the mission item.                                   |
| `current`      | `bool`              | True if this mission item is the currently active one.                       |
| `autoContinue` | `bool`              | True to automatically continue to the next waypoint after this mission item. |
| `param1`       | `float`             | PARAM1 for the scheduled action (refer to MAV_CMD enum for details).         |
| `param2`       | `float`             | PARAM2 for the scheduled action (refer to MAV_CMD enum for details).         |
| `param3`       | `float`             | PARAM3 for the scheduled action (refer to MAV_CMD enum for details).         |
| `param4`       | `float`             | PARAM4 for the scheduled action (refer to MAV_CMD enum for details).         |
| `x`            | `float`             | The x-coordinate of the mission item's position.                             |
| `y`            | `float`             | The y-coordinate of the mission item's position.                             |
| `z`            | `float`             | The z-coordinate of the mission item's position.                             |
| `missionType`  | `MavMissionType`    | The type of mission item.                                                    |
| `cancel`       | `CancellationToken` | Optional cancel token argument.                                              |

### `IMissionClient.WriteMissionItem(MissionItem missionItem)`
| Parameter     | Type                | Description                     |
|---------------|---------------------|---------------------------------|
| `missionItem` | `MissionItem`       | The mission item to write.      |
| `cancel`      | `CancellationToken` | Optional cancel token argument. |

### `IMissionClient.WriteMissionIntItem`
| Parameter      | Type                            | Description                                              |
|----------------|---------------------------------|----------------------------------------------------------|
| `fillCallback` | `Action<MissionItemIntPayload>` | A callback function that fills the mission item payload. |
| `cancel`       | `CancellationToken`             | Optional cancel token argument.                          |

### `IMissionClient.ClearAll`
| Parameter | Type                | Description                                                                            |
|-----------|---------------------|----------------------------------------------------------------------------------------|
| `type`    | `MavMissionType`    | The type of mission items to clear. Default value is MavMissionType.MavMissionTypeAll. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                                                        |

### `IMissionClient.SendMissionAck`
| Parameter           | Type                | Description                                  |
|---------------------|---------------------|----------------------------------------------|
| `result`            | `MavMissionResult`  | The result of the mission.                   |
| `targetSystemId`    | `byte`              | The target system ID. Default value is 0.    |
| `targetComponentId` | `byte`              | The target component ID. Default value is 0. |
| `type`              | `MavMissionType?`   | The mission type. Default value is null.     |
| `cancel`            | `CancellationToken` | Optional cancel token argument.              |
