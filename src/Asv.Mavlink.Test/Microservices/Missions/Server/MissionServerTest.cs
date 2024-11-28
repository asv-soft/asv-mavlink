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
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MissionServer _server;
    
    public MissionServerTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1000), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    protected override MissionServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
    
    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MissionServer(null!, Core));
        Assert.Throws<ArgumentNullException>(() => new MissionServer(Identity, null!));
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
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _server.SendMissionAck(missionResult);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(1, called);
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
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _server.SendMissionAck(MavMissionResult.MavMissionAccepted, type: missionType);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(1, called);
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
        var called = 0;
        MissionCountPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionCountPacket;
        });

        
        // Act
        await _server.SendMissionCount(count);

        // Assert
        var result = await _taskCompletionSource.Task as MissionCountPacket;
        Assert.Equal(1, called);
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
        var called = 0;
        MissionItemReachedPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionItemReachedPacket;
        });
        
        // Act
        await _server.SendReached(seq);

        // Assert
        var result = await _taskCompletionSource.Task as MissionItemReachedPacket;
        Assert.Equal(1, called);
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
        var called = 0;
        MissionCurrentPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionCurrentPacket;
        });
        
        // Act
        await _server.SendMissionCurrent(current);

        // Assert
        var result = await _taskCompletionSource.Task as MissionCurrentPacket;
        Assert.Equal(1, called);
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
        var called = 0;
        var serverItem = new ServerMissionItem();
        MissionItemIntPacket? packetFromServer = null;
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionItemIntPacket;
        });
        
        // Act
        await _server.SendMissionItemInt(serverItem);

        // Assert
        var result = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SendMissionItemInt_NullServerMissionItem_Success()
    {
        // Arrange
        var called = 0;
        using var s = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        
        // Act + Assert
        // ReSharper disable once NullableWarningSuppressionIsUsed
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _server.SendMissionItemInt(null!));
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
        
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionItemIntPacket;
        });
        
        // Act
        await _server.SendMissionItemInt(serverItem);

        // Assert
        var result = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
}