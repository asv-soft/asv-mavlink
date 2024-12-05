using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using R3;
using Xunit;
using Xunit.Abstractions;

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

    public AsvRsgaComplexTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<AsvRsgaCompatibilityResponsePayload>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AsvRsgaServerEx CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        var status = new StatusTextServer(identity, _statusConfig, core);
        var command = new CommandLongServerEx(new CommandServer(identity, core));
        return new AsvRsgaServerEx(new AsvRsgaServer(identity, core), status, command);
    }

    protected override AsvRsgaClientEx CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new AsvRsgaClientEx(new AsvRsgaClient(identity, core), new CommandClient(identity, _commandConfig, core));
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

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}