# Frame

This microservice provides a high-level interface for reading and changing the motor frame configuration of a MAVLink device.

The frame microservice currently supports only a [client](FrameClient.md) role implementing [IFrameClient](FrameClient.md#iframeclient-source).

Read more about supported devices in the [client implementations](FrameClient.md#implementations) section.

## Data structures

### IMotorFrame ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/IMotorFrame.cs))

Represents a motor frame configuration.

| Property | Type                                    | Description                                          |
|----------|-----------------------------------------|------------------------------------------------------|
| `Id`     | `string`                                | Unique identifier of the frame.                      |
| `Meta`   | `IReadOnlyDictionary<string, string>`   | Metadata with device-specific parameter information. |

## Exceptions

### FrameMicroserviceException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/FrameMicroserviceException.cs)) 
Base exception for the Frame microservice.

### MotorFrameIsNotAvailableException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/Exceptions/MotorFrameIsNotAvailableException.cs))
Thrown when attempting to set a frame that is not supported by the current device.
