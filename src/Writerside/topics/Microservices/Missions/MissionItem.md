# MissionItem

[Source code](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Client/MissionItem.cs)

`MissionItem` is a client-side mission item wrapper around `MissionItemIntPayload`.
It exposes item fields as reactive properties and emits `OnChanged` when payload changes.

## Constructor

| Constructor                               | Description                           |
|-------------------------------------------|---------------------------------------|
| `MissionItem(MissionItemIntPayload item)` | Creates wrapper from mission payload. |

### `MissionItem(MissionItemIntPayload item)`
| Parameter | Type                    | Description                 |
|-----------|-------------------------|-----------------------------|
| `item`    | `MissionItemIntPayload` | Source payload.             |

## API

| Property       | Type                               | Description                                       |
|----------------|------------------------------------|---------------------------------------------------|
| `Index`        | `ushort`                           | Mission item sequence number.                     |
| `Location`     | `ReactiveProperty<GeoPoint>`       | Latitude/longitude/altitude view of payload XYZ.  |
| `AutoContinue` | `ReactiveProperty<bool>`           | Autocontinue to next waypoint.                    |
| `Command`      | `ReactiveProperty<MavCmd>`         | The scheduled action for the waypoint.            |
| `Current`      | `ReactiveProperty<bool>`           | Current item flag.                                |
| `Frame`        | `ReactiveProperty<MavFrame>`       | The coordinate system of the waypoint.            |
| `MissionType`  | `ReactiveProperty<MavMissionType>` | Mission type.                                     |
| `Param1`       | `ReactiveProperty<float>`          | MAV_CMD param 1.                                  |
| `Param2`       | `ReactiveProperty<float>`          | MAV_CMD param 2.                                  |
| `Param3`       | `ReactiveProperty<float>`          | MAV_CMD param 3.                                  |
| `Param4`       | `ReactiveProperty<float>`          | MAV_CMD param 4.                                  |
| `OnChanged`    | `Observable<Unit>`                 | Emits when payload changed via `Edit`/properties. |
| `Tag`          | `object?`                          | Optional user data.                               |

| Method                                             | Return Type | Description                              |
|----------------------------------------------------|-------------|------------------------------------------|
| `Edit(Action<MissionItemIntPayload> editCallback)` | `void`      | Updates payload and notifies on changes. |
| `ToString()`                                       | `string`    | Returns debug string.                    |
| `Dispose()`                                        | `void`      | Disposes reactive resources.             |
| `DisposeAsync()`                                   | `ValueTask` | Asynchronous disposal.                   |

### `MissionItem.Edit`
| Parameter      | Type                             | Description                         |
|----------------|----------------------------------|-------------------------------------|
| `editCallback` | `Action<MissionItemIntPayload>`  | Callback that edits payload fields. |
