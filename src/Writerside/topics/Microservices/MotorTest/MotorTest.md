# MotorTest

A microservice that manages motor test execution, including PWM telemetry collection and local test state tracking
for a MAVLink device. The test state is based on accepted commands and a client-side timeout; it does not confirm the physical state of a motor.

The motor test microservice currently supports only a [client](MotorTestClient.md)
implementing [IMotorTestClient](MotorTestClient.md#imotortestclient).

 See [client implementations](MotorTestClient.md#implementations) for details on test state tracking.

## Data structures

### [ITestMotor](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Client/ITestMotor.cs)

[ITestMotor](TestMotor.md) defines a single testable motor, exposing its identifier, servo output channel, 
test execution state, and PWM telemetry, and provides asynchronous operations to run motor tests.

## Exceptions

### [MotorTestException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Exceptions/MotorTestException.cs)

Base exception for the MotorTest microservice.

### [UnsupportedVehicleException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Exceptions/UnsupportedVehicleException.cs)

Vehicle type is unknown or unsupported. Mapping a motor to its servo channel is not possible.
