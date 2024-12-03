using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaClientEx))]
public class AsvRsgaClientExTest : ClientTestBase<AsvRsgaClientEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly AsvRsgaClientEx _client;

    private readonly CommandProtocolConfig _commandCore = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30,
    };

    protected override AsvRsgaClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AsvRsgaClientEx(new AsvRsgaClient(identity, core), new CommandClient(identity, _commandCore, core));
    }

    public AsvRsgaClientExTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
    }

    [Fact]
    public void Constructor_Null_Throws()
    {
        // Act
        Assert.Throws<NullReferenceException>(() =>
            new AsvRsgaClientEx(null!, new CommandClient(Client.Base.Identity, _commandCore, Client.Base.Core)));
        Assert.Throws<ArgumentNullException>(() =>
            new AsvRsgaClientEx(new AsvRsgaClient(Client.Base.Identity, Client.Base.Core), null!));
    }

    [Fact]
    public async Task ReadOnce_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var attempts = 5;
        var timeout = 1000;

        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.RefreshInfo(_cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() => { Time.Advance(TimeSpan.FromMilliseconds((timeout * attempts) + 1)); });

        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(attempts, Link.Client.TxPackets);
    }

    [Fact]
    public async Task SetMode_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var attempts = 5;
        var timeout = 1000;

        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.SetMode(AsvRsgaCustomMode.AsvRsgaCustomModeIdle, _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() => { Time.Advance(TimeSpan.FromMilliseconds((timeout * attempts) + 1)); });

        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(attempts, Link.Client.TxPackets);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}