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
public class ParamsComplexTest : ComplexTestBase<ParamsClientEx, ParamsServerEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<ParamValuePayload> _taskCompletionSource;
    private readonly MavParamCStyleEncoding _encoding;
    private readonly ParamsServerEx _serverEx;
    private readonly ParamsClientEx _client;
    private ParamsServer? _server;

    public ParamsComplexTest(ITestOutputHelper log) : base(log)
    {
        _encoding = new MavParamCStyleEncoding();
        _serverEx = Server;
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<ParamValuePayload>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

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

    protected override ParamsServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _server = new ParamsServer(identity, core);
        var statusTextServer = new StatusTextServer(identity, _statusTextServerConfig, core);
        var paramDict = new List<MavParamTypeMetadata>
        {
            new("BARO_PRIMARY", MavParamType.MavParamTypeUint8)
            {
                DefaultValue = new MavParamValue((byte)0),
                MinValue = new MavParamValue((byte)0),
                MaxValue = new MavParamValue((byte)2),
                ShortDesc = "Selects the primary barometer when multiple are found",
            },
            new("SIM_CAN_TYPE1", MavParamType.MavParamTypeInt8)
            {
                DefaultValue = new MavParamValue((sbyte)0),
                MinValue = new MavParamValue((sbyte)0),
                MaxValue = new MavParamValue((sbyte)3),
                ShortDesc = "transport type for first CAN interface",
            },
            new("FENCE_ACTION", MavParamType.MavParamTypeUint16)
            {
                DefaultValue = new MavParamValue((ushort)1),
                MinValue = new MavParamValue((ushort)0),
                MaxValue = new MavParamValue((ushort)2),
            },
            new("RC7_DZ", MavParamType.MavParamTypeInt16)
            {
                DefaultValue = new MavParamValue((short)10),
                MinValue = new MavParamValue((short)0),
                MaxValue = new MavParamValue((short)200),
                ShortDesc = "Example parameter",
            },
            new("LOG_BITMASK", MavParamType.MavParamTypeUint32)
            {
                DefaultValue = new MavParamValue((uint)176126),
                MinValue = new MavParamValue((uint)0),
                MaxValue = new MavParamValue((uint)4294967295),
                ShortDesc = "Mask for logging which messages to include",
            },
            new("WPNAV_SPEED", MavParamType.MavParamTypeInt32)
            {
                DefaultValue = new MavParamValue(500),
                MinValue = new MavParamValue(10),
                MaxValue = new MavParamValue(2000),
                ShortDesc = "Example parameter",
            },
            new("BARO_ALT_OFFSET", MavParamType.MavParamTypeReal32)
            {
                DefaultValue = new MavParamValue((float)0f),
                MinValue = new MavParamValue((float)-1000f),
                MaxValue = new MavParamValue((float)1000f),
                ShortDesc = "Maximum Bank Angle",
            },
        };
        var configuration = new InMemoryConfiguration();
        return new ParamsServerEx(_server, statusTextServer, paramDict, _encoding, configuration, _serverExConfig);
    }

    protected override ParamsClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var client = new ParamsClient(identity, _clientConfig, core);
        var existDescription = new List<ParamDescription>
        {
            new() { Name = "BARO_PRIMARY", ParamType = MavParamType.MavParamTypeUint8 },
            new() { Name = "SIM_CAN_TYPE1", ParamType = MavParamType.MavParamTypeInt8 },
            new() { Name = "FENCE_ACTION", ParamType = MavParamType.MavParamTypeUint16 },
            new() { Name = "RC7_DZ", ParamType = MavParamType.MavParamTypeInt16 },
            new() { Name = "LOG_BITMASK", ParamType = MavParamType.MavParamTypeUint32 },
            new() { Name = "WPNAV_SPEED", ParamType = MavParamType.MavParamTypeInt32 },
            new() { Name = "BARO_ALT_OFFSET", ParamType = MavParamType.MavParamTypeReal32 },
        };
        return new ParamsClientEx(client, _clientExConfig, _encoding, existDescription);
    }

    [Fact]
    public async Task SendRequestList_ServerShouldSendParamList_Success()
    {
        // Arrange
        using var sub = Client.Base.OnParamValue.Subscribe(p => _taskCompletionSource.TrySetResult(p));

        // Act
        await Client.Base.SendRequestList(_cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.Equal(Server.AllParamsList.Count, res.ParamCount);
        Assert.True(_serverEx.AllParamsDict.ContainsKey(MavlinkTypesHelper.GetString(res.ParamId)));
    }

    [Fact]
    public async Task ReadOnce_ServerShouldSendParamValue_Success()
    {
        // Arrange
        var name = "LOG_BITMASK";
        using var sub = _client.Base.OnParamValue.Subscribe(p => _taskCompletionSource.TrySetResult(p));

        // Act
        await _client.ReadOnce(name, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.NotNull(res);
        Assert.True(_serverEx.AllParamsDict.ContainsKey(MavlinkTypesHelper.GetString(res.ParamId)));
    }

    [Fact]
    public async Task WriteOnce_ServerShouldSetParameter_Success()
    {
        // Arrange
        var name = "BARO_ALT_OFFSET";
        var value = new MavParamValue(-500f);
        var tcs = new TaskCompletionSource<ParamSetPacket>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        using var sub1 = _server?.OnParamSet.Subscribe(p => tcs.TrySetResult(p));

        // Act
        var payload = await Client.WriteOnce(name, value, _cancellationTokenSource.Token);

        // Assert
        var res = await tcs.Task;
        Assert.Equal(-500f, res.Payload.ParamValue);
        Assert.NotEqual(payload, _serverEx.AllParamsList.FirstOrDefault(_ => _.Name == name).DefaultValue);
    }

    [Fact]
    public async Task ReadAll_ClientShouldReadAllParamsFromServer_Success()
    {
        // Arrange
        var called = 0;
        var param = new List<ParamValuePayload>();
        using var sub = Client.Base.OnParamValue.Subscribe(p =>
        {
            called++;
            param.Add(p);
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.ReadTimeouMs));
            if (called == _serverEx.AllParamsList.Count)
            {
                _taskCompletionSource.TrySetResult(p);
            }
            
        });

        // Act
        await _client.ReadAll(null, true, _cancellationTokenSource.Token);

        // Assert
        await _taskCompletionSource.Task;
        Assert.NotNull(param);
        Assert.True(_client.IsSynced.CurrentValue);
        Assert.Equal(_serverEx.AllParamsList.Count, _client.Items.Count);
        foreach (var payload in param)
        {
            Assert.True(_client.Items.ContainsKey(MavlinkTypesHelper.GetString(payload.ParamId)));
        }
    }

    [Fact]
    public async Task ReadAll_ClientShouldSyncLocalAndRemoteCounts_Success()
    {
        // Arrange
        var server = Server;
        server.Start();

        // Act
        var t1 = _client.ReadAll(null, false, _cancellationTokenSource.Token);
        var t2 = Task.Factory.StartNew(() =>
        {
            // this is for chunk update
            while (t1.IsCompleted == false)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.ReadTimeouMs));
            }
        });

        await Task.WhenAll(t1, t2);
        // Assert
        Assert.Equal(_serverEx.AllParamsList.Count, _client.LocalCount.CurrentValue);
        Assert.Equal(_serverEx.AllParamsList.Count, _client.RemoteCount.CurrentValue);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}