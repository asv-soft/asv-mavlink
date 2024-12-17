using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;


public class FakeWorkMode:IWorkModeHandler
{
    public static IWorkModeHandler IdleMode = new FakeWorkMode(UnknownMode.Instance);
    public static IWorkModeHandler ErrorMode = new FakeWorkMode(UnknownMode.Instance);
    public FakeWorkMode(ICustomMode mode)
    {
        Mode = mode;
    }

    public void Dispose()
    {
        
    }

    public ICustomMode Mode { get; }
    public Task Init(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

    public Task Destroy(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
}

public class TestServer: ModeServer<IWorkModeHandler>
{
    public TestServer(MavlinkIdentity identity, IHeartbeatServer hb, ICommandServerEx<CommandLongPacket> command, IStatusTextServer status, IWorkModeHandler idleMode, IWorkModeHandler errorMode) 
        : base(identity, hb, command, status, idleMode, errorMode)
    {
        
    }

    public override IEnumerable<ICustomMode> AvailableModes => ArduCopterMode.AllModes;
    protected override ValueTask<IWorkModeHandler> InternalCreateMode(ICustomMode mode, CancellationToken cancel)
    {
        return new ValueTask<IWorkModeHandler>(new FakeWorkMode(mode));
    }
}

[TestSubject(typeof(ParamsClientEx))]
[TestSubject(typeof(ParamsServerEx))]
public class ModeComplexTest(ITestOutputHelper log) : ComplexTestBase<IModeClient, IModeServer>(log)
{
    private readonly HeartbeatClientConfig _clientHbConfig = new();
    private readonly CommandProtocolConfig _clientCommandConfig = new();
    private readonly MavlinkHeartbeatServerConfig _hbServer = new();
    private readonly StatusTextLoggerConfig _serverStatus = new();

    protected override IModeServer CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        return new TestServer(identity,
            new HeartbeatServer(identity, _hbServer, core),
            new CommandLongServerEx(new CommandServer(identity, core)),
            new StatusTextServer(identity, _serverStatus, core),
            FakeWorkMode.IdleMode, FakeWorkMode.IdleMode);
    }

    protected override IModeClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new ModeClient(new HeartbeatClient(identity,_clientHbConfig, core), new CommandClient(identity, _clientCommandConfig, core), ArduCopterMode.Unknown,ArduCopterMode.AllModes);
    }
}