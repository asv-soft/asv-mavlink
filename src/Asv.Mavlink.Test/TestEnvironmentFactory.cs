using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public static class TestEnvironmentFactory
{
    public static VirtualMavlinkConnection CreateFullQualityConnection()
    {
        return new VirtualMavlinkConnection();
    }
    public static ICoreServices CreateCoreServices(IMavlinkV2Connection link, ITestOutputHelper output, string prefix, out FakeTimeProvider time, out PacketSequenceCalculator seq)
    {
        time = new FakeTimeProvider();
        seq = new PacketSequenceCalculator();
        return new CoreServices(link, seq, new TestLoggerFactory(output, time, prefix), time,
            new DefaultMeterFactory());
    }
    public static ICoreServices CreateClientCore(IMavlinkV2Connection link, ITestOutputHelper output, string prefix, out FakeTimeProvider time, out PacketSequenceCalculator seq)
    {
        time = new FakeTimeProvider();
        seq = new PacketSequenceCalculator();
        return new CoreServices(link, seq, new TestLoggerFactory(output, time, prefix), time,
            new DefaultMeterFactory());
    }
}