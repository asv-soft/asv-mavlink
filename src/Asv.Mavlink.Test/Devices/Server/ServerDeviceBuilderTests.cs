using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.Common;
using Asv.Mavlink.Diagnostic;
using Asv.Mavlink.Diagnostic.Server;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ServerDeviceBuilder))]
public class ServerDeviceBuilderTests : ServerTestBase<ServerDeviceBuilder>
{
    public ServerDeviceBuilderTests(ITestOutputHelper log) : base(log)
    {
    }

    protected override ServerDeviceBuilder CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new ServerDeviceBuilder(identity, core);
    }

    [Fact]
    public void Builder_RegisterAdsb_Success()
    {
        Server.RegisterAdsb();
        var device = Server.Build();
        var res = device.Get<IAdsbVehicleServer>();
        Assert.True(res is not null);
        Assert.True(res is AdsbVehicleServer);
    }

    [Fact]
    public void Builder_RegisterCharts_Success()
    {
        Server.RegisterCharts();
        var device = Server.Build();
        var res = device.Get<IAsvChartServer>();
        Assert.True(res is not null);
        Assert.True(res is AsvChartServer);
    }

    [Fact]
    public void Builder_RegisterCommand_Success()
    {
        Server.RegisterCommand();
        var device = Server.Build();
        var res = device.Get<ICommandServer>();
        Assert.True(res is not null);
        Assert.True(res is CommandServer);
    }

    [Fact]
    public async Task Builder_RegisterFtp_Success()
    {
        Server.RegisterFtp();
        var device = Server.Build();
        var res = device.Get<IFtpServer>();
        Assert.True(res is not null);
        Assert.True(res is FtpServer);
        await device.DisposeAsync();
    }

    [Fact]
    public void Builder_RegisterGbs_Success()
    {
        Server.RegisterGbs();
        var device = Server.Build();
        var res = device.Get<IAsvGbsServer>();
        Assert.True(res is not null);
        Assert.True(res is AsvGbsServer);
    }

    [Fact]
    public void Builder_RegisterHeartbeat_Success()
    {
        Server.RegisterHeartbeat();
        var device = Server.Build();
        var res = device.Get<IHeartbeatServer>();
        Assert.True(res is not null);
        Assert.True(res is HeartbeatServer);
    }

    [Fact]
    public void Builder_RegisterMission_Success()
    {
        Server.RegisterMission();
        var device = Server.Build();
        var res = device.Get<IMissionServer>();
        Assert.True(res is not null);
        Assert.True(res is MissionServer);
    }


    [Fact]
    public async Task Builder_RegisterMode_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandIntEx();
        Server.RegisterCommandLongEx();
        Server.RegisterHeartbeat();
        Server.RegisterMode(ArduCopterMode.Auto, ArduCopterMode.AllModes, _ => new FakeWorkMode(ArduCopterMode.Acro));
        var device = Server.Build();
        var res = device.Get<IModeServer>();
        Assert.True(res is not null);
        Assert.True(res is ModeServer);
        await device.DisposeAsync();
    }

    [Fact]
    public void Builder_RegisterRadio_Success()
    {
        Server.RegisterRadio();
        var device = Server.Build();
        var res = device.Get<IAsvRadioServer>();
        Assert.True(res is not null);
        Assert.True(res is AsvRadioServer);
    }

    [Fact]
    public void Builder_RegisterSdr_Success()
    {
        Server.RegisterSdr();
        var device = Server.Build();
        var res = device.Get<IAsvSdrServer>();
        Assert.True(res is not null);
        Assert.True(res is AsvSdrServer);
    }

    [Fact]
    public void Builder_RegisterParams_Success()
    {
        Server.RegisterParams();
        var device = Server.Build();
        var res = device.Get<IParamsServer>();
        Assert.True(res is not null);
        Assert.True(res is ParamsServer);
    }

    [Fact]
    public void Builder_RegisterStatus_Success()
    {
        Server.RegisterStatus();
        var device = Server.Build();
        var res = device.Get<IStatusTextServer>();
        Assert.True(res is not null);
        Assert.True(res is StatusTextServer);
    }

    [Fact]
    public void Builder_RegisterDiagnostic_Success()
    {
        Server.RegisterDiagnostic();
        var device = Server.Build();
        var res = device.Get<IDiagnosticServer>();
        Assert.True(res is not null);
        Assert.True(res is DiagnosticServer);
    }

    [Fact]
    public async Task Builder_RegisterCommandLongEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandLongEx();
        var device = Server.Build();
        var res = device.Get<ICommandServerEx<CommandLongPacket>>();
        Assert.True(res is not null);
        Assert.True(res is CommandLongServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterCommandIntEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandIntEx();
        var device = Server.Build();
        var res = device.Get<ICommandServerEx<CommandIntPacket>>();
        Assert.True(res is not null);
        Assert.True(res is CommandIntServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public void Builder_RegisterRsga_Success()
    {
        Server.RegisterRsga();
        var device = Server.Build();
        var res = device.Get<IAsvRsgaServer>();
        Assert.True(res is not null);
        Assert.True(res is AsvRsgaServer);
    }

    [Fact]
    public async Task Builder_RegisterMissionEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterMission();
        Server.RegisterMissionEx();
        var device = Server.Build();
        var res = device.Get<IMissionServerEx>();
        Assert.True(res is not null);
        Assert.True(res is MissionServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterParamsExtEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterParamsExt();
        Server.RegisterParamsExtEx([new MavParamExtTypeMetadata("Custom", MavParamExtType.MavParamExtTypeCustom)]);
        var device = Server.Build();
        var res = device.Get<IParamsExtServerEx>();
        Assert.True(res is not null);
        Assert.True(res is ParamsExtServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterFtpEx_Success()
    {
        Server.RegisterFtp();
        Server.RegisterFtpEx();
        var device = Server.Build();
        var res = device.Get<IFtpServerEx>();
        Assert.True(res is not null);
        Assert.True(res is FtpServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public void Builder_RegisterV2Ext_Success()
    {
        Server.RegisterV2Ext();
        var device = Server.Build();
        var res = device.Get<IV2ExtensionServer>();
        Assert.True(res is not null);
        Assert.True(res is V2ExtensionServer);
    }

    [Fact]
    public async Task Builder_RegisterRsgaEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandLongEx();
        Server.RegisterRsga();
        Server.RegisterRsgaEx(AsvRadioCapabilities.Empty, new HashSet<AsvAudioCodec>(5));
        var device = Server.Build();
        var res = device.Get<IAsvRsgaServerEx>();
        Assert.True(res is not null);
        Assert.True(res is AsvRsgaServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterParamsExt_Success()
    {
        Server.RegisterParamsExt();
        var device = Server.Build();
        var res = device.Get<IParamsExtServer>();
        Assert.True(res is not null);
        Assert.True(res is ParamsExtServer);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterRadioEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandLongEx();
        Server.RegisterHeartbeat();
        Server.RegisterRadio();
        Server.RegisterRadioEx(AsvRadioCapabilities.Empty, new HashSet<AsvAudioCodec>(5));
        var device = Server.Build();
        var res = device.Get<IAsvRadioServerEx>();
        Assert.True(res is not null);
        Assert.True(res is AsvRadioServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterGbsEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandLongEx();
        Server.RegisterHeartbeat();
        Server.RegisterGbs();
        Server.RegisterGbsEx();
        var device = Server.Build();
        var res = device.Get<IAsvGbsServerEx>();
        Assert.True(res is not null);
        Assert.True(res is AsvGbsExServer);
        await device.DisposeAsync();
    }

    [Fact]
    public async Task Builder_RegisterSdrEx_Success()
    {
        Server.RegisterStatus();
        Server.RegisterCommand();
        Server.RegisterCommandLongEx();
        Server.RegisterHeartbeat();
        Server.RegisterSdr();
        Server.RegisterSdrEx();
        var device = Server.Build();
        var res = device.Get<IAsvSdrServerEx>();
        Assert.True(res is not null);
        Assert.True(res is AsvSdrServerEx);
        await device.DisposeAsync();
    }

    [Fact]
    public void Builder_RegisterMoreThanOneSameService_Fail()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                Server.RegisterStatus();
            }
            Server.Build();
        });
    }

    [Fact]
    public void Builder_TryRegisterWithNoDeps_Fail()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Server.RegisterSdrEx();
            Server.Build();
        });
    }
}