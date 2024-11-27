using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(StatusTextClient))]
[TestSubject(typeof(StatusTextServer))]
public class StatusTextComplexTest : ComplexTestBase<StatusTextClient, StatusTextServer>, IDisposable
{
    private readonly TaskCompletionSource<StatusMessage> _tcs;
    private readonly CancellationTokenSource _cts;

    private readonly StatusTextLoggerConfig _serverConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 10
    };

    public StatusTextComplexTest(ITestOutputHelper log) : base(log)
    {
        _tcs = new TaskCompletionSource<StatusMessage>();
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }

    protected override StatusTextServer CreateServer(MavlinkIdentity identity, ICoreServices core) =>
        new(identity, _serverConfig, core);

    protected override StatusTextClient CreateClient(MavlinkClientIdentity identity, ICoreServices core) =>
        new(identity, core);

    [Fact]
    public void OnMessage_Received_Success()
    {
        // Arrange
        const string serverMessage = "Test message";
        var clientMessage = string.Empty;
        using var sub = Client.OnMessage.Subscribe(p =>
        {
            clientMessage = p.Text;
            _tcs.TrySetResult(p);
        });

        // Act
        Server.Log(MavSeverity.MavSeverityCritical, serverMessage);
        ServerTime.Advance(TimeSpan.FromMilliseconds(200));

        // Assert
        Assert.Equal(serverMessage, clientMessage);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}