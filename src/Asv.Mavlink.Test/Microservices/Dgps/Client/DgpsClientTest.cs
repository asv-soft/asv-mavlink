using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Dgps.Client;

[TestSubject(typeof(DgpsClient))]
public class DgpsClientTest : ClientTestBase<DgpsClient>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    protected override DgpsClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);

    public DgpsClientTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(359, 2)]
    [InlineData(719, 4)]
    public async Task SendRtcmData_WhenDataSizeVaries_ShouldSplitIntoExpectedNumberOfPackets(int size, int packetCount)
    {
        // Arrange
        var data = new byte[size];
        new Random().NextBytes(data);
        var sentPackets = new List<GpsRtcmDataPacket>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        cancel.Token.Register(() => _taskCompletionSource.TrySetCanceled());
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            if (_ is GpsRtcmDataPacket gpsPacket)
            {
                sentPackets.Add(gpsPacket);
                if (Link.Client.Statistic.TxMessages == packetCount)
                    _taskCompletionSource.TrySetResult(gpsPacket);
            }
        });

        // Act
        await Client.SendRtcmData(data, size, cancel.Token);

        // Assert
        var res = await _taskCompletionSource.Task as GpsRtcmDataPacket;
        Assert.NotNull(res);
        Assert.Equal(packetCount, sentPackets.Count);
        Assert.Equal(packetCount, (int)Link.Client.Statistic.TxMessages);
        
        var maxMessageLength = 180;
        for (int i = 0; i < sentPackets.Count; i++)
        {
            var expectedLength = Math.Min(maxMessageLength, size - i * maxMessageLength);
            var expectedData = data.Skip(i * maxMessageLength).Take(expectedLength).ToArray();

            Assert.Equal(expectedLength, sentPackets[i].Payload.Len);
            Assert.Equal(expectedData, sentPackets[i].Payload.Data);
        }
    }

    [Fact]
    public async Task SendRtcmData_WhenCancelled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var data = new byte[100];
        new Random().NextBytes(data);
        await _cancellationTokenSource.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            Client.SendRtcmData(data, data.Length, _cancellationTokenSource.Token));
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Fact]
    public async Task SendRtcmData_WhenDataTooLarge_ShouldLogError()
    {
        // Arrange
        var data = new byte[1000];
        new Random().NextBytes(data);

        // Act
        await Client.SendRtcmData(data, data.Length, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(0, (int)Link.Client.Statistic.TxMessages);
    }
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}