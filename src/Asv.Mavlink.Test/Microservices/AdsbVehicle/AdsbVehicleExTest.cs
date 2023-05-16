using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class AdsbVehicleExTest
{
    // [Fact]
    // public async Task TestAdsbVehicleEx()
    // {
    //     var link = new VirtualLink();
    //
    //     var server = new AdsbVehicleServer(
    //         link.Server,
    //         new MavlinkServerIdentity { ComponentId = 2, SystemId = 2 },
    //         new PacketSequenceCalculator(),
    //         Scheduler.Default);
    //
    //     var client = new AdsbVehicleClient(
    //         link.Client,
    //         new MavlinkClientIdentity { ComponentId = 1, SystemId = 1, TargetComponentId = 2, TargetSystemId = 2 },
    //         new PacketSequenceCalculator(),
    //         new AdsbVehicleClientConfig(),
    //         Scheduler.Default);
    //
    //     
    //
    //     server.Set(_ =>
    //     {
    //         _.Altitude = 100;
    //         _.Callsign = new[] { 'U', 'F', 'O' };
    //         _.Lon = 45;
    //         _.Flags = AdsbFlags.AdsbFlagsSimulated;
    //         _.Squawk = 15;
    //         _.Heading = 13;
    //         _.Lat = 100;
    //         _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
    //         _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
    //         _.HorVelocity = 150;
    //         _.VerVelocity = 75;
    //         _.IcaoAddress = 1313;
    //     });
    //     
    //    
    //
    //     await client.OnTarget.FirstAsync();
    //     await Task.Delay(1000);
    //     
    //     server.Set(_ =>
    //     {
    //         _.Altitude = 100;
    //         _.Callsign = new[] { 'U', 'A', 'P' };
    //         _.Lon = 45;
    //         _.Flags = AdsbFlags.AdsbFlagsSimulated;
    //         _.Squawk = 15;
    //         _.Heading = 13;
    //         _.Lat = 100;
    //         _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
    //         _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
    //         _.HorVelocity = 150;
    //         _.VerVelocity = 75;
    //         _.IcaoAddress = 2323;
    //     });
    //     
    //     await client.OnTarget.FirstAsync();
    //     await Task.Delay(1000);
    //     
    //     Assert.Equal(2, clientEx.Targets.Count);
    //     
    //     server.Set(_ =>
    //     {
    //         _.Altitude = 100;
    //         _.Callsign = new[] { 'U', 'F', 'O' };
    //         _.Lon = 45;
    //         _.Flags = AdsbFlags.AdsbFlagsSimulated;
    //         _.Squawk = 15;
    //         _.Heading = 19;
    //         _.Lat = 100;
    //         _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
    //         _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
    //         _.HorVelocity = 150;
    //         _.VerVelocity = 75;
    //         _.IcaoAddress = 1313;
    //     });
    //
    //     await client.OnTarget.FirstAsync();
    //     await Task.Delay(1000);
    //
    //     Assert.Equal(new[] {'U', 'F', 'O', '\0', '\0', '\0', '\0', '\0', '\0'}, clientEx.Targets[1].Callsign);
    //     Assert.Equal(19, clientEx.Targets[1].Heading);
    // }
}