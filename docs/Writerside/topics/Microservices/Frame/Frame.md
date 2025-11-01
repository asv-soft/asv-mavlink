# Frame

This microservice provides a high-level interface for reading and changing the drone frame configuration of a MAVLink device.

The frame microservice currently supports only a [client](FrameClient.md) role implementing [IFrameClient](FrameClient.md#iframeclient-source).

Read more about supported devices in the [client implementations](FrameClient.md#implementations) section.

## Data structures

### IDroneFrame ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/IDroneFrame.cs))

Represents a drone frame configuration.

| Property | Type                                    | Description                                          |
|----------|-----------------------------------------|------------------------------------------------------|
| `Id`     | `string`                                | Unique identifier of the frame.                      |
| `Meta`   | `IReadOnlyDictionary<string, string>`   | Metadata with device-specific parameter information. |

## Exceptions

### FrameMicroserviceException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/FrameMicroserviceException.cs)) 
Base exception for the Frame microservice.

### DroneFrameIsNotAvailableException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/Exceptions/DroneFrameIsNotAvailableException.cs))
Thrown when attempting to set a frame that is not supported by the current device.
