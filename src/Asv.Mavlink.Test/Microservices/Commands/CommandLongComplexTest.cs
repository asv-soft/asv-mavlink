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

public class CommandLongComplexTest : ComplexTestBase<CommandClient, CommandLongServerEx>
{
    private const int MaxCommandAttempts = 5;
    private const int MaxTimeoutInMs = 5000;
    
    private readonly CommandProtocolConfig _config = new()
    {
        CommandTimeoutMs = MaxTimeoutInMs,
        CommandAttempt = MaxCommandAttempts
    };
    
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CommandLongServerEx _server;
    private readonly CommandClient _client;
    
    public CommandLongComplexTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override CommandLongServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var commandServer = new CommandServer(identity, core);
        return new CommandLongServerEx(commandServer);
    }

    protected override CommandClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new CommandClient(identity, _config, core);
    }
    
    [Theory]
    [InlineData(MavCmd.MavCmdActuatorTest)]
    [InlineData(MavCmd.MavCmdUser5)]
    [InlineData(MavCmd.MavCmdAirframeConfiguration)]
    [InlineData(MavCmd.MavCmdDoFlighttermination)]
    [InlineData(MavCmd.MavCmdDoOrbit)]
    [InlineData(MavCmd.MavCmdNavWaypoint)]
    public async Task CommandLong_DifferentMavCmdsWithResultAccepted_Success(MavCmd cmd)
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        _server[cmd] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as CommandLongPacket;
        });
        
        // Act
        var result = await _client.CommandLong(
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
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(cmd, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
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
    public async Task CommandLong_DifferentMavResults_Success(MavResult mavResult)
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        _server[MavCmd.MavCmdActuatorTest] = (_, args, _) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(mavResult));
        };
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as CommandLongPacket;
        });
        
        // Act
        var result = await _client.CommandLong(
            MavCmd.MavCmdActuatorTest,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(mavResult, result.Result);
        Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
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
        float.PositiveInfinity, 
        float.PositiveInfinity, 
        float.PositiveInfinity,
        float.PositiveInfinity,
        float.PositiveInfinity,
        float.PositiveInfinity,
        float.PositiveInfinity
    )]
    [InlineData(
        float.E, 
        float.E, 
        float.E,
        float.E,
        float.E,
        float.E,
        float.E
    )]
    [InlineData(
        float.Epsilon, 
        float.Epsilon, 
        float.Epsilon,
        float.Epsilon,
        float.Epsilon,
        float.Epsilon,
        float.Epsilon
    )]
    [InlineData(
        float.Tau, 
        float.Tau, 
        float.Tau,
        float.Tau,
        float.Tau,
        float.Tau,
        float.Tau
    )]
    [InlineData(
        float.Pi, 
        float.Pi, 
        float.Pi,
        float.Pi,
        float.Pi,
        float.Pi,
        float.Pi
    )]
    [InlineData(
        float.NegativeZero, 
        float.NegativeZero, 
        float.NegativeZero,
        float.NegativeZero,
        float.NegativeZero,
        float.NegativeZero,
        float.NegativeZero
    )]
    [InlineData(
        float.NegativeInfinity, 
        float.NegativeInfinity, 
        float.NegativeInfinity,
        float.NegativeInfinity,
        float.NegativeInfinity,
        float.NegativeInfinity,
        float.NegativeInfinity
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
    [InlineData(
        float.MaxValue, 
        float.MinValue, 
        float.E,
        float.Epsilon,
        float.NegativeZero,
        float.PositiveInfinity,
        float.Tau
    )]
    public async Task CommandLong_DifferentParams_Success(
        float param1,
        float param2,
        float param3,
        float param4,
        float param5,
        float param6,
        float param7
    )
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        _server[MavCmd.MavCmdCanForward] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as CommandLongPacket;
        });
        
        // Act
        var result = await _client.CommandLong(
            MavCmd.MavCmdCanForward,
            param1,
            param2,
            param3,
            param4,
            param5,
            param6,
            param7,
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdCanForward, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(4000)]
    public async Task CommandLong_SeveralCallsWithSingleCallback_Success(int callsCount)
    {
        // Arrange
        var called = 0;
        var results = new List<CommandAckPayload>();
        var packetsFromServer = new List<CommandLongPacket>();
        var packetsFromClient = new List<CommandLongPacket>();
        _server[MavCmd.MavCmdActuatorTest] = (_, args, _) =>
        {
            called++;
            packetsFromServer.Add(args);
            
            if (called >= callsCount)
            {
                _taskCompletionSource.TrySetResult(args);
            }
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultAccepted));
        };
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is CommandLongPacket longPacket)
            {
                packetsFromClient.Add(longPacket);
            }
        });
        
        // Act
        for (var i = 0; i < callsCount; i++)
        {
            var result = await _client.CommandLong(
                MavCmd.MavCmdActuatorTest,
                1,
                1,
                3,
                4,
                5,
                6,
                7,
                _cancellationTokenSource.Token);
            results.Add(result);
        }
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(callsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        foreach (var result in results)
        {
            Assert.Equal(MavResult.MavResultAccepted, result.Result);
            Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
        }
        Assert.True(packetsFromClient.IsDeepEqual(packetsFromServer));
    }
    
    [Fact]
    public async Task CommandLong_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        _server[MavCmd.MavCmdActuatorTest] = (_, args, _) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultAccepted));
        };
        
        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _client.CommandLong(
            MavCmd.MavCmdActuatorTest,
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
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Fact]
    public async Task CommandLong_SeveralCallbacksWithOneCall_Success()
    {
        // Arrange
        var calledFirst = 0;
        var calledSecond = 0;
        _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10), TimeProvider.System);
        _server[MavCmd.MavCmdActuatorTest] = (_, _, _) =>
        {
            calledFirst++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        }; 
        _server[MavCmd.MavCmdUser5] = (_, _, _) =>
        {
            calledSecond++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var result = await _client.CommandLong(
            MavCmd.MavCmdActuatorTest,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token);
        
        // Assert
        Assert.Equal(1, calledFirst);
        Assert.Equal(0, calledSecond);
        Assert.Equal(calledFirst, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
    }
    
    [Fact]
    public async Task CommandLong_SeveralCallbacksWithCallForEach_Success()
    {
        // Arrange
        var calledFirst = 0;
        var calledSecond = 0;
        var tcs1 = new TaskCompletionSource();
        var tcs2 = new TaskCompletionSource();
        var cancel1 = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        var cancel2 = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        cancel1.Token.Register(() => tcs1.TrySetCanceled());
        cancel2.Token.Register(() => tcs2.TrySetCanceled());
        _server[MavCmd.MavCmdActuatorTest] = (_, _, _) =>
        {
            calledFirst++;
            var result = MavResult.MavResultAccepted;
            tcs1.TrySetResult();
            return Task.FromResult(CommandResult.FromResult(result));
        }; 
        _server[MavCmd.MavCmdUser5] = (_, _, _) =>
        {
            calledSecond++;
            var result = MavResult.MavResultAccepted;
            tcs2.TrySetResult();
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var result1 = await _client.CommandLong(
            MavCmd.MavCmdActuatorTest,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            cancel1.Token
        );
        
        var result2 = await _client.CommandLong(
            MavCmd.MavCmdUser5,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            cancel2.Token
        );
        
        // Assert
        await tcs1.Task;
        await tcs2.Task;
        Assert.Equal(1, calledFirst);
        Assert.Equal(1, calledSecond);
        Assert.Equal(calledFirst + calledSecond, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, result1.Result);
        Assert.Equal(MavCmd.MavCmdActuatorTest, result1.Command);
        Assert.Equal(MavResult.MavResultAccepted, result2.Result);
        Assert.Equal(MavCmd.MavCmdUser5, result2.Command);
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
    public async Task SendCommandLong_DifferentMavResults_Success(MavResult mavResult)
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        _server[MavCmd.MavCmdActuatorTest] = (_, args, _) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(mavResult));
        };
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as CommandLongPacket;
        });
        
        // Act
        await _client.SendCommandLong(
            MavCmd.MavCmdActuatorTest,
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
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Fact(Skip = "Test is not relevant")]
    public async Task CommandLong_WaitBeforeResponse_Success()
    {
        // Arrange
        var called = 0;
        _server[MavCmd.MavCmdUser1] = (_, _, _) =>
        {
            called++;
            if (called != MaxCommandAttempts)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds((MaxCommandAttempts+1) * MaxTimeoutInMs));
            }
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultInProgress));
        };
        
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
        var result = await task;
        Assert.Equal(MaxCommandAttempts, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultInProgress, result.Result);
        Assert.Equal(MavCmd.MavCmdUser1, result.Command);
    }
    
    [Fact(Timeout = 2000)]
    public async Task CommandLong_TimeoutHappened_Throws()
    {
        // Arrange
        var identityToNothing = new MavlinkClientIdentity(
            1, 
            2,
            (byte)(Identity.Target.SystemId + 1),
            (byte)(Identity.Target.ComponentId + 1)
        );
        
        var client = CreateClient(identityToNothing, ClientCore);
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(MaxTimeoutInMs * (MaxCommandAttempts + 1)));
        });
        
        // Act
        var task = client.CommandLong(
            MavCmd.MavCmdUser1,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            default
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(MaxCommandAttempts, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Theory(Skip = "Test is not relevant")]
    [InlineData(0, 0)]
    [InlineData(0, 1000)]
    [InlineData(1, 1000)]
    [InlineData(5, 3000)]
    [InlineData(10, 9000)]
    public async Task CommandLong_TimeoutWithCustomConfig_Throws(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, ClientCore);
        
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(maxTimeoutInMs * (maxCommandAttempts + 1)));
        });
        
        // Act
        var task = client.CommandLong(
            MavCmd.MavCmdUser1,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            default
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(maxCommandAttempts, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Theory]
    [InlineData(1, 1000)]
    [InlineData(5, 5000)]
    [InlineData(10, 20000)]
    public async Task CommandLong_WaitBeforeResponseWithCustomConfig_Success(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var called = 0;
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, ClientCore);
        
        _server[MavCmd.MavCmdUser1] = (_, args, _) =>
        {
            called++;
            Log.WriteLine($"confirmation______ === {args.Payload.Confirmation}");
            if (called != maxCommandAttempts)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds((maxCommandAttempts+1) * maxTimeoutInMs));
            }
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultInProgress));
        };
        
        // Act
        var task = client.CommandLong(
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
        var result = await task;
        Assert.Equal(maxCommandAttempts, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultInProgress, result.Result);
        Assert.Equal(MavCmd.MavCmdUser1, result.Command);
    }
    
    [Theory]
    [InlineData(1, 1000)]
    [InlineData(5, 5000)]
    [InlineData(10, 20000)]
    public async Task CommandLong_WithCustomConfig_Success(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var called = 0;
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, ClientCore);
        CommandLongPacket? packetFromClient = null;
        _server[MavCmd.MavCmdUser1] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as CommandLongPacket;
        });
        
        // Act
        var result = await client.CommandLong(
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
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdUser1, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }

    [Fact]
    public async Task CommandLong_ConfirmationValueIncreases_Success()
    {
        // Arrange
        var called = 0;
        var packetsFromServer = new List<CommandLongPacket>();
        _server[MavCmd.MavCmdDoParachute] = (_, args, _) =>
        {
            called++;
            packetsFromServer.Add(args);
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            if (called != MaxCommandAttempts)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds((MaxCommandAttempts+1) * MaxTimeoutInMs));
            }
            
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var result = await _client.CommandLong(
            MavCmd.MavCmdDoParachute,
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
        await _taskCompletionSource.Task;
        Assert.Equal(MaxCommandAttempts, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdDoParachute, result.Command);
        Assert.Equal(MaxCommandAttempts, packetsFromServer.Count);
        var confirmation = 0;
        foreach (var packet in packetsFromServer.OrderBy(p => p.Payload.Confirmation))
        {
            Assert.Equal(confirmation, packet.Payload.Confirmation);
            confirmation++;
        }
    }
    
    [Fact]
    public async Task CommandLong_ConfirmationValueUnchanged_Success()
    {
        // Arrange
        var called = 0;
        var packetsFromServer = new List<CommandLongPacket>();
        _server[MavCmd.MavCmdDoParachute] = (_, args, _) =>
        {
            called++;
            packetsFromServer.Add(args);
            var result = MavResult.MavResultAccepted;
            if (called >= 2)
            {
                _taskCompletionSource.TrySetResult(args);
            }
            
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var call1 = await _client.CommandLong(
            MavCmd.MavCmdDoParachute,
            1,
            1,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token
        );
        var call2 = await _client.CommandLong(
            MavCmd.MavCmdDoParachute,
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
        await _taskCompletionSource.Task;
        Assert.Equal(2, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(MavResult.MavResultAccepted, call1.Result);
        Assert.Equal(MavCmd.MavCmdDoParachute, call1.Command);
        Assert.Equal(MavResult.MavResultAccepted, call2.Result);
        Assert.Equal(MavCmd.MavCmdDoParachute, call2.Command);
        Assert.Equal(called, packetsFromServer.Count);
        Assert.False(packetsFromServer[0].IsDeepEqual(packetsFromServer[1]));
        Assert.Equal(0, packetsFromServer[0].Payload.Confirmation);
        Assert.Equal(0, packetsFromServer[1].Payload.Confirmation);
    }
}