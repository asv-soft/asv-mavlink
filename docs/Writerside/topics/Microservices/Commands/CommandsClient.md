# Commands client

Use [`ICommandClient`](#icommandclient) to send MAVLink commands (`COMMAND_LONG` and `COMMAND_INT`) to a remote system/component.

## Use case

You can get `CommandClient` through the microservice locator:

```C#
var command = device.GetMicroservice<ICommandClient>()
    ?? throw new Exception("Command client not found");
```

Request-response call example:

```C#
var ack = await command.CommandLong(
    MavCmd.MavCmdDoSetMode,
    mode,
    customMode,
    customSubMode,
    0,
    0,
    0,
    0,
    cancel
);

Console.WriteLine($"Result: {ack.Result}");
```

Fire-and-forget call example (without waiting for `COMMAND_ACK`):

```C#
await command.SendCommandLong(
    MavCmd.MavCmdDoSetMode,
    mode,
    customMode,
    customSubMode,
    0,
    0,
    0,
    0,
    cancel
);
```

## API {collapsible="true"}

### [CommandProtocolConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Client/CommandClient.cs)

| Property           | Type  | Default | Description                                 |
|--------------------|-------|---------|---------------------------------------------|
| `CommandTimeoutMs` | `int` | `5000`  | Timeout for request/response mission calls. |
| `CommandAttempt`   | `int` | `5`     | Retry count for request/response calls.     |

### [ICommandClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Client/ICommandClient.cs)

Represents a command client that is capable of sending commands to an external system.

| Property       | Type                            | Description                                                                  |
|----------------|---------------------------------|------------------------------------------------------------------------------|
| `OnCommandAck` | `Observable<CommandAckPayload>` | Gets the observable sequence for receiving command acknowledgement payloads. |

| Method                                                                                                                                                                                               | Return Type               | Description                                                                                                 |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------|-------------------------------------------------------------------------------------------------------------|
| `CommandLong(Action<CommandLongPayload> edit, CancellationToken cancel = default)`                                                                                                                   | `Task<CommandAckPayload>` | Sends a command with long parameter to the vehicle.                                                         |
| `CommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default)`                                  | `Task<CommandAckPayload>` | Executes a long-duration command with specified parameters.                                                 |
| `CommandLongAndWaitPacket<TAnswerPacket>(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default)`      | `Task<TAnswerPacket>`     | Executes a long command and waits for the response packet.                                                  |
| `SendCommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel = default)`                              | `Task`                    | Sends a long command to the specified MavCmd with the provided parameters.                                  |
| `SendCommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue, float param1, float param2, float param3, float param4, int x, int y, float z, CancellationToken cancel = default)` | `ValueTask`               | Message encoding a command with parameters as scaled integers. Scaling depends on the actual command value. |
| `CommandInt(MavCmd command, MavFrame frame, bool current, bool autoContinue, float param1, float param2, float param3, float param4, int x, int y, float z, CancellationToken cancel = default)`     | `Task<CommandAckPayload>` | Message encoding a command with parameters as scaled integers. Scaling depends on the actual command value. |

#### `ICommandClient.CommandLong` (delegate overload)
| Parameter | Type                         | Description                                    |
|-----------|------------------------------|------------------------------------------------|
| `edit`    | `Action<CommandLongPayload>` | The delegate used to edit the command payload. |
| `cancel`  | `CancellationToken`          | The cancellation token to cancel the task.     |

#### `ICommandClient.CommandLong` (params overload)
| Parameter | Type                | Description                                            |
|-----------|---------------------|--------------------------------------------------------|
| `command` | `MavCmd`            | The MAVLink command to execute.                        |
| `param1`  | `float`             | The first parameter value.                             |
| `param2`  | `float`             | The second parameter value.                            |
| `param3`  | `float`             | The third parameter value.                             |
| `param4`  | `float`             | The fourth parameter value.                            |
| `param5`  | `float`             | The fifth parameter value.                             |
| `param6`  | `float`             | The sixth parameter value.                             |
| `param7`  | `float`             | The seventh parameter value.                           |
| `cancel`  | `CancellationToken` | A `CancellationToken` to cancel the command execution. |

#### `ICommandClient.CommandLongAndWaitPacket<TAnswerPacket>`
| Parameter | Type                | Description                                             |
|-----------|---------------------|---------------------------------------------------------|
| `command` | `MavCmd`            | The command to be executed.                             |
| `param1`  | `float`             | The first command parameter.                            |
| `param2`  | `float`             | The second command parameter.                           |
| `param3`  | `float`             | The third command parameter.                            |
| `param4`  | `float`             | The fourth command parameter.                           |
| `param5`  | `float`             | The fifth command parameter.                            |
| `param6`  | `float`             | The sixth command parameter.                            |
| `param7`  | `float`             | The seventh command parameter.                          |
| `cancel`  | `CancellationToken` | The cancellation token to cancel the command execution. |

#### `ICommandClient.SendCommandLong`
| Parameter | Type                | Description                                              |
|-----------|---------------------|----------------------------------------------------------|
| `command` | `MavCmd`            | The MavCmd command to send.                              |
| `param1`  | `float`             | The first parameter of the command.                      |
| `param2`  | `float`             | The second parameter of the command.                     |
| `param3`  | `float`             | The third parameter of the command.                      |
| `param4`  | `float`             | The fourth parameter of the command.                     |
| `param5`  | `float`             | The fifth parameter of the command.                      |
| `param6`  | `float`             | The sixth parameter of the command.                      |
| `param7`  | `float`             | The seventh parameter of the command.                    |
| `cancel`  | `CancellationToken` | The cancellation token to cancel the command (optional). |

#### `ICommandClient.SendCommandInt`
| Parameter      | Type                | Description                                                                                 |
|----------------|---------------------|---------------------------------------------------------------------------------------------|
| `command`      | `MavCmd`            | The scheduled action for the mission item.                                                  |
| `frame`        | `MavFrame`          | The coordinate system of the COMMAND.                                                       |
| `current`      | `bool`              | Not used (set false).                                                                       |
| `autocontinue` | `bool`              | Autocontinue to next wp.                                                                    |
| `param1`       | `float`             | PARAM1, see MAV_CMD enum.                                                                   |
| `param2`       | `float`             | PARAM2, see MAV_CMD enum.                                                                   |
| `param3`       | `float`             | PARAM3, see MAV_CMD enum.                                                                   |
| `param4`       | `float`             | PARAM4, see MAV_CMD enum.                                                                   |
| `x`            | `int`               | PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7.             |
| `y`            | `int`               | PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7.            |
| `z`            | `float`             | PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame). |
| `cancel`       | `CancellationToken` | The cancellation token to cancel the command (optional).                                    |

