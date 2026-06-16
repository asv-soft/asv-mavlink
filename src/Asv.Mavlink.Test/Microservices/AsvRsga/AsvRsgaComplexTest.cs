using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

public class AsvRsgaComplexTest : ComplexTestBase<AsvRsgaClientEx, AsvRsgaServerEx>, IDisposable
{
    private readonly TaskCompletionSource<AsvRsgaCompatibilityResponsePayload> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly AsvRsgaClientEx _client;
    private readonly AsvRsgaServerEx _server;

    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30,
    };
    
    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    private HeartbeatClientConfig _heartbeatConfig = new();

    public AsvRsgaComplexTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<AsvRsgaCompatibilityResponsePayload>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override AsvRsgaServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var status = new StatusTextServer(identity, _statusConfig, core);
        var command = new CommandLongServerEx(new CommandServer(identity, core));
        return new AsvRsgaServerEx(new AsvRsgaServer(identity, core), command);
    }

    protected override AsvRsgaClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new AsvRsgaClientEx(new AsvRsgaClient(identity, core), new CommandClient(identity, _commandConfig, core), new HeartbeatClient(identity, _heartbeatConfig, core));
    }

    [Fact]
    public async Task RefreshInfo_WhenCalled_ShouldUpdateAvailableModes()
    {
        // Arrange
        Assert.Empty(Client.AvailableModes);
        using var sub3 = Client.Base.OnCompatibilityResponse.Subscribe(_ =>
        {
            if (_ is not null)
                _taskCompletionSource.TrySetResult(_);
        });
        
        // Act
        await Client.RefreshInfo(_cancellationTokenSource.Token);
        
        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.NotNull(res);
        Assert.True(res.Result == AsvRsgaRequestAck.AsvRsgaRequestAckOk);
        Assert.Equal(1, res.RequestId);
        Assert.NotEmpty(Client.AvailableModes);
    }
    
    [Fact]
    public async Task SetMode_WhenCalledAfterRefreshInfo_ShouldUpdateAvailableModesAndRespondSuccessfully()
    {
        // Arrange
        using var sub = Client.Base.OnCompatibilityResponse.Subscribe(_ =>
        {
            if (_ is not null)
                _taskCompletionSource.TrySetResult(_);
        });
        
        // Act
        await Client.RefreshInfo(_cancellationTokenSource.Token);

        await Client.SetMode(AsvRsgaCustomMode.AsvRsgaCustomModeIdle, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.NotNull(res);
        Assert.True(res.Result == AsvRsgaRequestAck.AsvRsgaRequestAckOk);
        Assert.Equal(1, res.RequestId);
        Assert.NotEmpty(Client.AvailableModes);
    }

    [Fact]
    public async Task StartRecord_WhenServerAcceptsCommand_ShouldReturnAccepted()
    {
        // Arrange
        const string expectedRecordName = "Test_Record";
        var called = 0;
        Server.StartRecord = (name, cancel) =>
        {
            Assert.False(cancel.IsCancellationRequested);
            Assert.Equal(expectedRecordName, name);
            called++;
            return Task.FromResult(MavResult.MavResultAccepted);
        };

        // Act
        var result = await Client.StartRecord(expectedRecordName, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(MavResult.MavResultAccepted, result);
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task StopRecord_WhenServerAcceptsCommand_ShouldReturnAccepted()
    {
        // Arrange
        var called = 0;
        Server.StopRecord = cancel =>
        {
            Assert.False(cancel.IsCancellationRequested);
            called++;
            return Task.FromResult(MavResult.MavResultAccepted);
        };

        // Act
        var result = await Client.StopRecord(_cancellationTokenSource.Token);

        // Assert
        Assert.Equal(MavResult.MavResultAccepted, result);
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task SendChart_WhenCalled_ShouldPublishDecodedFrameOnClient()
    {
        // Arrange
        var timestamp = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var tcs = new TaskCompletionSource<RsgaChartFrame>();
        using var sub = Client.ChartFrames.Subscribe(tcs.SetResult);

        // Act
        await Server.SendChart(
            new[] { 10.0, 20.0, 30.0, 40.0, 50.0 },
            new RsgaChartSendOptions
            {
                ChartType = AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeYChannel,
                Encoding = RsgaChartEncoding.Float,
                Resampling = RsgaChartResampling.Linear,
                MaxSamples = 3,
                DataIndex = 12,
                Timestamp = timestamp,
            },
            _cancellationTokenSource.Token
        );

        // Assert
        var frame = await tcs.Task;
        Assert.Equal(AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeYChannel, frame.ChartType);
        Assert.Equal(AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat, frame.Format);
        Assert.Equal(12u, frame.DataIndex);
        Assert.Equal(new[] { 10.0, 30.0, 50.0 }, frame.Values);
        Assert.Single(Client.ChartSources);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
