# Mode

The mode microservice provides a high-level interface for reading and changing the operating mode of a MAVLink device.

It is a wrapper around MAVLink primitives: the microservice **reads** the current mode from `HEARTBEAT` (base_mode / custom_mode) and **requests** mode changes via the Command Protocol (client implementations populate `MAV_CMD_DO_SET_MODE`).

The mode microservice can be used in two roles:

- A [client](ModeClient.md) implementing [IModeClient](ModeClient.md#imodeclient-source)  
  (for example, a ground control app) — sends mode change requests and observes the current mode via heartbeats.

- A [server](ModeServer.md) implementing [IModeServer](ModeServer.md#imodeserver-source)  
  (for example, an autopilot or simulator) — accepts mode change requests, applies them and reflects the current mode in heartbeats.

## ICustomMode ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Mode/ICustomMode.cs))

Reference for the `ICustomMode` shape — an implementation describes a single autopilot-specific flight mode (examples: [ArduCopter](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/ArduCopterMode.cs), [ArduPlane](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Plane/ArduPlaneMode.cs), [PX4](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Px4/Px4Mode.cs), [Unknown](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Mode/UnknownMode.cs)).

| Property       | Type     | Description                                                                                                                                                     |
|----------------|----------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Name`         | `string` | Gets the name of the property.                                                                                                                                  |
| `Description`  | `string` | Gets the description of the property.                                                                                                                           |
| `InternalMode` | `bool`   | This flag indicates whether the vehicle can be set into this mode by the user command. You shouldn't show this mode in the user interface if this flag is true. |

| Method                                                                               | Return Type | Description                                                    |
|--------------------------------------------------------------------------------------|-------------|----------------------------------------------------------------|
| `GetCommandLongArgs(out uint baseMode, out uint customMode, out uint customSubMode)` | `void`      | Get args.                                                      |
| `IsCurrentMode(HeartbeatPayload? hb)`                                                | `bool`      | Checks if the given `HEARTBEAT` payload represents this mode.  |
| `IsCurrentMode(CommandLongPayload payload)`                                          | `bool`      | Checks if the given `COMMAND_LONG` payload requests this mode. |
| `Fill(HeartbeatPayload hb)`                                                          | `void`      | Fills mode-related fields in a `HEARTBEAT` payload.            |

### `ICustomMode.GetCommandLongArgs`
| Parameter       | Type       | Description     |
|-----------------|------------|-----------------|
| `baseMode`      | `out uint` | Base mode.      |
| `customMode`    | `out uint` | Custom mode.    |
| `customSubMode` | `out uint` | Custom submode. |

### `ICustomMode.IsCurrentMode(HeartbeatPayload? hb)`
| Parameter | Type                | Description                                   |
|-----------|---------------------|-----------------------------------------------|
| `hb`      | `HeartbeatPayload?` | Heartbeat payload to check against this mode. |

### `ICustomMode.IsCurrentMode(CommandLongPayload payload)`
| Parameter | Type                 | Description                                 |
|-----------|----------------------|---------------------------------------------|
| `payload` | `CommandLongPayload` | Command payload to check against this mode. |

### `ICustomMode.Fill`
| Parameter | Type               | Description                                           |
|-----------|--------------------|-------------------------------------------------------|
| `hb`      | `HeartbeatPayload` | Heartbeat payload to fill with the current mode info. |
