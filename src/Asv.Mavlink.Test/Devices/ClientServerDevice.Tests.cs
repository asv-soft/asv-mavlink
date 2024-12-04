using System;
using Asv.Common;
using Asv.IO;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkClientDevice))]
[TestSubject(typeof(MavlinkSer))]
public class ComplexDeviceTests(ITestOutputHelper log)
    : ComplexTestBase<ClientDevice, ServerDevice>(log)
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

    protected override ServerDevice CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        return new ServerDevice(identity,_serverConfig,core);
    }

    protected override ClientDevice CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new ClientDevice(identity,_clientConfig,core, DeviceClass.Copter);
    }
    
    [Fact]
    public void HeartbeatClientConnectToServer_Success()
    {
        var client = Client; // to ensure that client is created
        Server.Start();
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Server.Dispose();
        Assert.Equal(LinkState.Connected,Client.Heartbeat.Link.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(LinkState.Downgrade,Client.Heartbeat.Link.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        
        Assert.Equal(LinkState.Downgrade,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.CurrentValue);
    }

    
}