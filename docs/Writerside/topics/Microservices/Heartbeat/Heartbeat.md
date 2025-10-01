# Heartbeat

The heartbeat microservice is used to detect and monitor devices in a MAVLink network. 
It works by sending and receiving a special message — the [`HeartbeatPayload`](#heartbeatpayload-source). 
This message contains basic information about a device (such as type, system status, modes, etc.) 
and is used to determine whether the device is alive.

The heartbeat microservice can be used in two roles:

- A [client](HeartbeatClient.md) implementing [IHeartbeatClient](HeartbeatClient.md#iheartbeatclient-source)  
  (for example, a ground control app) receives heartbeat messages and can decide whether the device is online or disconnected.

- A [server](HeartbeatServer.md) implementing [IHeartbeatServer](HeartbeatServer.md#iheartbeatserver-source)  
  (for example, a drone) sends out heartbeat messages with its current state.

>You can read more about the heartbeat microservice in the official [MAVLink docs](https://mavlink.io/en/services/heartbeat.html). 
>See the [HeartbeatPayload](#heartbeatpayload-source) section below for the exact structure of the payload.
{style="info"}

## HeartbeatPayload ([source](https://github.com/asv-soft/asv-mavlink/blob/2ae4bb9c1dbca2c916379c9bfac36e1f8fe94789/src/Asv.Mavlink/Protocol/Messages/minimal.cs#L1755))

Here is a quick reference for our `HeartbeatPayload` type, an implementation of [MAVLink heartbeat message](https://mavlink.io/en/messages/common.html#messages).

| Property         | Type           | Description                                      |
|------------------|----------------|--------------------------------------------------|
| `Type`           | `MavType`      | Type of the component.                           |
| `Autopilot`      | `MavAutopilot` | Autopilot type.                                  |
| `BaseMode`       | `MavModeFlag`  | System mode bitmap.                              |
| `CustomMode`     | `uint`         | A bitfield for use for autopilot-specific flags. |
| `SystemStatus`   | `MavState`     | System status flag.                              |
| `MavlinkVersion` | `byte`         | MAVLink version.                                 |
