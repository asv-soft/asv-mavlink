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
            ArduCopterMode.AllModes,
            mode => new SomeWorkMode(mode));
    });
```

`RegisterMode` depends on the status-text, command-long, and heartbeat services.

After building, you can get the service and request mode changes:

```C#
var modeServer = serverDevice.GetMode();

await modeServer.SetMode(ArduCopterMode.Loiter, handler =>
{
    Console.WriteLine("Mode change requested: Loiter");
    // optional: configure handler / initial state
});
```

> The current `ModeServer.SetMode` implementation has two known limitations:
>
> - after a different mode handler is initialized successfully, it is not assigned to `CurrentMode`, so the previous mode remains in outgoing heartbeats;
> - requesting the already active mode leaves `IsBusy` set to `true`, causing later mode requests to be ignored.
{style="warning"}

## [IModeServer](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Mode/Server/IModeServer.cs)

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

> If another mode change is already in progress, `SetMode` returns without applying the requested mode.
{style="note"}

## [IWorkModeHandler](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Mode/Server/IWorkModeHandler.cs)

Represents the runtime handler associated with a custom mode.

| Property | Type          | Description                         |
|----------|---------------|-------------------------------------|
| `Mode`   | `ICustomMode` | Mode represented by this handler.   |

| Method                                      | Return Type | Description                                  |
|---------------------------------------------|-------------|----------------------------------------------|
| `Init(CancellationToken cancel)`            | `Task`      | Initializes the handler when entering mode.  |
| `Destroy(CancellationToken cancel)`         | `Task`      | Destroys the handler when leaving mode.      |
| `Dispose()`                                 | `void`      | Releases resources owned by the handler.     |
