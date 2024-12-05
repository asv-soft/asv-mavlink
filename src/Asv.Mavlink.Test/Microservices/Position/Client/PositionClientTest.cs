using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;

using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Position.Client;

[TestSubject(typeof(PositionClient))]
public class PositionClientTest : ClientTestBase<PositionClient>
{
    private readonly PositionClient _client;
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public PositionClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(200), 
            TimeProvider.System
        );
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    protected override PositionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) 
        => new(identity, core);

    [Theory]
    [InlineData(
        uint.MaxValue,
        MavFrame.MavFrameLocalFlu, 
        int.MaxValue, 
        int.MaxValue, 
        float.MaxValue,
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue,
        float.MaxValue, 
        PositionTargetTypemask.PositionTargetTypemaskYawRateIgnore
    )]
    [InlineData(
        uint.MinValue,
        MavFrame.MavFrameGlobal, 
        int.MinValue, 
        int.MinValue, 
        float.MinValue,
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue,
        float.MinValue, 
        PositionTargetTypemask.PositionTargetTypemaskXIgnore
    )]
    public async Task SetTargetGlobalInt_SinglePacket_Success
    (
        uint timeBootMs, 
        MavFrame coordinateFrame, 
        int latInt, 
        int lonInt, 
        float alt,
        float vx, 
        float vy, 
        float vz, 
        float afx, 
        float afy, 
        float afz, 
        float yaw,
        float yawRate, 
        PositionTargetTypemask typeMask
    )
    {
        // Arrange
        var called = 0;
        SetPositionTargetGlobalIntPacket? packetFromClient = null;
        using var sub1 = Link.Server.OnRxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Server.OnTxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetGlobalIntPacket;
        });
        
        // Act
        await _client.SetTargetGlobalInt(
            timeBootMs,
            coordinateFrame, 
            latInt, 
            lonInt, 
            alt,
            vx, 
            vy, 
            vz, 
            afx, 
            afy, 
            afz, 
            yaw,
            yawRate, 
            typeMask, 
            _cancellationTokenSource.Token
        );
        
        // Assert
        var result = await _taskCompletionSource.Task as SetPositionTargetGlobalIntPacket;
        Assert.NotNull(result);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(timeBootMs, packetFromClient?.Payload.TimeBootMs);
        Assert.Equal(coordinateFrame, packetFromClient?.Payload.CoordinateFrame);
        Assert.Equal(latInt, packetFromClient?.Payload.LatInt);
        Assert.Equal(lonInt, packetFromClient?.Payload.LonInt);
        Assert.Equal(alt, packetFromClient?.Payload.Alt);
        Assert.Equal(vx, packetFromClient?.Payload.Vx);
        Assert.Equal(vy, packetFromClient?.Payload.Vy);
        Assert.Equal(vz, packetFromClient?.Payload.Vz);
        Assert.Equal(afx, packetFromClient?.Payload.Afx);
        Assert.Equal(afy, packetFromClient?.Payload.Afy);
        Assert.Equal(afz, packetFromClient?.Payload.Afz);
        Assert.Equal(yaw, packetFromClient?.Payload.Yaw);
        Assert.Equal(yawRate, packetFromClient?.Payload.YawRate);
        Assert.Equal(typeMask, packetFromClient?.Payload.TypeMask);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SetTargetGlobalInt_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        SetPositionTargetGlobalIntPacket? packetFromClient = null;
        using var sub1 = Link.Server.OnRxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Server.OnTxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetGlobalIntPacket;
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>( async () =>
        {
            await _client.SetTargetGlobalInt(
                10,
                MavFrame.MavFrameMission,
                23,
                24,
                25f,
                53f,
                24.2f,
                45.1f,
                10.2f,
                14.23f,
                11.0f,
                49.22453f,
                45.0f,
                PositionTargetTypemask.PositionTargetTypemaskVyIgnore,
                _cancellationTokenSource.Token
            );
        });
        Assert.Null(packetFromClient);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(
        uint.MaxValue,
        MavFrame.MavFrameLocalFlu, 
        PositionTargetTypemask.PositionTargetTypemaskYawRateIgnore,
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue,
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue,
        float.MaxValue
    )]
    [InlineData(
        uint.MinValue,
        MavFrame.MavFrameGlobal, 
        PositionTargetTypemask.PositionTargetTypemaskXIgnore,
        float.MinValue, 
        float.MinValue, 
        float.MinValue,
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue,
        float.MinValue
    )]
    public async Task SetPositionTargetLocalNed_SinglePacket_Success
    (
        uint timeBootMs, 
        MavFrame coordinateFrame, 
        PositionTargetTypemask typeMask,
        float x, 
        float y,
        float z,
        float vx, 
        float vy, 
        float vz, 
        float afx, 
        float afy, 
        float afz, 
        float yaw,
        float yawRate
    )
    {
        // Arrange
        var called = 0;
        SetPositionTargetLocalNedPacket? packetFromClient = null;
        using var sub1 = Link.Server.OnRxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Server.OnTxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetLocalNedPacket;
        });
        
        // Act
        await _client.SetPositionTargetLocalNed(
            timeBootMs, 
            coordinateFrame, 
            typeMask, 
            x,
            y, 
            z, 
            vx, 
            vy, 
            vz, 
            afx, 
            afy, 
            afz, 
            yaw, 
            yawRate,
            _cancellationTokenSource.Token
        );
        
        // Assert
        var result = await _taskCompletionSource.Task as SetPositionTargetLocalNedPacket;
        Assert.NotNull(result);
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(timeBootMs, packetFromClient?.Payload.TimeBootMs);
        Assert.Equal(coordinateFrame, packetFromClient?.Payload.CoordinateFrame);
        Assert.Equal(typeMask, packetFromClient?.Payload.TypeMask);
        Assert.Equal(x, packetFromClient?.Payload.X);
        Assert.Equal(y, packetFromClient?.Payload.Y);
        Assert.Equal(z, packetFromClient?.Payload.Z);
        Assert.Equal(vx, packetFromClient?.Payload.Vx);
        Assert.Equal(vy, packetFromClient?.Payload.Vy);
        Assert.Equal(vz, packetFromClient?.Payload.Vz);
        Assert.Equal(afx, packetFromClient?.Payload.Afx);
        Assert.Equal(afy, packetFromClient?.Payload.Afy);
        Assert.Equal(afz, packetFromClient?.Payload.Afz);
        Assert.Equal(yaw, packetFromClient?.Payload.Yaw);
        Assert.Equal(yawRate, packetFromClient?.Payload.YawRate);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SetPositionTargetLocalNed_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        SetPositionTargetLocalNedPacket? packetFromClient = null;
        using var sub1 = Link.Server.OnRxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Server.OnTxMessage.Cast<IProtocolMessage,MavlinkMessage>().Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetLocalNedPacket;
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>( async () =>
        {
            await _client.SetPositionTargetLocalNed(
                123,
                MavFrame.MavFrameLocalFrd, 
                PositionTargetTypemask.PositionTargetTypemaskYawIgnore,
                1, 
                2, 
                3,
                4, 
                5, 
                6, 
                7, 
                8, 
                9, 
                10,
                11,
                _cancellationTokenSource.Token
            );
        });
        Assert.Null(packetFromClient);
        Assert.Equal(0, called);
        Assert.Equal(called, (int)Link.Server.Statistic.RxMessages);
        Assert.Equal((int)Link.Server.Statistic.RxMessages, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
    }

    #region GlobalPosition

    [Theory]
    [InlineData(
        uint.MaxValue,
        int.MaxValue,
        int.MaxValue,
        int.MaxValue,
        int.MaxValue,
        short.MaxValue,
        short.MaxValue,
        short.MaxValue,
        ushort.MaxValue
    )]
    [InlineData(
        uint.MinValue,
        int.MinValue,
        int.MinValue,
        int.MinValue,
        int.MinValue,
        short.MinValue,
        short.MinValue,
        short.MinValue,
        ushort.MinValue
    )]
    public async Task ReceiveGlobalPosition_SinglePacket_Success
    (
        uint timeBootMs,
        int lat, 
        int lon, 
        int alt, 
        int relativeAlt, 
        short vx, 
        short vy, 
        short vz, 
        ushort hdg
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<GlobalPositionIntPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new GlobalPositionIntPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = timeBootMs,
                Lat = lat, 
                Lon = lon, 
                Alt = alt, 
                RelativeAlt = relativeAlt, 
                Vx = vx, 
                Vy = vy, 
                Vz = vz, 
                Hdg = hdg,
            }
        };
        using var sub1 = _client.GlobalPosition.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveGlobalPosition_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<GlobalPositionIntPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<GlobalPositionIntPayload>();
        var packet = new GlobalPositionIntPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = 10,
                Lat = 21, 
                Lon = 34, 
                Alt = 45, 
                RelativeAlt = 34, 
                Vx = 9, 
                Vy = 10, 
                Vz = 11, 
                Hdg = 23,
            }
        };
        using var sub1 = _client.GlobalPosition.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }

    #endregion
    
    #region Home
    
    [Theory]
    [InlineData(
        int.MaxValue,
        int.MaxValue,
         int.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        ulong.MaxValue
    )]
    [InlineData(
        int.MinValue,
        int.MinValue,
        int.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MaxValue,
        ulong.MaxValue
    )]
    [InlineData(
        int.MinValue,
        int.MinValue,
        int.MinValue,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        ulong.MaxValue
    )]
    public async Task ReceiveHome_SinglePacket_Success(
        int latitude,
        int longitude,
        int altitude,
        float x,
        float y,
        float z,
        float approachX,
        float approachY,
        float approachZ,
        ulong timeUsec
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<HomePositionPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new HomePositionPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                Latitude = latitude,
                Longitude = longitude,
                Altitude = altitude,
                X = x,
                Y = y,
                Z = z,
                ApproachX = approachX,
                ApproachY = approachY,
                ApproachZ = approachZ,
                TimeUsec = timeUsec
            }
        };
        using var sub1 = _client.Home.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveHome_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<HomePositionPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<HomePositionPayload>();
        var packet = new HomePositionPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                Latitude = 10,
                Longitude = 11,
                Altitude = 12,
                X = 13,
                Y = 14,
                Z = 15,
                ApproachX = 16,
                ApproachY = 17,
                ApproachZ = 18,
                TimeUsec = 19
            }
        };
        using var sub1 = _client.Home.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    #endregion

    #region Target

    [Theory]
    [InlineData(
        uint.MaxValue, 
        int.MaxValue, 
        int.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue,
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        PositionTargetTypemask.PositionTargetTypemaskYawRateIgnore, 
        MavFrame.MavFrameLocalFlu
    )]
    [InlineData(
        uint.MinValue, 
        int.MinValue, 
        int.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue,
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        PositionTargetTypemask.PositionTargetTypemaskXIgnore, 
        MavFrame.MavFrameGlobal
    )]
    [InlineData(
        uint.MinValue, 
        int.MinValue, 
        int.MinValue, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN,
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        PositionTargetTypemask.PositionTargetTypemaskYIgnore, 
        MavFrame.MavFrameReserved17
    )]
    public async Task ReceiveTarget_SinglePacket_Success(
        uint timeBootMs,
        int latInt,
        int lonInt,
        float alt,
        float vx,
        float vy,
        float vz,
        float afx,
        float afy,
        float afz,
        float yaw,
        float yawRate,
        PositionTargetTypemask typeMask,
        MavFrame coordinateFrame
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<PositionTargetGlobalIntPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new PositionTargetGlobalIntPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                 TimeBootMs = timeBootMs,
                 LatInt = latInt,
                 LonInt = lonInt,
                 Alt = alt,
                 Vx = vx,
                 Vy = vy,
                 Vz = vz,
                 Afx = afx,
                 Afy = afy,
                 Afz = afz,
                 Yaw = yaw,
                 YawRate = yawRate,
                 TypeMask = typeMask,
                 CoordinateFrame = coordinateFrame,
            }
        };
        using var sub1 = _client.Target.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveTarget_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<PositionTargetGlobalIntPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<PositionTargetGlobalIntPayload>();
        var packet = new PositionTargetGlobalIntPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = 1,
                LatInt = 2,
                LonInt = 3,
                Alt = 4,
                Vx = 5,
                Vy = 6,
                Vz = 7,
                Afx = 8,
                Afy = 9,
                Afz = 10,
                Yaw = 11,
                YawRate = 12,
                TypeMask = PositionTargetTypemask.PositionTargetTypemaskForceSet,
                CoordinateFrame = MavFrame.MavFrameReserved13,
            }
        };
        using var sub1 = _client.Target.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }

    #endregion
    
    #region Altitude
    
    [Theory]
    [InlineData(
        ulong.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue
    )]
    [InlineData(
        ulong.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue
    )]
    [InlineData(
        ulong.MinValue, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN
    )]
    public async Task ReceiveAltitude_SinglePacket_Success(
        ulong timeUsec, 
        float altitudeMonotonic, 
        float altitudeAmsl, 
        float altitudeLocal,
        float altitudeRelative,
        float altitudeTerrain,
        float bottomClearance
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<AltitudePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new AltitudePacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                 TimeUsec = timeUsec, 
                 AltitudeMonotonic = altitudeMonotonic, 
                 AltitudeAmsl = altitudeAmsl, 
                 AltitudeLocal = altitudeLocal, 
                 AltitudeRelative = altitudeRelative, 
                 AltitudeTerrain = altitudeTerrain, 
                 BottomClearance = bottomClearance,
            }
        };
        using var sub1 = _client.Altitude.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveAltitude_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<AltitudePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<AltitudePayload>();
        var packet = new AltitudePacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeUsec = 1, 
                AltitudeMonotonic = 2, 
                AltitudeAmsl = 3, 
                AltitudeLocal = 4, 
                AltitudeRelative = 5, 
                AltitudeTerrain = 6, 
                BottomClearance = 7,
            }
        };
        using var sub1 = _client.Altitude.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    #endregion
    
    #region VfrHud

    [Theory]
    [InlineData(
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        float.MaxValue, 
        short.MaxValue, 
        ushort.MaxValue
    )]
    [InlineData(
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        float.MinValue, 
        short.MinValue, 
        ushort.MinValue
    )]
    [InlineData(
        float.NaN, 
        float.NaN, 
        float.NaN, 
        float.NaN, 
        short.MinValue, 
        ushort.MinValue
    )]
    public async Task ReceiveVfrHud_SinglePacket_Success(
        float airspeed, 
        float groundspeed, 
        float alt, 
        float climb, 
        short heading, 
        ushort throttle
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<VfrHudPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new VfrHudPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                Airspeed = airspeed, 
                Groundspeed = groundspeed, 
                Alt = alt, 
                Climb = climb, 
                Heading = heading, 
                Throttle = throttle,
            }
        };
        using var sub1 = _client.VfrHud.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveVfrHud_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<VfrHudPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<VfrHudPayload>();
        var packet = new VfrHudPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                Airspeed = 1, 
                Groundspeed = 2, 
                Alt = 3,
                Climb = 4,
                Heading = 9,
                Throttle = 19,
            }
        };
        using var sub1 = _client.VfrHud.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    #endregion
    
    #region Imu
    
    [Theory]
    [InlineData(
        ulong.MaxValue, 
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue, 
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        HighresImuUpdatedFlags.HighresImuUpdatedAll,
        byte.MaxValue
    )]
    [InlineData(
        ulong.MinValue, 
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue, 
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        HighresImuUpdatedFlags.HighresImuUpdatedNone,
        byte.MinValue
    )]
    [InlineData(
        ulong.MinValue, 
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN, 
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        HighresImuUpdatedFlags.HighresImuUpdatedYacc,
        byte.MinValue
    )]
    public async Task ReceiveImu_SinglePacket_Success(
        ulong timeUsec, 
        float xacc, 
        float yacc, 
        float zacc, 
        float xgyro, 
        float ygyro, 
        float zgyro, 
        float xmag, 
        float ymag, 
        float zmag, 
        float absPressure, 
        float diffPressure, 
        float pressureAlt, 
        float temperature, 
        HighresImuUpdatedFlags fieldsUpdated, 
        byte id
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<HighresImuPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new HighresImuPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeUsec = timeUsec, 
                Xacc = xacc,
                Yacc = yacc,
                Zacc = zacc,
                Xgyro = xgyro,
                Ygyro = ygyro,
                Zgyro = zgyro,
                Xmag = xmag,
                Ymag = ymag,
                Zmag = zmag,
                AbsPressure = absPressure,
                DiffPressure = diffPressure,
                PressureAlt = pressureAlt,
                Temperature = temperature,
                FieldsUpdated = fieldsUpdated,
                Id = id,
            }
        };
        using var sub1 = _client.Imu.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveImu_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<HighresImuPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<HighresImuPayload>();
        var packet = new HighresImuPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeUsec = 1, 
                Xacc = 2,
                Yacc = 3,
                Zacc = 4,
                Xgyro = 5,
                Ygyro = 6,
                Zgyro = 7,
                Xmag = 8,
                Ymag = 9,
                Zmag = 10,
                AbsPressure = 11,
                DiffPressure = 12,
                PressureAlt = 13,
                Temperature = 14,
                FieldsUpdated = HighresImuUpdatedFlags.HighresImuUpdatedAll,
                Id = 3,
            }
        };
        using var sub1 = _client.Imu.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    #endregion
    
    #region Attitude
    
    [Theory]
    [InlineData(
        uint.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue,
        float.MaxValue
    )]
    [InlineData(
        uint.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue,
        float.MinValue
    )]
    [InlineData(
        uint.MinValue,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN,
        float.NaN
    )]
    public async Task ReceiveAttitude_SinglePacket_Success(
        uint timeBootMs, 
        float roll, 
        float pitch, 
        float yaw, 
        float rollspeed, 
        float pitchspeed, 
        float yawspeed
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<AttitudePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new AttitudePacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = timeBootMs,
                Roll = roll,
                Pitch = pitch,
                Yaw = yaw,
                Rollspeed = rollspeed,
                Pitchspeed = pitchspeed,
                Yawspeed = yawspeed,
            }
        };
        using var sub1 = _client.Attitude.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult(p!);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.True(packet.Payload.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10_000)]
    public async Task ReceiveAttitude_SeveralPackets_Success(int packetsCount)
    {
        // Arrange
        var tcs = new TaskCompletionSource<AttitudePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var payloads = new List<AttitudePayload>();
        var packet = new AttitudePacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = 10,
                Roll = 12,
                Pitch = 3,
                Yaw = 15,
                Rollspeed = 5,
                Pitchspeed = 90,
                Yawspeed = 112,
            }
        };
        using var sub1 = _client.Attitude.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);
            
            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });
        
        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal((int)Link.Server.Statistic.TxMessages, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    #endregion
}