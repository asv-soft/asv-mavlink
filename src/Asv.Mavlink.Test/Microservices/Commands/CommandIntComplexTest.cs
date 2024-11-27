using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using DeepEqual.Syntax;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class CommandIntComplexTest : ComplexTestBase<CommandClient, CommandIntServerEx>
{
    private const int MaxCommandAttempts = 5;
    private const int MaxTimeoutInMs = 5000;
    
    private readonly CommandProtocolConfig _config = new()
    {
        CommandTimeoutMs = MaxTimeoutInMs,
        CommandAttempt = MaxCommandAttempts
    };
    
    private readonly TaskCompletionSource<IPacketV2<IPayload>> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CommandIntServerEx _server;
    private readonly CommandClient _client;
    
    public CommandIntComplexTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IPacketV2<IPayload>>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override CommandIntServerEx CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        var commandServer = new CommandServer(identity, core);
        return new CommandIntServerEx(commandServer);
    }

    protected override CommandClient CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new CommandClient(identity, _config, core);
    }

    [Theory]
    [InlineData(MavCmd.MavCmdActuatorTest)]
    [InlineData(MavCmd.MavCmdUser5)]
    [InlineData(MavCmd.MavCmdAirframeConfiguration)]
    public async Task CommandInt_DifferentMavCmdsWithResultAccepted_Success(MavCmd cmd)
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        _server[cmd] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        var result = await _client.CommandInt(
            cmd,
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
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
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
    public async Task CommandInt_DifferentMavResults_Success(MavResult mavResult)
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(mavResult));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        var result = await _client.CommandInt(
            MavCmd.MavCmdActuatorTest,
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
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(mavResult, result.Result);
        Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Theory]
    [InlineData(MavFrame.MavFrameMission)]
    [InlineData(MavFrame.MavFrameGlobal)]
    [InlineData(MavFrame.MavFrameLocalFlu)]
    [InlineData(MavFrame.MavFrameReserved13)]
    [InlineData(MavFrame.MavFrameReserved14)]
    [InlineData(MavFrame.MavFrameReserved15)]
    [InlineData(MavFrame.MavFrameReserved17)]
    [InlineData(MavFrame.MavFrameReserved18)]
    [InlineData(MavFrame.MavFrameReserved19)]
    [InlineData(MavFrame.MavFrameBodyFrd)]
    [InlineData(MavFrame.MavFrameBodyNed)]
    [InlineData(MavFrame.MavFrameGlobalInt)]
    [InlineData(MavFrame.MavFrameLocalEnu)]
    [InlineData(MavFrame.MavFrameLocalFrd)]
    [InlineData(MavFrame.MavFrameLocalNed)]
    [InlineData(MavFrame.MavFrameBodyOffsetNed)]
    [InlineData(MavFrame.MavFrameGlobalRelativeAlt)]
    [InlineData(MavFrame.MavFrameGlobalTerrainAlt)]
    [InlineData(MavFrame.MavFrameLocalOffsetNed)]
    [InlineData(MavFrame.MavFrameGlobalRelativeAltInt)]
    [InlineData(MavFrame.MavFrameGlobalTerrainAltInt)]
    public async Task CommandInt_DifferentMavFrames_Success(MavFrame mavFrame)
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultAccepted));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        var result = await _client.CommandInt(
            MavCmd.MavCmdActuatorTest,
            mavFrame,
            true,
            true,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task CommandInt_AllCurrentAndAutoContinueValues_Success(bool current, bool autoContinue)
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        _server[MavCmd.MavCmdCanForward] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        var result = await _client.CommandInt(
            MavCmd.MavCmdCanForward,
            MavFrame.MavFrameGlobal,
            current,
            autoContinue,
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
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdCanForward, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
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
        float.PositiveInfinity, 
        float.PositiveInfinity, 
        float.PositiveInfinity,
        float.PositiveInfinity,
        int.MaxValue,
        int.MaxValue,
        float.PositiveInfinity
    )]
    [InlineData(
        float.E, 
        float.E, 
        float.E,
        float.E,
        int.MaxValue,
        int.MaxValue,
        float.E
    )]
    [InlineData(
        float.Epsilon, 
        float.Epsilon, 
        float.Epsilon,
        float.Epsilon,
        int.MaxValue,
        int.MaxValue,
        float.Epsilon
    )]
    [InlineData(
        float.Tau, 
        float.Tau, 
        float.Tau,
        float.Tau,
        int.MaxValue,
        int.MaxValue,
        float.Tau
    )]
    [InlineData(
        float.Pi, 
        float.Pi, 
        float.Pi,
        float.Pi,
        int.MaxValue,
        int.MaxValue,
        float.Pi
    )]
    [InlineData(
        float.NegativeZero, 
        float.NegativeZero, 
        float.NegativeZero,
        float.NegativeZero,
        int.MaxValue,
        int.MaxValue,
        float.NegativeZero
    )]
    [InlineData(
        float.NegativeInfinity, 
        float.NegativeInfinity, 
        float.NegativeInfinity,
        float.NegativeInfinity,
        int.MaxValue,
        int.MaxValue,
        float.NegativeInfinity
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
    public async Task CommandInt_DifferentParams_Success(
        float param1,
        float param2,
        float param3,
        float param4,
        int param5,
        int param6,
        float param7
    )
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        _server[MavCmd.MavCmdCanForward] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        var result = await _client.CommandInt(
            MavCmd.MavCmdCanForward,
            MavFrame.MavFrameGlobal,
            true,
            true,
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
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdCanForward, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(4000)]
    public async Task CommandInt_SeveralCallsWithSingleCallback_Success(int callsCount)
    {
        // Arrange
        var called = 0;
        var results = new List<CommandAckPayload>();
        var packetsFromServer = new List<CommandIntPacket>();
        var packetsFromClient = new List<CommandIntPacket>();
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            called++;
            packetsFromServer.Add(args);
            
            if (called >= callsCount)
            {
                _taskCompletionSource.TrySetResult(args);
            }
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultAccepted));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            if (p is CommandIntPacket intPacket)
            {
                packetsFromClient.Add(intPacket);
            }
        });
        
        // Act
        for (var i = 0; i < callsCount; i++)
        {
            var result = await _client.CommandInt(
                MavCmd.MavCmdActuatorTest,
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
                _cancellationTokenSource.Token);
            results.Add(result);
        }
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(callsCount, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        foreach (var result in results)
        {
            Assert.Equal(MavResult.MavResultAccepted, result.Result);
            Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
        }
        Assert.True(packetsFromClient.IsDeepEqual(packetsFromServer));
    }
    
    [Fact]
    public async Task CommandInt_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultAccepted));
        };
        
        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _client.CommandInt(
            MavCmd.MavCmdActuatorTest,
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
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
    }
    
    [Fact]
    public async Task CommandInt_SeveralCallbacksWithOneCall_Success()
    {
        // Arrange
        var calledFirst = 0;
        var calledSecond = 0;
        _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10), TimeProvider.System);
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            calledFirst++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        }; 
        _server[MavCmd.MavCmdUser5] = (id, args, cancel) =>
        {
            calledSecond++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var result = await _client.CommandInt(
            MavCmd.MavCmdActuatorTest,
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
            _cancellationTokenSource.Token);
        
        // Assert
        Assert.Equal(1, calledFirst);
        Assert.Equal(0, calledSecond);
        Assert.Equal(calledFirst, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdActuatorTest, result.Command);
    }
    
    [Fact]
    public async Task CommandInt_SeveralCallbacksWithCallForEach_Success()
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
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            calledFirst++;
            var result = MavResult.MavResultAccepted;
            tcs1.TrySetResult();
            return Task.FromResult(CommandResult.FromResult(result));
        }; 
        _server[MavCmd.MavCmdUser5] = (id, args, cancel) =>
        {
            calledSecond++;
            var result = MavResult.MavResultAccepted;
            tcs2.TrySetResult();
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var result1 = await _client.CommandInt(
            MavCmd.MavCmdActuatorTest,
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
            cancel1.Token);
        
        var result2 = await _client.CommandInt(
            MavCmd.MavCmdUser5,
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
            cancel2.Token);
        
        // Assert
        await tcs1.Task;
        await tcs2.Task;
        Assert.Equal(1, calledFirst);
        Assert.Equal(1, calledSecond);
        Assert.Equal(calledFirst + calledSecond, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
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
    public async Task SendCommandInt_DifferentMavResults_Success(MavResult mavResult)
    {
        // Arrange
        var called = 0;
        CommandIntPacket? packetFromClient = null;
        _server[MavCmd.MavCmdActuatorTest] = (id, args, cancel) =>
        {
            called++;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(mavResult));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        await _client.SendCommandInt(
            MavCmd.MavCmdActuatorTest,
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
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.NotNull(packetFromClient);
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Fact]
    public async Task CommandInt_WaitBeforeResponse_Success()
    {
        // Arrange
        var called = 0;
        _server[MavCmd.MavCmdUser1] = (id, args, cancelToken) =>
        {
            called++;
            if (called != MaxCommandAttempts)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds((MaxCommandAttempts+1) * MaxTimeoutInMs));
            }
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultInProgress));
        };
        
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
        var result = await task;
        Assert.Equal(MaxCommandAttempts, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultInProgress, result.Result);
        Assert.Equal(MavCmd.MavCmdUser1, result.Command);
    }
    
    [Fact(Timeout = 2000)]
    public async Task CommandInt_TimeoutHappened_Throws()
    {
        // Arrange
        var identityToNothing = new MavlinkClientIdentity(
            1, 
            2,
            (byte)(Identity.Target.SystemId + 1),
            (byte)(Identity.Target.ComponentId + 1)
        );
        
        var client = CreateClient(identityToNothing, ClientCore);
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(MaxTimeoutInMs * (MaxCommandAttempts + 1)));
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
            default
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(MaxCommandAttempts, Link.Client.TxPackets);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1000)]
    [InlineData(1, 0)]
    [InlineData(1, 1000)]
    [InlineData(5, 5000)]
    [InlineData(10, 20000)]
    public async Task CommandInt_TimeoutWithCustomConfig_Throws(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, ClientCore);
        
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(maxTimeoutInMs * (maxCommandAttempts + 1)));
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
            default
        );
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(maxCommandAttempts, Link.Client.TxPackets);
    }
    
    [Theory]
    [InlineData(1, 1000)]
    [InlineData(5, 5000)]
    [InlineData(10, 20000)]
    public async Task CommandInt_WaitBeforeResponseWithCustomConfig_Success(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var called = 0;
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, ClientCore);
        
        _server[MavCmd.MavCmdUser1] = (id, args, cancelToken) =>
        {
            called++;
            if (called != maxCommandAttempts)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds((maxCommandAttempts+1) * maxTimeoutInMs));
            }
            
            return Task.FromResult(CommandResult.FromResult(MavResult.MavResultInProgress));
        };
        
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
        var result = await task;
        Assert.Equal(maxCommandAttempts, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultInProgress, result.Result);
        Assert.Equal(MavCmd.MavCmdUser1, result.Command);
    }
    
    [Theory]
    [InlineData(1, 1000)]
    [InlineData(5, 5000)]
    [InlineData(10, 20000)]
    public async Task CommandInt_WithCustomConfig_Success(int maxCommandAttempts, int maxTimeoutInMs)
    {
        // Arrange
        var called = 0;
        var customCfg = new CommandProtocolConfig
        {
            CommandTimeoutMs = maxTimeoutInMs,
            CommandAttempt = maxCommandAttempts
        };
        var client = new CommandClient(Identity, customCfg, ClientCore);
        CommandIntPacket? packetFromClient = null;
        _server[MavCmd.MavCmdUser1] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as CommandIntPacket;
        });
        
        // Act
        var result = await client.CommandInt(
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
            _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
        Assert.Equal(MavCmd.MavCmdUser1, result.Command);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
}