using System;
using System.Collections.Immutable;
using Asv.Common;
using Asv.IO;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkClientDevice))]
[TestSubject(typeof(ServerDevice))]
public class ComplexDeviceTests(ITestOutputHelper log)
    : ComplexTestBase<MavlinkClientDevice, ServerDevice>(log)
{
    private readonly ServerDeviceConfig _serverConfig = new()
    {
        Heartbeat =
        {
            HeartbeatRateMs = 1000,
        },
        StatusText =
        {
            MaxQueueSize = 100,
            MaxSendRateHz = 10
        }

    };

    private readonly MavlinkClientDeviceConfig _clientConfig = new()
    {
        Heartbeat =
        {
            HeartbeatTimeoutMs = 1000,
            LinkQualityWarningSkipCount = 3,
            RateMovingAverageFilter = 10,
            PrintStatisticsToLogDelayMs = 10_000,
            PrintLinkStateToLog = true
        }
    };

    protected override ServerDevice CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        return new ServerDevice(identity,_serverConfig,core);
    }

    protected override MavlinkClientDevice CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new MavlinkClientDevice(new MavlinkClientDeviceId("TEST",identity),_clientConfig, [], core);
    }
    
    [Fact(Skip = "Test execution is not stable")]
    public void HeartbeatClientConnectToServer_Success()
    {
        var client = Client; // to ensure that client is created
        Server.Start();
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.State.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Server.Dispose();
        Assert.Equal(LinkState.Connected,Client.Heartbeat.Link.State.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(LinkState.Downgrade,Client.Heartbeat.Link.State.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        
        Assert.Equal(LinkState.Downgrade,Client.Heartbeat.Link.State.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.State.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.State.CurrentValue);
    }

    
}