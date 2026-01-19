using System;
using Asv.Common;
using Asv.IO;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(IClientDevice))]
[TestSubject(typeof(IServerDevice))]
public class ComplexDeviceTest(ITestOutputHelper log)
    : ComplexTestBase<IClientDevice, IServerDevice>(log)
{
    private readonly MavlinkHeartbeatServerConfig _serverHb = new()
    {
        HeartbeatRateMs = 1000,
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
    
    [LocalTheory]
    [InlineData(1000, 100000)]
    [InlineData(1500, 20000)]
    [InlineData(2000, 2000)]
    public void Start_FullLinkLifeCycle_Success(int heartbeatRate, int heartbeatTimeoutMs)
    {
        // Arrange
        MavlinkHeartbeatServerConfig serverHb = new()
        {
            HeartbeatRateMs = heartbeatRate,
        };
    
        MavlinkClientDeviceConfig clientConfig = new()
        {
            Heartbeat =
            {
                HeartbeatTimeoutMs = heartbeatTimeoutMs,
                LinkQualityWarningSkipCount = 3,
                RateMovingAverageFilter = 10,
                PrintStatisticsToLogDelayMs = 10_000,
                PrintLinkStateToLog = true
            }
        };
        
        var server = ServerDevice.Create(
            Identity.Target, 
            ServerCore,
            builder =>
            {
                builder.RegisterHeartbeat(serverHb);
            }
        );
        
        using var client = new MavlinkClientDevice(new MavlinkClientDeviceId("TEST", Identity), clientConfig, [], ClientCore);
        
        client.Initialize();
        while (client.State.CurrentValue != ClientDeviceState.Complete)
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(10));
        }
        
        var heartbeatClient = client.GetMicroservice<IHeartbeatClient>() 
                              ?? throw new Exception("Microservice should not be null");
        
        // Act + Assert
        Assert.Equal(LinkState.Disconnected, heartbeatClient.Link.State.CurrentValue);
        
        server.Start();
        ServerTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        ClientTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        Assert.Equal(LinkState.Connected, heartbeatClient.Link.State.CurrentValue);
        
        server.Dispose();
        ServerTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        ClientTime.Advance(TimeSpan.FromMilliseconds(clientConfig.Heartbeat.HeartbeatTimeoutMs));
        Assert.Equal(LinkState.Downgrade, heartbeatClient.Link.State.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        ClientTime.Advance(TimeSpan.FromMilliseconds(clientConfig.Heartbeat.HeartbeatTimeoutMs));
        Assert.Equal(LinkState.Disconnected, heartbeatClient.Link.State.CurrentValue);
        
        ServerTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        ClientTime.Advance(TimeSpan.FromMilliseconds(clientConfig.Heartbeat.HeartbeatTimeoutMs));
        Assert.Equal(LinkState.Disconnected, heartbeatClient.Link.State.CurrentValue);
    }
    
    protected override IServerDevice CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        return ServerDevice.Create(identity, core, builder =>
        {
            builder.RegisterHeartbeat(_serverHb);
        });
    }

    protected override IClientDevice CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var client = new MavlinkClientDevice(new MavlinkClientDeviceId("TEST",identity), _clientConfig, [], core);
        return client;
    }
}