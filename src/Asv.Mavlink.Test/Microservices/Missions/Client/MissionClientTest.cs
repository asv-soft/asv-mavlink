using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MissionClient))]
public class MissionClientTest : ClientTestBase<MissionClient>
{
    private readonly MissionClientConfig _config = new()
    {
        CommandTimeoutMs = 1000,
        AttemptToCallCount = 5
    };
    
    private readonly TaskCompletionSource<IPacketV2<IPayload>> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MissionClient _client;

    public MissionClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<IPacketV2<IPayload>>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override MissionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) 
        => new(identity, _config, core);

    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MissionClient(null!, _config, Core));
        Assert.Throws<ArgumentNullException>(() => new MissionClient(Identity, null!, Core));
        Assert.Throws<ArgumentNullException>(() => new MissionClient(Identity, _config, null!));
    }
    
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
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _client.SendMissionAck(missionResult, _cancellationTokenSource.Token);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(Link.Client.RxPackets, Link.Server.TxPackets);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeAll)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    [InlineData(null)]
    public async Task SendMissionAck_DifferentTypes_Success(MavMissionType? missionType)
    {
        // Arrange
        var called = 0;
        MissionAckPacket? packetFromServer = null;
        using var s1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;
            _taskCompletionSource.TrySetResult(p);
        });
        using var s2 = Link.Client.TxPipe.Subscribe(p =>
        {
            packetFromServer = p as MissionAckPacket;
        });
        
        // Act
        await _client.SendMissionAck(MavMissionResult.MavMissionAccepted, type: missionType);

        // Assert
        var result = await _taskCompletionSource.Task as MissionAckPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(Link.Client.RxPackets, Link.Server.TxPackets);
        Assert.NotNull(result);
        Assert.NotNull(packetFromServer);
        Assert.True(packetFromServer.IsDeepEqual(result));
    }
    
    [Fact]
    public async Task SendMissionAck_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        await _cancellationTokenSource.CancelAsync();
        using var s1 = Link.Server.RxPipe.Subscribe(p =>
        {
            called++;
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => 
            await _client.SendMissionAck(
                MavMissionResult.MavMissionAccepted, 
                _cancellationTokenSource.Token, 
                type: MavMissionType.MavMissionTypeMission
            )
        );
        Assert.Equal(0, called);
        Assert.Equal(called, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
        Assert.Equal(Link.Client.RxPackets, Link.Server.TxPackets);
    }
}