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
using TimeProviderExtensions;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class MissionExComplexTest : ComplexTestBase<MissionClientEx, MissionServerEx>
{
    private const int MaxCommandTimeoutMs = 1000;
    private const int MaxAttemptsToCallCount = 5;
    private const int MaxDeviceUploadTimeoutMs = 10_000;
    private const int MaxQueueSizeValue = 100;
    private const int MaxSendRateHzValue = 30;
    
    private readonly MissionClientConfig _clientConfig = new()
    {
        CommandTimeoutMs = MaxCommandTimeoutMs,
        AttemptToCallCount = MaxAttemptsToCallCount,
    };
    
    private readonly MissionClientExConfig _clientExConfig = new()
    {
        DeviceUploadTimeoutMs  = MaxDeviceUploadTimeoutMs,
    };
    
    private readonly StatusTextLoggerConfig _statusTextServerConfig = new()
    {
        MaxQueueSize = MaxQueueSizeValue,
        MaxSendRateHz = MaxSendRateHzValue,
    };
    
    private readonly MissionClientEx _client;
    private readonly MissionServerEx _server;
    
    public MissionExComplexTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _client = Client;
    }

    protected override MissionServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var statusTextServer = new StatusTextServer(identity, _statusTextServerConfig, core);
        var server = new MissionServer(identity, core);
        return new MissionServerEx(server, statusTextServer);
    }

    protected override MissionClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var client = new MissionClient(identity, _clientConfig, core);
        return new MissionClientEx(client, _clientExConfig);
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(ushort.MaxValue)]
    public async Task SetCurrent_DifferentMissionItemsIds_Success(ushort missionItemsIndex)
    {
        // Arrange
        var tcs = new TaskCompletionSource<ushort>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = false;
        var items = new ServerMissionItem[missionItemsIndex+1];
        for (var i = 0; i <= missionItemsIndex; i++)
        {
            items[i] = new ServerMissionItem();
        }

        _server.AddItems(items);
        using var s1 = _server.Current.Subscribe(p =>
        {
            if (!isInit)
            {
                isInit = true;
                return;
            }
            
            called++;
            tcs.TrySetResult(p);
        });
        
        // Act
        await _client.SetCurrent(missionItemsIndex, cancel.Token);
        
        // Assert
        var currentId = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(missionItemsIndex, currentId);
        Assert.False(_client.IsSynced.CurrentValue);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(400)]
    [InlineData(90_000)]
    public async Task SetCurrent_SeveralCallsWithTheSameId_Success(int callsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = false;
        ushort missionItemsIndex = 3;
        var items = new ServerMissionItem[missionItemsIndex+1];
        for (var i = 0; i <= missionItemsIndex; i++)
        {
            items[i] = new ServerMissionItem();
        }
        var results = new List<ushort>();
        
        _server.AddItems(items);
        using var s1 = _server.Current.Subscribe(p =>
        {
            if (!isInit)
            {
                isInit = true;
                return;
            }
            
            called++;
            results.Add(p);
            if (called >= callsCount)
            {
                tcs.SetResult();
            }
        });
        
        // Act
        for (var i = 0; i < callsCount; i++)
        {
            await _client.SetCurrent(missionItemsIndex, cancel.Token);
        }
        
        // Assert
        await tcs.Task;
        Assert.Equal(callsCount, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(results.All(id => id == missionItemsIndex));
    }
    
    [Fact]
    public async Task SetCurrent_Canceled_Throws()
    {
        // Arrange
        var tcs = new TaskCompletionSource<ushort>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        await cancel.CancelAsync();
        
        var called = 0;
        var isInit = false;
        
        using var s1 = _server.Current.Subscribe(p =>
        {
            if (!isInit)
            {
                isInit = true;
                return;
            }
            
            called++;
            tcs.SetResult(p);
        });
        
        // Act
        var task = _client.SetCurrent(1234, cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
    }
    
    
    [Fact(Skip = "Test is not relevant")]
    public async Task SetCurrent_Timeout_Throws()
    {
        // Arrange
        var tcs = new TaskCompletionSource<ushort>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            // ClientTime.Advance(
            //     TimeSpan.FromMilliseconds((MaxAttemptsToCallCount * MaxCommandTimeoutMs * 2) + 1)
            // );
        });
        _server.AddItems([new ServerMissionItem()]);
        
        
        // Act
        var task = _client.SetCurrent(0, cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(10_000)]
    public async Task Download_DifferentItemsCount_Success(ushort itemsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<MissionAckPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var calledAckPacket = 0;
        var calledMissionRequestList = 0;
        var called = 0;
        var progress = 0d;
        var itemsFromServer = new ServerMissionItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            itemsFromServer[i] = new ServerMissionItem
            {
                Seq = (ushort) i,
                Command = MavCmd.MavCmdConditionGate,
                Autocontinue = 1,
                Param1 = 3,
                Param2 = 4, 
                Param3 = 5,
                Param4 = 6,
                Frame = MavFrame.MavFrameGlobal,
                MissionType = MavMissionType.MavMissionTypeMission,
                X = 1,
                Y = 2,
                Z = float.MaxValue,
            };
        }
        
        _server.AddItems(itemsFromServer);
        using var s1 = _server.Base.OnMissionRequestList.Subscribe(_ =>
        {
            calledMissionRequestList++;
        });
        using var s2 = _server.Base.OnMissionRequestInt.Subscribe(_ =>
        {
            called++;
        });
        using var s3 = _server.Base.OnMissionAck.Subscribe(p =>
        {
            calledAckPacket++;

            if (p.Type == MavMissionResult.MavMissionAccepted)
            {
                tcs.TrySetResult(p);
            }
        });
        
        // Act
        var items = await _client.Download(cancel.Token, pr => progress = pr);
        
        // Assert
        await tcs.Task;
        Assert.Equal(1, calledMissionRequestList);
        Assert.Equal(1, progress);
        Assert.Equal(1, calledAckPacket);
        Assert.Equal(itemsCount, called);
        Assert.Equal(called + calledMissionRequestList + calledAckPacket, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        // Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages); // TODO: Counter for Server.TxPackets shows strange results
        Assert.Equal(itemsCount, items.Length);
        Assert.Equal(items.Length, itemsFromServer.Length);
        for (var i = 0; i < items.Length; i++)
        {
            Assert.True(IsEqual(itemsFromServer[i], items[i]));
        }
        Assert.NotNull(items.Single(it => it.Current.CurrentValue));
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Fact]
    public async Task Download_WithItemsOnClient_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MissionAckPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var itemsCount = 10;
        var calledAckPacket = 0;
        var calledMissionRequestList = 0;
        var called = 0;
        var progress = 0d;
        var itemsFromServer = new ServerMissionItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            itemsFromServer[i] = new ServerMissionItem
            {
                Seq = (ushort) i,
            };
        }

        _client.Create();
        _client.Create();
        _server.AddItems(itemsFromServer);
        using var s1 = _server.Base.OnMissionRequestList.Subscribe(_ =>
        {
            calledMissionRequestList++;
        });
        using var s2 = _server.Base.OnMissionRequestInt.Subscribe(_ =>
        {
            called++;
        });
        using var s3 = _server.Base.OnMissionAck.Subscribe(p =>
        {
            calledAckPacket++;

            if (p.Type == MavMissionResult.MavMissionAccepted)
            {
                tcs.TrySetResult(p);
            }
        });
        
        // Act
        var items = await _client.Download(cancel.Token, pr => progress = pr);
        
        // Assert
        await tcs.Task;
        Assert.Equal(1, calledMissionRequestList);
        Assert.Equal(1, progress);
        Assert.Equal(1, calledAckPacket);
        Assert.Equal(itemsCount, called);
        Assert.Equal(called + calledMissionRequestList + calledAckPacket, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(itemsCount, items.Length);
        Assert.Equal(items.Length, itemsFromServer.Length);
        for (var i = 0; i < items.Length; i++)
        {
            Assert.True(IsEqual(itemsFromServer[i], items[i]));
        }
        Assert.NotNull(items.Single(it => it.Current.CurrentValue));
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Fact]
    public async Task Download_ZeroItems_Success()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);

        var calledMissionRequestList = 0;
        var called = 0;
        var progress = 0d;
        
        using var s1 = _server.Base.OnMissionRequestList.Subscribe(_ =>
        {
            calledMissionRequestList++;
        });
        using var s2 = _server.Base.OnMissionRequestInt.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        var items = await _client.Download(cancel.Token, pr => progress = pr);
        
        // Assert
        Assert.Equal(1, calledMissionRequestList);
        Assert.Equal(0, progress);
        Assert.Equal(0, called);
        Assert.Empty(items);
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Fact(Skip = "Test is not relevant")]
    public async Task Download_Timeout_Throws()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10), TimeProvider.System);
        
        var progress = 0d;
        
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
             ClientTime.Advance(
                 TimeSpan.FromMilliseconds((MaxAttemptsToCallCount * MaxCommandTimeoutMs * 2) + 1)
             );
        });
        
        // Act
        var task = _client.Download(cancel.Token, pr => progress = pr);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(0, progress);
        Assert.False(_client.IsSynced.CurrentValue);
    }
    
    [Fact(Skip = "Test is not relevant")]
    public async Task Download_Cancel_Throws()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        await cancel.CancelAsync();
        
        var calledMissionRequestList = 0;
        var called = 0;
        var progress = 0d;
        
        using var s1 = _server.Base.OnMissionRequestList.Subscribe(_ =>
        {
            calledMissionRequestList++;
        });
        using var s2 = _server.Base.OnMissionRequestInt.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        var task = _client.Download(cancel.Token, pr => progress = pr);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, calledMissionRequestList);
        Assert.Equal(0, progress);
        Assert.Equal(0, called);
        Assert.False(_client.IsSynced.CurrentValue);
    }

    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task ClearRemote_DifferentItemsCount_Success(ushort itemsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<MissionAckPacket>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var itemsFromServer = new ServerMissionItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            itemsFromServer[i] = new ServerMissionItem
            {
                Seq = (ushort) i
            };
        }
        
        _server.AddItems(itemsFromServer);
        using var s = _server.Base.OnMissionClearAll.Subscribe(_ =>
        {
            called++;
        });
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            if (p is MissionAckPacket ackP)
            {
                tcs.TrySetResult(ackP);
            }
        });
        
        // Act
        await _client.ClearRemote(cancel.Token);
        
        // Assert
        var ackPacket = await tcs.Task;
        var items = _server.Items.ToList();
        Assert.Equal(1, called);
        Assert.NotNull(ackPacket);
        Assert.Equal(MavMissionResult.MavMissionAccepted, ackPacket.Payload.Type);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Empty(items);
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    
    [Fact]
    public async Task ClearRemote_ClearNull_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MissionAckPacket>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        using var s = _server.Base.OnMissionClearAll.Subscribe(_ =>
        {
            called++;
        });
        using var s1 = Link.Client.OnRxMessage.Subscribe(p =>
        {
            if (p is MissionAckPacket ackP)
            {
                tcs.TrySetResult(ackP);
            }
        });
        
        // Act
        await _client.ClearRemote(cancel.Token);
        
        // Assert
        var ackPacket = await tcs.Task;
        var items = _server.Items.ToList();
        Assert.Equal(1, called);
        Assert.Equal(MavMissionResult.MavMissionAccepted, ackPacket.Payload.Type);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Empty(items);
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Fact(Skip = "Test is not relevant")]
    public async Task ClearRemote_Timeout_Throws()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds((MaxAttemptsToCallCount * MaxCommandTimeoutMs * 2) + 1));
        });
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async ()=>
        {
            await _client.ClearRemote(cancel.Token);
        });
        Assert.False(_client.IsSynced.CurrentValue);
    }
    
    [Fact]
    public async Task ClearRemote_Cancel_Throws()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        await cancel.CancelAsync();
        
        var called = 0;
        using var s = _server.Base.OnMissionClearAll.Subscribe(_ =>
        {
            called++;
        });
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async ()=>
        {
            await _client.ClearRemote(cancel.Token);
        });
        Assert.Equal(0, called);
        Assert.False(_client.IsSynced.CurrentValue);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(ushort.MaxValue)]
    public async Task OnReached_DifferentSeqValues_Success(ushort seq)
    {
        // Arrange
        var tcs = new TaskCompletionSource<ushort>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = false;
        
        _client.Reached.Subscribe(v =>
        {
            if (!isInit)
            {
                isInit = true;
                return;
            }
            
            called++;
            tcs.TrySetResult(v);
        });
        
        // Act
        _server.Reached.OnNext(seq);
        
        // Assert
        var seqFromClient = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(seq, seqFromClient);
        Assert.False(_client.IsSynced.CurrentValue);
    }

    [Fact]
    public async Task Upload_SinglePacket_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var calledAck = 0;
        using var sub1 = _client.Base.OnMissionAck.Subscribe(p =>
        {
            if (p.Type != MavMissionResult.MavMissionAccepted)
            {
                throw new Exception($"{p.Type} is not acceptable");
            }
            
            calledAck++;

            if (calledAck >= 2)
            {
                tcs.TrySetResult();
            }
        });
        
        var progress = 0d;
        var item1 = _client.Create();
        item1.Command.OnNext(MavCmd.MavCmdUser1);
        item1.Current.OnNext(true);
        item1.AutoContinue.OnNext(true);
        item1.Param1.OnNext(1);
        item1.Param2.OnNext(2);
        item1.Param3.OnNext(3);
        item1.Param4.OnNext(4);
        item1.Frame.OnNext(MavFrame.MavFrameGlobal);
        item1.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
        item1.Location.OnNext(new GeoPoint(1, 2, 3));
        
        // Act
        await _client.Upload(cancel.Token, p => progress = p);
        
        // Assert
        await tcs.Task;
        var items = _server.Items.ToList();
        Assert.Equal(2, calledAck);
        Assert.Equal(1, progress);
        Assert.Equal(3, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Single(items);
        Assert.True(IsEqual(items[0], item1));
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(100)]
    [InlineData(10_000)]
    public async Task Upload_ManyPackets_Success(int itemsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var calledAck = 0;
        using var sub1 = _client.Base.OnMissionAck.Subscribe(p =>
        {
            if (p.Type != MavMissionResult.MavMissionAccepted)
            {
                throw new Exception($"{p.Type} is not acceptable");
            }
            
            calledAck++;

            if (calledAck >= 2)
            {
                tcs.TrySetResult();
            }
        });
        
        var progress = 0d;
        for (var i = 0; i < itemsCount; i++)
        {
            _client.Create();
        }
        
        // Act
        await _client.Upload(cancel.Token, p => progress = p);
        
        // Assert
        await tcs.Task;
        var clientItems = _client.MissionItems.ToList();
        var serverItems = _server.Items.ToList();
        Assert.NotEmpty(clientItems);
        Assert.NotEmpty(serverItems);
        Assert.Equal(2, calledAck);
        Assert.Equal(1, progress);
        Assert.Equal(itemsCount + 2, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(clientItems.Count, serverItems.Count);
        for (var i = 0; i < clientItems.Count; i++)
        {
            Assert.True(IsEqual(serverItems[i], clientItems[i]));
        }
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Fact]
    public async Task Upload_ManyPacketsWithPacketsOnServer_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var itemsCount = 30;
        var calledAck = 0;
        using var sub1 = _client.Base.OnMissionAck.Subscribe(p =>
        {
            if (p.Type != MavMissionResult.MavMissionAccepted)
            {
                throw new Exception($"{p.Type} is not acceptable");
            }
            
            calledAck++;

            if (calledAck >= 2)
            {
                tcs.TrySetResult();
            }
        });

        var items = new ServerMissionItem[10];
        for (var i = 0; i < 10; i++)
        {
            items[i] = new ServerMissionItem();
        }
        
        _server.AddItems(items);
        
        var progress = 0d;
        for (var i = 0; i < itemsCount; i++)
        {
            _client.Create();
        }
        
        // Act
        await _client.Upload(cancel.Token, p => progress = p);
        
        // Assert
        await tcs.Task;
        var clientItems = _client.MissionItems.ToList();
        var serverItems = _server.Items.ToList();
        Assert.NotEmpty(clientItems);
        Assert.NotEmpty(serverItems);
        Assert.Equal(2, calledAck);
        Assert.Equal(1, progress);
        Assert.Equal(itemsCount + 2, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(clientItems.Count, serverItems.Count);
        for (var i = 0; i < clientItems.Count; i++)
        {
            Assert.True(IsEqual(serverItems[i], clientItems[i]));
        }
        Assert.True(_client.IsSynced.CurrentValue);
    }
    
    [Fact]
    public async Task Upload_ManyPacketsWithDownload_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var itemsCount = 30;
        var progress = 0d;
        for (var i = 0; i < itemsCount; i++)
        {
            _client.Create();
        }
        
        // Act1
        await _client.Upload(cancel.Token, p => progress = p);
        
        // Assert1
        var clientItems = _client.MissionItems.ToArray();
        var serverItems = _server.Items.ToArray();
        Assert.NotEmpty(clientItems);
        Assert.NotEmpty(serverItems);
        Assert.Equal(clientItems.Length, serverItems.Length);
        for (var i = 0; i < clientItems.Length; i++)
        {
            Assert.True(IsEqual(serverItems[i], clientItems[i]));
        }
        Assert.Equal(1, progress);
        Assert.True(_client.IsSynced.CurrentValue);
        
        // Act2
        _server.AddItems([new ServerMissionItem()]);
        var downloadItems = await _client.Download(cancel.Token);
        
        // Assert2
        clientItems = _client.MissionItems.ToArray();
        Assert.Equal(itemsCount+1, downloadItems.Length);
        Assert.Equal(clientItems.Length, downloadItems.Length);
        Assert.True(clientItems.IsDeepEqual(downloadItems));
    }
    
    [Fact]
    public async Task Upload_Cancel_Throws()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        await cancel.CancelAsync();
        var progress = 0d;
        _client.Create();
        
        // Act
        var task = _client.Upload(cancel.Token, p => progress = p);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        var items = _server.Items.ToList();
        Assert.Equal(0, progress);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Empty(items);
        Assert.False(_client.IsSynced.CurrentValue);
    }
    
    [Fact(Skip = "Test is not relevant")]
    public async Task Upload_Timeout_Throws()
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(1), TimeProvider.System);
        var progress = 0d;
        _client.Create();

        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
           // ClientTime.Advance(TimeSpan.FromMilliseconds((MaxAttemptsToCallCount * MaxCommandTimeoutMs * 2) + 1));
        });
        
        // Act
        var task = _client.Upload(cancel.Token, p => progress = p);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(0, progress);
    }

    #region Utils

    private bool IsEqual(ServerMissionItem? serverItem, MissionItem? clientItem)
    {
        if (serverItem is null && clientItem is null)
        {
            return true;
        }

        return (serverItem?.Autocontinue != 0).Equals(clientItem?.AutoContinue.Value)
               && serverItem?.Command == clientItem.Command.Value
               && serverItem.Seq == clientItem.Index
               && serverItem.Frame == clientItem.Frame.Value
               && Math.Abs(serverItem.Param1 - clientItem.Param1.Value) <= float.Epsilon
               && Math.Abs(serverItem.Param2 - clientItem.Param2.Value) <= float.Epsilon
               && Math.Abs(serverItem.Param3 - clientItem.Param3.Value) <= float.Epsilon
               && Math.Abs(serverItem.Param4 - clientItem.Param4.Value) <= float.Epsilon
               && Math.Abs(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(serverItem.X) 
                           - clientItem.Location.Value.Latitude) <= float.Epsilon
               && Math.Abs(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(serverItem.Y) 
                           - clientItem.Location.Value.Longitude) <= float.Epsilon
               && Math.Abs(serverItem.Z - clientItem.Location.Value.Altitude) <= float.Epsilon
               && serverItem.MissionType == clientItem.MissionType.Value;
    }

    #endregion
}