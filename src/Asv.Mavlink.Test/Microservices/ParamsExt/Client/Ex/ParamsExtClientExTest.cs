using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtClientEx))]
public class ParamsExtClientExTest : ClientTestBase<ParamsExtClientEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private List<ParamDescription> _existDescription;
    private readonly ParamsExtClientEx _clientEx;
    private ParamsExtClient _client;
    
    private readonly ParamsExtClientExConfig _config = new()
    {
        ReadAttemptCount = 5,
        ReadTimeoutMs = 100,
        ChunkUpdateBufferMs = 100
    };

    private IEnumerable<ParamExtDescription> ParamDescription { get; } = new List<ParamExtDescription>
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

    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var client = new ParamsExtClient(identity, _config, core);
        return new ParamsExtClientEx(client, _config, ParamDescription);
    }


    public ParamsExtClientExTest(ITestOutputHelper log) : base(log)
    {
        _clientEx = Client;
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    [Fact]
    public void Constructor_Null_Throws()
    {
        // Act
        Assert.Throws<NullReferenceException>(() => new ParamsExtClientEx(null!, _config, ParamDescription));
        Assert.Throws<NullReferenceException>(() => new ParamsExtClientEx(_client, null!, ParamDescription));
        Assert.Throws<NullReferenceException>(() => new ParamsExtClientEx(_client, _config, null!));
    }
    
    [Fact]
    public async Task ReadOnce_ShouldThrowTimeout_Exception()
    {
        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.ReadOnce("BARO_PRIMARY", _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeoutMs * _config.ReadAttemptCount) + 1));
        });
        
        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task ReadAll_ShouldThrowTimeout_Exception()
    {
        // Act
        var t1 = Client.ReadAll(null, _cancellationTokenSource.Token);

        var t2 = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeoutMs * _config.ReadAttemptCount) + 100));
        });
        
        //Assert
        await Task.WhenAll(t1, t2);
        Assert.False(t1.Result);
        Assert.Equal(1, (int) Link.Client.Statistic.TxMessages);
    }
    
    [Fact]
    public async Task WriteOnce_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var value = new MavParamExtValue(2000);
        
        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.WriteOnce("BARO_PRIMARY", value, _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeoutMs * _config.ReadAttemptCount) + 1));
        });
        
        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Fact]
    public async Task ReadOnce_PacketSend_ThrowWhenTimeout()
    {
        // Arrange
        var name = "BARO_PRIMARY";
        
        // Act
        var task = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.ReadOnce(name, _cancellationTokenSource.Token);
        });
        
        var taskTime = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeoutMs * _config.ReadAttemptCount) + 1));
        });
        
        //Assert
        await Task.WhenAll(task, taskTime);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}