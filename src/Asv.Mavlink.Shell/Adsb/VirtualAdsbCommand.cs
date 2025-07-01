using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class VirtualAdsbCommand
{
    private const double VelocityCoefficient = 1e2;
    private const int DefaultTslc = 1; 
    private const int PercentageCoefficient = 100;
    
    private string _file = "adsb.json";
    private AdsbCommandConfig? _adsbCommandConfig;

    /// <summary>
    /// Generate virtual ADSB Vehicle.
    /// </summary>
    /// <param name="cfg">-cfg, File path with ADSB config. Will be created if not exist. Default 'adsb.json'</param>
    [Command("adsb")]
    public async Task<int> RunAdsb(string? cfg = null)
    {
        _file = cfg ?? _file;
        
        AnsiConsole.MarkupLine($"[blue]info[/]: Check config file exist: [blue]{_file}[/]");

        if (!File.Exists(_file))
        {
            AnsiConsole.MarkupLine($"[blue]info[/]: Creating default config file: [blue]{_file}[/]");
            await File.WriteAllTextAsync(
                _file, 
                JsonConvert.SerializeObject(AdsbCommandConfig.Default, Formatting.Indented));
        }

        var configFileTextContent = await File.ReadAllTextAsync(_file);
        _adsbCommandConfig = JsonConvert.DeserializeObject<AdsbCommandConfig>(configFileTextContent);

        if (_adsbCommandConfig?.Vehicles == null || _adsbCommandConfig.Vehicles.Length == 0)
        {
            AnsiConsole.MarkupLine($"[red]error[/]: Vehicles settings not found in the configuration file: [yellow]{_file}[/]");
            return 1;
        }

        try
        {
            await AnsiConsole.Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new ElapsedTimeColumn(),
                    new SpinnerColumn())
                .StartAsync(RunAsync);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return 1;
        }
        
        return 0;
    }

    private async Task RunAsync(ProgressContext ctx)
    {
        if (_adsbCommandConfig is null) return;

        var systemId = $"SystemId: [blue]{_adsbCommandConfig.SystemId}[/]";
        var componentId = $"ComponentId: [blue]{_adsbCommandConfig.ComponentId}[/]";
        AnsiConsole.MarkupLine($"[blue]info[/]: Start virtual ADSB receiver with {systemId}, {componentId}");

        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        await using var router = protocol.CreateRouter("ADSB");
        
        AnsiConsole.MarkupLine("[green]note[/]: The actual connection strings may differ from those shown below");
        
        foreach (var port in _adsbCommandConfig.Ports)
        {
            AnsiConsole.MarkupLine($"[blue]info[/]: Add connection port: [blue]{port}[/]");
            router.AddPort(port);
        }

        var core = new CoreServices(
            router, 
            new PacketSequenceCalculator(), 
            NullLoggerFactory.Instance,
            TimeProvider.System, 
            new DefaultMeterFactory());

        await using var device = ServerDevice.Create(
            new MavlinkIdentity(_adsbCommandConfig.SystemId, _adsbCommandConfig.ComponentId), core, builder =>
            {
                builder.RegisterAdsb();
            });
        device.Start();
        
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        AnsiConsole.MarkupLine($"[blue]info[/]: Found config for {_adsbCommandConfig.Vehicles.Length} vehicles");

        var tasks = new List<Task>();
        foreach (var vehicle in _adsbCommandConfig.Vehicles)
        {
            tasks.Add(RunVehicleAsync(ctx, vehicle, device, cancellationToken));
        }
        await Task.WhenAll(tasks);

        AnsiConsole.MarkupLine("[blue]info[/]: Finished!");
    }

    private async Task RunVehicleAsync(
        ProgressContext ctx, 
        AdsbCommandVehicleConfig cfg, 
        IServerDevice srv, 
        CancellationToken cancellationToken)
    {
        var vehicleTask = ctx.AddTask(cfg.CallSign ?? throw new InvalidOperationException());
        
        if (cfg.Route.Length < 2)
        {
            AnsiConsole.MarkupLine($"[red]error[/]: Vehicle '{cfg.CallSign}' route must contain more than two points");
            return;
        }

        var route = new List<GeoPoint>();
        var totalDistance = 0.0;
        
        for (var index = 0; index < cfg.Route.Length; index++)
        {
            var point = cfg.Route[index];
            
            if (!GeoPointLatitude.TryParse(point.Lat, out var lat))
            {
                AnsiConsole.MarkupLine($"[red]error[/]: Config of '{cfg.CallSign}': invalid latitude at route point {index}");
                return;
            }
            if (!GeoPointLatitude.TryParse(point.Lon, out var lon))
            {
                AnsiConsole.MarkupLine($"[red]error[/]: Config of '{cfg.CallSign}': invalid longitude at route point {index}");
                return;
            }
            
            route.Add(new GeoPoint(lat, lon, point.Alt));
        }
        
        for (var i = 1; i < route.Count; i++)
        {
            totalDistance += route[i - 1].DistanceTo(route[i]);
        }
        
        var currentDistance = 0.0;
        for (var i = 1; i < route.Count; i++)
        {
            currentDistance += await RunVehicleTaskAsync(
                i,
                currentDistance, 
                totalDistance,
                srv, cfg, ctx, 
                vehicleTask,
                route[i - 1],
                cfg.Route[i - 1].Velocity, 
                route[i],
                cfg.Route[i].Velocity,
                cancellationToken);
        }
    }

    private async Task<double> RunVehicleTaskAsync(int index, double dist, double total,
        IServerDevice srv, AdsbCommandVehicleConfig cfg, ProgressContext ctx,
        ProgressTask vehicleTask, GeoPoint from, double velocityFrom, GeoPoint to,
        double velocityTo, CancellationToken cancellationToken)
    {
        var spatialDistance = from.DistanceTo(to);
        var horizontalDistance = from.SetAltitude(0).DistanceTo(to.SetAltitude(0));
        var verticalDistance = Math.Abs(to.Altitude - from.Altitude);
        var azimuth = from.Azimuth(to);
        var timeStep = cfg.UpdateRateMs / 1000.0;

        var currentDistance = 0.0;
        var currentHorizontalDistance = 0.0;
        var currentVerticalDistance = 0.0;

        var tcs = new TaskCompletionSource<bool>();
        cancellationToken.Register(() => tcs.TrySetCanceled());

        using var subscription = Observable.Interval(TimeSpan.FromMilliseconds(timeStep * 1000), cancellationToken)
            .TakeWhile(_ => currentDistance < spatialDistance)
            .SubscribeAwait(
                async (_, ct) =>
                {
                    ct.ThrowIfCancellationRequested();
                    
                    var fractionOfJourney = currentDistance / spatialDistance;
                    var currentVelocity = velocityFrom + (velocityTo - velocityFrom) * fractionOfJourney;
                    var vHorizontal = currentVelocity * (horizontalDistance / spatialDistance);
                    var vVertical = currentVelocity * (verticalDistance / spatialDistance);
                    currentHorizontalDistance += vHorizontal * timeStep;
                    currentVerticalDistance += vVertical * timeStep;
                    var vehiclePos = from.RadialPoint(currentHorizontalDistance, azimuth)
                        .AddAltitude(currentVerticalDistance);
                    currentDistance = from.DistanceTo(vehiclePos);

                    await srv.GetAdsb().Send(x =>
                    {
                        x.Tslc = (byte)(timeStep + DefaultTslc);
                        x.Altitude = MavlinkTypesHelper.AltFromDoubleMeterToInt32Mm(vehiclePos.Altitude);
                        x.Lon = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(vehiclePos.Longitude);
                        x.Lat = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(vehiclePos.Latitude);
                        MavlinkTypesHelper.SetString(x.Callsign, cfg.CallSign ?? throw new InvalidOperationException());
                        x.Flags = AdsbFlags.AdsbFlagsSimulated |
                                  AdsbFlags.AdsbFlagsValidAltitude |
                                  AdsbFlags.AdsbFlagsValidHeading |
                                  AdsbFlags.AdsbFlagsValidCoords |
                                  AdsbFlags.AdsbFlagsValidSquawk;
                        x.Squawk = cfg.Squawk;
                        x.Heading = (ushort)(azimuth * VelocityCoefficient);
                        x.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
                        x.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
                        x.HorVelocity = (ushort)(vHorizontal * VelocityCoefficient);
                        x.VerVelocity = (short)(vVertical * VelocityCoefficient);
                        x.IcaoAddress = cfg.IcaoAddress;
                    }, ct);

                    try
                    {
                        vehicleTask.Description =
                            $"[green]{cfg.CallSign}[/] {index:0}=>{index + 1:0} " +
                            $"V:[blue]{vVertical,-5:F1} m/s[/] " +
                            $"H:[blue]{vHorizontal,-5:F1} m/s[/] " +
                            $"D:[blue]{currentDistance,-5:F0} m[/] from [blue]{spatialDistance,-5:F0} m[/]";
                        vehicleTask.Value = (dist + currentDistance) / total * PercentageCoefficient;
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]error[/]: Cannot update progress for vehicle '{cfg.CallSign}': {ex.Message}");
                    }

                    if (currentDistance >= spatialDistance)
                    {
                        tcs.TrySetResult(true);
                    }
                },
                ex =>
                {
                    AnsiConsole.MarkupLine($"[red]error[/]: Observable stream error in vehicle '{cfg.CallSign}': {ex.Message}");
                    tcs.TrySetResult(true);
                },
                _ =>
                {
                    AnsiConsole.MarkupLine($"[green]note[/]: Stream completed for vehicle '{cfg.CallSign}'");
                },
                AwaitOperation.Sequential,
                false,
                true,
                1
            );
        
        try
        {
            await tcs.Task;
            
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }
        }
        catch (TaskCanceledException)
        {
            AnsiConsole.MarkupLine($"[yellow]warn[/]: Cancelled segment {index} of vehicle '{cfg.CallSign}'");
        }

        return currentDistance;
    }
}