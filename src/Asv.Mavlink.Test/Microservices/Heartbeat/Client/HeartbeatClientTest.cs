using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Minimal;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatClientTest(ITestOutputHelper log) : ClientTestBase<HeartbeatClient>(log)
{
  
    private readonly HeartbeatClientConfig _config = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 10
    };

    protected override HeartbeatClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new HeartbeatClient(identity, _config, core);
    }
    
    [Theory(Skip = "Test execution is not stable")]
    [InlineData(100,0,1.0)]
    [InlineData(100,1,0.5)]
    [InlineData(300,1,0.5)]
    [InlineData(300,0,1.0)]
    [InlineData(1000,2,0.33)]
    public async Task LinkQuality_Changed_Success(int packetCount, int skip, double quality)
    {
        Assert.Equal(0,Client.LinkQuality.CurrentValue);
        var seq = new PacketSequenceCalculator();
        
        for (var i = 0; i < packetCount; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await Link.Server.Send(p, CancellationToken.None);
            for (var j = 0; j < skip; j++)
            {
                seq.GetNextSequenceNumber();
            }
        }
        
        Assert.Equal(quality,Client.LinkQuality.CurrentValue, 1);
        Log.WriteLine($"RESULT: {quality:F3} ~ {Client.LinkQuality.CurrentValue:F3}");
    }
    
    [Theory (Skip = "Test execution is not sable ")]
    [InlineData(1000,100_000)]
    [InlineData(100,10_000)]
    [InlineData(10,10_000)]
    [InlineData(20,10_000)]
    public async Task PacketRateHz_Changed_Success(int delayMs, int count)
    {
        Assert.Equal(0,Client.PacketRateHz.CurrentValue);
        var seq = new PacketSequenceCalculator();
        
        var tcs = new TaskCompletionSource();
        var inx = 0;
        Link.Client.OnRxMessage.Subscribe(x =>
        {
            inx++;
            if (inx == count) tcs.SetResult();
        });
        for (var i = 0; i < count; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await Link.Server.Send(p, CancellationToken.None);
            Time.Advance(TimeSpan.FromMilliseconds(delayMs));
        }
        
        await tcs.Task;
        Assert.NotEqual(0,Client.PacketRateHz.CurrentValue);
        Log.WriteLine($"RESULT: {Client.PacketRateHz.CurrentValue:F3}");
    }
    
    [Theory]
    [InlineData(1000)]
    [InlineData(100)]
    [InlineData(10)]
    public async Task RawPackets_Client_Catch_All_Packets_Success(int packets)
    {
        var count = 0;
        Client.RawHeartbeat.Skip(1).Subscribe(_ =>
        {
            count++;
        });
        var seq = new PacketSequenceCalculator();
        for (var i = 0; i < packets; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await Link.Server.Send(p, CancellationToken.None);
            Time.Advance(TimeSpan.FromMilliseconds(100));
        }
        Assert.Equal(packets,count);
    }

}