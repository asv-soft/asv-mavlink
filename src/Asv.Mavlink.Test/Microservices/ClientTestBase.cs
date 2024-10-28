using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ClientTestBase<TClient>
{
    private TClient? _client;

    protected ClientTestBase(ITestOutputHelper log, VirtualMavlinkConnection? link = null)
    {
        Log = log;
        Link = link ?? new VirtualMavlinkConnection();
        ClientTime = new FakeTimeProvider();
        ClientSeq = new PacketSequenceCalculator();
        ClientCore = new CoreServices(Link.Client, ClientSeq, new TestLoggerFactory(log, ClientTime, "CLIENT"), ClientTime, new DefaultMeterFactory());
    }

    protected abstract TClient CreateClient(MavlinkClientIdentity identity, CoreServices core);
    public ITestOutputHelper Log { get; }

    protected TClient Client => _client ??= CreateClient(new MavlinkClientIdentity(1,2,3,4), ClientCore);

    protected CoreServices ClientCore { get; }

    protected PacketSequenceCalculator ClientSeq { get; }

    protected FakeTimeProvider ClientTime { get; }

    protected VirtualMavlinkConnection Link { get; }
}