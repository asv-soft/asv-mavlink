# Mode server

If you implement a server (autopilot or simulator), register the mode microservice during server device build:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(config.SystemId, config.ComponentId),
    core,
    builder =>
    {
        // register other services...
        
        // you need to pass your own work mode 
        builder.RegisterMode(
            ArduCopterMode.Guided, 
            [], 
            _ => new SomeWorkMode());
    });
```

After building, you can get the service and handle mode changes. The server will typically apply the requested mode and then reflect it in outgoing `HEARTBEAT` messages.

```C#
var modeServer = serverDevice.GetMode();

await modeServer.SetMode(ArduCopterMode.Loiter, handler =>
{
    Console.WriteLine("Mode change requested: Loiter");
    // optional: configure handler / initial state
});
```

## IModeServer ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Mode/Server/IModeServer.cs))

| Property         | Type                                         | Description                       |
|------------------|----------------------------------------------|-----------------------------------|
| `IsBusy`         | `ReadOnlyReactiveProperty<bool>`             | Is server busy.                   |
| `AvailableModes` | `ImmutableArray<ICustomMode>`                | Supported modes on this server.   |
| `CurrentMode`    | `ReadOnlyReactiveProperty<IWorkModeHandler>` | Current active work-mode handler. |

| Method                                                                                            | Return Type | Description                                                                             |
|---------------------------------------------------------------------------------------------------|-------------|-----------------------------------------------------------------------------------------|
| `SetMode(ICustomMode mode, Action<IWorkModeHandler>? update, CancellationToken cancel = default)` | `Task`      | Requests a mode change on the server; optional callback allows configuring the handler. |

### `IModeServer.SetMode`
| Parameter | Type                        | Description                                            |
|-----------|-----------------------------|--------------------------------------------------------|
| `mode`    | `ICustomMode`               | Target mode to apply.                                  |
| `update`  | `Action<IWorkModeHandler>?` | Optional callback invoked with the `IWorkModeHandler`. |
| `cancel`  | `CancellationToken`         | Optional cancellation token.                           |
