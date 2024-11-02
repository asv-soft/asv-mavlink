using Asv.Mavlink.Diagnostic.Client;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(DiagnosticClient))]
public class DiagnosticClientTest(ITestOutputHelper log) : ClientTestBase<DiagnosticClient>(log)
{
    private readonly DiagnosticClientConfig _config = new()
    {
        DeleteProbesTimeoutMs = 30_000,
        CheckProbesDelayMs = 1000,
        MaxCollectionSize = 100
    };

    protected override DiagnosticClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new DiagnosticClient(identity, _config, core);
    }
}