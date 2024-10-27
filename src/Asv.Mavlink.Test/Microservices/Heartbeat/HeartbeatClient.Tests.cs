using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatClientTests
{
    private readonly ITestOutputHelper _log;
    private readonly HeartbeatClient _client;
    private readonly FakeTimeProvider _time;
    private readonly VirtualMavlinkConnection _link;

    public HeartbeatClientTests(ITestOutputHelper log)
    {
        _log = log;
        var identity = new MavlinkClientIdentity(1,2,3, 4);
        var config = new HeartbeatClientConfig
        {
            HeartbeatTimeoutMs = 2000,
            LinkQualityWarningSkipCount = 3,
            RateMovingAverageFilter = 10
        };
        _link = new VirtualMavlinkConnection();
        var seq = new PacketSequenceCalculator();
        _time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(_link.Client,seq,new TestLoggerFactory(log,_time,"CLIENT"), _time, meter);
        _client = new HeartbeatClient(identity, config, core);
    }
    
    [Theory]
    [InlineData(100,0,1.0)]
    [InlineData(100,1,0.5)]
    [InlineData(300,1,0.5)]
    [InlineData(300,0,1.0)]
    [InlineData(1000,2,0.33)]
    public async Task LinkQuality_Changed_Success(int packetCount, int skip, double quality)
    {
        Assert.Equal(0,_client.LinkQuality.Value);
        var seq = new PacketSequenceCalculator();
        
        for (var i = 0; i < packetCount; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await _link.Server.Send(p, default);
            for (var j = 0; j < skip; j++)
            {
                seq.GetNextSequenceNumber();
            }
            _time.Advance(TimeSpan.FromSeconds(0.05));
        }
        Assert.Equal(quality,_client.LinkQuality.Value, 1);
        _log.WriteLine($"RESULT: {quality:F3} ~ {_client.LinkQuality.Value:F3}");
    }
    
    
    [Theory]
    [InlineData(1000,1)]
    [InlineData(100,10)]
    [InlineData(10,100)]
    public async Task PacketRateHz_Changed_Success(int delayMs, double rate)
    {
        Assert.Equal(0,_client.PacketRateHz.Value);
        var seq = new PacketSequenceCalculator();
        
        for (var i = 0; i < 1000; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await _link.Server.Send(p, default);
            _time.Advance(TimeSpan.FromMilliseconds(delayMs));
        }
        Assert.Equal(rate,_client.PacketRateHz.Value, 1);
        _log.WriteLine($"RESULT: {rate:F3} ~ {_client.PacketRateHz.Value:F3}");
    }
}