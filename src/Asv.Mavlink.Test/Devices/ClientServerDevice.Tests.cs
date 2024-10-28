using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ClientDevice))]
[TestSubject(typeof(ServerDevice))]
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

    private readonly ClientDeviceConfig _clientConfig = new()
    {
        Heartbeat =
        {
            HeartbeatTimeoutMs = 2000,
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
    public async Task HeartbeatClientConnectToServer_Success()
    {
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        await Client.WaitUntilConnect();
        await Server.DisposeAsync();
        Assert.Equal(LinkState.Connected,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        Assert.Equal(LinkState.Downgrade,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        Assert.Equal(LinkState.Downgrade,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.CurrentValue);
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        Assert.Equal(LinkState.Disconnected,Client.Heartbeat.Link.CurrentValue);
    }

    
}