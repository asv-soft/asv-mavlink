using Asv.Common;
using Asv.IO;
using TimeProviderExtensions;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ClientTestBase<TClient>
{
    private TClient? _client;

    protected ClientTestBase(ITestOutputHelper log)
    {
        Log = log;
        
        Time = new ManualTimeProvider();
        Seq = new PacketSequenceCalculator();
        Identity = new MavlinkClientIdentity(1, 2, 3, 4);
        var loggerFactory = new TestLoggerFactory(log, Time, "SERVER");
        
        var protocol = Protocol.Create(builder =>
        {
            builder.SetLog(loggerFactory);
            builder.SetTimeProvider(Time);
            builder.RegisterMavlinkV2Protocol();
            
        });
        Link = protocol.CreateVirtualConnection();
        Context = new CoreServices(Link.Client, Seq, new TestLoggerFactory(log, Time, "CLIENT"), Time, new DefaultMeterFactory());
    }
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, CoreServices core);
    protected MavlinkClientIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected TClient Client => _client ??= CreateClient(Identity, Context);
    protected CoreServices Context { get; }
    protected PacketSequenceCalculator Seq { get; }
    protected ManualTimeProvider Time { get; }
    protected IVirtualConnection Link { get; }
}