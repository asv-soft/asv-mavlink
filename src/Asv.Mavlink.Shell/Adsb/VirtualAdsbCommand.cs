using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class VirtualAdsbCommand
{
    private string _file = "adsb.json";
    private readonly CancellationTokenSource _cancellationTokenSource = new();
        
    /// <summary>
    /// Generate virtual ADSB Vehicle.
    /// </summary>
    /// <param name="cfg">-cfg, File path with ADSB config. Will be created if not exist. Default 'adsb.json'</param>
    [Command("adsb")]
    public async Task RunAdsb(string cfg = null)
    {
        _file = cfg ?? _file;

        await AnsiConsole.Progress()
            .Columns(new ProgressColumn[] 
            {
                new TaskDescriptionColumn(),    // Task description
                new ProgressBarColumn(),        // Progress bar
                new PercentageColumn(),         // Percentage
                new ElapsedTimeColumn(),        // Elapsed time
                new SpinnerColumn(),            // Spinner
            })
            .StartAsync(RunAsync);
    }

    private async Task RunAsync(ProgressContext ctx)
    {
        AnsiConsole.MarkupLine($"[blue]info[/]: Check config file exist: [green]{_file}[/]");

        if (File.Exists(_file) == false)
        {
            AnsiConsole.MarkupLine($"[yellow]warn[/]: Creating default config file: {_file}");
            await File.WriteAllTextAsync(_file, JsonConvert.SerializeObject(AdsbCommandConfig.Default, Formatting.Indented));
        }

        var config = JsonConvert.DeserializeObject<AdsbCommandConfig>(await File.ReadAllTextAsync(_file));

        if (config?.Vehicles == null || config.Vehicles.Length == 0)
        {
            AnsiConsole.MarkupLine($"[red]error[/]: Vehicles settings not found in the configuration file: [yellow]{_file}[/]");
            return;
        }

        AnsiConsole.MarkupLine($"[blue]info[/]: Start virtual ADSB receiver with SystemId: [yellow]{config.SystemId}[/], ComponentId: [yellow]{config.ComponentId}[/]");

        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        var router = protocol.CreateRouter("ADSB");
        foreach (var port in config.Ports)
        {
            AnsiConsole.MarkupLine($"[green]Add connection port [/]: [yellow]{port}[/]");
            router.AddPort(port);
        }

        var core = new CoreServices(router, new PacketSequenceCalculator(), NullLoggerFactory.Instance,
            TimeProvider.System, new DefaultMeterFactory());

        var srv = ServerDevice.Create(new MavlinkIdentity(config.SystemId, config.ComponentId), core, builder =>
        {
            builder.RegisterAdsb();
        });
        srv.Start();

        AnsiConsole.MarkupLine($"[green]Found config for {config.Vehicles.Length} vehicles[/]");
        await Task.WhenAll(config.Vehicles.Select(x => RunVehicleAsync(ctx, x, srv)));

        AnsiConsole.MarkupLine("[green]Finish simulation![/]");
    }

    private async Task RunVehicleAsync(ProgressContext ctx, AdsbCommandVehicleConfig cfg, IServerDevice srv)
    {
        var vehicleTask = ctx.AddTask(cfg.CallSign);
        if (cfg.Route == null || cfg.Route.Length < 2)
        {
            AnsiConsole.MarkupLine($"[red]Vehicle {cfg.CallSign}[/] route must contain more than two points");
            return;
        }

        var route = new List<GeoPoint>();
        var totalDistance = 0.0;
        for (var index = 0; index < cfg.Route.Length; index++)
        {
            var point = cfg.Route[index];
            if (GeoPointLatitude.TryParse(point.Lat, out var lat) == false)
            {
                AnsiConsole.MarkupLine($"[red]Config error {cfg.CallSign}[/]. Invalid latitude at route point {index}");
                return;
            }
            if (GeoPointLatitude.TryParse(point.Lon, out var lon) == false)
            {
                AnsiConsole.MarkupLine($"[red]Config error {cfg.CallSign}[/]. Invalid longitude at route point {index}");
                return;
            }
            route.Add(new GeoPoint(lat,lon,point.Alt));
        }
        for (int i = 1; i < route.Count; i++)
        {
            totalDistance += route[i - 1].DistanceTo(route[i]);
        }
        var currentDistance = 0.0;
        for (int i = 1; i < route.Count; i++)
        {
            currentDistance+= await RunVehicleTaskAsync(i,currentDistance, totalDistance,srv,cfg,ctx, vehicleTask,route[i - 1],cfg.Route[i - 1].Velocity, route[i],cfg.Route[i].Velocity);
        }
    }

    private async Task<double> RunVehicleTaskAsync(int index, double dist, double total, IServerDevice srv,
        AdsbCommandVehicleConfig cfg, ProgressContext ctx, ProgressTask vehicleTask, GeoPoint from,
        double velocityFrom, GeoPoint to, double velocityTo)
    {
        var spatialDistance = from.DistanceTo(to);
        var horizontalDistance = from.SetAltitude(0).DistanceTo(to.SetAltitude(0));
        var verticalDistance = Math.Abs(to.Altitude - from.Altitude);
        var azimuth = from.Azimuth(to);
        var timeStep = (double)cfg.UpdateRateMs / 1000;
        
        var currentDistance = 0.0;
        var currentHorizontalDistance = 0.0;
        var currentVerticalDistance = 0.0;

        while (currentDistance < spatialDistance)
        {
            var fractionOfJourney = currentDistance / spatialDistance;
            var currentVelocity = velocityFrom + (velocityTo - velocityFrom) * fractionOfJourney;
            var vHorizontal = currentVelocity * (horizontalDistance / spatialDistance);
            var vVertical = currentVelocity * (verticalDistance / spatialDistance);
            currentHorizontalDistance += vHorizontal / timeStep;
            currentVerticalDistance += vVertical / timeStep;
            var vehiclePos = from.RadialPoint(currentHorizontalDistance, azimuth)
                .AddAltitude(currentVerticalDistance);
            currentDistance = from.DistanceTo(vehiclePos);
            await srv.GetAdsb().Send(x =>
            {
                x.Tslc = (byte)(timeStep + 1);
                x.Altitude = MavlinkTypesHelper.AltFromDoubleMeterToInt32Mm(vehiclePos.Altitude);
                x.Lon = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(vehiclePos.Longitude);
                x.Lat = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(vehiclePos.Latitude);
                MavlinkTypesHelper.SetString(x.Callsign, cfg.CallSign);
                x.Flags = AdsbFlags.AdsbFlagsSimulated |
                          AdsbFlags.AdsbFlagsValidAltitude |
                          AdsbFlags.AdsbFlagsValidHeading |
                          AdsbFlags.AdsbFlagsValidCoords |
                          AdsbFlags.AdsbFlagsValidSquawk;
                x.Squawk = cfg.Squawk;
                x.Heading = (ushort)(azimuth * 1e2);
                x.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
                x.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
                x.HorVelocity = (ushort)(vHorizontal * 1e2);
                x.VerVelocity = (short)(vVertical * 1e2);
                x.IcaoAddress = cfg.IcaoAddress;
            },  _cancellationTokenSource.Token);

            // Simulate delay for realistic movement
            vehicleTask.Description =
                $"[green]{cfg.CallSign}[/] {index:0}=>{index + 1:0} V:[blue]{vVertical,-5:F1} m/s[/] H:[blue]{vHorizontal,-5:F1}[/] m/s D:[blue]{currentDistance,-5:F0} m[/] from [blue]{spatialDistance,-5:F0} m[/]";
            vehicleTask.Value = ((dist + currentDistance) / total) * 100.0;
            await Task.Delay((int)(timeStep * 1000));
        }

        return spatialDistance;
    }
}
