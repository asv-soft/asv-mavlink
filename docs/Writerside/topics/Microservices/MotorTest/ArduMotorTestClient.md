# Ardu motor test client

This page describes the ArduPilot-specific implementations of the `IMotorTestClient` microservice.

- [ArduCopterMotorTestClient](#arducopterframeclient-source)

Both clients work with the [Params client](Params.md) to read and write the underlying ArduPilot parameters. 
The concrete parameter names differ between Copter and QuadPlane.

They share a common `ITestMotor` implementation — [`ArduTestMotor`](#ardumotorframe-source).

## Implementations

### ArduCopterMotorTestClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/Microservices/Frame/ArduCopterFrameCompability.cs))

Reads/writes ArduPilot params:
- `FRAME_CLASS`
- `FRAME_TYPE`

Uses [map](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/Microservices/Frame/ArduCopterFrameCompability.cs) as a compability reference.

### ArduTestMotor ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Microservices/Frame/ArduMotorFrame.cs))

Represents an ArduPilot motor frame. It exposes the `MotorFrameClass` and an optional `MotorFrameType`; 
their values align with the corresponding device parameters.

The optional `Meta` dictionary contains device‑specific parameter metadata and maps parameter names to enum names 
(e.g., `FRAME_CLASS`: "Quad", `FRAME_TYPE`: "X").

The `Id` is a human‑readable identifier composed from the class and (optionally) the type (e.g., "Quad (1) / X (1)").

