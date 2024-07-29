using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using ManyConsole;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class VirtualAdsbCommand : ConsoleCommand,ILogger
{
    private string _file = "adsb.json";

    public VirtualAdsbCommand()
    {
        IsCommand("adsb", "Generate virtual ADSB Vehicle");
        HasOption("cfg=", $"File path with ADSB config. Will be created if not exist. Default '{_file}'", x => _file = x);
        
    }
   
    public override int Run(string[] remainingArguments)
    {
        return AnsiConsole.Progress()
            .Columns(new ProgressColumn[] 
            {
                new TaskDescriptionColumn(),    // Task description
                new ProgressBarColumn(),        // Progress bar
                new PercentageColumn(),         // Percentage
                new ElapsedTimeColumn(),        // Elapsed time
                new SpinnerColumn(),            // Spinner
            })
            .StartAsync(RunAsync).Result;

        return -1;
    }

    private async Task<int> RunAsync(ProgressContext ctx)
    {
        this.LogInformation("Check config file exist {ConfigFile}", _file);
        if (File.Exists(_file) == false)
        {
            this.LogWarning("Create default config file {ConfigFile}", _file);
            await File.WriteAllTextAsync(_file,JsonConvert.SerializeObject(AdsbCommandConfig.Default, Formatting.Indented));
        }
        var config = JsonConvert.DeserializeObject<AdsbCommandConfig>(await File.ReadAllTextAsync(_file));
        
        if (config.Vehicles == null || config.Vehicles.Length == 0)
        {
            this.LogError("Vehicles settings not found in the configuration file: {ConfigFile}", _file);
            return -1;
        }
        
        this.LogInformation("Start virtual ADSB receiver with SystemId:{SystemId}, ComponentId:{ComponentId}", config.SystemId, config.ComponentId);

        using var router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects);
        foreach (var port in config.Ports)
        {
            this.LogInformation("Add connection port {PortName}: {PortConnectionString}", port.Name, port.ConnectionString);
            router.AddPort(port);
        }
        
        var srv = new AdsbServerDevice(
            router,
            new PacketSequenceCalculator(),
            new MavlinkIdentity(config.SystemId, config.ComponentId),
            new AdsbServerDeviceConfig(),
            Scheduler.Default);
        srv.Start();
        
        this.LogInformation("Found config for {Count} vehicle", config.Vehicles.Length);
        await Task.WhenAll(config.Vehicles.Select(x=>RunVehicleAsync(ctx, x,srv)));
        this.LogInformation("Finish simulation");
        
        return 0;
    }

    private async Task RunVehicleAsync(ProgressContext ctx, AdsbCommandVehicleConfig cfg, AdsbServerDevice srv)
    {
        var vehicleTask = ctx.AddTask(cfg.CallSign);
        if (cfg.Route == null || cfg.Route.Length < 2)
        {
            this.LogError("Vehicle {ConfigIcaoCodeSign} route must contain more than two points", cfg.CallSign);
            return;
        }

        var route = new List<GeoPoint>();
        var totalDistance = 0.0;
        for (var index = 0; index < cfg.Route.Length; index++)
        {
            var point = cfg.Route[index];
            if (GeoPointLatitude.TryParse(point.Lat, out var lat) == false)
            {
                var err = GeoPointLatitude.GetErrorMessage(point.Lat);
                this.LogError("Config error {ConfigIcaoCodeSign}. route point '{Index}.{LatName}={LatString}' parse error:{Err}", cfg.CallSign, index, nameof(point.Lat),point.Lat, err);
            }
            if (GeoPointLatitude.TryParse(point.Lon, out var lon) == false)
            {
                var err = GeoPointLongitude.GetErrorMessage(point.Lon);
                this.LogError("Vehicle {ConfigIcaoCodeSign} route point '{Index}.{LonName}={LonString}' parse error:{Err}", cfg.CallSign, index, nameof(point.Lon),point.Lon, err);
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

    private async Task<double> RunVehicleTaskAsync(int index, double dist, double total, AdsbServerDevice srv,
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
            await srv.Adsb.Send(x =>
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
            });

            // Simulate delay for realistic movement
            vehicleTask.Description =
                $"[green]{cfg.CallSign}[/] {index:0}=>{index + 1:0} V:[blue]{vVertical,-5:F1} m/s[/] H:[blue]{vHorizontal,-5:F1}[/] m/s D:[blue]{currentDistance,-5:F0} m[/] from [blue]{spatialDistance,-5:F0} m[/]";
            vehicleTask.Value = ((dist + currentDistance) / total) * 100.0;
            await Task.Delay((int)(timeStep * 1000));
        }

        return spatialDistance;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                AnsiConsole.MarkupLine("[italic dim grey]trce[/]: " + formatter(state, exception));
                break;
            case LogLevel.Debug:
                AnsiConsole.MarkupLine("[dim grey]dbug[/]: " + formatter(state, exception));
                break;
            case LogLevel.Information:
                AnsiConsole.MarkupLine("[dim deepskyblue2]info[/]: " + formatter(state, exception));
                break;
            case LogLevel.Warning:
                AnsiConsole.MarkupLine("[bold orange3]warn[/]: " + formatter(state, exception));
                break;
            case LogLevel.Error:
                AnsiConsole.MarkupLine("[bold red]fail[/]: " + formatter(state, exception));
                break;
            case LogLevel.Critical:
                AnsiConsole.MarkupLine("[bold underline red on white]crit[/]: " + formatter(state, exception));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel));
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
}