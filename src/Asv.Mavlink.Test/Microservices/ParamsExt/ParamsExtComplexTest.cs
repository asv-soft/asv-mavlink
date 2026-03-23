using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Asv.Mavlink.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class ParamsExtComplexTest : ComplexTestBase<ParamsExtClientEx, ParamsExtServerEx>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<ParamExtValuePayload> _taskCompletionSource;
    private readonly ParamsExtClientConfig _clientConfig = new()
    {
        ReadAttemptCount = 3,
        ReadTimeoutMs = 5000,
    };
    private readonly ParamsExtClientExConfig _clientExConfig = new()
    {
        ChunkUpdateBufferMs = 0,
    };
    private readonly StatusTextLoggerConfig _statusTextServerConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30,
    };
    private readonly ParamsExtServerExConfig _serverExConfig = new()
    {
        SendingParamItemDelayMs = 0,
        CfgPrefix = "MAV_CFG_",
    };
    private ParamsExtServer? _server;

    public ParamsExtComplexTest(ITestOutputHelper log) : base(log)
    {
        _ = Server;
        _taskCompletionSource = new TaskCompletionSource<ParamExtValuePayload>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task SendRequestList_ServerShouldSendAllParams_Success()
    {
        // Arrange
        var receivedParams = new List<ParamExtValuePayload>();
        using var sub = Client.Base.OnParamExtValue.Synchronize().Subscribe(p =>
        {
            receivedParams.Add(p);
            
            if (p.ParamCount == receivedParams.Count)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        await Client.Base.SendRequestList(_cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.Equal(receivedParams.Count, res.ParamCount);
        Assert.Equal(Client.RemoteCount.Value, Client.LocalCount.Value);
    }

    [Fact]
    public async Task Read_ServerShouldSendExtendedParamValue_Success()
    {
        // Arrange
        using var sub = Client.Base.OnParamExtValue
            .Subscribe(p => _taskCompletionSource.TrySetResult(p));
        
        var serverSentPacketTcs = new TaskCompletionSource<ParamExtValuePacket>();
        using var sub1 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            serverSentPacketTcs.TrySetResult((ParamExtValuePacket)p);
        });
        
        var clientReceivedPacketTcs = new TaskCompletionSource<ParamExtValuePacket>();
        using var sub2 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            clientReceivedPacketTcs.TrySetResult((ParamExtValuePacket)p);
        });
        
        const int paramIndex = 1;

        // Act
        await Client.Base.Read(paramIndex, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        var serverSentPacket = await serverSentPacketTcs.Task;
        var clientReceivedPacket = await clientReceivedPacketTcs.Task;
        Assert.Equivalent(serverSentPacket, clientReceivedPacket);
        Assert.Equivalent(res, clientReceivedPacket.Payload);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(1, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(1, res.ParamIndex);
    }
    
    // TODO: fix parameter value encoding/decoding in the char array
    [Fact]
    public async Task ReadOnce_ServerShouldSendExtendedParamValue_Success()
    {
        // Arrange
        var paramName = ParamsExtTestHelper.ServerParamsMeta.First().Name;
        
        using var sub = Client.Base.OnParamExtValue.Synchronize().Subscribe(p =>
        {
            _taskCompletionSource.TrySetResult(p);
        });
        
        var serverSentPacketTcs = new TaskCompletionSource<ParamExtValuePacket>();
        using var sub1 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            if (p is ParamExtValuePacket paramExtValuePacket)
            {
                serverSentPacketTcs.TrySetResult(paramExtValuePacket);    
            }
        });

        // Act
        await Client.ReadOnce(paramName, _cancellationTokenSource.Token);

        // Assert
        var clientReceivedPayload = await _taskCompletionSource.Task;
        var serverSentPacket = await serverSentPacketTcs.Task;
        ParamsExtTestHelper.AssertParamsEqual(clientReceivedPayload, serverSentPacket.Payload);
        Assert.Equal(1, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(1, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(paramName, MavlinkTypesHelper.GetString(clientReceivedPayload.ParamId));
    }
    
    // TODO: fix OnParamSet Encoding/Decoding
    [Fact(Skip = "fix OnParamSet Encoding/Decoding")]
    public async Task WriteOnce_ServerShouldSetParameter_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource<ParamExtAckPayload>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        
        const string name = "SERVO1_MIN";
        var value = new MavParamExtValue(2000);
        using var sub = Client.Base.OnParamExtAck.Synchronize().Subscribe(p =>
        {
            if (p.ParamResult == ParamAck.ParamAckAccepted)
            {
                tcs.TrySetResult(p);
            }
        });

        // Act
        var res = await Client.WriteOnce(name, value, _cancellationTokenSource.Token);

        // Assert
        var payload = await tcs.Task;
        Assert.NotNull(payload);
        Assert.Equal(ParamAck.ParamAckAccepted, payload.ParamResult);
        Assert.Equal(value, res);
    }

    // TODO: fix OnParamSet Encoding/Decoding
    [Fact(Skip = "fix OnParamSet Encoding/Decoding")]
    public async Task WriteOnce_ShouldFail_WhenValueIsOutOfBounds()
    {
        // Arrange
        var tcs = new TaskCompletionSource<ParamExtAckPayload>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        
        const string name = "WP_RADIUS";
        var outOfBoundsValue = new MavParamExtValue((byte)0x01);
        using var sec = Client.Base.OnParamExtAck.Synchronize().Subscribe(p =>
        {
            if (p.ParamResult != ParamAck.ParamAckAccepted)
            {
                tcs.TrySetResult(p);
            }
        });

        // Act
        await Client.WriteOnce(name, outOfBoundsValue);

        // Assert
        var payload = await tcs.Task;
        Assert.NotNull(payload);
        Assert.Equal(ParamAck.ParamAckFailed, payload.ParamResult);
    }

    [Fact]
    public async Task ReadAll_ClientShouldReadAllParamsFromServer_Success()
    {
        // Arrange
        var clientParamsPayload = new List<ParamExtValuePayload>();
        using var sub = Client.Base.OnParamExtValue.Synchronize().Subscribe(p =>
        {
            clientParamsPayload.Add(p);
            
            if (clientParamsPayload.Count == p.ParamCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
            
            ClientTime.Advance(TimeSpan.FromMilliseconds(_serverExConfig.SendingParamItemDelayMs));
        });
        
        var serverParamsPayload = new List<ParamExtValuePayload>();
        using var sub2 = Link.Server.OnTxMessage
            .FilterByType<ParamExtValuePacket>()
            .Synchronize()
            .Subscribe(p =>
            {
                serverParamsPayload.Add(p.Payload);
            });

        // Act
        await Client.ReadAll(null, _cancellationTokenSource.Token);

        // Assert
        await _taskCompletionSource.Task;
        Assert.NotNull(clientParamsPayload);
        Assert.True(Client.IsSynced.Value);
        ParamsExtTestHelper.AssertParamsEqual(serverParamsPayload, clientParamsPayload);
        ParamsExtTestHelper.AssertParamsEqual(serverParamsPayload,  Client.Items.Values.ToList());
    }
    
    protected override ParamsExtServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _server = new ParamsExtServer(identity, core);
        var statusTextServer = new StatusTextServer(identity, _statusTextServerConfig, core);
        var configuration = new InMemoryConfiguration();
        return new ParamsExtServerEx(_server, statusTextServer, ParamsExtTestHelper.ServerParamsMeta, configuration, _serverExConfig);
    }

    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var client = new ParamsExtClient(identity, _clientConfig, core);
        return new ParamsExtClientEx(client, _clientExConfig, ParamsExtTestHelper.ClientParamsDescriptions);
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