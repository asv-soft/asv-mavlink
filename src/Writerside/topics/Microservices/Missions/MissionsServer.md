# Missions server

Use `IMissionServer` to implement low-level mission protocol handling on the server side (autopilot/simulator).

First register the service:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(config.SystemId, config.ComponentId),
    core,
    builder =>
    {
        // register other services...
        builder.RegisterMission();
    });
```

Then get the service:

```C#
var missionServer = serverDevice.GetMission();
```

Typical handling example:

```C#
var items = new List<ServerMissionItem>();

using var onList = missionServer.OnMissionRequestList.SubscribeAwait(async (req, ct) =>
{
    await missionServer.SendMissionCount((ushort)items.Count, req.SystemId, req.ComponentId);
});

using var onItem = missionServer.OnMissionRequestInt.SubscribeAwait(async (req, ct) =>
{
    if (req.Payload.Seq >= items.Count)
    {
        await missionServer.SendMissionAck(MavMissionResult.MavMissionInvalid, req.SystemId, req.ComponentId);
        return;
    }

    await missionServer.SendMissionItemInt(items[req.Payload.Seq], req.SystemId, req.ComponentId, ct);
});

using var onClear = missionServer.OnMissionClearAll.SubscribeAwait(async (req, ct) =>
{
    items.Clear();
    await missionServer.SendMissionAck(MavMissionResult.MavMissionAccepted, req.SystemId, req.ComponentId);
});
```

## [IMissionServer](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Server/IMIssionServer.cs)

| Property               | Type                                   | Description                                                                                                                       |
|------------------------|----------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|
| `OnMissionCount`       | `Observable<MissionCountPacket>`       | Event that is raised whenever the mission count is updated..                                                                      |
| `OnMissionRequestList` | `Observable<MissionRequestListPacket>` | Gets an observable sequence of MissionRequestListPacket that represents the event raised when a mission request list is received. |
| `OnMissionRequestInt`  | `Observable<MissionRequestIntPacket>`  | Gets an observable sequence of <see cref="MissionRequestIntPacket"/> for mission requests of type int.                            |
| `OnMissionClearAll`    | `Observable<MissionClearAllPacket>`    | Gets an observable sequence of MissionClearAllPacket events.                                                                      |
| `OnMissionSetCurrent`  | `Observable<MissionSetCurrentPacket>`  | Represents an event that is raised when a mission is set as the current mission.                                                  |
| `OnMissionAck`         | `Observable<MissionAckPayload>`        | Gets the observable sequence that emits MissionAckPayload when a mission acknowledgement is received.                             |

| Method                                                                                                                                           | Return Type               | Description                                                                              |
|--------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------|------------------------------------------------------------------------------------------|
| `SendMissionAck(MavMissionResult result, byte targetSystemId = 0, byte targetComponentId = 0, MavMissionType? type = null)`                      | `ValueTask`               | Sends a mission acknowledgment message to the specified target system and component IDs. |
| `SendMissionCount(ushort count, byte targetSystemId = 0, byte targetComponentId = 0)`                                                            | `ValueTask`               | Sends the mission count to the specified target system and component IDs.                |
| `SendReached(ushort seq, CancellationToken cancel = default)`                                                                                    | `ValueTask`               | Sends the reached value of a sequence.                                                   |
| `SendMissionCurrent(ushort current, CancellationToken cancel = default)`                                                                         | `ValueTask`               | Sends the current mission index to the system.                                           |
| `SendMissionItemInt(ServerMissionItem item, byte targetSystemId = 0, byte targetComponentId = 0, CancellationToken cancel = default)`            | `ValueTask`               | Sends a mission item to the server.                                                      |
| `RequestMissionItem(ushort index, MavMissionType type, byte targetSystemId = 0, byte targetComponentId = 0, CancellationToken cancel = default)` | `Task<ServerMissionItem>` | Requests a mission item from the server.                                                 |

### `IMissionServer.SendMissionAck`
| Parameter           | Type               | Description                                  |
|---------------------|--------------------|----------------------------------------------|
| `result`            | `MavMissionResult` | The result of the mission.                   |
| `targetSystemId`    | `byte`             | The target system ID. Default value is 0.    |
| `targetComponentId` | `byte`             | The target component ID. Default value is 0. |
| `type`              | `MavMissionType?`  | The mission type. Default value is null.     |

### `IMissionServer.SendMissionCount`
| Parameter           | Type     | Description                                                    |
|---------------------|----------|----------------------------------------------------------------|
| `count`             | `ushort` | The number of missions to be sent.                             |
| `targetSystemId`    | `byte`   | The ID of the target system. (optional, default value is 0)    |
| `targetComponentId` | `byte`   | The ID of the target component. (optional, default value is 0) |

### `IMissionServer.SendReached`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `seq`     | `ushort`            | The sequence number.            |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `IMissionServer.SendMissionCurrent`
| Parameter | Type                | Description                                    |
|-----------|---------------------|------------------------------------------------|
| `current` | `ushort`            | Sends the current mission index to the system. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                |

### `IMissionServer.SendMissionItemInt`
| Parameter           | Type                | Description                                  |
|---------------------|---------------------|----------------------------------------------|
| `item`              | `ServerMissionItem` | The mission item to be sent.                 |
| `targetSystemId`    | `byte`              | The target system ID. Default value is 0.    |
| `targetComponentId` | `byte`              | The target component ID. Default value is 0. |
| `cancel`            | `CancellationToken` | Optional cancel token argument.              |

### `IMissionServer.RequestMissionItem`
| Parameter           | Type                | Description                                  |
|---------------------|---------------------|----------------------------------------------|
| `index`             | `ushort`            | The index of the mission item to request.    |
| `type`              | `MavMissionType`    | The type of the mission item.                |
| `targetSystemId`    | `byte`              | The target system ID. Default value is 0.    |
| `targetComponentId` | `byte`              | The target component ID. Default value is 0. |
| `cancel`            | `CancellationToken` | Optional cancel token argument.              |
