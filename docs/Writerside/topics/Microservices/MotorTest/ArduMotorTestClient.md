# Ardu motor test client

This page describes the ArduPilot-specific implementations of `IMotorTestClient` and `ITestMotor`.

- [ArduCopterMotorTestClient](#arducoptermotortestclient-source)
- [ArduTestMotor](#ardutestmotor-source)

## Implementations

### ArduCopterMotorTestClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/Microservices/MotorTest/ArduCopterMotorTestClient.cs))

Monitors frame configuration changes by reading the `FRAME_CLASS` and `FRAME_TYPE` parameters, maps servo output 
channels to motor instance numbers via `ArduPilotMotorsLayout`, and is responsible for disposing the ArduTestMotor resources.
For test state tracking it uses `ArduMotorTestTimer` to emulate ArduPilot’s motor test timer behavior.

`ArduPilotMotorsLayout` is generated from the [APMotorLayout.json](https://github.com/ArduPilot/MissionPlanner/blob/master/APMotorLayout.json)
motor-mixing configuration file provided by the [ArduPilot Mission Planner](https://ardupilot.org/planner/) project.

> ArduPilot resets and reinitializes the motor test timer to the timeout value from the most recently received `MAV_CMD_DO_MOTOR_TEST` MAVLink command.
{style="info"}

### ArduTestMotor ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Microservices/MotorTest/ArduTestMotor.cs))

Issues the motor test MAVLink commands and updates the test status timer.

