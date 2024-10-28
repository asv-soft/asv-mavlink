using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ServerTestBase<TClient>
{
    private TClient? _server;

    protected ServerTestBase(ITestOutputHelper log, VirtualMavlinkConnection? link = null)
    {
        Log = log;
        Link = link ?? new VirtualMavlinkConnection();
        ServerTime = new FakeTimeProvider();
        ServerSeq = new PacketSequenceCalculator();
        ServerCore = new CoreServices(Link.Client, ServerSeq, new TestLoggerFactory(log, ServerTime, "SERVER"), ServerTime, new DefaultMeterFactory());
    }

    protected abstract TClient CreateClient(MavlinkIdentity identity, CoreServices core);
    public ITestOutputHelper Log { get; }

    protected TClient Server => _server ??= CreateClient(new MavlinkIdentity(3,4), ServerCore);

    protected CoreServices ServerCore { get; }

    protected PacketSequenceCalculator ServerSeq { get; }

    protected FakeTimeProvider ServerTime { get; }

    protected VirtualMavlinkConnection Link { get; }
}