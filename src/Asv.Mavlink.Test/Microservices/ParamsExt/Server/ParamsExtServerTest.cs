using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DeepEqual.Syntax;
using JetBrains.Annotations;
using Xunit;
using R3;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtServer))]
public class ParamsExtServerTest : ServerTestBase<ParamsExtServer>,IDisposable
{
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public ParamsExtServerTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    protected override ParamsExtServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);

    [Fact]
    public async Task Send_SinglePacket_Success()
    {
        // Arrange
        var payload = new ParamExtValuePayload();
        MavlinkTypesHelper.SetString(payload.ParamValue, "test");
        using var sub = Link.Client.RxPipe.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamExtValue(p => p.ParamValue = "test".ToCharArray(), _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamExtValuePacket;
        Assert.NotNull(result);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(payload.ParamValue, result.Payload.ParamValue);
    }
    
    [Fact]
    public async Task Send_SingleAckPacket_Success()
    {
        // Arrange
        var payload = new ParamExtValuePayload();
        MavlinkTypesHelper.SetString(payload.ParamValue, "test");
        using var sub = Link.Client.RxPipe.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamExtAck(p => p.ParamValue = "test".ToCharArray(), _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamExtAckPacket;
        Assert.NotNull(result);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(payload.ParamValue, result.Payload.ParamValue);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task SendCompatibilityResponse_SendManyPacket_Success(int packetCount)
    {
        // Arrange
        var called = 0;
        var results = new List<ParamExtValuePayload>();
        var serverResults = new List<ParamExtValuePayload>();
        using var sub = Link.Client.RxPipe.Subscribe(p =>
        {
            called++;
            if (p is ParamExtValuePacket packet)
            {
                results.Add(packet.Payload);
            }
            if (called >= packetCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetCount; i++)
        {
            await Server.SendParamExtValue(p => serverResults.Add(p), _cancellationTokenSource.Token);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(packetCount, Link.Server.TxPackets);
        Assert.Equal(packetCount, Link.Client.RxPackets);
        Assert.Equal(packetCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(results[i].IsDeepEqual(serverResults[i]));
        }
    }
    
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}