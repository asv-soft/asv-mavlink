using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Minimal;
using FluentAssertions;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatClientTest : ClientTestBase<HeartbeatClient>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly HeartbeatClientConfig _config = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 10
    };

    public HeartbeatClientTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Fact]
    public async Task LinkQuality_Changed_Success()
    {
        // Arrange
        var qualities = new List<double>();

        using var sub = Client.LinkQuality.Synchronize().Subscribe(q =>
        {
            qualities.Add(q);
        });
        
        var seq = new PacketSequenceCalculator();
        
        // Act
        Log.WriteLine("Start quality: {0}", Client.LinkQuality.CurrentValue);
        
        await Link.Server.Send(new HeartbeatPacket
        {
            SystemId = 3,
            ComponentId = 4,
            Sequence = seq.GetNextSequenceNumber(),
        }, _cancellationTokenSource.Token);
        await Link.Server.Send(new HeartbeatPacket
        {
            SystemId = 3,
            ComponentId = 4,
            Sequence = seq.GetNextSequenceNumber(),
        }, _cancellationTokenSource.Token);
        Time.Advance(HeartbeatClient.CheckConnectionDelay);
        Log.WriteLine("Quality after some successful packets: {0}", Client.LinkQuality.CurrentValue);
        
        await Link.Server.Send(new HeartbeatPacket
        {
            SystemId = 3,
            ComponentId = 4,
            Sequence = seq.GetNextSequenceNumber(),
        }, _cancellationTokenSource.Token);
        seq.GetNextSequenceNumber();
        seq.GetNextSequenceNumber();
        await Link.Server.Send(new HeartbeatPacket
        {
            SystemId = 3,
            ComponentId = 4,
            Sequence = seq.GetNextSequenceNumber(),
        }, _cancellationTokenSource.Token);
        Time.Advance(HeartbeatClient.CheckConnectionDelay);
        Log.WriteLine("Quality after losses: {0}", Client.LinkQuality.CurrentValue);
        
        Time.Advance(HeartbeatClient.CheckConnectionDelay * 4 + TimeSpan.FromMilliseconds(_config.HeartbeatTimeoutMs));
        Log.WriteLine("Quality after disconnect: {0}", Client.LinkQuality.CurrentValue);
        
        // Assert
        Assert.Equal(0, qualities.First());
        Assert.Equal(1, qualities[1]);
        Assert.True(qualities[2] > 0 && qualities[2] < 1);
        Assert.Equal(0, qualities.Last());
    }
    
    [Theory]
    [InlineData(1000,100_000)]
    [InlineData(100,10_000)]
    [InlineData(10,10_000)]
    [InlineData(20,10_000)]
    public async Task PacketRateHz_Changed_Success(int delayMs, int packetsToSendCount)
    {
        // Arrange
        _ = Client;
        
        var neededPacketsReceivedTcs = new TaskCompletionSource();
        var receivedPacketsCount = 0;
        using var sub1 = Link.Client.OnRxMessage.Synchronize().Subscribe(_ =>
        {
            receivedPacketsCount++;
            
            if (receivedPacketsCount == packetsToSendCount)
            {
                neededPacketsReceivedTcs.TrySetResult();
            }
        });
        
        var packetRateValues = new List<double>();
        using var sub2 = Client.PacketRateHz.Synchronize().Subscribe(rate =>
        {
            packetRateValues.Add(rate);
        });
        
        var seq = new PacketSequenceCalculator();
        
        // Act
        for (var i = 0; i < packetsToSendCount; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await Link.Server.Send(p, _cancellationTokenSource.Token);
            Time.Advance(TimeSpan.FromMilliseconds(delayMs));
        }
        
        await neededPacketsReceivedTcs.Task;
        
        Log.WriteLine("Rate: {0}", string.Join("; ", packetRateValues));
        
        // Assert
        Assert.NotEqual(0, Client.PacketRateHz.CurrentValue);
        packetRateValues.Should().BeInAscendingOrder();
    }
    
    [Theory]
    [InlineData(1000)]
    [InlineData(100)]
    [InlineData(10)]
    public async Task RawHeartbeat_ClientCatchAllPackets_Success(int packetsToSendCount)
    {
        // Arrange
        var receivedPayloads = new List<HeartbeatPayload>();
        using var sub = Client.RawHeartbeat.Synchronize().Subscribe(p =>
        {
            if (p is not null)
            {
                receivedPayloads.Add(p);
            }
        });
        
        var seq = new PacketSequenceCalculator();
        
        // Act
        var sentPayloads = new List<HeartbeatPayload>();
        for (var i = 0; i < packetsToSendCount; i++)
        {
            var p = new HeartbeatPacket
            {
                SystemId = 3,
                ComponentId = 4,
                Sequence = seq.GetNextSequenceNumber(),
            };
            await Link.Server.Send(p, _cancellationTokenSource.Token);
            sentPayloads.Add(p.Payload);
            Time.Advance(TimeSpan.FromMilliseconds(_config.HeartbeatTimeoutMs / 10));
        }
        
        // Assert
        Assert.Equal(packetsToSendCount, receivedPayloads.Count);
        Assert.Equal(packetsToSendCount, sentPayloads.Count);
        Assert.Equal(packetsToSendCount, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        receivedPayloads.Should().BeEquivalentTo(sentPayloads);
    }
    
    protected override HeartbeatClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new HeartbeatClient(identity, _config, core);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }
        
        base.Dispose(disposing);
    }
}