# Mode client

To work with modes from the client side, request the `IModeClient` microservice from a device instance.

```C#
var modeClient = device.GetMicroservice<IModeClient>()
    ?? throw new Exception("No mode client found");
```

You can enumerate available modes:
```C#
foreach (var mode in modeClient.AvailableModes)
{
    Console.WriteLine($"Available mode: {mode.Name}");
}
```

Or request a mode change (the client populates `MAV_CMD_DO_SET_MODE` and sends it via the Command Protocol):

```C#
await modeClient.SetMode(ArduCopterMode.Guided);
```

Subscribe to current mode updates (values are driven from HEARTBEAT):

```C#
var subscription = modeClient.CurrentMode.Subscribe(mode =>
{
Console.WriteLine($"Current mode: {mode?.Name}");
});
```

> Don't forget to dispose subscriptions when they are no longer needed.  
{style="warning"}

## IModeClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Mode/Client/IModeClient.cs))

| Property         | Type                                    | Description                                        |
|------------------|-----------------------------------------|----------------------------------------------------|
| `AvailableModes` | `ImmutableArray<ICustomMode>`           | List of all supported modes for the target system. |
| `CurrentMode`    | `ReadOnlyReactiveProperty<ICustomMode>` | Current mode.                                      |

| Method                                                          | Return Type | Description                                       |
|-----------------------------------------------------------------|-------------|---------------------------------------------------|
| `SetMode(ICustomMode mode, CancellationToken cancel = default)` | `Task`      | Sends a mode change request to the target system. |

### `IModeClient.SetMode`
| Parameter | Type                | Description                  |
|-----------|---------------------|------------------------------|
| `mode`    | `ICustomMode`       | Mode to set.                 |
| `cancel`  | `CancellationToken` | Optional cancellation token. |
