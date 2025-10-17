# Frame

The frame microservice provides a high-level interface for reading and changing the motor frame configuration of a MAVLink device.

The frame microservice currently supports only a [client](FrameClient.md) role implementing [IFrameClient](FrameClient.md#iframeclient-source).

## Supported devices

- **ArduCopter**
- **ArduPlane - QuadPlane**

Each device has its own set of available frame configurations based on ArduPilot compatibility tables.

## Data structures

### IMotorFrame ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/IMotorFrame.cs))

Represents a motor frame configuration.

| Property | Type                                    | Description                                          |
|----------|-----------------------------------------|------------------------------------------------------|
| `Id`     | `string`                                | Unique identifier of the frame.                      |
| `Meta`   | `IReadOnlyDictionary<string, string>`   | Metadata with device-specific parameter information. |

### MotorFrameIsNotAvailableException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/MotorFrameIsNotAvailableException.cs))

Exception thrown when attempting to set a frame that is not supported by the current device.

## Implementations

Different vehicle types have their own implementations of `IMotorFrame`:

- **ArduMotorFrame** ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/ArduMotorFrame.cs)) — used by ArduCopter and ArduQuadPlane devices.