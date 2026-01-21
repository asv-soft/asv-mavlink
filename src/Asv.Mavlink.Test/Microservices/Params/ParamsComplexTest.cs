using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsClientEx))]
[TestSubject(typeof(ParamsServerEx))]
public class ParamsComplexTest : ComplexTestBase<ParamsClientEx, ParamsServerEx>
{
    private readonly ParameterClientConfig _clientConfig = new()
    {
        ReadAttemptCount = 3,
        ReadTimeouMs = 1000
    };
    private readonly ParamsClientExConfig _clientExConfig = new()
    {
        ChunkUpdateBufferMs = 100,
    };
    private readonly StatusTextLoggerConfig _statusTextServerConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30,
    };
    private readonly ParamsServerExConfig _serverExConfig = new()
    {
        SendingParamItemDelayMs = 0,
        CfgPrefix = "MAV_CFG_",
    };
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<ParamValuePayload> _taskCompletionSource;
    private ParamsServer? _server;

    public ParamsComplexTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<ParamValuePayload>();
        _cancellationTokenSource = new CancellationTokenSource();
        _ = Server;
    }

    [Fact]
    public async Task SendRequestList_ServerShouldSendParamList_Success()
    {
        // Arrange
        using var sub = Client.Base.OnParamValue
            .Synchronize()
            .Subscribe(p => _taskCompletionSource.TrySetResult(p));

        // Act
        await Client.Base.SendRequestList(_cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.Equal(Server.AllParamsList.Count, res.ParamCount);
        Assert.True(Server.AllParamsDict.ContainsKey(MavlinkTypesHelper.GetString(res.ParamId)));
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ReadOnce_ServerShouldSendParamValue_Success()
    {
        // Arrange
        var paramName = ParamsTestHelper.ServerParamsMeta.First().Name;
        using var sub = Client.Base.OnParamValue
            .Synchronize()
            .Subscribe(p => _taskCompletionSource.TrySetResult(p));

        // Act
        await Client.ReadOnce(paramName, _cancellationTokenSource.Token);
        
        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.NotNull(res);
        Assert.True(Server.AllParamsDict.ContainsKey(MavlinkTypesHelper.GetString(res.ParamId)));
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task WriteOnce_ServerShouldSetParameter_Success()
    {
        // Arrange
        var paramName = ParamsTestHelper.ServerParamsMeta.First().Name;
        var paramValue = ParamsTestHelper.ServerParamsMeta.First().DefaultValue;
        var tcs = new TaskCompletionSource<ParamSetPacket>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        using var sub1 = _server?.OnParamSet.Synchronize().Subscribe(p => tcs.TrySetResult(p));

        // Act
        var payload = await Client.WriteOnce(paramName, paramValue, _cancellationTokenSource.Token);

        // Assert
        var res = await tcs.Task;
        Assert.Equal(
            paramValue, 
            ParamsTestHelper.Encoding.ConvertFromMavlinkUnion(res.Payload.ParamValue, res.Payload.ParamType));
        Assert.Equal(payload, Server.AllParamsList.First(m => m.Name == paramName).DefaultValue);
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ReadAll_ClientShouldReadAllParamsFromServer_Success()
    {
        // Arrange
        var called = 0;
        var receivedParams = new List<ParamValuePayload>();
        using var sub = Client.Base.OnParamValue.Synchronize().Subscribe(p =>
        {
            called++;
            receivedParams.Add(p);
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.ReadTimeouMs));
            if (called == Server.AllParamsList.Count)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        await Client.ReadAll(null, true, _cancellationTokenSource.Token);

        // Assert
        await _taskCompletionSource.Task;
        Assert.NotNull(receivedParams);
        Assert.True(Client.IsSynced.CurrentValue);
        foreach (var payload in receivedParams)
        {
            var item = Client.Items[MavlinkTypesHelper.GetString(payload.ParamId)];
            Assert.Equal(item.Type, payload.ParamType);
            Assert.Equal(ParamsTestHelper.Encoding.ConvertToMavlinkUnion(item.Value.CurrentValue), payload.ParamValue);
        }
        Assert.NotEmpty(Server.AllParamsList);
        Assert.Equal(Server.AllParamsList.Count, Client.Items.Count);
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ReadAll_ClientShouldReadSomeParamsFromServer_Success()
    {
        // Arrange
        var paramsCountWhenStop = Server.AllParamsList.Count / 2;
        
        var tcs = new TaskCompletionSource();
        using var sub = Client.Base.OnParamValue.Synchronize().Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.ReadTimeouMs));

            if (Client.LocalCount.CurrentValue == paramsCountWhenStop)
            {
                Link.Server.Dispose();
                ClientTime.Advance(
                    TimeSpan.FromMilliseconds(_clientConfig.ReadTimeouMs * (_clientConfig.ReadAttemptCount + 1) * 2));
                tcs.TrySetResult();
            }
        });
        
        // Act
        await Client.ReadAll(null, false, _cancellationTokenSource.Token);
        
        // Assert
        await tcs.Task;
        Assert.NotEmpty(Server.AllParamsList);
        Assert.Equal(paramsCountWhenStop, Client.LocalCount.CurrentValue);
        Assert.Equal(Server.AllParamsList.Count, Client.RemoteCount.CurrentValue);
        Assert.NotEqual(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    protected override ParamsServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _server = new ParamsServer(identity, core);
        var statusTextServer = new StatusTextServer(identity, _statusTextServerConfig, core);
        var configuration = new InMemoryConfiguration();
        return new ParamsServerEx(
            _server, 
            statusTextServer, 
            ParamsTestHelper.ServerParamsMeta, 
            ParamsTestHelper.Encoding, 
            configuration, 
            _serverExConfig);
    }

    protected override ParamsClientEx CreateClient(
        MavlinkClientIdentity identity, 
        IMavlinkContext core)
    {
        var client = new ParamsClient(identity, _clientConfig, core);
        return new ParamsClientEx(client, _clientExConfig, ParamsTestHelper.Encoding, ParamsTestHelper.ClientParamsDescriptions);
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