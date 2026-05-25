# Ardu frame client

This page describes the ArduPilot-specific implementations of the `IFrameClient` microservice.

- [ArduCopterFrameClient](#arducopterframeclient)
- [ArduQuadPlaneFrameClient](#arduquadplaneframeclient)

Both clients work with the [Params client](Params.md) to read and write the underlying ArduPilot parameters. 
The concrete parameter names differ between Copter and QuadPlane.

They share a common `IDroneFrame` implementation — [`ArduDroneFrame`](#ardudroneframe).

## Implementations

### [ArduCopterFrameClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/Microservices/Frame/ArduCopterFrameClient.cs)

Reads/writes ArduPilot params:
- `FRAME_CLASS`
- `FRAME_TYPE`

Uses [map](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/Microservices/Frame/ArduCopterFrameCompability.cs) as a compability reference.

### [ArduQuadPlaneFrameClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Plane/Quad/Microservices/Frame/ArduQuadPlaneFrameClient.cs)

Reads/writes ArduPilot params:
- `Q_FRAME_CLASS`
- `Q_FRAME_TYPE`

Uses [map](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Plane/Quad/Microservices/Frame/ArduQuadPlaneFrameCompability.cs) as a compability reference.

### [ArduDroneFrame](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Microservices/Frame/ArduDroneFrame.cs)

Represents an ArduPilot frame. It exposes the `FrameClass` and an optional `FrameType`; 
their values align with the corresponding device parameters.

The optional `Meta` dictionary contains device‑specific parameter metadata and maps parameter names to enum names 
(e.g., `FRAME_CLASS`: "Quad", `FRAME_TYPE`: "X").

The `Id` is a human‑readable identifier composed from the class and (optionally) the type (e.g., "Quad (1) / X (1)").

## Exceptions

### [ArduFrameClassUnknownException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Microservices/Frame/Exceptions/ArduFrameClassUnknownException.cs)
Raised when the current device frame class can’t be mapped to the known enum values.

### [ArduFrameTypeUnknownException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Microservices/Frame/Exceptions/ArduFrameTypeUnknownException.cs)
Raised when the current device frame type can’t be mapped to the known enum values.
