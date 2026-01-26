using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.XUnit;
using FluentAssertions;
using JetBrains.Annotations;
using R3;
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
    [InlineData(1000, 1000)]
    [InlineData(10, 1000)]
    public async Task Start_FullLinkLifeCycle_Success(int heartbeatRate, int heartbeatTimeoutMs)
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
        
        var tcs = new TaskCompletionSource();

        var expectedStates = new List<LinkState>()
        {
            LinkState.Disconnected,
            LinkState.Connected,
            LinkState.Downgrade,
            LinkState.Disconnected,
        };
        var states = new List<LinkState>();
        
        using var sub = heartbeatClient.Link.State.Synchronize().Subscribe(state =>
        {
            states.Add(state);

            if (states.Count == expectedStates.Count)
            {
                tcs.TrySetResult();
            }
        });
        
        // Act
        server.Start();
        ServerTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        ClientTime.Advance(TimeSpan.FromMilliseconds(serverHb.HeartbeatRateMs));
        server.Dispose();
        ClientTime.Advance(TimeSpan.FromMilliseconds((clientConfig.Heartbeat.HeartbeatTimeoutMs + (HeartbeatClient.CheckConnectionDelay.TotalMilliseconds * clientConfig.Heartbeat.LinkQualityWarningSkipCount)) * 2));
        
        // Assert
        await tcs.Task;
        states.Should().BeEquivalentTo(expectedStates);
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