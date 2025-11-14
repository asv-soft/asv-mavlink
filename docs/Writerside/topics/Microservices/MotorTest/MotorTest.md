# MotorTest

This microservice provides a high-level interface for reading and changing the motor frame configuration of a MAVLink device.

The frame microservice currently supports only a [client](FrameClient.md) role implementing [IFrameClient](FrameClient.md#iframeclient-source).

Read more about supported devices in the [client implementations](FrameClient.md#implementations) section.

## Data structures

### ITestMotor ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Client/ITestMotor.cs))

Represents testing motor.

| Property       | Type                               | Description                     |
|----------------|------------------------------------|---------------------------------|
| `Id`           | `int`                              | Unique identifier of the motor. |
| `ServoChannel` | `int`                              | Unique identifier of the motor. |
| `IsTestRun`    | `ReadOnlyReactiveProperty<bool>`   | Unique identifier of the motor. |
| `Pwm`          | `ReadOnlyReactiveProperty<ushort>` | Unique identifier of the motor. |
| `Id`           | `int`                              | Unique identifier of the motor. |

| Method                                                           | Return Type       | Description                                             |
|------------------------------------------------------------------|-------------------|---------------------------------------------------------|
| `StartTest(int throttle, int timeout, CancellationToken cancel)` | `Task<MavResult>` | Sets the change callback for the heartbeat payload.     |
| `StopTest(CancellationToken cancel)`                             | `Task<MavResult>` | Sets the change callback for the `CustomMode` bitfield. |

## Exceptions

### MotorTestException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Exceptions/MotorTestException.cs)) 
Base exception for the MotorTest microservice.

### UnsupportedVehicleException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Exceptions/UnsupportedVehicleException.cs))
Thrown when attempting to set a frame that is not supported by the current device.
