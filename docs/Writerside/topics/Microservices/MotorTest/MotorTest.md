# MotorTest

A microservice that manages motor test execution, including telemetry collection and real-time test state tracking 
for a MAVLink device.

The motor test microservice currently supports only a [client](MotorTestClient.md)
implementing [IMotorTestClient](MotorTestClient.md#imotortestclient-source).

 See [client implementations](MotorTestClient.md#implementations) for details on test state tracking.

## Data structures

### ITestMotor ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Client/ITestMotor.cs))

[ITestMotor](TestMotor.md) defines a single testable motor, exposing its identifier, servo output channel, 
test execution state, and PWM telemetry, and provides asynchronous operations to run motor tests.

## Exceptions

### MotorTestException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Exceptions/MotorTestException.cs))

Base exception for the MotorTest microservice.

### UnsupportedVehicleException ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Exceptions/UnsupportedVehicleException.cs))

Vehicle type is unknown or unsupported. Mapping a motor to its servo channel is not possible.