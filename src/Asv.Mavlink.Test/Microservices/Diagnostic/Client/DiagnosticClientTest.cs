using System;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(DiagnosticClient))]
public class DiagnosticClientTest(ITestOutputHelper log) : ClientTestBase<DiagnosticClient>(log)
{
    private TaskCompletionSource _taskCompletionSource;
    private readonly DiagnosticClientConfig _config = new()
    {
        DeleteProbesTimeoutMs = 30_000,
        CheckProbesDelayMs = 1000,
        MaxCollectionSize = 100
    };

    protected override DiagnosticClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new DiagnosticClient(identity, _config, core);
    }
    
    [Fact]
    public void Client_NotNullProperties_Success()
    {
       Assert.NotNull(Client.DebugFloatArray);
       Assert.NotNull(Client.IntProbes);
       Assert.NotNull(Client.FloatProbes);
       Assert.NotNull(Client.MemoryVector);
       Assert.NotNull(Client.OnFloatProbe);
       Assert.NotNull(Client.OnIntProbe);
    }

    [Fact]
    public void Ctor_InitWithNullArguments_Failed()
    {
        //Assert
        Assert.Throws<ArgumentNullException>(() => new DiagnosticClient(null!, _config, Context));
        Assert.Throws<ArgumentNullException>(() => new DiagnosticClient(Identity, null!, Context));
        Assert.Throws<ArgumentNullException>(() => new DiagnosticClient(Identity, _config, null!));
    }

    [Fact]
    public void Client_alwk()
    {
    }
}