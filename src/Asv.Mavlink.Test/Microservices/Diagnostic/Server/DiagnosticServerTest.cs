using Asv.Mavlink.Diagnostic.Server;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(DiagnosticServer))]
public class DiagnosticServerTest(ITestOutputHelper log) : ServerTestBase<DiagnosticServer>(log)
{
    private readonly DiagnosticServerConfig _config = new()
    {
        MaxSendIntervalMs = 100,
        IsEnabled = true
    };

    protected override DiagnosticServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity,_config,core);
}