using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Asv.Mavlink.Common;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

public abstract class ArduMotorTestComplexTestBase<TClient>(ITestOutputHelper log)
    : ComplexTestBase<TClient, ParamsServerEx>(log)
    where TClient : ArduMotorTestClient
{
    private readonly HeartbeatClientConfig _heartbeatConfig = new();
    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 60_000,
        CommandAttempt = 3
    };
    private readonly ParamsClientExConfig _paramsClientConfig = new()
    {
        ChunkUpdateBufferMs = 0,
        ReadTimeouMs = 60_000,
        ReadAttemptCount = 3
    };
    
    private CommandServer? _commandServer;
    private CommandLongServerEx? _commandLongServerEx;
    private StatusTextServer? _statusTextServer;
    private ParamsServer? _paramsServer;
    
    protected readonly CancellationTokenSource Cts = new();
    protected readonly List<CommandLongPacket> MotorTestCommands = [];
    protected CommandResult MotorTestCommandResult = CommandResult.FromResult(MavResult.MavResultAccepted);

    protected abstract string FrameClassParam { get; }
    protected abstract string FrameTypeParam { get; }
    protected abstract int DefaultFrameClass { get; }
    protected abstract int DefaultFrameType { get; }
    
    protected abstract TClient CreateMotorTestClient(
        HeartbeatClient heartbeatClient,
        CommandClient commandClient,
        ParamsClientEx paramsClientEx);

    protected override ParamsServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _commandServer = new CommandServer(identity, core);
        _commandLongServerEx = new CommandLongServerEx(_commandServer);
        
        _commandLongServerEx[MavCmd.MavCmdDoMotorTest] = (_, packet, _) =>
        {
            MotorTestCommands.Add(packet);
            return Task.FromResult(MotorTestCommandResult);
        };

        var paramsMeta = new[]
        {
            new MavParamTypeMetadata(FrameClassParam, MavParamType.MavParamTypeInt32)
            {
                DefaultValue = new MavParamValue(DefaultFrameClass),
                MinValue = new MavParamValue(0),
                MaxValue = new MavParamValue(100),
            },
            new MavParamTypeMetadata(FrameTypeParam, MavParamType.MavParamTypeInt32)
            {
                DefaultValue = new MavParamValue(DefaultFrameType),
                MinValue = new MavParamValue(0),
                MaxValue = new MavParamValue(100),
            }
        };

        _statusTextServer = new StatusTextServer(identity, new StatusTextLoggerConfig(), core);
        _paramsServer = new ParamsServer(identity, core);
        
        return new ParamsServerEx(
            _paramsServer,
            _statusTextServer,
            paramsMeta,
            MavParamHelper.CStyleEncoding,
            new InMemoryConfiguration(),
            new ParamsServerExConfig { SendingParamItemDelayMs = 0 });
    }

    protected override TClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var paramsDescriptions = new[]
        {
            new ParamDescription { Name = FrameClassParam, ParamType = MavParamType.MavParamTypeInt32 },
            new ParamDescription { Name = FrameTypeParam, ParamType = MavParamType.MavParamTypeInt32 }
        };

        var heartbeatClient = new HeartbeatClient(identity, _heartbeatConfig, core);
        var commandClient = new CommandClient(identity, _commandConfig, core);
        var paramsClient = new ParamsClient(identity, _paramsClientConfig, core);
        var paramsClientEx = new ParamsClientEx(
            paramsClient,
            _paramsClientConfig,
            MavParamHelper.CStyleEncoding,
            paramsDescriptions);

        return CreateMotorTestClient(heartbeatClient, commandClient, paramsClientEx);
    }

    internal static Layout GetLayout(ArduFrameClass frameClass, ArduFrameType frameType)
    {
        return ArduPilotMotorsLayout.Layouts.Single(layout =>
            layout.Class == (int)frameClass &&
            layout.Type == (int)frameType);
    }

    protected async Task SendServoOutputRaw(Action<ServoOutputRawPayload> configurePayload)
    {
        var packet = new ServoOutputRawPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Sequence = ServerSeq.GetNextSequenceNumber()
        };
        
        configurePayload(packet.Payload);
        
        await Link.Server.Send(packet, Cts.Token);
    }

    [Fact]
    public async Task StartTest_NotAccepted_Fail()
    {
        // Arrange
        _ = Server;
        MotorTestCommandResult = CommandResult.FromResult(MavResult.MavResultDenied);
        await Client.Init(Cts.Token);
        var motor = Client.TestMotors[0];

        // Act
        var result = await motor.StartTest(50, 10, Cts.Token);

        // Assert
        Assert.Equal(MavResult.MavResultDenied, result);
        Assert.False(motor.IsTestRun.CurrentValue);
    }

    [Fact]
    public async Task StartTest_Canceled_Throws()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var motor = Client.TestMotors[0];
        var txBefore = Link.Client.Statistic.TxMessages;
        var rxBefore = Link.Server.Statistic.RxMessages;
        await Cts.CancelAsync();

        // Act
        var task = motor.StartTest(50, 10, Cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await task);
        Assert.False(motor.IsTestRun.CurrentValue);
        Assert.Equal(txBefore, Link.Client.Statistic.TxMessages);
        Assert.Equal(rxBefore, Link.Server.Statistic.RxMessages);
    }

    [Fact]
    public async Task Refresh_OnCall_ReadsBothFrameParams_Success()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var requestedParams = new List<string>();
        using var sub = Link.Server.OnRxMessage
            .FilterByType<ParamRequestReadPacket>()
            .Subscribe(p => requestedParams.Add(MavlinkTypesHelper.GetString(p.Payload.ParamId)));

        // Act
        await Client.Refresh(Cts.Token);

        // Assert
        Assert.Contains(FrameClassParam, requestedParams);
        Assert.Contains(FrameTypeParam, requestedParams);
    }

    [Fact]
    public async Task StartTest_TimeoutElapsed_ClearsIsTestRun_Success()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var motor = Client.TestMotors[0];
        const int timeoutSeconds = 5;
        await motor.StartTest(50, timeoutSeconds, Cts.Token);
        Assert.True(motor.IsTestRun.CurrentValue);

        // Act
        ClientTime.Advance(TimeSpan.FromSeconds(timeoutSeconds + 1));

        // Assert
        Assert.False(motor.IsTestRun.CurrentValue);
    }

    [Fact]
    public async Task StopTest_Canceled_Throws()
    {
        // Arrange
        _ = Server;
        await Client.Init(Cts.Token);
        var motor = Client.TestMotors[0];
        var txBefore = Link.Client.Statistic.TxMessages;
        var rxBefore = Link.Server.Statistic.RxMessages;
        await Cts.CancelAsync();

        // Act
        var task = motor.StopTest(Cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(txBefore, Link.Client.Statistic.TxMessages);
        Assert.Equal(rxBefore, Link.Server.Statistic.RxMessages);
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing)
            {
                Cts.Dispose();
            }

            base.Dispose(disposing);
        }
        finally
        {
            if (disposing)
            {
                _commandLongServerEx?.Dispose();
                _commandServer?.Dispose();
                _statusTextServer?.Dispose();
                _paramsServer?.Dispose();
            }
        }
    }
}
