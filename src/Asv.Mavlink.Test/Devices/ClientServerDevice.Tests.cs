using System;
using System.Collections.Immutable;
using Asv.Common;
using Asv.IO;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkClientDevice))]
[TestSubject(typeof(IServerDevice))]
public class ComplexDeviceTests(ITestOutputHelper log)
    : ComplexTestBase<MavlinkClientDevice, IServerDevice>(log)
{
    private readonly MavlinkHeartbeatServerConfig _serverHb = new()
    {
        HeartbeatRateMs = 1000,
    };
    
    private StatusTextLoggerConfig _serverStatusText = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 10
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

    protected override IServerDevice CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        return ServerDevice.Create(identity, core, builder =>
        {
            builder.RegisterHeartbeat(_serverHb);
        });
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
        Assert.Equal(LinkState.Disconnected,Client.GetMicroservice<IHeartbeatClient>()!.Link.State.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Server.Dispose();
        Assert.Equal(LinkState.Connected,Client.GetMicroservice<IHeartbeatClient>()!.Link.State.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(LinkState.Downgrade,Client.GetMicroservice<IHeartbeatClient>()!.Link.State.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        
        Assert.Equal(LinkState.Downgrade,Client.GetMicroservice<IHeartbeatClient>()!.Link.State.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(LinkState.Disconnected,Client.GetMicroservice<IHeartbeatClient>()!.Link.State.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        
        Assert.Equal(LinkState.Disconnected,Client.GetMicroservice<IHeartbeatClient>()!.Link.State.CurrentValue);
    }

    
}