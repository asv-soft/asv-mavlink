using Microsoft.Extensions.Time.Testing;
using TimeProviderExtensions;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ServerTestBase<TServer>
{
    private TServer? _server;

    protected ServerTestBase(ITestOutputHelper log)
    {
        Log = log;
        Link = new VirtualMavlinkConnection();
        ServerTime = new ManualTimeProvider();
        Seq = new PacketSequenceCalculator();
        Identity = new MavlinkIdentity(3, 4);
        Core = new CoreServices(Link.Server, Seq, new TestLoggerFactory(log, ServerTime, "SERVER"), ServerTime, new DefaultMeterFactory());
    }

    

    protected abstract TServer CreateClient(MavlinkIdentity identity, CoreServices core);
    protected TServer Server => _server ??= CreateClient(Identity, Core);
    protected MavlinkIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected CoreServices Core { get; }
    protected PacketSequenceCalculator Seq { get; }
    protected ManualTimeProvider ServerTime { get; }
    protected VirtualMavlinkConnection Link { get; }
}