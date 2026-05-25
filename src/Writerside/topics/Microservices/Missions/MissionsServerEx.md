# Missions ex-server

`IMissionServerEx` is a higher-level mission server API.
It wraps `IMissionServer` and adds mission storage, mission execution, and command handlers for mission items.

Register required services:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(config.SystemId, config.ComponentId),
    core,
    builder =>
    {
        builder.RegisterStatus();
        builder.RegisterMission();
        builder.RegisterCommand();
        builder.RegisterCommandLongEx();
        builder.RegisterMissionEx();
    });
```

Get the service:

```C#
var missionEx = serverDevice.GetMissionEx();
```

Simple mission execution setup:

```C#
missionEx.AddItems(new[]
{
    new ServerMissionItem { Command = MavCmd.MavCmdUser1, Autocontinue = 1 },
    new ServerMissionItem { Command = MavCmd.MavCmdUser2, Autocontinue = 1 },
});

missionEx[MavCmd.MavCmdUser1] = async (item, cancel) =>
{
    Console.WriteLine("Execute USER1");
};

missionEx[MavCmd.MavCmdUser2] = async (item, cancel) =>
{
    Console.WriteLine("Execute USER2");
};

missionEx.StartMission(0);
```

## [MissionServerState](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Server/Ex/IMissionServerEx.cs)

| Value             | Description                               |
|-------------------|-------------------------------------------|
| `Idle`            | No mission execution in progress.         |
| `Running`         | Mission execution is running.             |
| `CompleteSuccess` | Mission execution completed successfully. |
| `CompleteError`   | Mission execution ended with error.       |
| `Canceled`        | Mission execution was canceled.           |

## [IMissionServerEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Server/Ex/IMissionServerEx.cs)

| Property            | Type                                           | Description                                       |
|---------------------|------------------------------------------------|---------------------------------------------------|
| `Base`              | `IMissionServer`                               | Underlying low-level mission server.              |
| `Current`           | `ReadOnlyReactiveProperty<ushort>`             | Current mission item index.                       |
| `Reached`           | `ReadOnlyReactiveProperty<ushort>`             | Last reached mission item index.                  |
| `Items`             | `IReadOnlyObservableList<ServerMissionItem>`   | Mission storage.                                  |
| `State`             | `ReadOnlyReactiveProperty<MissionServerState>` | Current mission execution state.                  |
| `SupportedCommands` | `IEnumerable<MavCmd>`                          | Commands that currently have registered handlers. |

| Indexer                           | Description                                 |
|-----------------------------------|---------------------------------------------|
| `this[MavCmd mavCmd] { set; }`    | Register/remove mission command handler.    |

| Method                                                | Return Type                         | Description                                         |
|-------------------------------------------------------|-------------------------------------|-----------------------------------------------------|
| `AddItems(IEnumerable<ServerMissionItem> items)`      | `void`                              | Adds items to mission storage.                      |
| `RemoveItems(IEnumerable<ServerMissionItem> items)`   | `void`                              | Removes items from mission storage.                 |
| `ClearItems()`                                        | `void`                              | Clears mission storage.                             |
| `GetItemsSnapshot()`                                  | `ImmutableArray<ServerMissionItem>` | Returns snapshot copy of mission items.             |
| `ChangeCurrentMissionItem(ushort index)`              | `ValueTask`                         | Changes current mission item index.                 |
| `StartMission(ushort missionIndex = 0)`               | `void`                              | Starts mission execution.                           |
| `StopMission(CancellationToken cancel)`               | `void`                              | Stops mission execution.                            |

### `IMissionServerEx.AddItems`
| Parameter | Type                             | Description      |
|-----------|----------------------------------|------------------|
| `items`   | `IEnumerable<ServerMissionItem>` | Items to append. |

### `IMissionServerEx.RemoveItems`
| Parameter | Type                             | Description      |
|-----------|----------------------------------|------------------|
| `items`   | `IEnumerable<ServerMissionItem>` | Items to remove. |

### `IMissionServerEx.ChangeCurrentMissionItem`
| Parameter | Type     | Description                |
|-----------|----------|----------------------------|
| `index`   | `ushort` | Mission item index.        |

### `IMissionServerEx.StartMission`
| Parameter      | Type     | Description                                |
|----------------|----------|--------------------------------------------|
| `missionIndex` | `ushort` | Start index (`0` by default).              |

### `IMissionServerEx.StopMission`
| Parameter | Type                | Description            |
|-----------|---------------------|------------------------|
| `cancel`  | `CancellationToken` | Cancel token argument. |

## [MissionTaskDelegate](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Server/Ex/IMissionServerEx.cs)

| Parameter | Type                | Description              |
|-----------|---------------------|--------------------------|
| `item`    | `ServerMissionItem` | Mission item to execute. |
| `cancel`  | `CancellationToken` | Cancel token argument.   |
