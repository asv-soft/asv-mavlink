using System.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using ObservableCollections;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ArduCopterMotorTestClient))]
[TestSubject(typeof(ArduMotorTestClient))]
[TestSubject(typeof(ArduTestMotor))]
public class ArduCopterMotorTestComplexTest(ITestOutputHelper log)
    : ArduMotorTestComplexTestBase<ArduCopterMotorTestClient>(log)
{
    protected override string FrameClassParam => "FRAME_CLASS";
    protected override string FrameTypeParam => "FRAME_TYPE";
    protected override int DefaultFrameClass => (int)ArduFrameClass.Quad;
    protected override int DefaultFrameType => (int)ArduFrameType.X;

    protected override ArduCopterMotorTestClient CreateMotorTestClient(
        HeartbeatClient heartbeatClient,
        CommandClient commandClient,
        ParamsClientEx paramsClientEx)
    {
        return new ArduCopterMotorTestClient(heartbeatClient, commandClient, paramsClientEx);
    }

    [Fact]
    public async Task ServoOutputRaw_Received_Success()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);

        // Act
        await SendServoOutputRaw(p =>
        {
            p.Servo1Raw = 1100;
            p.Servo2Raw = 1200;
            p.Servo3Raw = 1300;
            p.Servo4Raw = 1400;
        });

        // Assert
        Assert.Equal(1100, Client.TestMotors.Single(m => m.ServoChannel == 1).Pwm.CurrentValue);
        Assert.Equal(1200, Client.TestMotors.Single(m => m.ServoChannel == 2).Pwm.CurrentValue);
        Assert.Equal(1300, Client.TestMotors.Single(m => m.ServoChannel == 3).Pwm.CurrentValue);
        Assert.Equal(1400, Client.TestMotors.Single(m => m.ServoChannel == 4).Pwm.CurrentValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task StartTest_Accepted_Success(int throttle)
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var motor = Client.TestMotors.Single(m => m.Id == 1);
        var expectedMotor = GetLayout(ArduFrameClass.Quad, ArduFrameType.X).Motors.Single(m => m.Number == motor.Id);

        // Act
        var result = await motor.StartTest(throttle, 10, Cts.Token);

        // Assert
        var command = Assert.Single(MotorTestCommands);
        Assert.Equal(MavResult.MavResultAccepted, result);
        Assert.True(motor.IsTestRun.CurrentValue);
        Assert.Equal(MavCmd.MavCmdDoMotorTest, command.Payload.Command);
        Assert.Equal(expectedMotor.TestOrder, command.Payload.Param1);
        Assert.Equal((float)MotorTestThrottleType.MotorTestThrottlePercent, command.Payload.Param2);
        Assert.Equal(throttle, command.Payload.Param3);
        Assert.Equal(10, command.Payload.Param4);
        Assert.Equal(0, command.Payload.Param5);
        Assert.Equal(0, command.Payload.Param6);
        Assert.Equal(0, command.Payload.Param7);
    }

    [Fact]
    public async Task StopTest_AfterStart_Success()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var motor = Client.TestMotors.Single(m => m.Id == 1);
        var expectedMotor = GetLayout(ArduFrameClass.Quad, ArduFrameType.X).Motors.Single(m => m.Number == motor.Id);
        await motor.StartTest(50, 10, Cts.Token);

        // Act
        var result = await motor.StopTest(Cts.Token);

        // Assert
        Assert.Equal(2, MotorTestCommands.Count);
        var command = MotorTestCommands[1];
        Assert.Equal(MavResult.MavResultAccepted, result);
        Assert.False(motor.IsTestRun.CurrentValue);
        Assert.Equal(MavCmd.MavCmdDoMotorTest, command.Payload.Command);
        Assert.Equal(expectedMotor.TestOrder, command.Payload.Param1);
        Assert.Equal((float)MotorTestThrottleType.MotorTestThrottlePercent, command.Payload.Param2);
        Assert.Equal(0, command.Payload.Param3);
        Assert.Equal(0, command.Payload.Param4);
    }

    [Fact]
    public async Task Init_FrameChanged_Success()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var expectedLayout = GetLayout(ArduFrameClass.Hexa, ArduFrameType.X);
        var motorsCountTcs = new TaskCompletionSource<int>();
        Cts.Token.Register(() => motorsCountTcs.TrySetCanceled());
        using var sub = Client.TestMotors.ObserveCountChanged(cancellationToken: Cts.Token).Subscribe(count =>
        {
            if (count == expectedLayout.Motors.Length)
                motorsCountTcs.TrySetResult(count);
        });

        // Act
        Server[FrameClassParam] = new MavParamValue((int)ArduFrameClass.Hexa);
        Server[FrameTypeParam] = new MavParamValue((int)ArduFrameType.X);

        // Assert
        await motorsCountTcs.Task;
        Assert.Equal(expectedLayout.Motors.Length, Client.TestMotors.Count);
        foreach (var expectedMotor in expectedLayout.Motors)
        {
            Assert.Contains(Client.TestMotors, m => m.Id == expectedMotor.Number);
        }
    }

    [Theory]
    [InlineData(ArduFrameClass.Quad, ArduFrameType.X, 4)]
    [InlineData(ArduFrameClass.Hexa, ArduFrameType.X, 6)]
    [InlineData(ArduFrameClass.Octa, ArduFrameType.X, 8)]
    public async Task Init_DifferentFrames_Success(
        ArduFrameClass frameClass,
        ArduFrameType frameType,
        int expectedMotorsCount)
    {
        // Arrange
        _ = Server;
        Server[FrameClassParam] = new MavParamValue((int)frameClass);
        Server[FrameTypeParam] = new MavParamValue((int)frameType);
        var expectedLayout = GetLayout(frameClass, frameType);

        // Act
        await Client.Init(Cts.Token);

        // Assert
        Assert.Equal(expectedMotorsCount, Client.TestMotors.Count);
        Assert.Equal(expectedLayout.Motors.Length, Client.TestMotors.Count);
    }
}
