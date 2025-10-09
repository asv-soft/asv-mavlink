# Heartbeat server

If you implement a server device or simulator, you can use [IHeartbeatServer](#iheartbeatserver-source) to send heartbeats.

First of all, you have to register the heartbeat service when building server device:

```c#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(config.SystemId, config.ComponentId), 
    core, 
    builder =>
{
    // register some other services here...
    
    builder.RegisterHeartbeat();
});
```

After building a service, you can get it and, for example, register callbacks to modify the heartbeat payload being sent from the server:

```c#
var heartbeatServer = serverDevice.GetHeartbeat();

heartbeatServer.Set(hb =>
{
    hb.Type = MavType.MavTypeQuadrotor;
    hb.Autopilot = MavAutopilot.MavAutopilotPx4;
});

heartbeatServer.SetCustomMode(bits =>
{
    // set some bits...
});
```

## IHeartbeatServer ([source](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink/Microservices/Heartbeat/Server/IHeartbeatServer.cs#L11))

Defines the interface for a heartbeat server.

| Method                                               | Type   | Description                                             |
|------------------------------------------------------|--------|---------------------------------------------------------|
| `Set(Action<HeartbeatPayload> changeCallback)`       | `void` | Sets the change callback for the heartbeat payload.     |
| `SetCustomMode(Action<UintBitArray> changeCallback)` | `void` | Sets the change callback for the `CustomMode` bitfield. |

#### `IHeartbeatServer.Set`
| Parameter        | Type                        | Description                                                                                                             |
|------------------|-----------------------------|-------------------------------------------------------------------------------------------------------------------------|
| `changeCallback` | `Action<HeartbeatPayload> ` | Callback that is invoked whenever the heartbeat payload is sent, allowing you to modify its fields before transmission. |

#### `IHeartbeatServer.SetCustomMode`
| Parameter        | Type                    | Description                                                                                            |
|------------------|-------------------------|--------------------------------------------------------------------------------------------------------|
| `changeCallback` | `Action<UintBitArray> ` | Callback that is invoked to modify the CustomMode bitfield of the heartbeat payload before it is sent. |
