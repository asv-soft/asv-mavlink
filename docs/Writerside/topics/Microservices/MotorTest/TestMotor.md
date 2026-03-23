# TestMotor
[Source code](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Client/ITestMotor.cs)

`ITestMotor` represents testable motor.

| Property       | Type                               | Description            |
|----------------|------------------------------------|------------------------|
| `Id`           | `int`                              | Motor instance number. |
| `ServoChannel` | `int`                              | Servo output.          |
| `IsTestRun`    | `ReadOnlyReactiveProperty<bool>`   | Motor test status.     |
| `Pwm`          | `ReadOnlyReactiveProperty<ushort>` | PWM in microseconds.   |

| Method                                                           | Return Type       | Description      |
|------------------------------------------------------------------|-------------------|------------------|
| `StartTest(int throttle, int timeout, CancellationToken cancel)` | `Task<MavResult>` | Run motor test.  |
| `StopTest(CancellationToken cancel)`                             | `Task<MavResult>` | Stop motor test. |

### `ITestMotor.StartTest`

| Parameter  | Type                | Description                                     |
|------------|---------------------|-------------------------------------------------|
| `throttle` | `int`               | Throttle value in percentage.                   |
| `timeout`  | `int`               | Timeout between tests that are run in sequence. |
| `cancel`   | `CancellationToken` | An optional token to cancel the operation.      |

### `ITestMotor.StopTest`

| Parameter | Type                | Description                                |
|-----------|---------------------|--------------------------------------------|
| `cancel`  | `CancellationToken` | An optional token to cancel the operation. |