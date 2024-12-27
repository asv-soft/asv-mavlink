using System;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using TimeProviderExtensions;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ComplexTestBase<TClient,TServer>: IDisposable
{
    private TClient? _client;
    private TServer? _server;

    protected ComplexTestBase(ITestOutputHelper log)
    {
        
        Log = log;
        RouterTime = new ManualTimeProvider();
        var loggerFactory = new TestLoggerFactory(log, RouterTime, "ROUTER");
        var protocol = Protocol.Create(builder =>
        {
            builder.SetLog(loggerFactory);
            builder.SetTimeProvider(RouterTime);
            builder.RegisterMavlinkV2Protocol();
            builder.Formatters.RegisterSimpleFormatter();
        });
        
        Link = protocol.CreateVirtualConnection();
        
        Identity = new MavlinkClientIdentity(1,2,3,4);
        
        ClientTime = new ManualTimeProvider();
        ClientSeq = new PacketSequenceCalculator();
        ClientCore = new CoreServices(Link.Client, ClientSeq, new TestLoggerFactory(log, ClientTime, "CLIENT"), ClientTime, new DefaultMeterFactory());
        
        ServerTime = new ManualTimeProvider();
        ServerSeq = new PacketSequenceCalculator();
        ServerCore = new CoreServices(Link.Server, ServerSeq, new TestLoggerFactory(log, ServerTime, "SERVER"), ServerTime, new DefaultMeterFactory());
    }

    public ManualTimeProvider RouterTime { get; }
    protected MavlinkClientIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected IVirtualConnection Link { get; }
    protected ManualTimeProvider ClientTime{ get; }
    protected PacketSequenceCalculator ClientSeq{ get; }
    protected CoreServices ClientCore{ get; }

    protected TClient Client => _client ??= CreateClient(Identity, ClientCore);
    protected ManualTimeProvider ServerTime{ get; }
    protected PacketSequenceCalculator ServerSeq{ get; }
    protected CoreServices ServerCore{ get; }

    protected TServer Server => _server ??=CreateServer(Identity.Target, ServerCore);

    protected abstract TServer CreateServer(MavlinkIdentity identity, IMavlinkContext core);
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core);


    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Link.Dispose();
            if (_client is IDisposable client)
            {
                client.Dispose();
            }
            if (_server is IDisposable server)
            {
                server.Dispose();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}