# Heartbeat client

To work with heartbeat packets from a client, you can request the [IHeartbeatClient](#iheartbeatclient) from the device instance. 
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

## [HeartbeatClientConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Heartbeat/Client/HeartbeatClient.cs#L18)

| Property                      | Type   | Default | Description                                                                          |
|-------------------------------|--------|---------|--------------------------------------------------------------------------------------|
| `HeartbeatTimeoutMs`          | `int`  | `2000`  | Time without a heartbeat before the link indicator is downgraded.                    |
| `LinkQualityWarningSkipCount` | `int`  | `3`     | Number of link downgrade events tolerated before disconnection.                      |
| `RateMovingAverageFilter`     | `int`  | `3`     | Moving-average filter size used to calculate `PacketRateHz`.                         |
| `PrintStatisticsToLogDelayMs` | `int`  | `10000` | Interval for logging packet rate and link quality. A non-positive value disables it. |
| `PrintLinkStateToLog`         | `bool` | `true`  | Enables logging of link state changes.                                               |

## [IHeartbeatClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Heartbeat/Client/IHeartbeatClient.cs#L11)

Represents a client that receives heartbeats and monitors the connection to a target device.

| Property       | Type                                          | Description                                                                        |
|----------------|-----------------------------------------------|------------------------------------------------------------------------------------|
| `FullId`       | `ushort`                                      | Combined system and component ID of the target device.                             |
| `RawHeartbeat` | `ReadOnlyReactiveProperty<HeartbeatPayload?>` | Most recently received heartbeat payload.                                          |
| `PacketRateHz` | `ReadOnlyReactiveProperty<double>`            | Receive rate of all MAVLink packets from the target device, in packets per second. |
| `LinkQuality`  | `ReadOnlyReactiveProperty<double>`            | Packet sequence quality for all MAVLink packets received from the target device.   |
| `Link`         | `ILinkIndicator`                              | State of the heartbeat connection.                                                 |

### [WhenRepairConnection](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Heartbeat/Client/HeartbeatClientHelper.cs#L11)

`WhenRepairConnection(TimeSpan lostTime, CancellationToken cancel = default)` creates an observable intended to report a restored connection after it has been disconnected longer than `lostTime`.

> The current implementation does not emit recovery notifications because its previous-state tracking does not advance from the initial disconnected state. The `cancel` argument is also currently unused.
{style="warning"}
