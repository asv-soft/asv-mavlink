using System;
using System.Collections.Generic;
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

[TestSubject(typeof(CommandClient))]
public class CommandClientTest : ClientTestBase<CommandClient>
{
    private readonly CommandProtocolConfig _config = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CommandClient _client;

    public CommandClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override CommandClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new CommandClient(identity, _config, core);
    }

    #region SendCommandInt

    [Theory]
    [InlineData(
        MavCmd.MavCmdUser1, 
        MavFrame.MavFrameGlobal, 
        true, 
        false
    )]
    [InlineData(
        MavCmd.MavCmdUser2, 
        MavFrame.MavFrameGlobal, 
        false, 
        false
    )]
    [InlineData(
        MavCmd.MavCmdUser3, 
        MavFrame.MavFrameGlobal, 
        true, 
        true
    )]
    [InlineData(
        MavCmd.MavCmdNavFencePolygonVertexInclusion, 
        MavFrame.MavFrameLocalFlu, 
        false, 
        true
    )]
    public async Task SendCommandInt_ProperInput_Success(
        MavCmd cmd, 
        MavFrame frame, 
        bool current,
        bool autoContinue
    )
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Client.OnTxMessage
            .Subscribe(
                p =>
                {
                    packetFromClient = p as CommandIntPacket;
                }
            );

        // Act
        await _client.SendCommandInt(
            cmd,
            frame,
            current,
            autoContinue,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandIntPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }

    [Theory]
    [InlineData(
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        int.MaxValue,
        int.MaxValue,
        float.MaxValue
    )]
    [InlineData(
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        int.MinValue,
        int.MinValue,
        float.MinValue
    )]
    public async Task SendCommandInt_DifferentParams_Success(
        float p1,
        float p2,
        float p3,
        float p4,
        int p5,
        int p6,
        float p7
    )
    {
        // Arrange 
        var called = 0;
        
        var tcs = new TaskCompletionSource<CommandIntPacket>();
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is CommandIntPacket commandIntPacket)
            {
                tcs.TrySetResult(commandIntPacket);
            }
        });

        // Act
        await _client.SendCommandInt(
            MavCmd.MavCmdUser1,
            MavFrame.MavFrameGlobal,
            true,
            true,
            p1,
            p2,
            p3,
            p4,
            p5,
            p6,
            p7,
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandIntPacket;
        var packetFromClient = await tcs.Task;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(400)]
    [InlineData(40000)]
    public async Task SendCommandInt_SendSeveralCommands_Success(int sendCount)
    {
        // Arrange 
        var called = 0;
        var results = new List<CommandIntPacket>();
        var packetsFromClient = new List<CommandIntPacket>();
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            if (p is not CommandIntPacket commandPacket)
            {
                return;
            }

            results.Add(commandPacket);

            if (called >= sendCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });
        using var sub1 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is not CommandIntPacket commandPacket)
            {
                return;
            }

            packetsFromClient.Add(commandPacket);
        });

        // Act
        for (var i = 0; i < sendCount; i++)
        {
            await _client.SendCommandInt(
                MavCmd.MavCmdUser1,
                MavFrame.MavFrameGlobal,
                true,
                true,
                1,
                1,
                3,
                4,
                5,
                6,
                7,
                _cancellationTokenSource.Token
            );
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(sendCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(packetsFromClient.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(packetsFromClient[i].IsDeepEqual(results[i]));
        }
    }

    [Fact(Skip="aaa")]
    public async Task SendCommandInt_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _client.SendCommandInt(
            MavCmd.MavCmdUser1,
            MavFrame.MavFrameMission,
            true,
            true,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => 
        {
            await task;
        });
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
    }

    #endregion

    #region SendCommandLong

    [Theory]
    [InlineData(
        MavCmd.MavCmdNavWaypoint
    )]
    [InlineData(
        MavCmd.MavCmdCanForward
    )]
    public async Task SendCommandLong_ProperInput_Success(
        MavCmd cmd
    )
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Client.OnTxMessage
            .Subscribe(
                p =>
                {
                    packetFromClient = p as CommandLongPacket;
                }
            );

        // Act
        await _client.SendCommandLong(
            cmd,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }

    [Theory]
    [InlineData(
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue
    )]
    [InlineData(
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue
    )]
    public async Task SendCommandLong_DifferentParams_Success(
        float p1,
        float p2,
        float p3,
        float p4,
        float p5,
        float p6,
        float p7
    )
    {
        // Arrange 
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var sub1 = Link.Client.OnTxMessage.Subscribe(p => { packetFromClient = p as CommandLongPacket; });

        // Act
        await _client.SendCommandLong(
            MavCmd.MavCmdUser1,
            p1,
            p2,
            p3,
            p4,
            p5,
            p6,
            p7,
            _cancellationTokenSource.Token
        );

        // Assert
        var result = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(400)]
    [InlineData(40000)]
    public async Task SendCommandLong_SendSeveralCommands_Success(int sendCount)
    {
        // Arrange 
        var called = 0;
        var results = new List<CommandLongPacket>();
        var packetsFromClient = new List<CommandLongPacket>();
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            if (p is not CommandLongPacket commandPacket)
            {
                return;
            }

            results.Add(commandPacket);

            if (called >= sendCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });
        using var sub1 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is not CommandLongPacket commandPacket)
            {
                return;
            }

            packetsFromClient.Add(commandPacket);
        });

        // Act
        for (var i = 0; i < sendCount; i++)
        {
            await _client.SendCommandLong(
                MavCmd.MavCmdUser1,
                1,
                1,
                3,
                4,
                5,
                6,
                7,
                _cancellationTokenSource.Token);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(sendCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(packetsFromClient.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(packetsFromClient[i].IsDeepEqual(results[i]));
        }
    }

    [Fact]
    public async Task SendCommandLong_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        using var sub = Link.Server.OnRxMessage.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _client.SendCommandLong(
            MavCmd.MavCmdUser1,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
    }

    #endregion
    
    #region Timeout
    
    [Fact]
    public async Task CommandInt_TimeoutHappened_Throws()
    {
        // Arrange
        var called = 0;
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.CommandInt(
            MavCmd.MavCmdUser1,
            MavFrame.MavFrameGlobal,
            true,
            true,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.CommandAttempt, called);
        Assert.Equal(_config.CommandAttempt, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task CommandLong_TimeoutHappened_Throws()
    {
        // Arrange
        var called = 0;
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.CommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.CommandLong(
            MavCmd.MavCmdUser1,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.CommandAttempt, called);
        Assert.Equal(_config.CommandAttempt, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1000)]
    [InlineData(1, 1000)]
    [InlineData(5, 5000)]
    [InlineData(10, 10000)]
    public async Task CommandInt_TimeoutWithCustomConfig_Throws(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, Context);
        
        var called = 0;
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(maxTimeoutInMs + 1));
        });
        
        // Act
        var task = client.CommandInt(
            MavCmd.MavCmdUser1,
            MavFrame.MavFrameGlobal,
            true,
            true,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(maxCommandAttempts, called);
        Assert.Equal(maxCommandAttempts, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }
    
    #endregion
}