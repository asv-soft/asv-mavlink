using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class AdsbVehicleTest
{
    [Fact]
    public async Task TestAdsbVehicle()
    {
        var link = new VirtualLink();

        var server = new AdsbVehicleServer(
            link.Server, 
            new MavlinkServerIdentity { ComponentId = 2, SystemId = 2 },
            new PacketSequenceCalculator(), 
            Scheduler.Default, 
            new AdsbVehicleServerConfig());

        var client = new AdsbVehicleClient(
            link.Client,
            new MavlinkClientIdentity { ComponentId = 1, SystemId = 1, TargetComponentId = 2, TargetSystemId = 2 },
            new PacketSequenceCalculator(),
            Scheduler.Default);
        
        server.Set(_ =>
        {
            _.Altitude = 100;
            _.Callsign = new[] { 'U', 'F', 'O' };
            _.Lon = 45;
            _.Flags = AdsbFlags.AdsbFlagsSimulated;
            _.Squawk = 15;
            _.Heading = 13;
            _.Lat = 100;
            _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            _.HorVelocity = 150;
            _.VerVelocity = 75;
            _.IcaoAddress = 1313;
        });
        
        server.Start();

        await client.Target.FirstAsync();
        await Task.Delay(1000);
        
        Assert.Equal(100, client.Target.Value.Altitude);
        Assert.Equal(new[] {'U', 'F', 'O', '\0', '\0', '\0', '\0', '\0', '\0'}, client.Target.Value.Callsign);
        Assert.Equal(45, client.Target.Value.Lon);
        Assert.Equal(AdsbFlags.AdsbFlagsSimulated, client.Target.Value.Flags);
        Assert.Equal(15, client.Target.Value.Squawk);
        Assert.Equal(13, client.Target.Value.Heading);
        Assert.Equal(100, client.Target.Value.Lat);
        Assert.Equal(AdsbAltitudeType.AdsbAltitudeTypeGeometric, client.Target.Value.AltitudeType);
        Assert.Equal(AdsbEmitterType.AdsbEmitterTypeNoInfo, client.Target.Value.EmitterType);
        Assert.Equal(150, client.Target.Value.HorVelocity);
        Assert.Equal(75, client.Target.Value.VerVelocity);
        Assert.Equal(1313U, client.Target.Value.IcaoAddress);
    }
}