#### `ICommandClient.CommandInt`
| Parameter      | Type                | Description                                                                                 |
|----------------|---------------------|---------------------------------------------------------------------------------------------|
| `command`      | `MavCmd`            | The scheduled action for the mission item.                                                  |
| `frame`        | `MavFrame`          | The coordinate system of the COMMAND.                                                       |
| `current`      | `bool`              | Not used (set false).                                                                       |
| `autoContinue` | `bool`              | Autocontinue to next wp.                                                                    |
| `param1`       | `float`             | PARAM1, see MAV_CMD enum.                                                                   |
| `param2`       | `float`             | PARAM2, see MAV_CMD enum.                                                                   |
| `param3`       | `float`             | PARAM3, see MAV_CMD enum.                                                                   |
| `param4`       | `float`             | PARAM4, see MAV_CMD enum.                                                                   |
| `x`            | `int`               | PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7.             |
| `y`            | `int`               | PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7.            |
| `z`            | `float`             | PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame). |
| `cancel`       | `CancellationToken` | The cancellation token to cancel the command (optional).                                    |

### [CommandClientHelper](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Client/CommandClientHelper.cs)

| Method                                                                                                                                                                  | Return Type                     | Description                                                                                                                             |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| `CommandLongAndCheckResult(Action<CommandLongPayload> edit, CancellationToken cancel = default)`                                                                        | `Task`                          | Sends a command with long parameter to the vehicle and validates the result.                                                            |
| `CommandLongAndCheckResult(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel)` | `Task`                          | Executes a long-duration command with specified parameters and validates the result.                                                    |
| `SetMessageInterval(int messageId, int intervalUs, CancellationToken cancel = default)`                                                                                 | `Task`                          | Set the interval between messages for a particular MAVLink message ID. This interface replaces REQUEST_DATA_STREAM.                     |
| `SetMessageInterval<TPacket>(int intervalUs, CancellationToken cancel = default)`                                                                                       | `Task`                          | Set the interval between messages for a particular MAVLink message ID. This interface replaces REQUEST_DATA_STREAM (generic version).   |
| `RequestMessageOnce(int messageId, CancellationToken cancel = default)`                                                                                                 | `Task`                          | Request the target system(s) emit a single instance of a specified message (i.e. a "one-shot" version of MAV_CMD_SET_MESSAGE_INTERVAL). |
| `RequestMessageOnce<TPacket>(CancellationToken cancel = default)`                                                                                                       | `Task<TPacket>`                 | Requests single message and waits for packet `TPacket`.                                                                                 |
| `GetAutopilotVersion(CancellationToken cancel = default)`                                                                                                               | `Task<AutopilotVersionPayload>` | Requests the autopilot version from the target system by sending a one-shot `AutopilotVersionPacket` request.                           |
| `DoSetMode(uint mode, uint customMode, uint customSubMode, CancellationToken cancel = default)`                                                                         | `Task`                          | Sets the flight mode of the target system using `MavCmd.MavCmdDoSetMode`.                                                               |
| `PreflightRebootShutdown(AutopilotRebootShutdown autopilot, CompanionRebootShutdown companion, CancellationToken cancel = default)`                                     | `Task`                          | Request the reboot or shutdown of system components using `MavCmd.MavCmdPreflightRebootShutdown`.                                       |
| `GetAutopilotRebootShutdownDescription(this AutopilotRebootShutdown src)`                                                                                               | `string`                        | Returns a human-readable description for an `AutopilotRebootShutdown` action.                                                           |
| `GetCompanionRebootShutdownDescription(this CompanionRebootShutdown src)`                                                                                               | `string`                        | Returns a human-readable description for a `CompanionRebootShutdown` action.                                                            |

