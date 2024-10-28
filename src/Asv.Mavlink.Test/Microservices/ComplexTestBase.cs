using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ComplexTestBase<TClient,TServer>
{
    protected ComplexTestBase(ITestOutputHelper output)
    {
        Link = new VirtualMavlinkConnection();
        
    }

    protected readonly VirtualMavlinkConnection Link;
    protected readonly FakeTimeProvider ClientTime;
    protected readonly PacketSequenceCalculator ClientSeq;
    protected readonly CoreServices ClientCore;
    protected readonly TClient Client;
    protected readonly FakeTimeProvider ServerTime;
    protected readonly PacketSequenceCalculator ServerSeq;
    protected readonly CoreServices ServerCore;
    protected readonly TServer Server;
    
    protected abstract TServer CreateServer(MavlinkIdentity identityTarget, ICoreServices serverCore);
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, ICoreServices core);
}