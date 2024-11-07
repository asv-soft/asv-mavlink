using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AdsbVehicleClient))]
public class AdsbVehicleClientTest(ITestOutputHelper log): ClientTestBase<AdsbVehicleClient>(log)
{
    private readonly AdsbVehicleClientConfig _config = new()
    {
        TargetTimeoutMs = 10_000,
        CheckOldDevicesMs = 3_000,
    };

    protected override AdsbVehicleClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AdsbVehicleClient(identity, _config, core);
    }
}