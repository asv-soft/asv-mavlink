using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.V2.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class ParamsExtComplexTest : ComplexTestBase<ParamsExtClientEx, ParamsExtServerEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<ParamExtValuePayload> _taskCompletionSource;
    private readonly ParamsExtServerEx _serverEx;
    private readonly ParamsExtClientEx _client;
    private ParamsExtServer? _server;

    public ParamsExtComplexTest(ITestOutputHelper log) : base(log)
    {
        _serverEx = Server;
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<ParamExtValuePayload>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    private readonly ParamsExtClientConfig _clientConfig = new()
    {
        ReadAttemptCount = 3,
        ReadTimeouMs = 5000,
    };

    private readonly ParamsExtClientExConfig _clientExConfig = new()
    {
        ReadListTimeoutMs = 5000,
        ChunkUpdateBufferMs = 100,
    };

    private readonly StatusTextLoggerConfig _statusTextServerConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30,
    };

    private readonly ParamsExtServerExConfig _serverExConfig = new()
    {
        SendingParamItemDelayMs = 100,
        CfgPrefix = "MAV_CFG_",
    };

    protected override ParamsExtServerEx CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        _server = new ParamsExtServer(identity, core);
        var statusTextServer = new StatusTextServer(identity, _statusTextServerConfig, core);
        var paramDict = new List<MavParamExtTypeMetadata>
        {
            new("WP_RADIUS", MavParamExtType.MavParamExtTypeUint8)
            {
                DefaultValue = new MavParamExtValue((byte)200),
                MinValue = new MavParamExtValue((byte)10),
                MaxValue = new MavParamExtValue((byte)255),
                ShortDesc = "Waypoint radius",
            },
            new("FS_GCS_ENABL", MavParamExtType.MavParamExtTypeInt8)
            {
                DefaultValue = new MavParamExtValue((sbyte)0),
                MinValue = new MavParamExtValue((sbyte)-128),
                MaxValue = new MavParamExtValue((sbyte)127),
                ShortDesc = "Enable failsafe for GCS",
            },
            new("RC_MAP_THROTTLE", MavParamExtType.MavParamExtTypeUint16)
            {
                DefaultValue = new MavParamExtValue((ushort)3),
                MinValue = new MavParamExtValue((ushort)0),
                MaxValue = new MavParamExtValue((ushort)16),
                ShortDesc = "RC throttle channel mapping",
            },
            new("RC1_TRIM", MavParamExtType.MavParamExtTypeInt16)
            {
                DefaultValue = new MavParamExtValue((short)1500),
                MinValue = new MavParamExtValue((short)-32768),
                MaxValue = new MavParamExtValue((short)32767),
                ShortDesc = "RC channel 1 trim",
            },
            new("ARMING_CHECK", MavParamExtType.MavParamExtTypeUint32)
            {
                DefaultValue = new MavParamExtValue((uint)1),
                MinValue = new MavParamExtValue((uint)0),
                MaxValue = new MavParamExtValue((uint)4294967295),
                ShortDesc = "Arming checks flags",
            },
            new("SERVO1_MIN", MavParamExtType.MavParamExtTypeInt32)
            {
                DefaultValue = new MavParamExtValue(1000),
                MinValue = new MavParamExtValue(-2147483648),
                MaxValue = new MavParamExtValue(2147483647),
                ShortDesc = "Servo 1 minimum PWM value",
            },
            new("INS_FAST_SAMPLE", MavParamExtType.MavParamExtTypeUint64)
            {
                DefaultValue = new MavParamExtValue((ulong)0),
                MinValue = new MavParamExtValue((ulong)0),
                MaxValue = new MavParamExtValue((ulong)18446744073709551615),
                ShortDesc = "Fast IMU sampling rate",
            },
            new("SCHED_LOOP_RATE", MavParamExtType.MavParamExtTypeInt64)
            {
                DefaultValue = new MavParamExtValue((long)400),
                MinValue = new MavParamExtValue((long)-9223372036854775808),
                MaxValue = new MavParamExtValue((long)9223372036854775807),
                ShortDesc = "Loop rate for scheduler",
            },
            new("PITCH_MAX", MavParamExtType.MavParamExtTypeReal32)
            {
                DefaultValue = new MavParamExtValue((float)20f),
                MinValue = new MavParamExtValue((float)-90f),
                MaxValue = new MavParamExtValue((float)90f),
                ShortDesc = "Maximum pitch angle (degrees)",
            },
            new("GND_ABS_PRESS", MavParamExtType.MavParamExtTypeReal64)
            {
                DefaultValue = new MavParamExtValue((double)101325),
                MinValue = new MavParamExtValue((double)0),
                MaxValue = new MavParamExtValue((double)200000),
                ShortDesc = "Ground pressure in Pascals (absolute)",
            },
            new("SERIAL1_PROTOCOL", MavParamExtType.MavParamExtTypeCustom)
            {
                DefaultValue = new MavParamExtValue("test".ToCharArray()),
                ShortDesc = "Serial port 1 protocol",
            },
        };
        var configuration = new InMemoryConfiguration();
        return new ParamsExtServerEx(_server, statusTextServer, paramDict, configuration, _serverExConfig);
    }

    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        var client = new ParamsExtClient(identity, _clientConfig, core);
        var existDescription = new List<ParamExtDescription>
        {
            new() { Name = "WP_RADIUS", ParamExtType = MavParamExtType.MavParamExtTypeUint8 },
            new() { Name = "FS_GCS_ENABL", ParamExtType = MavParamExtType.MavParamExtTypeInt8 },
            new() { Name = "RC_MAP_THROTTLE", ParamExtType = MavParamExtType.MavParamExtTypeUint16 },
            new() { Name = "RC1_TRIM", ParamExtType = MavParamExtType.MavParamExtTypeInt16 },
            new() { Name = "ARMING_CHECK", ParamExtType = MavParamExtType.MavParamExtTypeUint32 },
            new() { Name = "SERVO1_MIN", ParamExtType = MavParamExtType.MavParamExtTypeInt32 },
            new() { Name = "INS_FAST_SAMPLE", ParamExtType = MavParamExtType.MavParamExtTypeUint64 },
            new() { Name = "SCHED_LOOP_RATE", ParamExtType = MavParamExtType.MavParamExtTypeInt64 },
            new() { Name = "PITCH_MAX", ParamExtType = MavParamExtType.MavParamExtTypeReal32 },
            new() { Name = "GND_ABS_PRESS", ParamExtType = MavParamExtType.MavParamExtTypeReal64 },
            new() { Name = "SERIAL1_PROTOCOL", ParamExtType = MavParamExtType.MavParamExtTypeCustom },
        };
        return new ParamsExtClientEx(client, _clientExConfig, existDescription);
    }

    [Fact]
    public async Task SendRequestList_ServerShouldSendAllParamList_Success()
    {
        Assert.Equal(0, Client.LocalCount.Value);

        // Arrange
        var param = new List<ParamExtValuePayload>();
        using var sub = Client.Base.OnParamExtValue.Subscribe(p =>
        {
            param.Add(p);
            if (p.ParamCount == param.Count)
                _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await Client.Base.SendRequestList(_cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.Equal(param.Count, res.ParamCount);
        Assert.Equal(Client.RemoteCount.Value, Client.LocalCount.Value);
    }

    [Fact]
    public async Task ReadByIndex_ServerShouldSendExtendedParamValue_Success()
    {
        // Arrange
        using var sub = Client.Base.OnParamExtValue.Subscribe(p => _taskCompletionSource.TrySetResult(p));

        // Act
        await Client.Base.Read(1, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.NotNull(res);
        Assert.Equal(1, res.ParamIndex);
    }
    
    [Fact]
    public async Task ReadOnce_ServerShouldSendExtendedParamValue_Success()
    {
        // Arrange
        var name = "PITCH_MAX";
        using var sub = Client.Base.OnParamExtValue.Subscribe(p =>
        {
            _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await Client.ReadOnce(name, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task;
        Assert.NotNull(res);
        Assert.Equal(name, MavlinkTypesHelper.GetString(res.ParamId));
    }
    
    [Fact] //TODO: fix OnParamSet Encoding/Decoding
    public async Task WriteOnce_ServerShouldSetParameter_Success()
    {
        // Arrange
        var name = "SERVO1_MIN";
        var value = new MavParamExtValue(2000);
        var tcs = new TaskCompletionSource<ParamExtAckPayload>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        using var sub = Client.Base.OnParamExtAck.Subscribe(p =>
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
        Assert.True(payload.ParamResult == ParamAck.ParamAckAccepted);
        Assert.Equal(value, res);
    }

    //TODO: fix OnParamSet Encoding/Decoding
    [Fact]
    public async Task WriteOnce_ShouldFail_WhenValueIsOutOfBounds()
    {
        // Arrange
        var name = "WP_RADIUS";
        var outOfBoundsValue = new MavParamExtValue((byte)0x01);
        var tcs = new TaskCompletionSource<ParamExtAckPayload>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        using var sec = Client.Base.OnParamExtAck.Subscribe(p =>
        {
            if (p.ParamResult != ParamAck.ParamAckAccepted)
                tcs.TrySetResult(p);
        });



        // Act
        await Client.WriteOnce(name, outOfBoundsValue, _cancellationTokenSource.Token);

        // Assert
        var payload = await tcs.Task;
        Assert.NotNull(payload);
        Assert.True(payload.ParamResult == ParamAck.ParamAckFailed);
    }

    [Fact]
    public async Task ReadAll_ClientShouldReadAllParamsFromServer_Success()
    {
        // Arrange
        var param = new List<ParamExtValuePayload>();
        using var sub = Client.Base.OnParamExtValue.Subscribe(p =>
        {
            param.Add(p);
            if (param.Count == p.ParamCount)
                _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await Client.ReadAll(null, _cancellationTokenSource.Token);

        // Assert
        await _taskCompletionSource.Task;
        Assert.NotNull(param);
        Assert.True(_client.IsSynced.Value);
        foreach (var payload in param)
        {
            Assert.True(Client.Items.ContainsKey(MavlinkTypesHelper.GetString(payload.ParamId)));
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}