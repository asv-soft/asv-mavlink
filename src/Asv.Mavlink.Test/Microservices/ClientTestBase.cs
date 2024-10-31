using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ClientTestBase<TClient>
{
    private TClient? _client;

    protected ClientTestBase(ITestOutputHelper log)
    {
        Log = log;
        Link = new VirtualMavlinkConnection();
        Time = new FakeTimeProvider();
        Seq = new PacketSequenceCalculator();
        Identity = new MavlinkClientIdentity(1, 2, 3, 4);
        Core = new CoreServices(Link.Client, Seq, new TestLoggerFactory(log, Time, "CLIENT"), Time, new DefaultMeterFactory());
    }
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, CoreServices core);
    protected MavlinkClientIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected TClient Client => _client ??= CreateClient(Identity, Core);
    protected CoreServices Core { get; }
    protected PacketSequenceCalculator Seq { get; }
    protected FakeTimeProvider Time { get; }
    protected VirtualMavlinkConnection Link { get; }
}