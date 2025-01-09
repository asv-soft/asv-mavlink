using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrServerEx))]
public class AsvSdrServerExTest(ITestOutputHelper log) : ServerTestBase<AsvSdrServerEx>(log)
{
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


    protected override AsvSdrServerEx CreateServer(MavlinkIdentity identity, CoreServices core)
    {
        var sdr = new AsvSdrServer(identity, _configSdr, core);
        var statusText = new StatusTextServer(Identity,_configStatus,core);
        var heartbeat = new HeartbeatServer(Identity,_configHeartbeat,core);
        var commandsEx = new CommandLongServerEx(new CommandServer(identity, core));
        return new AsvSdrServerEx(sdr, statusText, heartbeat, commandsEx);
    }
}