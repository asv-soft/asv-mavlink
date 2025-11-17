# MotorTest client

To test motors, request the [IMotorTestClient](#imotortestclient-source) microservice from a device instance.

```C#
var motorTestClient = device.GetMicroservice<IMotorTestClient>()
    ?? throw new Exception("No motor test client found");
```

## Usage

Retrieve the list of testable motors from the connected device and read their telemetry:

```C#
// Access telemetry for each test motor
foreach (var motor in motorTestClient.MotorFrames)
{
    Console.WriteLine($"ID: {motor.Id}, Servo channel: {motor.ServoChannel}, PWM: {motor.Pwm}, Test running: {motor.IsTestRun}");
}
```

To run a test, first select a motor:

```C#

var testMotor = motorTestClient.MotorFrames.First();

// Start a 10-second test of the selected motor at 50% power
var ack = await testMotor.StartTest(50, 10);

if (ack.Result != MavResult.MavResultAccepted)
{
    Console.WriteLine($"Test for motor #{testMotor.Id} has started");
};

// Stop a test
await testMotor.StopTest();
```

> Remember to dispose of subscriptions when they are no longer needed.
> {style="warning"}

Refresh the available motors if the vehicle configuration changes:

```C#
// Refresh
await motorTestClient.Refresh();
```

## IMotorTestClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Client/IMotorTestClient.cs))

| Property     | Type                                 | Description                             |
|--------------|--------------------------------------|-----------------------------------------|
| `TestMotors` | `ReadOnlyObservableList<ITestMotor>` | Testable motors with telemetry support. |

| Method                                                   | Return Type | Description                                                    |
|----------------------------------------------------------|-------------|----------------------------------------------------------------|
| `Refresh(CancellationToken cancellationToken = default)` | `Task`      | Updates `TestMotors` collection when the vehicle type changes. |

### `IMotorTestClient.Refresh`

| Parameter           | Type                | Description                                |
|---------------------|---------------------|--------------------------------------------|
| `cancellationToken` | `CancellationToken` | An optional token to cancel the operation. |

## Implementations

Vehicles controlled by different autopilots (e.g., ArduPilot, PX4) provide their own implementations 
of [`IMotorTestClient`](#imotortestclient-source) and [`ITestMotor`](TestMotor.md):

- [Ardu devices](ArduMotorTestClient.md)