using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;

using DeepEqual.Syntax;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class MissionComplexTest : ComplexTestBase<MissionClient, MissionServer>
{
    private const int MaxCommandTimeoutMs = 1400;
    private const int MaxAttemptsToCallCount = 4;
    
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
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    #region MissionSetCurrent
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(2)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionSetCurrent_DifferentMissionItemIds_Success(ushort missionItemsIndex)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionSetCurrentPacket? packetFromClient = null;
        using var s1 = _server.OnMissionSetCurrent.SubscribeAwait(
            async (p, ct) => 
            {
                called++;
                await _server.SendMissionCurrent(p.Payload.Seq, ct);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionSetCurrentPacket;
        });
        
        // Act
        await _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionSetCurrentPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(missionItemsIndex, packetFromServer?.Payload.Seq);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedCalls, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(400)]
    [InlineData(9_000)]
    public async Task MissionSetCurrent_SeveralCallsWithTheSameId_Success(int callsCount)
    {
        // Arrange
        var called = 0;
        ushort missionItemsIndex = 1234;
        var results = new List<MissionSetCurrentPacket>();
        var packetFromClientMany = new List<MissionSetCurrentPacket>();
        using var s1 = _server.OnMissionSetCurrent.SubscribeAwait(
            async (p, ct) =>
            {
                called++;
                results.Add(p);
                await _server.SendMissionCurrent(missionItemsIndex, ct);
                
                if (called >= callsCount)
                {
                    _taskCompletionSource.TrySetResult(p);
                }
            }
        );
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            if (p is not MissionSetCurrentPacket packet)
            {
                throw new Exception($"Packet is not {typeof(MissionSetCurrentPacket)}");
            }
            
            packetFromClientMany.Add(packet);
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
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(callsCount, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionSetCurrent_Canceled_Throws()
    {
        // Arrange
        await _cancellationTokenSource.CancelAsync();
        ushort missionItemsIndex = 1234;
        var called = 0;
        using var s1 = _server.OnMissionSetCurrent.SubscribeAwait(
            async (p, ct) => 
            {
                called++;
                await _server.SendMissionCurrent(p.Payload.Seq, ct);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        
        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await _client.MissionSetCurrent(missionItemsIndex, _cancellationTokenSource.Token));
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
        const int expectedCalls = 1;
        var called = 0;
        MissionRequestListPacket? packetFromClient = null;
        using var s1 = _server.OnMissionRequestList.SubscribeAwait(
            async (p, _) => 
            {
                called++;
                await _server.SendMissionCount(count);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionRequestListPacket;
        });
        
        // Act
        var result = await _client.MissionRequestCount(_cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionRequestListPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(count, result);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedCalls, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionRequestCount_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        ushort count = 10;
        using var s1 = _server.OnMissionRequestList.SubscribeAwait(
            async (p, _) => 
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
        const int expectedCalls = 1;
        var called = 0;
        MissionRequestIntPacket? packetFromClient = null;
        var itemFormServer = new ServerMissionItem
        {
            Seq = 12,
            Command = MavCmd.MavCmdAirframeConfiguration,
        };
        using var s1 = _server.OnMissionRequestInt.SubscribeAwait(
            async (p, ct) =>
            {
                called++;
                await _server.SendMissionItemInt(itemFormServer, cancel: ct);
                _taskCompletionSource.TrySetResult(p);
            }
        );
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionRequestIntPacket;
        });
        
        // Act
        var result = await _client.MissionRequestItem(index, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionRequestIntPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.True(IsEqual(result, itemFormServer));
        Assert.Equal(index, packetFromServer.Payload.Seq);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedCalls, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionRequestItem_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        var itemFormServer = new ServerMissionItem();
        await _cancellationTokenSource.CancelAsync();
        using var s1 = _server.OnMissionRequestInt.SubscribeAwait(
            async (p, ct) => 
            {
                called++;
                await _server.SendMissionItemInt(itemFormServer, cancel: ct);
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
    public async Task WriteMissionItem_SinglePacket_Success()
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionItemIntPacket? packetFromClient = null;
        var payload = new MissionItemIntPayload
        {
            Seq = 12,
            Command = MavCmd.MavCmdAirframeConfiguration,
        };
        var missionItem = new MissionItem(payload);
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionItemIntPacket;
        });
        
        // Act
        await _client.WriteMissionItem(missionItem, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionItem_SendNull_Throws()
    {
        // Arrange
        var called = 0;
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
        });

        // Act + Assert
        await Assert.ThrowsAsync<NullReferenceException>(
            // ReSharper disable once NullableWarningSuppressionIsUsed
           async () => await _client.WriteMissionItem(null!, cancel:_cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int) Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionItem_ManyArgumentsImplementation_Success()
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionItemPacket? packetFromClient = null;
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionItemPacket;
        });
        
        // Act
        await _client.WriteMissionItem(
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
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionItem_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        var missionItem = new MissionItem(new MissionItemIntPayload());
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        
        // Act
        var task = _client.WriteMissionItem(missionItem, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await task
        );
        Assert.Equal(0, called);
    }

    #endregion
    
    #region WriteMissionIntItem
    
    [Fact]
    public async Task WriteMissionIntItem_EmptyFillCallback_Success()
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionItemIntPacket? packetFromClient = null;
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionItemIntPacket;
        });
        
        // Act
        await _client.WriteMissionIntItem(_ => { }, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionIntItem_SinglePacket_Success()
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionItemIntPacket? packetFromClient = null;
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionItemIntPacket;
        });
        
        // Act
        await _client.WriteMissionIntItem(p =>
        {
            p.Seq = 12;
            p.Param2 = 10;
        }, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionItemIntPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionIntItem_NullFillCallback_Throws()
    {
        // Arrange
        var called = 0;
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
        });

        // Act + Assert
        await Assert.ThrowsAsync<NullReferenceException>(
            // ReSharper disable once NullableWarningSuppressionIsUsed
            async () => await _client.WriteMissionIntItem(null!, cancel:_cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteMissionIntItem_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = Link.Server.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
        });
        
        // Act
        var task = _client.WriteMissionIntItem(_ => { }, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await task
        );
        Assert.Equal(0, called);
    }
    
    #endregion
    
    #region ClearAll
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    [InlineData(MavMissionType.MavMissionTypeAll)]
    public async Task ClearAll_DifferentMissionTypes_Success(MavMissionType missionType)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionClearAllPacket? packetFromClient = null;
        using var s1 = _server.OnMissionClearAll.SubscribeAwait(
            async (p, _) =>
        {
            called++;
            await _server.SendMissionAck(MavMissionResult.MavMissionAccepted);
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionClearAllPacket;
        });
        
        // Act
        await _client.ClearAll(missionType, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionClearAllPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(missionType, packetFromServer.Payload.MissionType);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedCalls, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ClearAll_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = _server.OnMissionClearAll.SubscribeAwait(
            async (_, _) =>
        {
            called++;
            await _server.SendMissionAck(MavMissionResult.MavMissionAccepted);
        });
        
        // Act
        var task = _client.ClearAll(MavMissionType.MavMissionTypeAll, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await task
        );
        Assert.Equal(0, called);
    }
    
    #endregion

    #region MissionSetCount

    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task MissionSetCount_DifferentCountValues_Success(ushort count)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        MissionCountPacket? packetFromClient = null;
        using var s1 = Server.OnMissionCount.Synchronize().Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromClient = p as MissionCountPacket;
        });
        
        // Act
        await _client.MissionSetCount(count, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as MissionCountPacket;
        Assert.Equal(expectedCalls, called);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(count, packetFromServer.Payload.Count);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task MissionSetCount_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = Server.OnMissionCount.Synchronize().Subscribe(p =>
        {
            called++;
        });
        
        // Act
        var task = _client.MissionSetCount(10, cancel: _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await task
        );
        Assert.Equal(0, called);
    }

    #endregion

    #region MissionSendAck

    [Theory]
    [InlineData(MavMissionResult.MavMissionAccepted)]
    [InlineData(MavMissionResult.MavMissionError)]
    [InlineData(MavMissionResult.MavMissionInvalidParam1)]
    [InlineData(MavMissionResult.MavMissionOperationCancelled)]
    public async Task SendMissionAck_DifferentResults_Success(MavMissionResult missionResult)
    {
        // Arrange
        const int expectedCalls = 1;
        var tcs = new TaskCompletionSource<MissionAckPayload>();
        using var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => _taskCompletionSource.TrySetCanceled());
        
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = _server.OnMissionAck.Synchronize().Subscribe(p =>
        {
            called++;
            tcs.TrySetResult(p);
        });
        
        using var s2 = Link.Client.OnTxMessage.Synchronize().Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _client.SendMissionAck(missionResult, cancel: cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer?.Payload.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SendMissionAck_Cancelled_Throw()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = _server.OnMissionAck.Synchronize().Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        var task = _client.SendMissionAck(MavMissionResult.MavMissionDenied, cancel: _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    #endregion
    
    #region RequestMissionItem
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task RequestMissionItem_DifferentIds_Success(ushort id)
    {
        // Arrange
        var tcs = new TaskCompletionSource<MissionRequestPayload>();
        using var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        const int expectedCount = 1;
        var called = 0;
        
        using var s1 = _client.OnMissionRequest.SubscribeAwait(
            async (p, ct) =>
        {
            called++;
            var item = new MissionItem(new MissionItemIntPayload
            {
                Seq = id,
                MissionType = MavMissionType.MavMissionTypeAll
            });
            
            await _client.WriteMissionItem(item, ct);
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
        Assert.Equal(expectedCount, called);
        Assert.Equal(called, (int) Link.Client.Statistic.TxMessages);
        Assert.Equal((int) Link.Client.Statistic.TxMessages, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedCount, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(resultFormClient.Seq, result.Seq);
        Assert.Equal(resultFormClient.MissionType, result.MissionType);
    }
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeAll)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    public async Task RequestMissionItem_DifferentMissionTypes_Success(MavMissionType missionType)
    {
        // Arrange
        var tcs = new TaskCompletionSource<MissionRequestPayload>();
        using var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        const int expectedCount = 1;
        const ushort id = 10;
        var called = 0;
        
        using var s1 = _client.OnMissionRequest.SubscribeAwait(
            async (p, ct) => 
            { 
                called++; 
                var item = new MissionItem(
                    new MissionItemIntPayload 
                    { 
                        Seq = id, 
                        MissionType = missionType 
                    }
                );
                
                await _client.WriteMissionItem(item, ct); 
                tcs.TrySetResult(p); 
            }
        );
        
        // Act
        var result = await _server.RequestMissionItem(
            id,
            missionType,
            cancel: cancel.Token
        );

        // Assert
        var resultFormClient = await tcs.Task;
        Assert.Equal(expectedCount, called);
        Assert.Equal(called, (int) Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedCount, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(resultFormClient.Seq, result.Seq);
        Assert.Equal(resultFormClient.MissionType, result.MissionType);
    }
    
    [Fact]
    public async Task RequestMissionItem_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        
        using var s1 = _client.OnMissionRequest.Subscribe(_ =>
        {
            called++;
        });
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await _server.RequestMissionItem(
                10, 
                MavMissionType.MavMissionTypeAll, 
                cancel: _cancellationTokenSource.Token
            )
        );
        
        Assert.Equal(0, called);
    }
    
    #endregion
    
    protected override MissionServer CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    { 
        return new MissionServer(identity, core);
    }

    protected override MissionClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new MissionClient(identity, _clientConfig, core);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }
        
        base.Dispose(disposing);
    }

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
            return left.Value.ApproximatelyEquals(right.Value);
        }

        if (left is null && right is null)
        {
            return true;
        }

        return false;
    }
    
    #endregion
}