using System;
using System.Collections.Generic;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.Common;
using Asv.Mavlink.Diagnostic;
using Asv.Mavlink.Diagnostic.Server;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ServerDeviceBuilder))]
public class ServerDeviceBuilderTest : ServerTestBase<ServerDeviceBuilder>
{
    private readonly ServerDeviceBuilder _builder;
    
    public ServerDeviceBuilderTest(ITestOutputHelper log) 
        : base(log)
    {
        _builder = Server;
    }
    
    [Fact]
    public void Build_WithAdsb_Success()
    {
        // Arrange
        _builder.RegisterAdsb();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAdsbVehicleServer>();
        Assert.NotNull(res);
        Assert.IsType<IAdsbVehicleServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithCharts_Success()
    {
        // Arrange
        _builder.RegisterCharts();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvChartServer>();
        Assert.NotNull(res);
        Assert.IsType<IAsvChartServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithCommand_Success()
    {
        // Arrange
        _builder.RegisterCommand();
        
        // Act
        using var device = _builder.Build(); 
        
        // Assert
        var res = device.Get<ICommandServer>();
        Assert.NotNull(res);
        Assert.IsType<ICommandServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithFtp_Success()
    {
        // Arrange
        _builder.RegisterFtp();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IFtpServer>();
        Assert.NotNull(res);
        Assert.IsType<IFtpServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithGbs_Success()
    {
        // Arrange
        _builder.RegisterGbs();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvGbsServer>();
        Assert.NotNull(res);
        Assert.IsType<IAsvGbsServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithHeartbeat_Success()
    {
        // Arrange
        _builder.RegisterHeartbeat();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IHeartbeatServer>();
        Assert.NotNull(res);
        Assert.IsType<IHeartbeatServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithMission_Success()
    {
        // Arrange
        _builder.RegisterMission();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IMissionServer>();
        Assert.NotNull(res);
        Assert.IsType<IMissionServer>(res, exactMatch: false);
    }
    
    [Fact]
    public void Build_WithMode_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandIntEx();
        _builder.RegisterCommandLongEx();
        _builder.RegisterHeartbeat();
        _builder.RegisterMode(
            ArduCopterMode.Auto, 
            ArduCopterMode.AllModes, 
            _ => new FakeWorkMode(ArduCopterMode.Acro)
        );
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IModeServer>();
        Assert.NotNull(res);
        Assert.IsType<IModeServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithRadio_Success()
    {
        // Arrange
        _builder.RegisterRadio();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvRadioServer>();
        Assert.NotNull(res);
        Assert.IsType<IAsvRadioServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithSdr_Success()
    {
        // Arrange
        _builder.RegisterSdr();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvSdrServer>();
        Assert.NotNull(res);
        Assert.IsType<IAsvSdrServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithParams_Success()
    {
        // Arrange
        _builder.RegisterParams();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IParamsServer>();
        Assert.NotNull(res);
        Assert.IsType<IParamsServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithStatus_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IStatusTextServer>();
        Assert.NotNull(res);
        Assert.IsType<IStatusTextServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithDiagnostic_Success()
    {
        // Arrange
        _builder.RegisterDiagnostic();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IDiagnosticServer>();
        Assert.NotNull(res);
        Assert.IsType<IDiagnosticServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithCommandLongEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandLongEx();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<ICommandServerEx<CommandLongPacket>>();
        Assert.NotNull(res);
        Assert.IsType<ICommandServerEx<CommandLongPacket>>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithCommandIntEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandIntEx();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<ICommandServerEx<CommandIntPacket>>();
        Assert.NotNull(res);
        Assert.IsType<ICommandServerEx<CommandIntPacket>>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithRsga_Success()
    {
        // Arrange
        _builder.RegisterRsga();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvRsgaServer>();
        Assert.NotNull(res);
        Assert.IsType<IAsvRsgaServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithMissionEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterMission();
        _builder.RegisterCommand();
        _builder.RegisterCommandLongEx();
        _builder.RegisterMissionEx();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IMissionServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IMissionServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithParamsExtEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterParamsExt();
        _builder.RegisterParamsExtEx(
            [
                new MavParamExtTypeMetadata(
                    "Custom", 
                    MavParamExtType.MavParamExtTypeCustom
                )
            ]
        );
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IParamsExtServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IParamsExtServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithFtpEx_Success()
    {
        // Arrange
        _builder.RegisterFtp();
        _builder.RegisterFtpEx();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IFtpServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IFtpServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithV2Ext_Success()
    {
        // Arrange
        _builder.RegisterV2Ext();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IV2ExtensionServer>();
        Assert.NotNull(res);
        Assert.IsType<IV2ExtensionServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithRsgaEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandLongEx();
        _builder.RegisterRsga();
        _builder.RegisterRsgaEx(
            AsvRadioCapabilities.Empty, 
            new HashSet<AsvAudioCodec>(5)
        );
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvRsgaServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IAsvRsgaServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithParamsExt_Success()
    {
        // Arrange
        _builder.RegisterParamsExt();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IParamsExtServer>();
        Assert.NotNull(res);
        Assert.IsType<IParamsExtServer>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithRadioEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandLongEx();
        _builder.RegisterHeartbeat();
        _builder.RegisterRadio();
        _builder.RegisterRadioEx(
            AsvRadioCapabilities.Empty, 
            new HashSet<AsvAudioCodec>(5)
        );
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvRadioServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IAsvRadioServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithGbsEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandLongEx();
        _builder.RegisterHeartbeat();
        _builder.RegisterGbs();
        _builder.RegisterGbsEx();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvGbsServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IAsvGbsServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithSdrEx_Success()
    {
        // Arrange
        _builder.RegisterStatus();
        _builder.RegisterCommand();
        _builder.RegisterCommandLongEx();
        _builder.RegisterHeartbeat();
        _builder.RegisterSdr();
        _builder.RegisterSdrEx();
        
        // Act
        using var device = _builder.Build();
        
        // Assert
        var res = device.Get<IAsvSdrServerEx>();
        Assert.NotNull(res);
        Assert.IsType<IAsvSdrServerEx>(res, exactMatch: false);
    }

    [Fact]
    public void Build_WithNoServices_Fail()
    {
        // Act
        using var device = _builder.Build();
        
        // Assert
        Assert.NotNull(device);
    }

    [Fact]
    public void Build_RegisterMoreThanOneSameService_Fail()
    {
        // Arrange
        _builder.RegisterStatus();
        
        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _builder.RegisterStatus();
        });
    }

    [Fact]
    public void Build_TryBuildWithNotEnoughDependencies_Fail()
    {
        // Arrange
        _builder.RegisterSdrEx();
        
        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _builder.Build();
        });
    }
    
    protected override ServerDeviceBuilder CreateServer(MavlinkIdentity identity, CoreServices core)
    {
        return new ServerDeviceBuilder(identity, core);
    }
}