using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(TelemetryClient))]
public class TelemetryClientTest(ITestOutputHelper log)
    : ClientTestBase<TelemetryClient>(log)
{
    protected override TelemetryClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);


    
}