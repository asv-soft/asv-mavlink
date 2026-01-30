using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsServer))]
public class ParamsServerTest : ServerTestBase<ParamsServer>
{
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public ParamsServerTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    [Fact]
    public async Task SendParamValue_SinglePacket_Success()
    {
        // Arrange
        var paramValue = ParamsTestHelper.Encoding.ConvertToMavlinkUnion(ParamsTestHelper.ServerParamsMeta.First().DefaultValue);
        using var sub = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>().Synchronize().Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamValue(p => p.ParamValue = paramValue, _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamValuePacket;
        Assert.NotNull(result);
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(paramValue, result.Payload.ParamValue);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task SendParamValue_ManyPacket_Success(int packetCount)
    {
        // Arrange
        var called = 0;
        var clientReceivedPackets = new List<ParamValuePayload>();
        var serverSentPackets = new List<ParamValuePayload>();
        using var sub = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>()
            .Synchronize().Subscribe(p =>
        {
            called++;
            if (p is ParamValuePacket packet)
            {
                clientReceivedPackets.Add(packet.Payload);
            }
            if (called >= packetCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetCount; i++)
        {
            await Server.SendParamValue(
                p => serverSentPackets.Add(p), 
                _cancellationTokenSource.Token);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(packetCount, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(packetCount, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(packetCount, clientReceivedPackets.Count);
        Assert.Equivalent(serverSentPackets, clientReceivedPackets, strict: true);
    }
    
    protected override ParamsServer CreateServer(MavlinkIdentity identity, CoreServices core) => 
        new(identity, core);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}