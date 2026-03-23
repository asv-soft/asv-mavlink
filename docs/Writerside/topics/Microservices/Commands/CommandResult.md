# CommandResult

Represents a command execution result that is sent in `COMMAND_ACK`.

## Use case

You can use `CommandResult` when sending an ACK to the client from the [server](CommandsServer.md):

```C#
// command accepted
await commandServer.SendCommandAck(cmd, target, CommandResult.Accepted, cancel);

// command is still running (progress=42)
await commandServer.SendCommandAck(
    cmd,
    target,
    new CommandResult(MavResult.MavResultInProgress, progress: 42),
    cancel
);

// command denied
await commandServer.SendCommandAck(
    cmd,
    target,
    CommandResult.Denied,
    cancel
);
```

[ServerEx](CommandsServerEx.md) handler example:

```C#
commandLongEx[MavCmd.MavCmdUser1] = (_, _, _) =>
{
    // perform command logic
    return CommandResult.AcceptedTask;
};
```

## API {collapsible="true"}

### [CommandResult](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/ICommandServer.cs)

Represents the result of a command execution.

| Constructor                                                                                            | Description                         |
|--------------------------------------------------------------------------------------------------------|-------------------------------------|
| `CommandResult(MavResult resultCode, int result = 0, byte? progress = null, int? resultParam2 = null)` | Represents the result of a command. |

`CommandResult` exposes a set of pre-allocated singleton instances for supported `MavResult` values,
avoiding unnecessary allocations in hot paths. Each instance is lazy-initialized on first access.

| Property                     | Underlying `MavResult`                          |
|------------------------------|-------------------------------------------------|
| `Accepted`                   | `MavResult.MavResultAccepted`                   |
| `TemporarilyRejected`        | `MavResult.MavResultTemporarilyRejected`        |
| `Denied`                     | `MavResult.MavResultDenied`                     |
| `Unsupported`                | `MavResult.MavResultUnsupported`                |
| `Failed`                     | `MavResult.MavResultFailed`                     |
| `InProgress`                 | `MavResult.MavResultInProgress`                 |
| `Cancelled`                  | `MavResult.MavResultCancelled`                  |
| `CommandLongOnly`            | `MavResult.MavResultCommandLongOnly`            |
| `CommandIntOnly`             | `MavResult.MavResultCommandIntOnly`             |
| `CommandUnsupportedMavFrame` | `MavResult.MavResultCommandUnsupportedMavFrame` |

For each of the above, a corresponding `Task<CommandResult>` singleton is also available by appending `Task` to the property name (e.g. `AcceptedTask`, `FailedTask`).
These are pre-completed tasks wrapping the respective instance, intended for use in synchronous-path `async` implementations to avoid `Task.FromResult` allocations.

| Property       | Type        | Description                               |
|----------------|-------------|-------------------------------------------|
| `ResultParam2` | `int?`      | Gets or sets the second result parameter. |
| `ResultCode`   | `MavResult` | Gets the result code of a MAV operation.  |
| `Result`       | `int`       | Gets the result value.                    |
| `Progress`     | `byte?`     | Gets the progress of the property.        |

| Method                           | Return Type           | Description                                                                                                                  |
|----------------------------------|-----------------------|------------------------------------------------------------------------------------------------------------------------------|
| `FromResult(MavResult result)`   | `CommandResult`       | Returns a cached singleton `CommandResult` instance matching the given `MavResult` value.                                    |
| `AsTaskResult(MavResult result)` | `Task<CommandResult>` | Returns a cached singleton `Task<CommandResult>` wrapping the `CommandResult` instance matching the given `MavResult` value. |

### `CommandResult.FromResult`
| Parameter | Type        | Description                                |
|-----------|-------------|--------------------------------------------|
| `result`  | `MavResult` | The MAVLink result code to convert.        |

### `CommandResult.AsTaskResult`
| Parameter | Type        | Description                                |
|-----------|-------------|--------------------------------------------|
| `result`  | `MavResult` | The MAVLink result code to convert.        |
