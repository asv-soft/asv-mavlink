using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;

using DeepEqual.Syntax;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class MissionComplexTest : ComplexTestBase<MissionClient, MissionServer>
{
    private const int MaxCommandTimeoutMs = 1000;
    private const int MaxAttemptsToCallCount = 5;
    
    private readonly MissionClientConfig _clientConfig = new()
    {
        CommandTimeoutMs = MaxCommandTimeoutMs,
        AttemptToCallCount = MaxAttemptsToCallCount,
    };
    
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MissionClient _client;
    private readonly MissionServer _server;
    
    public MissionComplexTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override MissionServer CreateServer(MavlinkIdentity identity, ICoreServices core)
    { 
        return new MissionServer(identity, core);
    }

    protected override MissionClient CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new MissionClient(identity, _clientConfig, core);
    }

    #region MissionSetCurrent
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionSetCurrent_DifferentMissionItemsIds_Success(ushort missionItemsIndex)
    {
        // Arrange
        var called = 0;
        MissionSetCurrentPacket? packetFromClient = null;
        using var s1 = _server.OnMissionSetCurrent.SubscribeAwait(async (p, _) =>
            {
                called++;
                await _server.SendMissionCurrent(p.Payload.Seq);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionSetCurrentPacket;
        });
        
        // Act
        await _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionSetCurrentPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(missionItemsIndex, packetFromServer?.Payload.Seq);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionSetCurrent_Timeout_Throws(ushort missionItemsIndex)
    {
        // Arrange
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds((MaxCommandTimeoutMs * MaxAttemptsToCallCount * 2) + 1)
            );
        });
        
        // Act
        var task = _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(400)]
    [InlineData(9000)]
    public async Task MissionSetCurrent_SeveralCallsWithTheSameId_Success(int callsCount)
    {
        // Arrange
        var called = 0;
        ushort missionItemsIndex = 1234;
        var results = new List<MissionSetCurrentPacket>();
        var packetFromClientMany = new List<MissionSetCurrentPacket>();
        using var s1 = _server.OnMissionSetCurrent.SubscribeAwait(async (p, _) =>
            {
                called++;
                results.Add(p);
                await _server.SendMissionCurrent(missionItemsIndex);
                
                if (called >= callsCount)
                {
                    _taskCompletionSource.TrySetResult(p);
                }
            }
        );
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is MissionSetCurrentPacket packet)
            {
                packetFromClientMany.Add(packet);
            }
        });
        
        // Act
        for (var i = 0; i < callsCount; i++)
        {
            await _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token);
        }
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionSetCurrentPacket;
        Assert.Equal(callsCount, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClientMany.IsDeepEqual(results));
        Assert.Equal(called, results.Count);
        Assert.True(results.All(p => p.Payload.Seq == missionItemsIndex));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionSetCurrent_Canceled_Throws()
    {
        // Arrange
        await _cancellationTokenSource.CancelAsync();
        ushort missionItemsIndex = 1234;
        var called = 0;
        using var s1 = _server.OnMissionSetCurrent.SubscribeAwait(async (p, _) =>
            {
                called++;
                await _server.SendMissionCurrent(p.Payload.Seq);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        
        // Act
        var task = _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
    }

    #endregion

    #region MissionRequestCount
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionRequestCount_DifferentCountValues_Success(ushort count)
    {
        // Arrange
        var called = 0;
        MissionRequestListPacket? packetFromClient = null;
        using var s1 = _server.OnMissionRequestList.SubscribeAwait(async (p, _) => 
            {
                called++;
                await _server.SendMissionCount(count);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionRequestListPacket;
        });
        
        // Act
        var result = await _client.MissionRequestCount(_cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionRequestListPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(count, result);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionRequestCount_Timeout_Throws()
    {
        // Arrange
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds((MaxCommandTimeoutMs * MaxAttemptsToCallCount * 2) + 1)
            );
        });
        
        // Act
        var task = _client.MissionRequestCount(_cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
    }
    
    [Fact]
    public async Task MissionRequestCount_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        ushort count = 10;
        using var s1 = _server.OnMissionRequestList.SubscribeAwait(async (p, _) => 
            {
                called++;
                await _server.SendMissionCount(count);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        await _cancellationTokenSource.CancelAsync();
        
        // Act
        var task = _client.MissionRequestCount(_cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
    }
    
    #endregion

    #region MissionRequestItem

    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionRequestItem_DifferentIndexes_Success(ushort index)
    {
        // Arrange
        var called = 0;
        MissionRequestIntPacket? packetFromClient = null;
        var itemFormServer = new ServerMissionItem();
        using var s1 = _server.OnMissionRequestInt.SubscribeAwait(async (p, _) =>
            {
                called++;
                await _server.SendMissionItemInt(itemFormServer);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionRequestIntPacket;
        });
        
        // Act
        var result = await _client.MissionRequestItem(index, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionRequestIntPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.True(IsEqual(result, itemFormServer));
        Assert.Equal(index, packetFromServer?.Payload.Seq);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionRequestItem_Timeout_Throws(ushort index)
    {
        // Arrange
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds((MaxCommandTimeoutMs * MaxAttemptsToCallCount * 2) + 1)
            );
        });
        
        // Act
        var task = _client.MissionRequestItem(index, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
    }
    
    [Fact]
    public async Task MissionRequestItem_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        var itemFormServer = new ServerMissionItem();
        await _cancellationTokenSource.CancelAsync();
        using var s1 = _server.OnMissionRequestInt.SubscribeAwait(async (p, _) => 
            {
                called++;
                await _server.SendMissionItemInt(itemFormServer);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        
        // Act
        var task = _client.MissionRequestItem(5, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
    }

    #endregion

    #region WriteMissionItem

    [Fact]
    public async Task WriteMissionItem_DifferentIndexes_Success()
    {
        // Arrange
        var called = 0;
        MissionItemIntPacket? packetFromClient = null;
        var missionItem = new MissionItem(new MissionItemIntPayload());
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionItemIntPacket;
        });
        
        // Act
        await _client.WriteMissionItem(missionItem, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionItem_SendNull_Throws()
    {
        // Arrange
        var called = 0;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act + Assert
        await Assert.ThrowsAsync<NullReferenceException>(
            // ReSharper disable once NullableWarningSuppressionIsUsed
           async () => await _client.WriteMissionItem(null!, cancel:_cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionItem_OtherImplementation_Success()
    {
        // Arrange
        var called = 0;
        MissionItemPacket? packetFromClient = null;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionItemPacket;
        });
        
        // Act
        _ = _client.WriteMissionItem(
            10, 
            MavFrame.MavFrameGlobal, 
            MavCmd.MavCmdUser1, 
            true, 
            true,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            MavMissionType.MavMissionTypeAll,
            _cancellationTokenSource.Token
        );
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionItemPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task WriteMissionItem_Timeout_Throws(ushort index)
    {
        // Arrange
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds((MaxCommandTimeoutMs * MaxAttemptsToCallCount * 2) + 1)
            );
        });
        
        // Act
        var task = _client.MissionRequestItem(index, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
    }
    
    [Fact]
    public async Task WriteMissionItem_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        var missionItem = new MissionItem(new MissionItemIntPayload());
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await _client.WriteMissionItem(missionItem, cancel:_cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
    }

    #endregion
    
    #region WriteMissionIntItem
    
    [Fact]
    public async Task WriteMissionIntItem_DifferentIndexes_Success()
    {
        // Arrange
        var called = 0;
        MissionItemIntPacket? packetFromClient = null;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionItemIntPacket;
        });
        
        // Act
        await _client.WriteMissionIntItem(_ => { }, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public void WriteMissionIntItem_SendNull_Throws()
    {
        // Arrange
        var called = 0;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        
        // Act + Assert
        Assert.ThrowsAsync<NullReferenceException>(
            // ReSharper disable once NullableWarningSuppressionIsUsed
            async () => await _client.WriteMissionIntItem(null!, cancel:_cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionIntItem_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await _client.WriteMissionIntItem(_ => { }, cancel:_cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
    }
    
    #endregion
    
    #region ClearAll
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeAll)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    public async Task ClearAll_DifferentMissionTypes_Success(MavMissionType missionType)
    {
        // Arrange
        var called = 0;
        MissionClearAllPacket? packetFromClient = null;
        using var s1 = _server.OnMissionClearAll.SubscribeAwait(async (p, _) =>
        {
            called++;
            await _server.SendMissionAck(MavMissionResult.MavMissionAccepted);
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionClearAllPacket;
        });
        
        // Act
        await _client.ClearAll(missionType, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionClearAllPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(missionType, packetFromServer?.Payload.MissionType);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ClearAll_Timeout_Throws()
    {
        // Arrange
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds((MaxCommandTimeoutMs * MaxAttemptsToCallCount * 2) + 1)
            );
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<TimeoutException>(
            () => _client.ClearAll(cancel: _cancellationTokenSource.Token)
        );
    }
    
    [Fact]
    public async Task ClearAll_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = _server.OnMissionClearAll.SubscribeAwait(async (p, _) =>
        {
            called++;
            await _server.SendMissionAck(MavMissionResult.MavMissionAccepted);
            _taskCompletionSource.TrySetResult(p);
        });
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _client.ClearAll(MavMissionType.MavMissionTypeAll, _cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
    }
    
    #endregion

    #region MissionSetCount

    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionSetCount_DifferentMissionTypes_Success(ushort count)
    {
        // Arrange
        var called = 0;
        MissionCountPacket? packetFromClient = null;
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as MissionCountPacket;
        });
        
        // Act
        await _client.MissionSetCount(count, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionCountPacket;
        Assert.Equal(1, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(count, packetFromServer?.Payload.Count);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionSetCount_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await _client.MissionSetCount(10,cancel: _cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
    }

    #endregion

    #region MissionSendAck

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
        var tcs = new TaskCompletionSource<MissionAckPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => _taskCompletionSource.TrySetCanceled());
        
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = _server.OnMissionAck.Subscribe(p =>
        {
            called++;
            tcs.TrySetResult(p);
        });
        
        using var s2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _client.SendMissionAck(missionResult,cancel: cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer?.Payload.IsDeepEqual(result));
    }

    #endregion
    
    #region RequestMissionItem
    
    [Theory(Timeout = 10000)]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task RequestMissionItem_DifferentIds_Success(ushort id)
    {
        // Arrange
        var called = 0;
        var tcs = new TaskCompletionSource<MissionRequestPayload>();
        var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        using var s1 = _client.OnMissionRequest.SubscribeAwait(async (p, _) =>
        {
            called++;
            var item = new MissionItem(new MissionItemIntPayload()
            {
                Seq = id,
                MissionType = MavMissionType.MavMissionTypeAll
            });
            
            await _client.WriteMissionItem(item, default);
            tcs.TrySetResult(p);
        });
        
        // Act
        var result = await _server.RequestMissionItem(
            id, 
            MavMissionType.MavMissionTypeAll, 
            cancel: cancel.Token
        );

        // Assert
        var resultFormClient = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(resultFormClient.Seq, result.Seq);
        Assert.Equal(resultFormClient.MissionType, result.MissionType);
    }
    
    [Theory(Timeout = 10000)]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeAll)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    public async Task RequestMissionItem_DifferentMissionTypes_Success(MavMissionType missionType)
    {
        // Arrange
        var called = 0;
        var id = (ushort) 10;
        var tcs = new TaskCompletionSource<MissionRequestPayload>();
        var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        using var s1 = _client.OnMissionRequest.SubscribeAwait(async (p, _) =>
        {
            called++;
            var item = new MissionItem(new MissionItemIntPayload()
            {
                Seq = id,
                MissionType = missionType
            });
            
            await _client.WriteMissionItem(item, default);
            tcs.TrySetResult(p);
        });
        
        // Act
        var result = await _server.RequestMissionItem(
            id,
            missionType,
            cancel: cancel.Token
        );

        // Assert
        var resultFormClient = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(resultFormClient.Seq, result.Seq);
        Assert.Equal(resultFormClient.MissionType, result.MissionType);
    }
    
    [Fact]
    public async Task RequestMissionItem_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        await cancel.CancelAsync();
        
        using var s1 = _client.OnMissionRequest.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        var task = _server.RequestMissionItem(
            10,
            MavMissionType.MavMissionTypeAll,
            cancel: cancel.Token
        );

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
    }
    
    [Fact]
    public async Task RequestMissionItem_Timeout_Throws()
    {
        // Arrange
        using var s1 = Link.Server.OnTxMessage.Subscribe(_ =>
        {
            ServerTime.Advance(
                TimeSpan.FromMilliseconds((MaxAttemptsToCallCount * MaxCommandTimeoutMs * 2) + 1)
            );
        });
        
        // Act
        var task = _server.RequestMissionItem(
            10,
            MavMissionType.MavMissionTypeAll,
            cancel: _cancellationTokenSource.Token
        );

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(0, (int)Link.Server.Statistic.RxMessages);
    }
    
    #endregion
    
    #region Utils
    
    private static bool IsEqual(MissionItemIntPayload? payload, ServerMissionItem? item)
    {
        if (payload is null && item is null)
        {
            return true;
        }

        return payload?.Seq == item?.Seq
               && payload?.Autocontinue == item?.Autocontinue
               && payload?.Command == item?.Command
               && payload?.Frame == item?.Frame
               && IsEqualFloat(payload?.Param1, item?.Param1)
               && IsEqualFloat(payload?.Param2, item?.Param2)
               && IsEqualFloat(payload?.Param3, item?.Param3)
               && IsEqualFloat(payload?.Param4, item?.Param4)
               && payload?.X == item?.X
               && payload?.Y == item?.Y
               && IsEqualFloat(payload?.Z, item?.Z)
               && payload?.MissionType == item?.MissionType;
    }

    private static bool IsEqualFloat(float? left, float? right)
    {
        if (left is not null && right is not null)
        {
            return Math.Abs(left.Value - right.Value) <= 0.00001;
        }

        if (left is null && right is null)
        {
            return true;
        }

        return false;
    }
    
    #endregion
}