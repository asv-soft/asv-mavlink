using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Xunit;

namespace Asv.Mavlink.Test;

public class AdsbVehicleTest : DisposableOnceWithCancel
{
    [Fact]
    public async Task Check_Single_Adsb_Vehicle()
    {
        var link = new VirtualMavlinkConnection();
    
        var server = new AdsbServerDevice(
            link.Server,
            new PacketSequenceCalculator(),
            new MavlinkIdentity (2,2),
            new AdsbServerDeviceConfig(), 
            Scheduler.Default);
    
        var client = new AdsbClientDevice(
            link.Client,
            new MavlinkClientIdentity { ComponentId = 1, SystemId = 1, TargetComponentId = 2, TargetSystemId = 2 },
            new PacketSequenceCalculator(),
            new AdsbClientDeviceConfig(), Scheduler.Default);
        
        server.Start();
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 1313;
        });

        IAdsbVehicle target = new AdsbVehicle(new AdsbVehiclePayload());
        client.Adsb.Targets
            .Transform(_ => target = _)
            .Subscribe();
        await Task.Delay(1000);
        
        Assert.Equal(100, target.Location.Value.Altitude);
        Assert.Equal("UFO", target.CallSign.Value);
        Assert.Equal(45, target.Location.Value.Longitude);
        Assert.Equal(AdsbFlags.AdsbFlagsSimulated, target.Flags.Value);
        Assert.Equal(15, target.Squawk.Value);
        Assert.Equal(13, target.Heading.Value);
        Assert.Equal(100, target.Location.Value.Latitude);
        Assert.Equal(AdsbAltitudeType.AdsbAltitudeTypeGeometric, target.AltitudeType.Value);
        Assert.Equal(AdsbEmitterType.AdsbEmitterTypeNoInfo, target.EmitterType.Value);
        Assert.Equal(150, target.HorVelocity.Value);
        Assert.Equal(75, target.VerVelocity.Value);
        Assert.Equal(1313U, target.IcaoAddress);
    }
    
    [Fact]
    public async Task Check_Multiple_Adsb_Vehicles()
    {
        var link = new VirtualMavlinkConnection();
    
        var server = new AdsbServerDevice(
            link.Server,
            new PacketSequenceCalculator(),
            new MavlinkIdentity (2,2),
            new AdsbServerDeviceConfig(), 
            Scheduler.Default);
    
        var client = new AdsbClientDevice(
            link.Client,
            new MavlinkClientIdentity { ComponentId = 1, SystemId = 1, TargetComponentId = 2, TargetSystemId = 2 },
            new PacketSequenceCalculator(),
            new AdsbClientDeviceConfig(), Scheduler.Default);
        
        server.Start();
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 1313;
        });
        await Task.Delay(500);
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 2323;
        });
        await Task.Delay(500);
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 3333;
        });
        await Task.Delay(500);
    
        client.Adsb.Targets
            .Bind(out var targets)
            .Subscribe();
        await Task.Delay(1000);
        
        Assert.True(targets.Count == 3);
    }
    
    [Fact]
    public async Task Check_If_Old_Vehicles_Are_Deleted()
    {
        var link = new VirtualMavlinkConnection();
    
        var server = new AdsbServerDevice(
            link.Server,
            new PacketSequenceCalculator(),
            new MavlinkIdentity (2,2),
            new AdsbServerDeviceConfig(), 
            Scheduler.Default);
    
        var client = new AdsbClientDevice(
            link.Client,
            new MavlinkClientIdentity { ComponentId = 1, SystemId = 1, TargetComponentId = 2, TargetSystemId = 2 },
            new PacketSequenceCalculator(),
            new AdsbClientDeviceConfig(), Scheduler.Default);
        
        server.Start();
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 1313;
        });
        await Task.Delay(3100);
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 2323;
        });
        await Task.Delay(3100);
        
        await server.Adsb.Send(_ =>
        {
            _.Altitude = (int)(100 * 1e3);
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = (int)(45 * 1e7);
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = (int)(13 * 1e2);
            _.Lat = (int)(100 * 1e7);
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = (int)(150 * 1e2);
            _.VerVelocity = (int)(75 * 1e2);
            _.IcaoAddress = 3333;
        });

        client.Adsb.Targets
            .Bind(out var targets)
            .Subscribe()
            .DisposeItWith(Disposable);
        
        Assert.Equal(3, targets.Count);
        
        await Task.Delay(6100);
        
        Assert.Equal(2, targets.Count);

        await Task.Delay(5100);
        
        Assert.Single(targets);
    }
}