using System;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class StatusTextClientTests(ITestOutputHelper log, VirtualMavlinkConnection? link = null)
    : ClientTestBase<StatusTextClient>(log, link)
{
    protected override StatusTextClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
    
    [Fact]
    public void Ctor_Identity_Arg_Is_Null_Fail()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new StatusTextClient(null,Core);
        });
    }
    
    [Fact]
    public void Ctor_Core_Arg_Is_Null_Fail()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new StatusTextClient(Identity, null);
        });
    }
}