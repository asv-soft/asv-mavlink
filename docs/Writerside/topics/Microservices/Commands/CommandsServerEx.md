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

## Built-in implementations

- [`CommandLongServerEx`](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/CommandLongServerEx.cs): dispatcher over `ICommandServer.OnCommandLong`. Uses `payload.confirmation` to detect duplicate retries.
- [`CommandIntServerEx`](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/Ex/CommandIntServerEx.cs): dispatcher over `ICommandServer.OnCommandInt`. Confirmation is always treated as `0`.

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
