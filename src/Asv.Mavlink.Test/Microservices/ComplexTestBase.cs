using Microsoft.Extensions.Time.Testing;
using TimeProviderExtensions;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ComplexTestBase<TClient,TServer>
{
    private TClient? _client;
    private TServer? _server;

    protected ComplexTestBase(ITestOutputHelper log)
    {
        Link = new VirtualMavlinkConnection();
        Log = log;

        Identity = new MavlinkClientIdentity(1,2,3,4);
        
        ClientTime = new ManualTimeProvider();
        ClientSeq = new PacketSequenceCalculator();
        ClientCore = new CoreServices(Link.Client, ClientSeq, new TestLoggerFactory(log, ClientTime, "CLIENT"), ClientTime, new DefaultMeterFactory());
        
        ServerTime = new ManualTimeProvider();
        ServerSeq = new PacketSequenceCalculator();
        ServerCore = new CoreServices(Link.Server, ServerSeq, new TestLoggerFactory(log, ServerTime, "SERVER"), ServerTime, new DefaultMeterFactory());
    }

    protected MavlinkClientIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected VirtualMavlinkConnection Link { get; }
    protected ManualTimeProvider ClientTime{ get; }
    protected PacketSequenceCalculator ClientSeq{ get; }
    protected CoreServices ClientCore{ get; }

    protected TClient Client => _client ??= CreateClient(Identity, ClientCore);
    protected ManualTimeProvider ServerTime{ get; }
    protected PacketSequenceCalculator ServerSeq{ get; }
    protected CoreServices ServerCore{ get; }

    protected TServer Server => _server ??=CreateServer(Identity.Target, ServerCore);

    protected abstract TServer CreateServer(MavlinkIdentity identity, ICoreServices core);
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, ICoreServices core);
}