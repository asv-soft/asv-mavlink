using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class ClientServerDeviceTests
{
    private readonly ClientDevice _client;
    private readonly ServerDevice _server;
    private readonly FakeTimeProvider _clientTime;
    private readonly FakeTimeProvider _timeServer;

    public ClientServerDeviceTests(ITestOutputHelper output)
    {
        var link = new VirtualMavlinkConnection();
        
        _timeServer = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        
        var clientSeq = new PacketSequenceCalculator();
        _clientTime = new FakeTimeProvider();
        var clientLog = new TestLoggerFactory(output,_clientTime, "CLIENT");
        var clientCore = new CoreServices(link.Client,clientSeq, clientLog, _clientTime, meter);
        
        var serverSeq = new PacketSequenceCalculator();
        var serverLog = new TestLoggerFactory(output,_timeServer, "SERVER");
        var serverCore = new CoreServices(link.Server,serverSeq, serverLog, _timeServer, meter);
        var serverId = new MavlinkClientIdentity(1, 2, 3, 4);
        
        _client = new ClientDevice(serverId, new ClientDeviceBaseConfig
        {
            Heartbeat =
            {
                HeartbeatTimeoutMs = 1000,
                LinkQualityWarningSkipCount = 3,
                RateMovingAverageFilter = 3,
            }
        },clientCore, DeviceClass.Copter);
        _server = new ServerDevice(serverId.Target, new ServerDeviceConfig
        {
            Heartbeat =
            {
                HeartbeatRateMs = 1000,
            }
        },serverCore);
        _server.Start();
    }
    
    [Fact]
    public async Task Heartbeat_ClientConnectToServer_Success()
    {
        Assert.Equal(LinkState.Disconnected,_client.Heartbeat.Link.Value);
        _timeServer.Advance(TimeSpan.FromSeconds(1.1));
        _clientTime.Advance(TimeSpan.FromSeconds(1.1));
        await _client.WaitUntilConnect();
        await _server.DisposeAsync();
        Assert.Equal(LinkState.Connected,_client.Heartbeat.Link.Value);
        _timeServer.Advance(TimeSpan.FromSeconds(1.1));
        _clientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        Assert.Equal(LinkState.Downgrade,_client.Heartbeat.Link.Value);
        _timeServer.Advance(TimeSpan.FromSeconds(1.1));
        _clientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        Assert.Equal(LinkState.Downgrade,_client.Heartbeat.Link.Value);
        _timeServer.Advance(TimeSpan.FromSeconds(1.1));
        _clientTime.Advance(TimeSpan.FromSeconds(1.1));
        Assert.Equal(LinkState.Disconnected,_client.Heartbeat.Link.Value);
        _timeServer.Advance(TimeSpan.FromSeconds(1.1));
        _clientTime.Advance(TimeSpan.FromSeconds(1.1));
        
        Assert.Equal(LinkState.Disconnected,_client.Heartbeat.Link.Value);
    }
}