using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class MavlinkClientDeviceTestBase<TClientDevice>(ITestOutputHelper log)
    : ClientTestBase<TClientDevice>(log)
    where TClientDevice : MavlinkClientDevice
{
    [Fact]
    public void Ctor_WithDefaultArguments_Success()
    {
        Assert.NotNull(Client);
    }
}