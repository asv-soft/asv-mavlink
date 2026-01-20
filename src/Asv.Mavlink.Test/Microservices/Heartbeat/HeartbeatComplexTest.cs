using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Minimal;
using FluentAssertions;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatComplexTest: ComplexTestBase<HeartbeatClient, HeartbeatServer>
{
    private readonly HeartbeatClientConfig _clientConfig = new();
    private readonly MavlinkHeartbeatServerConfig _serverConfig = new();
    private readonly CancellationTokenSource _cancellationTokenSource;

    public HeartbeatComplexTest(ITestOutputHelper output) : base(output)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task Start_SendAndCatchHeartbeat_Success()
    {
        // Arrange
        _ = Client;
        
        var expectedHeartbeatPayload = new HeartbeatPayload
        {
            Autopilot = MavAutopilot.MavAutopilotGeneric,
            BaseMode = MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMode = 123U,
            SystemStatus = MavState.MavStateActive,
            MavlinkVersion = 3
        };
        
        Server.Set(p=>
        {
            p.Autopilot = expectedHeartbeatPayload.Autopilot;
            p.BaseMode = expectedHeartbeatPayload.BaseMode;
            p.CustomMode = expectedHeartbeatPayload.CustomMode;
            p.SystemStatus = expectedHeartbeatPayload.SystemStatus;
            p.MavlinkVersion = expectedHeartbeatPayload.MavlinkVersion;
        });

        var serverSentPacketTcs = new TaskCompletionSource<HeartbeatPacket>();
        using var sub1 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            serverSentPacketTcs.TrySetResult((HeartbeatPacket)p);
        });
        
        var clientReceivedPacketTcs = new TaskCompletionSource<HeartbeatPacket>();
        using var sub2 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            clientReceivedPacketTcs.TrySetResult((HeartbeatPacket)p);
        });
        
        // Act
        Server.Start();
        
        ServerTime.Advance(TimeSpan.FromSeconds(_clientConfig.HeartbeatTimeoutMs / 2));

        var serverSentPacket = await serverSentPacketTcs.Task;
        var clientReceivedPacket = await clientReceivedPacketTcs.Task;
        
        // Assert
        await Client.Link.State.FirstAsync(x => x == LinkState.Connected, _cancellationTokenSource.Token);

        expectedHeartbeatPayload
            .Should().BeEquivalentTo(clientReceivedPacket.Payload);

        expectedHeartbeatPayload
            .Should().BeEquivalentTo(serverSentPacket.Payload);
    }
    
    protected override HeartbeatServer CreateServer(MavlinkIdentity identity, IMavlinkContext core) => 
        new(identity, _serverConfig, core);
    
    protected override HeartbeatClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core) => 
        new(identity, _clientConfig, core);
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }
        
        base.Dispose(disposing);
    }
}