using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(StatusTextClient))]
public class StatusTextClientTest(ITestOutputHelper log) : ClientTestBase<StatusTextClient>(log)
{
    protected override StatusTextClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
    
    [Fact]
    public void Ctor_Identity_Arg_Is_Null_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = new StatusTextClient(null!, Core); });
    }
    
    [Fact]
    public void Ctor_Core_Arg_Is_Null_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = new StatusTextClient(Identity, null!); });
    }
}