using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Common;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Asv.Mavlink.Test;

public class AsvSdrExComplexTests : ComplexTestBase<AsvSdrClientEx, AsvSdrServerEx>
{
    private static ITestOutputHelper _log = new TestOutputHelper();

    private readonly HeartbeatClientConfig _configHeartbeat = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 3,
        PrintStatisticsToLogDelayMs = 10_000,
        PrintLinkStateToLog = true
    };

    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    private readonly StatusTextLoggerConfig _configStatus = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };

    private readonly MavlinkHeartbeatServerConfig _configHeartbeatServer = new()
    {
        HeartbeatRateMs = 1000,
    };

    private readonly AsvSdrServerConfig _configServerSdr = new()
    {
        StatusRateMs = 1000,
    };

    private readonly AsvSdrClientExConfig _configClientSdr = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000
    };

    public AsvSdrExComplexTests(ITestOutputHelper log) : base(log)
    {
    }

    protected override AsvSdrServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var sdr = new AsvSdrServer(identity, _configServerSdr, core);
        var statusText = new StatusTextServer(identity, _configStatus, core);
        var heartbeat = new HeartbeatServer(identity, _configHeartbeatServer, core);
        var commandsEx = new CommandLongServerEx(new CommandServer(identity, core));
        return new AsvSdrServerEx(sdr, statusText, heartbeat, commandsEx);
    }

    protected override AsvSdrClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new AsvSdrClientEx(new AsvSdrClient(identity, core),
            new HeartbeatClient(identity, _configHeartbeat, core), new CommandClient(identity, _commandConfig, core),
            _configClientSdr);
    }

    [Fact]
    public async Task Client_StartRecord_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.StartRecord += async (_, _) => MavResult.MavResultAccepted;
        var result = await Client.StartRecord("TestRecord", cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }

    [Fact]
    public async Task Client_StopRecord_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.StopRecord += async _ => MavResult.MavResultAccepted;
        var result = await Client.StopRecord(cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }

    [Fact]
    public async Task Client_StartCalibration_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.StartCalibration += async _ => MavResult.MavResultAccepted;
        var result = await Client.StartCalibration(cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }

    [Fact]
    public async Task Client_StopCalibration_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.StopCalibration += async _ => MavResult.MavResultAccepted;
        var result = await Client.StopCalibration(cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }

    [Fact]
    public async Task Client_SetMode_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.SetMode += async (mode, hz, rate, ratio, power, token) =>
        {
            if (token.IsCancellationRequested) return MavResult.MavResultDenied;
            Assert.True(mode == AsvSdrCustomMode.AsvSdrCustomModeGp);
            Assert.Equal((ulong)100, hz);
            Assert.Equal((float)100, rate);
            Assert.Equal((float)100, power);
            Assert.Equal((uint)100, ratio);
            Server.CustomMode.Value = AsvSdrCustomMode.AsvSdrCustomModeGp;
            return MavResult.MavResultAccepted;

        };
        var result = await Client.SetMode(AsvSdrCustomMode.AsvSdrCustomModeGp, 100, 100, 100, 100, cancel.Token);
        Assert.Equal(AsvSdrCustomMode.AsvSdrCustomModeGp, Server.CustomMode.CurrentValue);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }

    [Fact]
    public async Task Client_StartMission_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.StartMission += async (index, token) => cancel.IsCancellationRequested == false ? MavResult.MavResultAccepted : MavResult.MavResultDenied;
        var result = await Client.StartMission(0,cancel.Token);
        Assert.True(Server.);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }
    [Fact]
    public async Task Client_StopMission_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.StopMission += async (token) => token.IsCancellationRequested == false ? MavResult.MavResultAccepted : MavResult.MavResultDenied;
        var result = await Client.StopMission(cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }
    [Fact]
    public async Task Client_SystemControlAction_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.SystemControlAction +=async (action, token) => token.IsCancellationRequested ? MavResult.MavResultDenied : MavResult.MavResultAccepted;
        var result = await Client.SystemControlAction(AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot, cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }
    
    [Fact]
    public async Task Client_SystemControAction_Success()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server.SystemControlAction += async (action, token) => token.IsCancellationRequested ? MavResult.MavResultDenied : MavResult.MavResultAccepted;
        var result = await Client.SystemControlAction(AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot, cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }
    
    
    
    
    
}