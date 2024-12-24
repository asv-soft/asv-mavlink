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

[TestSubject(typeof(MissionClient))]
public class MissionClientTest : ClientTestBase<MissionClient>
{
    private readonly MissionClientConfig _config = new()
    {
        CommandTimeoutMs = 1000,
        AttemptToCallCount = 5
    };
    
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MissionClient _client;

    public MissionClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override MissionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) 
        => new(identity, _config, core);

    [Fact]
    public void Constructor_Null_Throws()
    {
        // ReSharper disable once NullableWarningSuppressionIsUsed
        Assert.Throws<ArgumentNullException>(() => new MissionClient(null!, _config, Context));
        // ReSharper disable once NullableWarningSuppressionIsUsed
        Assert.Throws<ArgumentNullException>(() => new MissionClient(Identity, null!, Context));
        // ReSharper disable once NullableWarningSuppressionIsUsed
        Assert.Throws<ArgumentNullException>(() => new MissionClient(Identity, _config, null!));
    }
    
    [Theory]
    [InlineData(MavMissionResult.MavMissionAccepted)]
    [InlineData(MavMissionResult.MavMissionError)]
    [InlineData(MavMissionResult.MavMissionUnsupportedFrame)]
    [InlineData(MavMissionResult.MavMissionUnsupported)]
    [InlineData(MavMissionResult.MavMissionNoSpace)]
    [InlineData(MavMissionResult.MavMissionInvalid)]
    [InlineData(MavMissionResult.MavMissionInvalidParam1)]
    [InlineData(MavMissionResult.MavMissionInvalidParam2)]
    [InlineData(MavMissionResult.MavMissionInvalidParam3)]
    [InlineData(MavMissionResult.MavMissionInvalidParam4)]
    [InlineData(MavMissionResult.MavMissionInvalidParam5X)]
    [InlineData(MavMissionResult.MavMissionInvalidParam6Y)]
    [InlineData(MavMissionResult.MavMissionInvalidParam7)]
    [InlineData(MavMissionResult.MavMissionInvalidSequence)]
    [InlineData(MavMissionResult.MavMissionDenied)]
    [InlineData(MavMissionResult.MavMissionOperationCancelled)]
    public async Task SendMissionAck_DifferentResults_Success(MavMissionResult missionResult)
    {
        // Arrange
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _client.SendMissionAck(missionResult, cancel: _cancellationTokenSource.Token);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeAll)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    [InlineData(null)]
    public async Task SendMissionAck_DifferentTypes_Success(MavMissionType? missionType)
    {
        // Arrange
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _client.SendMissionAck(MavMissionResult.MavMissionAccepted, type: missionType);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SendMissionAck_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = Link.Server.OnRxMessage.Subscribe(_ =>
        {
            called++;
        });

        var task = _client.SendMissionAck(
            MavMissionResult.MavMissionAccepted,
            cancel: _cancellationTokenSource.Token,
            type: MavMissionType.MavMissionTypeMission
        );
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => 
            await task
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionSetCurrent_Timeout_Throws(ushort missionItemsIndex)
    {
        // Arrange
        var called = 0;
        Link.SetServerToClientFilter(_ => false);
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionRequestCount_Timeout_Throws()
    {
        // Arrange
        var called = 0;
        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.MissionRequestCount(_cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionRequestItem_Timeout_Throws(ushort index)
    {
        // Arrange
        var called = 0;
        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.MissionRequestItem(index, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task WriteMissionItem_Timeout_Throws(ushort index)
    {
        // Arrange
        var called = 0;
        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.MissionRequestItem(index, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ClearAll_Timeout_Throws()
    {
        // Arrange
        var called = 0;
        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.ClearAll(cancel: _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
}