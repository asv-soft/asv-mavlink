using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ClientServerTestBase<TClient,TServer>
{
    private TClient? _client;
    private TServer? _server;

    protected ClientServerTestBase(ITestOutputHelper log)
    {
        Link = new VirtualMavlinkConnection();
        Log = log;

        Identity = new MavlinkClientIdentity(1,2,3,4);
        
        ClientTime = new FakeTimeProvider();
        ClientSeq = new PacketSequenceCalculator();
        ClientCore = new CoreServices(Link.Client, ClientSeq, new TestLoggerFactory(log, ClientTime, "CLIENT"), ClientTime, new DefaultMeterFactory());
        
        ServerTime = new FakeTimeProvider();
        ServerSeq = new PacketSequenceCalculator();
        ServerCore = new CoreServices(Link.Server, ServerSeq, new TestLoggerFactory(log, ServerTime, "SERVER"), ServerTime, new DefaultMeterFactory());
    }

    protected MavlinkClientIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected VirtualMavlinkConnection Link { get; }
    protected FakeTimeProvider ClientTime{ get; }
    protected PacketSequenceCalculator ClientSeq{ get; }
    protected CoreServices ClientCore{ get; }

    protected TClient Client => _client ??= CreateClient(Identity, ClientCore);
    protected FakeTimeProvider ServerTime{ get; }
    protected PacketSequenceCalculator ServerSeq{ get; }
    protected CoreServices ServerCore{ get; }

    protected TServer Server => _server ??=CreateServer(Identity.Target, ServerCore);

    protected abstract TServer CreateServer(MavlinkIdentity identity, ICoreServices core);
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, ICoreServices core);
}