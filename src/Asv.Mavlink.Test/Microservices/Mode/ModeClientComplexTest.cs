using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FakeWorkMode : IWorkModeHandler
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
        return new ModeServer(identity,
            new HeartbeatServer(identity, _hbServer, core),
            new CommandLongServerEx(new CommandServer(identity, core)),
            new StatusTextServer(identity, _serverStatus, core),
            ArduCopterMode.Unknown, ArduCopterMode.AllModes, x => new FakeWorkMode(x));
    }

    protected override IModeClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new ModeClient(new HeartbeatClient(identity, _clientHbConfig, core),
            new CommandClient(identity, _clientCommandConfig, core), ArduCopterMode.Unknown, ArduCopterMode.AllModes);
    }

    [Fact]
    public async Task Client_TrySetMode_Success()
    {
        var mode = Client.AvailableModes.First();
        Server.CurrentMode.Subscribe(m => { Assert.Equal(m.Mode, mode); });
        await Client.SetMode(mode);
        Assert.Equal(mode, Server.CurrentMode.CurrentValue.Mode);
    }

    [Fact]
    public async Task Client_ServerAvailableModesAreEqual_Success()
    {
        var clientModes = Client.AvailableModes.OrderByDescending(_ => _.Name);
        var serverModes = Server.AvailableModes.OrderByDescending(_ => _.Name);
        Assert.True(clientModes
            .IsDeepEqual(serverModes));
        await Server.DisposeAsync();
        await Client.DisposeAsync();
    }

    [Fact]
    public async Task Client_TrySetModeThatDoesNotCompatible_Fail()
    {
        var mode = Server.CurrentMode.CurrentValue.Mode; // if we not ask for mode - the test runs infinitely
        await Client.SetMode(Px4Mode.Acro);
        Server.CurrentMode.Subscribe(m => { Assert.Equal(m.Mode, ArduCopterMode.Unknown); });
        await Client.DisposeAsync();
        await Server.DisposeAsync();
    }

    [Fact]
    public async Task Client_TrySetModeCancellationCanceled_Fail()
    {
        var tcs = new CancellationTokenSource();
        var mode = Server.CurrentMode.CurrentValue.Mode;
        await tcs.CancelAsync();
        var t1 = Client.SetMode(mode, tcs.Token);
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await t1);
        await Client.DisposeAsync();
        await Server.DisposeAsync();
    }
}