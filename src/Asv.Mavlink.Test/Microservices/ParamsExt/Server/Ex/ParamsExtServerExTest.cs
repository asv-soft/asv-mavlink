using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.ParamsExt.Server.Ex;

[TestSubject(typeof(ParamsExtServerEx))]
public class ParamsExtServerExTest(ITestOutputHelper log)
    : ServerTestBase<ParamsExtServerEx>(log)
{
    protected override ParamsExtServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        throw new System.NotImplementedException();
    }
}