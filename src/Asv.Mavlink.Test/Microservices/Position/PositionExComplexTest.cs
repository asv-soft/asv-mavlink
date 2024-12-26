using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;


using DeepEqual.Syntax;
using R3;
using Xunit;
using Xunit.Abstractions;
using GeoPoint = Asv.Common.GeoPoint;

namespace Asv.Mavlink.Test.Position;

public class PositionExComplexTest : ComplexTestBase<PositionClientEx, CommandLongServerEx>
{
    private readonly HeartbeatClientConfig _heartbeatClientCfg = new()
    { 
        HeartbeatTimeoutMs = 2000, 
        LinkQualityWarningSkipCount  = 3, 
        RateMovingAverageFilter = 3, 
        PrintStatisticsToLogDelayMs = 10_000, 
        PrintLinkStateToLog = true,
    };

    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5,
    };

    private readonly MavlinkHeartbeatServerConfig _heartbeatServerCfg = new()
    {
        HeartbeatRateMs  = 1000,
    };

    private HeartbeatServer _heartBeatServer = null!;
    private readonly PositionClientEx _client;
    private readonly CommandLongServerEx _server;
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public PositionExComplexTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override CommandLongServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _heartBeatServer = new HeartbeatServer(identity, _heartbeatServerCfg, core);
        
        var commandServer = new CommandServer(identity, core);
        return new CommandLongServerEx(commandServer);
    }

    protected override PositionClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var positionClient = new PositionClient(identity, core);
        var heartBeatClient = new HeartbeatClient(identity, _heartbeatClientCfg, core);
        var commandClient = new CommandClient(identity, _commandConfig, core);
        return new PositionClientEx(positionClient, heartBeatClient, commandClient);
    }

    [Fact]
    public async Task ArmDisarm_LocalFalseSendTrue_Success()
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        _server[MavCmd.MavCmdComponentArmDisarm] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub2 = Link.Client.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandLongPacket; 
            });
        
        // Act
        await _client.ArmDisarm(true, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Fact]
    public async Task ArmDisarm_LocalTrueSendFalse_Success()
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        
        _heartBeatServer.Set(pld =>
        {
            pld.SystemStatus = MavState.MavStateActive;
            pld.BaseMode = MavModeFlag.MavModeFlagSafetyArmed;
            pld.Autopilot = MavAutopilot.MavAutopilotGeneric;
            pld.CustomMode = 123U;
            pld.MavlinkVersion = 3;
        });
        _heartBeatServer.Start();
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        _server[MavCmd.MavCmdComponentArmDisarm] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub2 = Link.Client.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandLongPacket;
            });
        
        // Act
        await _client.ArmDisarm(false, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called , (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(2, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
    }
    
    [Fact]
    public async Task ArmDisarm_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdComponentArmDisarm] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.ArmDisarm(true, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ArmDisarm_Unsupported_Throws()
    {
        // Arrange 
        var result = MavResult.MavResultAccepted;
        using var sub = Link.Client.OnRxMessage
            .FilterByType<CommandAckPacket>()
            .Subscribe(p =>
            {
                result = p.Payload.Result;
            });
        
        // Act
        var task = _client.ArmDisarm(true, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<CommandException>(async () => await task);
        Assert.Equal(MavResult.MavResultUnsupported, result);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(
        float.MaxValue,
        float.MaxValue,
        float.MaxValue
    )]
    [InlineData(
        float.MinValue,
        float.MinValue,
        float.MinValue
    )]
    [InlineData(
        12,
        14,
        16
    )]
    public async Task SetRoi_DifferentLatLongAlt_Success(
        float latitude,
        float longitude, 
        float altitude
    )
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        var geoPoint = new GeoPoint(latitude, longitude, altitude);
        _server[MavCmd.MavCmdDoSetRoi] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub2 = Link.Client.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandLongPacket;
            });
        
        // Act
        await _client.SetRoi(geoPoint, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(geoPoint, _client.Roi.CurrentValue);
        Assert.True(
            Math.Abs(latitude - (packetFromServer?.Payload.Param5 ?? 0f)) <= float.Epsilon
        );
        Assert.True(
            Math.Abs(longitude - (packetFromServer?.Payload.Param6 ?? 0f)) <= float.Epsilon
        );
        Assert.True(
            Math.Abs(altitude - (packetFromServer?.Payload.Param7 ?? 0f)) <= float.Epsilon
        );
    }
    
    [Theory]
    [InlineData(
        float.NaN,
        float.NaN,
        float.NaN
    )]
    public async Task SetRoi_DifferentNaN_Success(
        float latitude,
        float longitude, 
        float altitude
    )
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        var geoPoint = new GeoPoint(latitude, longitude, altitude);
        _server[MavCmd.MavCmdDoSetRoi] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub2 = Link.Client.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandLongPacket; 
            });
        
        // Act
        await _client.SetRoi(geoPoint, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Equal(float.NaN, packetFromServer!.Payload.Param5);
        Assert.Equal(float.NaN, packetFromServer.Payload.Param6);
        Assert.Equal(float.NaN, packetFromServer.Payload.Param7);
        Assert.Equal(geoPoint, _client.Roi.CurrentValue);
    }
    
    [Fact]
    public async Task SetRoi_DifferentGeoPoints_Success()
    {
        // Arrange
        var called = 0;
        var packetsFromServer = new List<CommandLongPacket>();
        var packetsFromClient = new List<CommandLongPacket>();
        _server[MavCmd.MavCmdDoSetRoi] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            packetsFromServer.Add(args);
            if (called >= 3)
            {
                _taskCompletionSource.TrySetResult(args);
            }
            
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub2 = Link.Client.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                if (p is CommandLongPacket packet) 
                { 
                    packetsFromClient.Add(packet); 
                }
            });
        
        // Act
        await _client.SetRoi(GeoPoint.Zero, _cancellationTokenSource.Token);
        await _client.SetRoi(GeoPoint.NaN, _cancellationTokenSource.Token);
        await _client.SetRoi(GeoPoint.ZeroWithAlt, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(3, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(3, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotEmpty(packetsFromClient);
        Assert.NotEmpty(packetsFromServer);
        Assert.True(packetsFromClient.IsDeepEqual(packetsFromServer));
    }
    
    [Fact]
    public async Task SetRoi_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        var geoPoint = new GeoPoint(1, 3, 11);
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdDoSetRoi] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.SetRoi(geoPoint, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ClearRoi_SingleCall_Success()
    {
        // Arrange
        var called = 0;
        CommandLongPacket? packetFromClient = null;
        _server[MavCmd.MavCmdDoSetRoiNone] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub2 = Link.Client.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandLongPacket;
            });
        
        // Act
        await _client.ClearRoi(_cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromClient.IsDeepEqual(packetFromServer));
        Assert.Null(_client.Roi.CurrentValue);
    }
    
    [Fact]
    public async Task ClearRoi_CallAfterSetRoi_Success()
    {
        // Arrange
        _server[MavCmd.MavCmdDoSetRoi] = (_, _, _) =>
        {
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        _server[MavCmd.MavCmdDoSetRoiNone] = (_, args, _) =>
        {
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        var gPoint = new GeoPoint(1, 3, 11);
        await _client.SetRoi(gPoint, _cancellationTokenSource.Token);
        var initialGPoint = _client.Roi.CurrentValue;
        
        // Act
        await _client.ClearRoi(_cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.NotNull(packetFromServer);
        Assert.Equal(gPoint, initialGPoint);
        Assert.Null(_client.Roi.CurrentValue);
    }
    
    [Fact]
    public async Task ClearRoi_Unsupported_Throws()
    {
        // Arrange 
        var result = MavResult.MavResultAccepted;
        using var sub = Link.Client.OnRxMessage 
            .FilterByType<CommandAckPacket>()
            .Subscribe(p =>
            {
                result = p.Payload.Result;
            });
        
        // Act
        var task = _client.ClearRoi(_cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<CommandException>(async () => await task);
        Assert.Equal(MavResult.MavResultUnsupported, result);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ClearRoi_Cancel_Throws()
    {
        // Arrange
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdDoSetRoiNone] = (_, _, _) =>
        {
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.ClearRoi(_cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Null(_client.Roi.CurrentValue);
    }
    
    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    public async Task TakeOff_SingleCall_Success(float altInMeters)
    {
        // Arrange
        var called = 0;
        IProtocolMessage? packetFromClient = null;
        _server[MavCmd.MavCmdNavTakeoff] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Server.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandAckPacket;
            });
        
        // Act
        await _client.TakeOff(altInMeters, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(
            Math.Abs(altInMeters - packetFromServer!.Payload.Param7) <= double.Epsilon
        );
    }
    
    [Fact]
    public async Task TakeOff_WithNaN_Success()
    {
        // Arrange
        var called = 0;
        IProtocolMessage? packetFromClient = null;
        _server[MavCmd.MavCmdNavTakeoff] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Server.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandAckPacket; 
            });
        
        // Act
        await _client.TakeOff(float.NaN, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.Equal(float.NaN, packetFromServer!.Payload.Param7);
    }
    
    [Fact]
    public async Task TakeOff_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdNavTakeoff] = (_, _, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.TakeOff(10, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task Takeoff_Unsupported_Throws()
    {
        // Arrange 
        var result = MavResult.MavResultAccepted;
        using var sub = Link.Client.OnRxMessage
            .FilterByType<CommandAckPacket>()
            .Subscribe(p =>
            {
                result = p.Payload.Result;
            });
        
        // Act
        var task = _client.TakeOff(10, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<CommandException>(async () => await task);
        Assert.Equal(MavResult.MavResultUnsupported, result);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    public async Task QTakeOff_SingleCall_Success(float altInMeters)
    {
        // Arrange
        var called = 0;
        IProtocolMessage? packetFromClient = null;
        
        var tcs2 = new TaskCompletionSource();
        _server[MavCmd.MavCmdNavVtolTakeoff] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
      
        using var sub = Link.Server.OnTxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            packetFromClient = p as CommandAckPacket;
            tcs2.TrySetResult();
        });
        
        // Act
        await _client.QTakeOff(altInMeters, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = (await _taskCompletionSource.Task) as CommandLongPacket;
        await tcs2.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(
            Math.Abs(altInMeters - packetFromServer!.Payload.Param7) <= double.Epsilon
        );
    }
    
    [Fact]
    public async Task QTakeOff_WithNaN_Success()
    {
        // Arrange
        var called = 0;
        IProtocolMessage? packetFromClient = null;
        _server[MavCmd.MavCmdNavVtolTakeoff] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Server.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandAckPacket;
            });
        
        // Act
        await _client.QTakeOff(float.NaN, _cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.Equal(float.NaN, packetFromServer!.Payload.Param7);
    }
    
    [Fact]
    public async Task QTakeOff_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdNavVtolTakeoff] = (_, _, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.QTakeOff(10, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task QTakeoff_Unsupported_Throws()
    {
        // Arrange 
        var result = MavResult.MavResultAccepted;
        using var sub = Link.Client
            .OnRxMessage.FilterByType<CommandAckPacket>()
            .Subscribe(p =>
            {
                result = p.Payload.Result;
            });
        
        // Act
        var task = _client.QTakeOff(10, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<CommandException>(async () => await task);
        Assert.Equal(MavResult.MavResultUnsupported, result);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task GetHomePosition_SingleCall_Success()
    {
        // Arrange
        var called = 0;
        CommandAckPacket? packetFromClient = null;
        _server[MavCmd.MavCmdGetHomePosition] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Server.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandAckPacket;
            });
        
        // Act
        await _client.GetHomePosition(_cancellationTokenSource.Token);
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
    }
    
    [Fact]
    public async Task GetHomePosition_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdGetHomePosition] = (_, _, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.GetHomePosition(_cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task GetHomePosition_Unsupported_MavResultUnsupported()
    {
        // Act
        var t =  _client.GetHomePosition(_cancellationTokenSource.Token);
        
        // Assert

        await Assert.ThrowsAsync<CommandException>(async () => await t);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(NavVtolLandOptions.NavVtolLandOptionsDefault)]
    [InlineData(NavVtolLandOptions.NavVtolLandOptionsFwDescent)]
    [InlineData(NavVtolLandOptions.NavVtolLandOptionsHoverDescent)]
    public async Task QLand_DifferentLandOptions_Success(NavVtolLandOptions navVtolLandOptions)
    {
        // Arrange
        var called = 0;
        IProtocolMessage? packetFromClient = null;
        _server[MavCmd.MavCmdNavVtolLand] = (_, args, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Server.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandAckPacket;
            });
        
        // Act
        await _client.QLand(
            navVtolLandOptions, 
            10,
            _cancellationTokenSource.Token
        );
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(
            Math.Abs((float)navVtolLandOptions - packetFromServer!.Payload.Param1) <= float.Epsilon
        );
        Assert.True(
            Math.Abs(10 - packetFromServer.Payload.Param3) <= float.Epsilon
        );
    }
    
    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    public async Task QLand_DifferentApproachAlt_Success(float approachAlt)
    {
        // Arrange
        var called = 0;
        IProtocolMessage? packetFromClient = null;
        _server[MavCmd.MavCmdNavVtolLand] = (id, args, cancel) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            _taskCompletionSource.TrySetResult(args);
            return Task.FromResult(CommandResult.FromResult(result));
        };
        using var sub = Link.Server.OnTxMessage
            .Cast<IProtocolMessage,MavlinkMessage>()
            .Subscribe(p => 
            { 
                packetFromClient = p as CommandAckPacket;
            });
        
        // Act
        await _client.QLand(
            NavVtolLandOptions.NavVtolLandOptionsFwDescent, 
            approachAlt,
            _cancellationTokenSource.Token
        );
        
        // Assert
        var packetFromServer = await _taskCompletionSource.Task as CommandLongPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.NotNull(packetFromClient);
        Assert.NotNull(packetFromServer);
        Assert.True(
            Math.Abs(
                (float)NavVtolLandOptions.NavVtolLandOptionsFwDescent 
                - packetFromServer!.Payload.Param1
            ) <= float.Epsilon
        );
        Assert.True(
            Math.Abs(approachAlt - packetFromServer.Payload.Param3) <= float.Epsilon
        );
    }
    
    [Fact]
    public async Task QLand_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        _server[MavCmd.MavCmdNavVtolLand] = (_, _, _) =>
        {
            called++;
            var result = MavResult.MavResultAccepted;
            return Task.FromResult(CommandResult.FromResult(result));
        };
        
        // Act
        var task = _client.QLand(
            NavVtolLandOptions.NavVtolLandOptionsDefault, 
            10, 
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task QLand_Timeout_Throws()
    {
        Link.SetClientToServerFilter(x=>false);
        
        
        
        // Act
        var task = _client.QLand(
            NavVtolLandOptions.NavVtolLandOptionsDefault, 
            10, 
            _cancellationTokenSource.Token
        );
        var t2 = Task.Factory.StartNew(() =>
        {
            while (task.IsCompleted == false)
            {
                ClientTime.Advance(
                    TimeSpan.FromMilliseconds(
                        _commandConfig.CommandTimeoutMs *
                        _commandConfig.CommandAttempt *
                        2
                    )
                );    
            }
            
        }, TaskCreationOptions.LongRunning);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Task.WhenAll(task, t2);
        });
    }
    
    [Fact]
    public async Task QLand_Unsupported_MavResultUnsupported()
    {
        // Act
        var t = _client.QLand(
            NavVtolLandOptions.NavVtolLandOptionsDefault, 
            10, 
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<CommandException>(async () => await t);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ArmedTimer_SeveralTimerCalls_Success()
    {
        // Arrange
        var called = 0;
        var times = new List<TimeSpan>();
        _heartBeatServer.Start();
        
        using var sub = _client.ArmedTime.Subscribe(p =>
        {
            called++;
            times.Add(p);

            if (called >= 5)
            {
                _taskCompletionSource.TrySetResult(new CommandAckPacket());
            }
        });
        
        // Act
        _heartBeatServer.Set(pld =>
        {
            pld.SystemStatus = MavState.MavStateActive;
            pld.BaseMode = MavModeFlag.MavModeFlagSafetyArmed;
            pld.Autopilot = MavAutopilot.MavAutopilotGeneric;
            pld.CustomMode = 123U;
            pld.MavlinkVersion = 3;
        });
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        _heartBeatServer.Set(pld =>
        {
            pld.SystemStatus = MavState.MavStateCalibrating;
            pld.BaseMode = MavModeFlag.MavModeFlagTestEnabled;
            pld.Autopilot = MavAutopilot.MavAutopilotGeneric;
            pld.CustomMode = 123U;
            pld.MavlinkVersion = 3;
        });
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        _heartBeatServer.Set(pld =>
        {
            pld.SystemStatus = MavState.MavStateCalibrating;
            pld.BaseMode = MavModeFlag.MavModeFlagTestEnabled;
            pld.Autopilot = MavAutopilot.MavAutopilotGeneric;
            pld.CustomMode = 123U;
            pld.MavlinkVersion = 3;
        });
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        _heartBeatServer.Set(pld =>
        {
            pld.SystemStatus = MavState.MavStateCalibrating;
            pld.BaseMode = MavModeFlag.MavModeFlagSafetyArmed;
            pld.Autopilot = MavAutopilot.MavAutopilotGeneric;
            pld.CustomMode = 123U;
            pld.MavlinkVersion = 3;
        });
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(5, called);
        Assert.Equal(0 , (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal(4, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        foreach (var t in times)
        {
            Log.WriteLine($"Time == {t}");
        }
    }
}