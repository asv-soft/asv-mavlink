using Asv.IO;
using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class TestContext
{
    public TestContext(ITestOutputHelper log, string logPrefix, IProtocolConnection connection)
    {
        Log = log;
        Time = new FakeTimeProvider();
        Seq = new PacketSequenceCalculator();
        Core = new CoreServices(connection, Seq, new TestLoggerFactory(log, Time, logPrefix), Time, new DefaultMeterFactory());
    }
    
    public ICoreServices Core { get;  }
    public IPacketSequenceCalculator Seq { get; }
    public FakeTimeProvider Time { get; }
    public ITestOutputHelper Log { get; }
}