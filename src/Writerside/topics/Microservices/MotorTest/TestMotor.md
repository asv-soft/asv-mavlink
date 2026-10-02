# TestMotor
[Source code](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/MotorTest/Client/ITestMotor.cs)

`ITestMotor` represents testable motor.

| Property       | Type                               | Description                                                           |
|----------------|------------------------------------|-----------------------------------------------------------------------|
| `Id`           | `int`                              | Motor instance number.                                                |
| `ServoChannel` | `int`                              | Servo output.                                                         |
| `IsTestRun`    | `ReadOnlyReactiveProperty<bool>`   | Locally tracked test state based on the accepted command and timeout. |
| `Pwm`          | `ReadOnlyReactiveProperty<ushort>` | PWM in microseconds.                                                  |

| Method                                                                     | Return Type       | Description      |
|----------------------------------------------------------------------------|-------------------|------------------|
| `StartTest(int throttle, int timeout, CancellationToken cancel = default)` | `Task<MavResult>` | Run motor test.  |
| `StopTest(CancellationToken cancel = default)`                             | `Task<MavResult>` | Stop motor test. |

### `ITestMotor.StartTest`

| Parameter  | Type                | Description                                     |
|------------|---------------------|-------------------------------------------------|
| `throttle` | `int`               | Throttle value in percentage.                   |
| `timeout`  | `int`               | Motor test duration in seconds.                 |
| `cancel`   | `CancellationToken` | An optional token to cancel the operation.      |

### `ITestMotor.StopTest`

| Parameter | Type                | Description                                |
|-----------|---------------------|--------------------------------------------|
| `cancel`  | `CancellationToken` | An optional token to cancel the operation. |
