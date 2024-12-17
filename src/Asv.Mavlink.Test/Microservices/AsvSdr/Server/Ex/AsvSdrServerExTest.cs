using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvSdr;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrServerEx))]
public class AsvSdrServerExTest : ServerTestBase<AsvSdrServerEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    private readonly AsvSdrServerConfig _configSdr = new()
    {
        StatusRateMs = 1000,
    };
    private readonly StatusTextLoggerConfig _configStatus = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };
    private readonly MavlinkHeartbeatServerConfig _configHeartbeat = new()
    {
        HeartbeatRateMs = 1000,
    };


    protected override AsvSdrServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        var sdr = new AsvSdrServer(identity, _configSdr, core);
        var statusText = new StatusTextServer(Identity,_configStatus,core);
        var heartbeat = new HeartbeatServer(Identity,_configHeartbeat,core);
        var commandsEx = new CommandLongServerEx(new CommandServer(identity, core));
        return new AsvSdrServerEx(sdr, statusText, heartbeat, commandsEx);
    }

    public AsvSdrServerExTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(25));
    }

    [Fact]
    public async Task StartAndStop_WhenCalled_ShouldStartAndStopSuccessfully()
    {
        // Arrange
        var time = (ulong)DateTimeOffset.Now.Ticks;
        var arr = new char[8];
        var name = "test";
        Array.Copy(name.ToCharArray(), arr, name.ToCharArray().Length);
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        
        Random r = new Random();
        var array = Enumerable.Range(0, 50).Select(_ => r.NextDouble()).ToArray();
        var data = new ReadOnlyMemory<double>(array);
        Server.Base.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(_ => 
            tcs.TrySetResult(_));
        
        // Act
        await Server.SendSignal(time, name, data, AsvSdrSignalFormat.AsvSdrSignalFormatFloat,
            _cancellationTokenSource.Token);
        
        // Assert
        var res = await tcs.Task as AsvSdrSignalRawPacket;
        Assert.NotNull(res);
        Assert.Equal(time, res?.Payload.TimeUnixUsec);
        Assert.Equal(arr, res?.Payload.SignalName);
    }
    
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}