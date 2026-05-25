# Commands server

Use [`ICommandServer`](#icommandserver) to implement low-level command protocol handling on the server side.

## Use case

Register the service:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(systemId, componentId),
    core,
    builder =>
    {
        builder.RegisterCommand();
    });
```

Then get the service:

```C#
var commandServer = serverDevice.GetCommand();
```

Basic handling example:

```C#
using var onLong = commandServer.OnCommandLong.SubscribeAwait(async (req, ct) =>
{
    await commandServer.SendCommandAck(
        req.Payload.Command,
        new DeviceIdentity(req.SystemId, req.ComponentId),
        CommandResult.Accepted,
        ct
    );
});

using var onInt = commandServer.OnCommandInt.SubscribeAwait(async (req, ct) =>
{
    await commandServer.SendCommandAck(
        req.Payload.Command,
        new DeviceIdentity(req.SystemId, req.ComponentId),
        CommandResult.Accepted,
        ct
    );
});
```

Here we are using [`CommandResult`](CommandResult.md) to send a response.

> Do not forget to dispose subscriptions.
{style="warning"}

## API {collapsible="true"}

### [ICommandServer](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/ICommandServer.cs)

Represents a command server that receives and sends commands.

| Property        | Type                            | Description                                                |
|-----------------|---------------------------------|------------------------------------------------------------|
| `OnCommandLong` | `Observable<CommandLongPacket>` | Gets the event stream for receiving `CommandLong` packets. |
| `OnCommandInt`  | `Observable<CommandIntPacket>`  | Gets an observable sequence of `CommandIntPacket` events.  |

| Method                                                                                                                | Return Type | Description                                                    |
|-----------------------------------------------------------------------------------------------------------------------|-------------|----------------------------------------------------------------|
| `SendCommandAck(MavCmd cmd, DeviceIdentity responseTarget, CommandResult result, CancellationToken cancel = default)` | `ValueTask` | Sends a command acknowledgement with the specified parameters. |

#### `ICommandServer.SendCommandAck`

| Parameter        | Type                | Description                                         |
|------------------|---------------------|-----------------------------------------------------|
| `cmd`            | `MavCmd`            | The command being acknowledged.                     |
| `responseTarget` | `DeviceIdentity`    | The target device identity for the acknowledgement. |
| `result`         | `CommandResult`     | The result of the command execution.                |
| `cancel`         | `CancellationToken` | Optional cancellation token.                        |

### [DeviceIdentity](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/ICommandServer.cs)

Represents the identity of a device.

| Constructor                                       | Description                                               |
|---------------------------------------------------|-----------------------------------------------------------|
| `DeviceIdentity()`                                | Represents a device identity.                             |
| `DeviceIdentity(byte systemId, byte componentId)` | Initializes a new instance of the `DeviceIdentity` class. |

| Property      | Type             | Description                                           |
|---------------|------------------|-------------------------------------------------------|
| `Broadcast`   | `DeviceIdentity` | Represents the broadcast device identity.             |
| `SystemId`    | `byte`           | Gets or sets the system ID.                           |
| `ComponentId` | `byte`           | Gets or sets the unique identifier for the component. |

### [CommandServerHelper](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/Server/CommandServerHelper.cs)

Helper class containing extension methods for ICommandServer interface.

| Method                                                                                                                            | Return Type | Description                                                                   |
|-----------------------------------------------------------------------------------------------------------------------------------|-------------|-------------------------------------------------------------------------------|
| `SendCommandAckAccepted(this ICommandServer server, CommandIntPacket req, MavResult result, CancellationToken cancel = default)`  | `ValueTask` | Sends command acknowledge accepted to the command server.                     |
| `SendCommandAckAccepted(this ICommandServer server, CommandLongPacket req, MavResult result, CancellationToken cancel = default)` | `ValueTask` | Sends a command acknowledgment indicating that the command has been accepted. |

#### `ICommandServer.SendCommandAckAccepted` (`CommandIntPacket` overload)
| Parameter | Type                | Description                                      |
|-----------|---------------------|--------------------------------------------------|
| `server`  | `ICommandServer`    | The command server.                              |
| `req`     | `CommandIntPacket`  | The command request packet.                      |
| `result`  | `MavResult`         | The mav result.                                  |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                  |

#### `ICommandServer.SendCommandAckAccepted` (`CommandLongPacket` overload)
| Parameter | Type                | Description                                      |
|-----------|---------------------|--------------------------------------------------|
| `server`  | `ICommandServer`    | The command server.                              |
| `req`     | `CommandLongPacket` | The command long packet.                         |
| `result`  | `MavResult`         | The result of the command.                       |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                  |

### [CommandHelper](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Commands/CommandHelper.cs)

Provides helper methods for registering and accessing command-related microservices, 
including base command server and extended CommandLong/CommandInt handlers.

| Property               | Type     | Description                               |
|------------------------|----------|-------------------------------------------|
| `MicroserviceTypeName` | `string` | Represents the broadcast device identity. |

| Method                                                     | Return Type                           | Description                                                                             |
|------------------------------------------------------------|---------------------------------------|-----------------------------------------------------------------------------------------|
| `RegisterCommand(this IServerDeviceBuilder builder)`       | `IServerDeviceBuilder`                | Registers `CommandServer` at device builder with default parameters.                    |
| `RegisterCommandLongEx(this IServerDeviceBuilder builder)` | `IServerDeviceBuilder`                | Registers `CommandLongServerEx` at device builder with default parameters.              |
| `RegisterCommandIntEx(this IServerDeviceBuilder builder)`  | `IServerDeviceBuilder`                | Registers `CommandIntServerEx` at device builder with default parameters.               |
| `GetCommand(this IServerDevice factory)`                   | `ICommandServer`                      | Resolves `ICommandServer` instance from the server device factory.                      |
| `GetCommandIntEx(this IServerDevice factory)`              | `ICommandServerEx<CommandIntPacket>`  | Resolves `ICommandServerEx<CommandIntPacket>` instance from the server device factory.  |
| `GetCommandLongEx(this IServerDevice factory)`             | `ICommandServerEx<CommandLongPacket>` | Resolves `ICommandServerEx<CommandLongPacket>` instance from the server device factory. |
