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
using NullReferenceException = System.NullReferenceException;

namespace Asv.Mavlink.Test.Server;

[TestSubject(typeof(MissionServer))]
public class MissionServerTest : ServerTestBase<MissionServer>
{
    private const int DefaultMaxAttempts = 5; // from default value in InternalCall
    private const int DefaultTimeoutMs = 1000; // from default value in InternalCall
    
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MissionServer _server;
    
    public MissionServerTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MissionServer(null!, Core));
        Assert.Throws<ArgumentNullException>(() => new MissionServer(Identity, null!));
    }
    
    [Theory]
    [InlineData(MavMissionResult.MavMissionAccepted)]
    [InlineData(MavMissionResult.MavMissionError)]
    [InlineData(MavMissionResult.MavMissionNoSpace)]
    [InlineData(MavMissionResult.MavMissionOperationCancelled)]
    public async Task SendMissionAck_DifferentResults_Success(MavMissionResult missionResult)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _server.SendMissionAck(missionResult);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int)Link.Client.Statistic.RxMessages);
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
        const int expectedCalls = 1;
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _server.SendMissionAck(MavMissionResult.MavMissionAccepted, type: missionType);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task SendMissionCount_DifferentCount_Success(ushort count)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionCountPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionCountPacket;
        });

        
        // Act
        await _server.SendMissionCount(count);

        // Assert
        var result = await _taskCompletionSource.Task as MissionCountPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task SendReached_DifferentSeq_Success(ushort seq)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionItemReachedPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionItemReachedPacket;
        });
        
        // Act
        await _server.SendReached(seq, CancellationToken.None);

        // Assert
        var result = await _taskCompletionSource.Task as MissionItemReachedPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }

    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task SendMissionCurrent_DifferentCurrentValues_Success(ushort current)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionCurrentPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionCurrentPacket;
        });
        
        // Act
        await _server.SendMissionCurrent(current);

        // Assert
        var result = await _taskCompletionSource.Task as MissionCurrentPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SendMissionItemInt_EmptyMissionItem_Success()
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        var serverItem = new ServerMissionItem();
        MissionItemIntPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionItemIntPacket;
        });
        
        // Act
        await _server.SendMissionItemInt(serverItem);

        // Assert
        var result = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SendMissionItemInt_NullServerMissionItem_Throws()
    {
        // Arrange
        var called = 0;
        using var s = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
        });
        
        // Act + Assert
        // ReSharper disable once NullableWarningSuppressionIsUsed
        await Assert.ThrowsAsync<NullReferenceException>(
            async () => await _server.SendMissionItemInt(null!)
        );
        
        Assert.Equal(0, called);
    }
    
    [Theory]
    [InlineData(
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        int.MaxValue,
        int.MaxValue,
        int.MaxValue,
        ushort.MaxValue,
        MavCmd.MavCmdNavRoi,
        MavFrame.MavFrameReserved16,
        byte.MaxValue,
        MavMissionType.MavMissionTypeAll
    )]
    [InlineData(
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        int.MinValue,
        int.MinValue,
        int.MinValue,
        ushort.MinValue,
        MavCmd.MavCmdAirframeConfiguration,
        MavFrame.MavFrameGlobalRelativeAlt,
        byte.MinValue,
        MavMissionType.MavMissionTypeRally
    )]
    [InlineData(
        float.MaxValue,
        21.3f,
        float.MaxValue,
        float.MinValue,
        int.MinValue,
        int.MaxValue,
        int.MinValue,
        ushort.MaxValue,
        MavCmd.MavCmdDoGripper,
        MavFrame.MavFrameLocalOffsetNed,
        byte.MaxValue,
        MavMissionType.MavMissionTypeAll
    )]
    public async Task SendMissionItemInt_DifferentServerMissionItems_Success
    (
        float param1,
        float param2,
        float param3,
        float param4,
        int x,
        int y,
        int z,
        ushort seq,
        MavCmd command,
        MavFrame frame,
        byte autoContinue,
        MavMissionType missionType
    )
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        var serverItem = new ServerMissionItem()
        {
            Param1 = param1, 
            Param2 = param2, 
            Param3 = param3,
            Param4 = param4, 
            X = x,
            Y = y,
            Z = z, 
            Seq = seq, 
            Command = command, 
            Frame = frame,
            Autocontinue = autoContinue, 
            MissionType = missionType,
        };
        MissionItemIntPacket? packetFromServer = null;
        
        using var s1 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionItemIntPacket;
        });
        
        // Act
        await _server.SendMissionItemInt(serverItem);

        // Assert
        var result = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task RequestMissionItem_Timeout_Throws()
    {
        // Arrange
        var called = 0;
        Link.SetClientToServerFilter(_ => false);
        using var s1 = Link.Server.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            ServerTime.Advance(TimeSpan.FromMilliseconds(DefaultTimeoutMs + 1));
        });
        
        // Act
        var task = _server.RequestMissionItem(
            10,
            MavMissionType.MavMissionTypeAll,
            cancel: _cancellationTokenSource.Token
        );

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(DefaultMaxAttempts, called);
        Assert.Equal(0, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
    }
    
    protected override MissionServer CreateServer(MavlinkIdentity identity, CoreServices core) => new(identity, core);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Cancel();
        }
        
        base.Dispose(disposing);
    }
}