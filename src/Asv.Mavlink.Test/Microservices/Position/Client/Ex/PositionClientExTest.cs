using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Position.Client;

[TestSubject(typeof(PositionClientEx))]
public sealed class PositionClientExTest : ClientTestBase<PositionClientEx>
{
    private readonly HeartbeatClientConfig _heartbeatConfig = new()
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
    
    private readonly PositionClientEx _client;
    private readonly TaskCompletionSource<IPacketV2<IPayload>> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public PositionClientExTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IPacketV2<IPayload>>();
        _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(200), 
            TimeProvider.System
        );
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override PositionClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var positionClient = new PositionClient(identity, core);
        var heartBeatClient = new HeartbeatClient(identity, _heartbeatConfig, core);
        var commandClient = new CommandClient(identity, _commandConfig, core);
        return new PositionClientEx(positionClient, heartBeatClient, commandClient);
    }

    [Fact]
    public async Task Init_ProperInput_Success()
    {
        await _client.Init();
    }

    [Theory]
    [InlineData(
        double.MaxValue,
        double.MaxValue,
        double.MaxValue
    )]
    [InlineData(
        double.MinValue,
        double.MinValue,
        double.MinValue
    )]
    [InlineData(
        double.NaN,
        double.NaN,
        double.NaN
    )]
    public async Task SetTarget_DifferentLatLonAlt_Success(
        double latitude,
        double longitude,
        double altitude
    )
    {
        // Arrange
        var called = 0;
        SetPositionTargetGlobalIntPacket? packetFromClient = null;
        var gPoint = new GeoPoint(
            latitude, 
            longitude, 
            altitude
        );
        using var sub1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetGlobalIntPacket;
        });
        
        // Act
        await _client.SetTarget(gPoint, _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as SetPositionTargetGlobalIntPacket;
        Assert.NotNull(result);
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SetTarget_DifferentStaticGeopoints_Success()
    {
        // Arrange
        var called = 0;
        var packetsFromServer = new List<SetPositionTargetGlobalIntPacket>();
        var packetsFromClient = new List<SetPositionTargetGlobalIntPacket>();
        using var sub1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;

            if (p is SetPositionTargetGlobalIntPacket packet)
            {
                packetsFromServer.Add(packet);
            }

            if (called >= 3)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });
        using var sub2 = Link.Client.TxPipe.Subscribe(p =>
        {
            if (p is SetPositionTargetGlobalIntPacket packet)
            {
                packetsFromClient.Add(packet);
            }
        });
        
        // Act
        await _client.SetTarget(GeoPoint.NaN, _cancellationTokenSource.Token);
        await _client.SetTarget(GeoPoint.Zero, _cancellationTokenSource.Token);
        await _client.SetTarget(GeoPoint.ZeroWithAlt, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(3, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.NotEmpty(packetsFromServer);
        Assert.NotEmpty(packetsFromClient);
        Assert.Equal(packetsFromClient.Count, packetsFromServer.Count);
        Assert.True(packetsFromClient.IsDeepEqual(packetsFromServer));
    }
    
    [Fact]
    public async Task SetTarget_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        var gPoint = new GeoPoint(1, 2, 3);
        await _cancellationTokenSource.CancelAsync();
        using var sub1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async ()
            => await _client.SetTarget(gPoint, _cancellationTokenSource.Token)
        );
        Assert.Equal(0, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
    }
    
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
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInitPitch = true;
        var isInitPitchSpeed = true;
        var isInitRoll = true;
        var isInitRollSpeed = true;
        var isInitYaw = true;
        var isInitYawSpeed = true;
        
        var pitchResult = 0d;
        var pitchSpeedResult = 0d;
        var rollResult = 0d;
        var rollSpeedResult = 0d;
        var yawResult = 0d;
        var yawSpeedResult = 0d;
        
        var packet = new  AttitudePacket
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
        using var sub1 = _client.Pitch.Subscribe(p =>
        {
            if (isInitPitch)
            {
                isInitPitch = false;
                return;
            }

            called++;
            
            if (called >= 6)
            {
                tcs.TrySetResult();
            }
        });
        using var sub2 = _client.PitchSpeed.Subscribe(p =>
        {
            if (isInitPitchSpeed)
            {
                isInitPitchSpeed = false;
                return;
            }
            called++;
            
            if (called >= 6)
            {
                tcs.TrySetResult();
            }
        });
        using var sub3 = _client.Roll.Subscribe(p =>
        {
            if (isInitRoll)
            {
                isInitRoll = false;
                return;
            }
            called++;
            
            if (called >= 6)
            {
                tcs.TrySetResult();
            }
        });
        using var sub4 = _client.RollSpeed.Subscribe(p =>
        {
            if (isInitRollSpeed)
            {
                isInitRollSpeed = false;
                return;
            }
            called++;
            
            if (called >= 6)
            {
                tcs.TrySetResult();
            }
        });
        using var sub5 = _client.Yaw.Subscribe(p =>
        {
            if (isInitYaw)
            {
                isInitYaw = false;
                return;
            }
            called++;
            
            if (called >= 6)
            {
                tcs.TrySetResult();
            }
        });
        using var sub6 = _client.YawSpeed.Subscribe(p =>
        {
            if (isInitYawSpeed)
            {
                isInitYawSpeed = false;
                return;
            }
            called++;
            
            if (called >= 6)
            {
                tcs.TrySetResult();
            }
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        await tcs.Task;
        Assert.Equal(6, called);
        Assert.Equal(1, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.NotEqual(double.NaN, pitchResult);
        Assert.NotEqual(double.NaN, pitchSpeedResult);
        Assert.NotEqual(double.NaN, rollResult);
        Assert.NotEqual(double.NaN, rollSpeedResult);
        Assert.NotEqual(double.NaN, yawResult);
        Assert.NotEqual(double.NaN, yawSpeedResult);
    }
    
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
        PositionTargetTypemask.PositionTargetTypemaskYawRateIgnore
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
        PositionTargetTypemask.PositionTargetTypemaskXIgnore
    )]
    public async Task ReceiveTarget_SinglePacketMavFrameGlobal_Success(
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
        PositionTargetTypemask typeMask
    )
    {
        // Arrange
        var tcs = new TaskCompletionSource<GeoPoint>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var expectedGPoint = new GeoPoint(
            MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(latInt),
            MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(lonInt),
            alt
        );
        var packet = new  PositionTargetGlobalIntPacket
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
                CoordinateFrame = MavFrame.MavFrameGlobal
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
            
            tcs.TrySetResult(p!.Value);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.Equal(expectedGPoint, result);
    }
    
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
        var tcs = new TaskCompletionSource<GeoPoint>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var expectedGPoint = new GeoPoint(
            MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(latitude),
            MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(longitude),
            MavlinkTypesHelper.AltFromMmToDoubleMeter(altitude)
        );
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
            
            tcs.TrySetResult(p!.Value);
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        var result = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.Equal(expectedGPoint, result);
    }
    
    [Theory] //TODO: fix
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
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInitCurrent = true;
        var isInitAltitudeAboveHome = true;
        var result = GeoPoint.Zero;
        var expectedGPoint = new GeoPoint(
            MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(lat),
            MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(lon),
            MavlinkTypesHelper.AltFromMmToDoubleMeter(alt)
        );
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
        using var sub1 = _client.Current.Subscribe(p =>
        {
            if (isInitCurrent)
            {
                isInitCurrent = false;
                return;
            }
            called++;

            result = p;

            if (called >= 2)
            {
                tcs.TrySetResult();
            }
        });
        using var sub2 = _client.AltitudeAboveHome.Subscribe(p =>
        {
            if (isInitAltitudeAboveHome)
            {
                isInitAltitudeAboveHome = false;
                return;
            }
            called++;

            if (called >= 2)
            {
                tcs.TrySetResult();
            }
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        await tcs.Task;
        Assert.Equal(2, called);
        Assert.Equal(1, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.Equal(expectedGPoint, result);
        Assert.NotEqual(double.NaN, _client.HomeDistance.CurrentValue);
        Assert.NotEqual(double.NaN, _client.AltitudeAboveHome.CurrentValue);
    }

    [Fact]
    public async Task ReceiveIsArmed_SinglePacketWIthTrueResult_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet = new HeartbeatPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                BaseMode = MavModeFlag.MavModeFlagSafetyArmed,
            }
        };
        using var sub1 = _client.IsArmed.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            
            tcs.TrySetResult();
        });
        
        // Act
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.True(_client.IsArmed.CurrentValue);
    }
    
    [Fact]
    public async Task ReceiveIsArmed_SinglePacketWIthFalseResult_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packetWithTrue = new HeartbeatPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                BaseMode = MavModeFlag.MavModeFlagSafetyArmed,
            }
        };
        var packet = new HeartbeatPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                BaseMode = MavModeFlag.MavModeFlagHilEnabled,
            }
        };
        using var sub1 = _client.IsArmed.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            if (called >= 2)
            {
                tcs.TrySetResult();
            }
        });
        
        // Act
        await Link.Server.Send(packetWithTrue, cancel.Token);
        await Link.Server.Send(packet, cancel.Token);

        // Assert
        await tcs.Task;
        Assert.Equal(2, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.False(_client.IsArmed.CurrentValue);
    }
    
    [Fact]
    public async Task ReceiveTargetDistance_TwoPackets_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());
        
        var called = 0;
        var isInit = true;
        var packet1 = new GlobalPositionIntPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = 10,
                Lat = 12, 
                Lon = 14, 
                Alt = 42, 
                RelativeAlt = 30, 
                Vx = 11, 
                Vy = 3, 
                Vz = 1, 
                Hdg = 222,
            }
        };
        var packet2 = new  PositionTargetGlobalIntPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                TimeBootMs = 12,
                LatInt = 66,
                LonInt = 12,
                Alt = 1,
                Vx = 0.23f,
                Vy = 23.4f,
                Vz = 111.2f,
                Afx = 54.03f,
                Afy = 32.4f,
                Afz = 20.000032f,
                Yaw = 3.0f,
                YawRate = 12.4f,
                TypeMask = PositionTargetTypemask.PositionTargetTypemaskForceSet,
                CoordinateFrame = MavFrame.MavFrameGlobal
            }
        };
        using var sub = _client.TargetDistance.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult();
        });
        
        // Act
        await Link.Server.Send(packet1, cancel.Token);
        await Link.Server.Send(packet2, cancel.Token);

        // Assert
        await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(2, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Client.TxPackets);
        Assert.Equal(Link.Client.TxPackets, Link.Server.RxPackets);
        Assert.NotEqual(double.NaN, _client.TargetDistance.CurrentValue);
    }
}