#### `ICommandClient.CommandLongAndCheckResult` (delegate overload)
| Parameter | Type                         | Description                                           |
|-----------|------------------------------|-------------------------------------------------------|
| `client`  | `ICommandClient`             | The command client.                                   |
| `edit`    | `Action<CommandLongPayload>` | Delegate to populate the `CommandLongPayload` fields. |
| `cancel`  | `CancellationToken`          | Optional cancel token argument.                       |

#### `ICommandClient.CommandLongAndCheckResult` (params overload)
| Parameter | Type                | Description                          |
|-----------|---------------------|--------------------------------------|
| `client`  | `ICommandClient`    | The command client.                  |
| `command` | `MavCmd`            | The MAVLink command.                 |
| `param1`  | `float`             | Command parameter 1.                 |
| `param2`  | `float`             | Command parameter 2.                 |
| `param3`  | `float`             | Command parameter 3.                 |
| `param4`  | `float`             | Command parameter 4.                 |
| `param5`  | `float`             | Command parameter 5.                 |
| `param6`  | `float`             | Command parameter 6.                 |
| `param7`  | `float`             | Command parameter 7.                 |
| `cancel`  | `CancellationToken` | Optional cancel token argument.      |

#### `ICommandClient.SetMessageInterval`
| Parameter     | Type                | Description                                        |
|---------------|---------------------|----------------------------------------------------|
| `client`      | `ICommandClient`    | The command client.                                |
| `messageId`   | `int`               | The MAVLink message ID to configure.               |
| `intervalUs`  | `int`               | Interval in microseconds between messages.         |
| `cancel`      | `CancellationToken` | Optional cancel token argument.                    |

