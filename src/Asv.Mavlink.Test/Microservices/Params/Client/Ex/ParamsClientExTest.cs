using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsClientEx))]
public class ParamsClientExTest : ClientTestBase<ParamsClientEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private List<ParamDescription> _existDescription;
    private readonly MavParamCStyleEncoding _encoding;
    private readonly ParamsClientEx _clientEx;
    private ParamsClient _client;

    private readonly ParamsClientExConfig _config = new()
    {
        ReadTimeouMs = 100,
        ReadAttemptCount = 3,
        ChunkUpdateBufferMs = 100,
    };

    public ParamsClientExTest(ITestOutputHelper log) : base(log)
    {
        _encoding = new MavParamCStyleEncoding();
        _clientEx = Client;
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override ParamsClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        _client = new ParamsClient(identity, _config, core);
        _existDescription = new List<ParamDescription>
        {
            new() { Name = "BARO_PRIMARY", ParamType = MavParamType.MavParamTypeUint8 },
            new() { Name = "SIM_CAN_TYPE1", ParamType = MavParamType.MavParamTypeInt8 },
            new() { Name = "FENCE_ACTION", ParamType = MavParamType.MavParamTypeUint16 },
            new() { Name = "RC7_DZ", ParamType = MavParamType.MavParamTypeInt16 },
            new() { Name = "LOG_BITMASK", ParamType = MavParamType.MavParamTypeUint32 },
            new() { Name = "WPNAV_SPEED", ParamType = MavParamType.MavParamTypeInt32 },
            new() { Name = "BARO_ALT_OFFSET", ParamType = MavParamType.MavParamTypeReal32 },
        };
        return new ParamsClientEx(_client, _config, _encoding, _existDescription);
    }

    [Fact]
    public void Constructor_Null_Throws()
    {
        // Act
        Assert.Throws<NullReferenceException>(() => new ParamsClientEx(null!, _config, _encoding, _existDescription));
        Assert.Throws<ArgumentNullException>(() => new ParamsClientEx(_client, null!, _encoding, _existDescription));
        Assert.Throws<ArgumentNullException>(() => new ParamsClientEx(_client, _config, null!, _existDescription));
        Assert.Throws<ArgumentNullException>(() => new ParamsClientEx(_client, _config, _encoding, null!));
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
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeouMs * _config.ReadAttemptCount) + 1));
        });
        
        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Fact(Skip = "Test is not relevant")]
    public async Task ReadAll_ShouldThrowTimeout_Exception()
    {
        // Act
        var t1 = Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await Client.ReadAll(default, default, _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeouMs * _config.ReadAttemptCount) + 1));
        });
        
        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages); 
    }
    
    [Fact]
    public async Task WriteOnce_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var value = new MavParamValue(2000);
        
        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.WriteOnce("BARO_PRIMARY", value, _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeouMs * _config.ReadAttemptCount) + 1));
        });

        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }
    
    [Fact]
    public async Task Read_PacketSend_ThrowWhenTimeout()
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
            Time.Advance(TimeSpan.FromMilliseconds((_config.ReadTimeouMs * _config.ReadAttemptCount) + 1));
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