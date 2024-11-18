using System;
using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(AsvChartClient))]
public class AsvChartClientTest(ITestOutputHelper log) : ClientTestBase<AsvChartClient>(log)
{
    private readonly AsvChartClientConfig _config = new ()
    {
        MaxTimeToWaitForResponseForListMs = 1000
    };

    protected override AsvChartClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, _config, core);
    
    [Fact]
    public void Ctor_ThrowsExceptions_ArgIsNullFail()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new AsvChartClient(null, _config, Core);
        });
        Assert.Throws<NullReferenceException>(() =>
        {
            var client = new AsvChartClient(Identity, null, Core);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new AsvChartClient(Identity, _config, null);
        });
    }
}