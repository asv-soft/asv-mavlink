using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaServerEx))]
public class AsvRsgaServerExTest(ITestOutputHelper log) : ServerTestBase<AsvRsgaServerEx>(log)
{
    private readonly StatusTextLoggerConfig _loggerConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };

    protected override AsvRsgaServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new AsvRsgaServerEx(new AsvRsgaServer(identity, core),
            new StatusTextServer(identity, _loggerConfig, core),
            new CommandLongServerEx(new CommandServer(identity, core)));
    }
}