#### `ICommandClient.RequestMessageOnce`
| Parameter   | Type                | Description                          |
|-------------|---------------------|--------------------------------------|
| `client`    | `ICommandClient`    | The MAVLink command client.          |
| `messageId` | `int`               | The MAVLink message ID to request.   |
| `cancel`    | `CancellationToken` | Optional cancel token argument.      |

#### `ICommandClient.SetMessageInterval<TPacket>`
| Parameter    | Type                | Description                                |
|--------------|---------------------|--------------------------------------------|
| `src`        | `ICommandClient`    | The command client.                        |
| `intervalUs` | `int`               | Interval in microseconds between messages. |
| `cancel`     | `CancellationToken` | Optional cancel token argument.            |

#### `ICommandClient.RequestMessageOnce<TPacket>`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `src`     | `ICommandClient`    | The command client.             |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `ICommandClient.GetAutopilotVersion`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `src`     | `ICommandClient`    | The command client.             |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `ICommandClient.DoSetMode`
| Parameter       | Type                | Description                                  |
|-----------------|---------------------|----------------------------------------------|
| `src`           | `ICommandClient`    | The command client.                          |
| `mode`          | `uint`              | The base mode bitmask.                       |
| `customMode`    | `uint`              | Autopilot-specific custom mode.              |
| `customSubMode` | `uint`              | Autopilot-specific custom sub-mode.          |
| `cancel`        | `CancellationToken` | Optional cancel token argument.              |

#### `ICommandClient.PreflightRebootShutdown`
| Parameter   | Type                       | Description                                        |
|-------------|----------------------------|----------------------------------------------------|
| `src`       | `ICommandClient`           | The command client.                                |
| `autopilot` | `AutopilotRebootShutdown`  | Reboot/shutdown action for the autopilot.          |
| `companion` | `CompanionRebootShutdown`  | Reboot/shutdown action for the companion computer. |
| `cancel`    | `CancellationToken`        | Optional cancel token argument.                    |

#### `CompanionRebootShutdown.GetAutopilotRebootShutdownDescription`
| Parameter | Type                      | Description                           |
|-----------|---------------------------|---------------------------------------|
| `src`     | `AutopilotRebootShutdown` | The autopilot reboot/shutdown action. |

#### `CompanionRebootShutdown.GetCompanionRebootShutdownDescription`
| Parameter | Type                      | Description                                    |
|-----------|---------------------------|------------------------------------------------|
| `src`     | `CompanionRebootShutdown` | The companion computer reboot/shutdown action. |

### [CommandException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/CommandException.cs)

Represents an exception that is thrown when a MAVLink command execution results in a non-success acknowledgment. 
Contains the `CommandAckPayload` detailing the result of the command.

| Constructor                                  | Description                                                                                                               |
|----------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| `CommandException(CommandAckPayload result)` | Initializes a new instance of the `CommandException` class with the acknowledgment payload returned by the remote device. |

| Property | Type                | Description                                                     |
|----------|---------------------|-----------------------------------------------------------------|
| `Result` | `CommandAckPayload` | Gets the command acknowledgment payload returned by the device. |

| Method                                 | Return Type | Description                                                       |
|----------------------------------------|-------------|-------------------------------------------------------------------|
| `GetMessage(CommandAckPayload result)` | `string`    | Builds a human-readable error message from a `CommandAckPayload`. |

#### `CommandException.GetMessage`
| Parameter | Type                | Description                           |
|-----------|---------------------|---------------------------------------|
| `result`  | `CommandAckPayload` | The acknowledgment payload to format. |
