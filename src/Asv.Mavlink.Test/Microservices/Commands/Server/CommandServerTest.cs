using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(CommandServer))]
public class CommandServerTest : ServerTestBase<CommandServer>
{
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CommandServer _server;

    public CommandServerTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());

    }
    
    protected override CommandServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);

    [Theory]
    [InlineData(MavCmd.MavCmdUser1)]
    [InlineData(MavCmd.MavCmdUser2)]
    [InlineData(MavCmd.MavCmdCanForward)]
    [InlineData(MavCmd.MavCmdNavFencePolygonVertexInclusion)]
    public async Task SendCommandAck_DifferentMavCmdMavResultAccepted_Success(MavCmd cmd)
    {
        // Arrange
        var called = 0;
        CommandAckPacket? packetFromClient = null;
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Server.OnTxMessage.Subscribe(p =>
            {
                packetFromClient = p as CommandAckPacket; 
            }
        );

        // Act
        await _server.SendCommandAck(
            cmd,
            new DeviceIdentity(Identity.SystemId, Identity.ComponentId),
            new CommandResult(MavResult.MavResultAccepted),
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandAckPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(MavResult.MavResultAccepted)]
    [InlineData(MavResult.MavResultCancelled)]
    [InlineData(MavResult.MavResultDenied)]
    [InlineData(MavResult.MavResultFailed)]
    [InlineData(MavResult.MavResultInProgress)]
    [InlineData(MavResult.MavResultUnsupported)]
    [InlineData(MavResult.MavResultTemporarilyRejected)]
    [InlineData(MavResult.MavResultCommandIntOnly)]
    [InlineData(MavResult.MavResultCommandLongOnly)]
    [InlineData(MavResult.MavResultCommandUnsupportedMavFrame)]
    public async Task SendCommandAck_DifferentMavResults_Success(MavResult mavRes)
    {
        // Arrange
        var called = 0;
        CommandAckPacket? packetFromClient = null;
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Server.OnTxMessage.Subscribe(p =>
            {
                packetFromClient = p as CommandAckPacket; 
            }
        );

        // Act
        await _server.SendCommandAck(
            MavCmd.MavCmdUser1,
            new DeviceIdentity(Identity.SystemId, Identity.ComponentId),
            new CommandResult(mavRes),
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandAckPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public async Task SendCommandAck_DifferentIntResultValues_Success(int res)
    {
        // Arrange
        var called = 0;
        CommandAckPacket? packetFromClient = null;
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Server.OnTxMessage.Subscribe(p =>
            {
                packetFromClient = p as CommandAckPacket; 
            }
        );

        // Act
        await _server.SendCommandAck(
            MavCmd.MavCmdUser1,
            new DeviceIdentity(Identity.SystemId, Identity.ComponentId),
            new CommandResult(MavResult.MavResultAccepted, res),
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandAckPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public async Task SendCommandAck_DifferentSecondIntResultValues_Success(int res2)
    {
        // Arrange
        var called = 0;
        CommandAckPacket? packetFromClient = null;
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Server.OnTxMessage.Subscribe(p =>
            {
                packetFromClient = p as CommandAckPacket; 
            }
        );

        // Act
        await _server.SendCommandAck(
            MavCmd.MavCmdUser1,
            new DeviceIdentity(Identity.SystemId, Identity.ComponentId),
            new CommandResult(MavResult.MavResultAccepted, resultParam2: res2),
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandAckPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact] 
    public async Task SendCommandAck_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await _cancellationTokenSource.CancelAsync();
        
        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            async () => await  _server.SendCommandAck(
                MavCmd.MavCmdUser1,
                new DeviceIdentity(Identity.SystemId, Identity.ComponentId),
                new CommandResult(MavResult.MavResultAccepted),
                _cancellationTokenSource.Token
            )
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }
}