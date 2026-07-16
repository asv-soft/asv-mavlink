# Commands ex-server

`ICommandServerEx<TArgPacket>` is a higher-level command dispatcher over `ICommandServer`.
It provides command handler registry by `MavCmd`.

## Use case

Register required services:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(systemId, componentId),
    core,
    builder =>
    {
        builder.RegisterCommand();
        builder.RegisterCommandLongEx();
        builder.RegisterCommandIntEx();
    });
```

Get services:

```C#
var commandLongEx = serverDevice.GetCommandLongEx();
var commandIntEx = serverDevice.GetCommandIntEx();
```

Register and remove handler:

```C#
commandLongEx[MavCmd.MavCmdUser1] = (_, _, _) => CommandResult.AcceptedTask;

// remove handler
commandLongEx[MavCmd.MavCmdUser1] = null;
```

The dispatcher sends an acknowledgement for every handled request:

- a handler's `CommandResult` when the handler completes;
- `Unsupported` when no handler is registered;
- `Failed` when the handler throws;
- `InProgress` for a repeated `COMMAND_LONG` with non-zero confirmation while the busy flag is set and the same command is already running;
- `TemporarilyRejected` for other requests received while the busy flag is set.

The handler receives the sender's system/component IDs as `DeviceIdentity`, the original packet, and a cancellation token that is cancelled when the dispatcher is disposed. Setting an indexer entry to `null` removes the handler. `SupportedCommands` reflects the currently registered handlers.

## Built-in implementations

- [`CommandServerEx<TArgPacket>`](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/CommandServerEx.cs): abstract dispatcher implementation. Its protected constructor accepts the base server, command packet observable, command ID selector, and confirmation selector.
- [`CommandLongServerEx`](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/CommandLongServerEx.cs): dispatcher over `ICommandServer.OnCommandLong`. Uses `payload.confirmation` to detect duplicate retries.
- [`CommandIntServerEx`](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/CommandIntServerEx.cs): dispatcher over `ICommandServer.OnCommandInt`. Confirmation is always treated as `0`.

Both implementations have a single constructor that accepts their base `ICommandServer` instance.

## API {collapsible="true"}

### [ICommandServerEx&lt;TArgPacket&gt;](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/ICommandServerEx.cs)

Represents an extended command server.

| Property            | Type                           | Description                                                  |
|---------------------|--------------------------------|--------------------------------------------------------------|
| `Base`              | `ICommandServer`               | Gets the `ICommandServer` base property.                     |
| `SupportedCommands` | `IEnumerable<MavCmd>`          | Commands supported by the server.                            |
| `this[MavCmd cmd]`  | `CommandDelegate<TArgPacket>?` | Sets the command delegate for the specified MAVLink command. |

### [CommandDelegate&lt;TArgPacket&gt;](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/ICommandServerEx.cs)

Represents a delegate that defines a command handler.

| Parameter | Type                | Description                                                              |
|-----------|---------------------|--------------------------------------------------------------------------|
| `from`    | `DeviceIdentity`    | The identity of the device that sent the command.                        |
| `args`    | `TArgPacket`        | The argument packet for the command handler.                             |
| `cancel`  | `CancellationToken` | The cancellation token that can be used to cancel the command execution. |

| Return Type           | Description                                                    |
|-----------------------|----------------------------------------------------------------|
| `Task<CommandResult>` | Result that the dispatcher sends to the requester in an ACK.   |
