using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtServer))]
public class ParamsExtServerTest : ServerTestBase<ParamsExtServer>
{
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public ParamsExtServerTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task SendParamExtValue_SinglePacket_Success()
    {
        // Arrange
        var payload = new ParamExtValuePayload();
        const string paramValue = "test";
        MavlinkTypesHelper.SetString(payload.ParamValue, paramValue);
        using var sub = Link.Client.OnRxMessage.Synchronize().Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamExtValue(p =>
        {
            paramValue.ToCharArray().CopyTo(p.ParamValue, 0);
        }, _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamExtValuePacket;
        Assert.NotNull(result);
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(payload.ParamValue, result.Payload.ParamValue);
    }
    
    [Fact]
    public async Task SendParamExtAck_SingleAckPacket_Success()
    {
        // Arrange
        var payload = new ParamExtValuePayload();
        const string paramValue = "test";
        MavlinkTypesHelper.SetString(payload.ParamValue, paramValue);
        using var sub = Link.Client.OnRxMessage.Synchronize().Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamExtAck(p =>
        {
            paramValue.ToCharArray().CopyTo(p.ParamValue,0);
        }, _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamExtAckPacket;
        Assert.NotNull(result);
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(payload.ParamValue, result.Payload.ParamValue);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(2000)]
    public async Task SendParamExtValue_ManyPackets_Success(int packetCount)
    {
        // Arrange
        var called = 0;
        var clientReceivedResults = new List<ParamExtValuePayload>();
        var serverSentResults = new List<ParamExtValuePayload>();
        using var sub = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            if (p is ParamExtValuePacket packet)
            {
                clientReceivedResults.Add(packet.Payload);
            }
            if (called >= packetCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetCount; i++)
        {
            await Server.SendParamExtValue(
                p => serverSentResults.Add(p), 
                _cancellationTokenSource.Token);
        }
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(packetCount, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(packetCount, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(packetCount, clientReceivedResults.Count);
        Assert.Equal(serverSentResults.Count, clientReceivedResults.Count);
        Assert.Equivalent(clientReceivedResults, clientReceivedResults, true);
    }
    
    protected override ParamsExtServer CreateServer(MavlinkIdentity identity, CoreServices core)
        => new(identity, core);
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}