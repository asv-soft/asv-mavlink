using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtServer))]
public class ParamsExtServerTest : ServerTestBase<ParamsExtServer>,IDisposable
{
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public ParamsExtServerTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    protected override ParamsExtServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);

    [Fact]
    public async Task Send_SinglePacket_Success()
    {
        // Arrange
        var payload = new ParamExtValuePayload();
        MavlinkTypesHelper.SetString(payload.ParamValue, "test");
        using var sub = Link.Client.OnRxMessage.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamExtValue(p => p.ParamValue = "test".ToCharArray(), _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamExtValuePacket;
        Assert.NotNull(result);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.NotNull(result);
        if (result == null) return; // just to make resharper happy
        Assert.Equal(payload.ParamValue, result.Payload.ParamValue);
    }
    
    [Fact]
    public async Task Send_SingleAckPacket_Success()
    {
        // Arrange
        var payload = new ParamExtValuePayload();
        MavlinkTypesHelper.SetString(payload.ParamValue, "test");
        using var sub = Link.Client.OnRxMessage.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        
        // Act
        await Server.SendParamExtAck(p => p.ParamValue = "test".ToCharArray(), _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as ParamExtAckPacket;
        Assert.NotNull(result);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.NotNull(result);
        if (result == null) return; // just to make resharper happy
        Assert.Equal(payload.ParamValue, result.Payload.ParamValue);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(2000)]
    public async Task SendCompatibilityResponse_SendManyPacket_Success(int packetCount)
    {
        // Arrange
        var called = 0;
        var results = new List<ParamExtValuePayload>();
        var serverResults = new List<ParamExtValuePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        cancel.Token.Register(() => _taskCompletionSource.TrySetCanceled());
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
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
            await Server.SendParamExtValue(p => serverResults.Add(p), cancel.Token);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(packetCount, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(packetCount, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(packetCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(results[i].IsDeepEqual(serverResults[i]));
        }
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