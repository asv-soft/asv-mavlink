# Heartbeat client

To work with heartbeat packets from a client, you can request the [IHeartbeatClient](#iheartbeatclient-source) from the device instance. 
It is registered by default.

```c#
var heartbeat = device.GetMicroservice<IHeartbeatClient>() 
    ?? throw new Exception("No heartbeat client found");
```

After that you can, for example, subscribe to new heartbeat events:

```c#
var subscription = heartbeat.RawHeartbeat.Subscribe(payload =>
{
    if (payload == null) return;
    Console.WriteLine($"Heartbeat: type={payload.Type}, status={payload.SystemStatus}");
});
```

Or subscribe to connection quality changes:

```c#
var subscription = heartbeat.LinkQuality.Subscribe(quality => 
{
    Console.WriteLine($"Heartbeat quality: {quality}");
});
```

>Don't forget to dispose subscriptions when they are no longer needed.
{style="warning"}

## IHeartbeatClient ([source](https://github.com/asv-soft/asv-mavlink/blob/2ae4bb9c1dbca2c916379c9bfac36e1f8fe94789/src/Asv.Mavlink/Microservices/Heartbeat/Client/IHeartbeatClient.cs#L11C1-L11C67))

Represents a client that sends and receives heartbeats.

| Property       | Type                                          | Description              |
|----------------|-----------------------------------------------|--------------------------|
| `FullId`       | `ushort`                                      | Full ID of the property. |
| `RawHeartbeat` | `ReadOnlyReactiveProperty<HeartbeatPayload?>` | Raw heartbeat value.     |
| `PacketRateHz` | `ReadOnlyReactiveProperty<double>`            | Packet rate in Hz.       |
| `LinkQuality`  | `ReadOnlyReactiveProperty<double>`            | Quality of the link.     |
| `Link`         | `ILinkIndicator`                              | State of the link.       |
