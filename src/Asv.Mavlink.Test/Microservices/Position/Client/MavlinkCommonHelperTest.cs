using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Position.Client;

[TestSubject(typeof(PositionClient))]
public class MavlinkCommonHelperTest : ClientTestBase<PositionClient>
{
    private readonly PositionClient _client;
    private readonly TaskCompletionSource<IPacketV2<IPayload>> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public MavlinkCommonHelperTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IPacketV2<IPayload>>();
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
        float.MaxValue
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
        float.MinValue
    )]
    [InlineData(
        uint.MinValue,
        MavFrame.MavFrameGlobal, 
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
        float.NaN
    )]
    [InlineData(
        uint.MinValue,
        MavFrame.MavFrameGlobal, 
        null, 
        null, 
        null,
        null, 
        null, 
        null, 
        null, 
        null, 
        null,
        null,
        null
    )]
    [InlineData(
        uint.MaxValue,
        MavFrame.MavFrameLocalFlu, 
        null, 
        null, 
        null,
        null, 
        null, 
        null, 
        null, 
        null, 
        null,
        null,
        null
    )]
    public async Task SetTargetGlobalInt_SinglePacket_Success
    (
        uint timeBootMs, 
        MavFrame coordinateFrame, 
        int? latInt, 
        int? lonInt, 
        float? alt,
        float? vx, 
        float? vy, 
        float? vz, 
        float? afx, 
        float? afy, 
        float? afz, 
        float? yaw,
        float? yawRate
    )
    {
        // Arrange
        var called = 0;
        SetPositionTargetGlobalIntPacket? packetFromClient = null;
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
        await _client.SetTargetGlobalInt(
            timeBootMs, 
            coordinateFrame,
            _cancellationTokenSource.Token,
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
            yawRate
        );
        
        // Assert
        var result = await _taskCompletionSource.Task as SetPositionTargetGlobalIntPacket;
        Assert.NotNull(result);
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(timeBootMs, packetFromClient?.Payload.TimeBootMs);
        Assert.Equal(coordinateFrame, packetFromClient?.Payload.CoordinateFrame);
        Assert.Equal(latInt ?? 0, packetFromClient?.Payload.LatInt);
        Assert.Equal(lonInt ?? 0, packetFromClient?.Payload.LonInt);
        Assert.Equal(alt ?? 0, packetFromClient?.Payload.Alt);
        Assert.Equal(vx ?? 0, packetFromClient?.Payload.Vx);
        Assert.Equal(vy ?? 0, packetFromClient?.Payload.Vy);
        Assert.Equal(vz ?? 0, packetFromClient?.Payload.Vz);
        Assert.Equal(afx ?? 0, packetFromClient?.Payload.Afx);
        Assert.Equal(afy ?? 0, packetFromClient?.Payload.Afy);
        Assert.Equal(afz ?? 0, packetFromClient?.Payload.Afz);
        Assert.Equal(yaw ?? 0, packetFromClient?.Payload.Yaw);
        Assert.Equal(yawRate ?? 0, packetFromClient?.Payload.YawRate);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SetTargetGlobalInt_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        SetPositionTargetGlobalIntPacket? packetFromClient = null;
        using var sub1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetGlobalIntPacket;
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>( async () =>
        {
            await _client.SetTargetGlobalInt(
                10, 
                MavFrame.MavFrameLocalFlu,
                _cancellationTokenSource.Token
            );
        });
        Assert.Null(packetFromClient);
        Assert.Equal(0, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
    }
    
    [Theory]
    [InlineData(
        uint.MaxValue,
        MavFrame.MavFrameLocalFlu, 
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
    [InlineData(
        uint.MinValue,
        MavFrame.MavFrameGlobal, 
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
        float.NaN
    )]
    [InlineData(
        uint.MinValue,
        MavFrame.MavFrameReserved17, 
        null, 
        null, 
        null,
        null, 
        null, 
        null, 
        null, 
        null, 
        null, 
        null,
        null
    )]
    public async Task SetPositionTargetLocalNed_SinglePacket_Success
    (
        uint timeBootMs, 
        MavFrame coordinateFrame, 
        float? x, 
        float? y,
        float? z,
        float? vx, 
        float? vy, 
        float? vz, 
        float? afx, 
        float? afy, 
        float? afz, 
        float? yaw,
        float? yawRate
    )
    {
        // Arrange
        var called = 0;
        SetPositionTargetLocalNedPacket? packetFromClient = null;
        using var sub1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetLocalNedPacket;
        });
        
        // Act
        await _client.SetPositionTargetLocalNed(
            timeBootMs, 
            coordinateFrame,
            _cancellationTokenSource.Token,
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
            yawRate
        );
        
        // Assert
        var result = await _taskCompletionSource.Task as SetPositionTargetLocalNedPacket;
        Assert.NotNull(result);
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(timeBootMs, packetFromClient?.Payload.TimeBootMs);
        Assert.Equal(coordinateFrame, packetFromClient?.Payload.CoordinateFrame);
        Assert.Equal(x ?? 0, packetFromClient?.Payload.X);
        Assert.Equal(y ?? 0, packetFromClient?.Payload.Y);
        Assert.Equal(z ?? 0, packetFromClient?.Payload.Z);
        Assert.Equal(vx ?? 0, packetFromClient?.Payload.Vx);
        Assert.Equal(vy ?? 0, packetFromClient?.Payload.Vy);
        Assert.Equal(vz ?? 0, packetFromClient?.Payload.Vz);
        Assert.Equal(afx ?? 0, packetFromClient?.Payload.Afx);
        Assert.Equal(afy ?? 0, packetFromClient?.Payload.Afy);
        Assert.Equal(afz ?? 0, packetFromClient?.Payload.Afz);
        Assert.Equal(yaw ?? 0, packetFromClient?.Payload.Yaw);
        Assert.Equal(yawRate ?? 0, packetFromClient?.Payload.YawRate);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SetPositionTargetLocalNed_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        SetPositionTargetLocalNedPacket? packetFromClient = null;
        using var sub1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromClient = p as SetPositionTargetLocalNedPacket;
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>( async () =>
        {
            await _client.SetPositionTargetLocalNed(
                123,
                MavFrame.MavFrameLocalFrd, 
                _cancellationTokenSource.Token
            );
        });
        Assert.Null(packetFromClient);
        Assert.Equal(0, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(0, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
    }
}