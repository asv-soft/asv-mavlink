using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

public class VirtualAdsbCommand : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private string _file = "adsb.json";
    private bool _disposed;
    
    private const double VelocityCoefficient = 1e2;
    private const int DefaultTslc = 1; 
    private const int PercentageCoefficient = 100;

    /// <summary>
    /// Generate virtual ADSB Vehicle.
    /// </summary>
    /// <param name="cfg">-cfg, File path with ADSB config. Will be created if not exist. Default 'adsb.json'</param>
    [Command("adsb")]
    public async Task RunAdsb(string? cfg = null)
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
        var vehicleTask = ctx.AddTask(cfg.CallSign ?? throw new InvalidOperationException());
        if (cfg.Route.Length < 2)
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
            route.Add(new GeoPoint(lat, lon, point.Alt));
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

    private async Task<double> RunVehicleTaskAsync(int index, double dist, double total,
        IServerDevice srv, AdsbCommandVehicleConfig cfg, ProgressContext ctx,
        ProgressTask vehicleTask, GeoPoint from, double velocityFrom, GeoPoint to,
        double velocityTo)
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
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
        cts.Token.Register(() => tcs.TrySetCanceled());
        IDisposable? subscription = null;
        
        subscription = R3.Observable.Interval(TimeSpan.FromMilliseconds(timeStep * 1000))
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
                    }, _cancellationTokenSource.Token);

                    try
                    {
                        vehicleTask.Description =
                            $"[green]{cfg.CallSign}[/] {index:0}=>{index + 1:0} " +
                            $"V:[blue]{vVertical,-5:F1} m/s[/] " +
                            $"H:[blue]{vHorizontal,-5:F1}[/] m/s " +
                            $"D:[blue]{currentDistance,-5:F0} m[/] from [blue]{spatialDistance,-5:F0} m[/]";
                        vehicleTask.Value = (dist + currentDistance) / total * PercentageCoefficient;
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Error updating progress for vehicle '{cfg.CallSign}': {ex.Message}[/]");
                    }

                    if (currentDistance >= spatialDistance)
                    {
                        tcs.TrySetResult(true);
                    }
                },
                ex =>
                {
                    AnsiConsole.MarkupLine($"[red]Observable stream error in vehicle '{cfg.CallSign}': {ex.Message}[/]");
                    tcs.TrySetResult(true);
                },
                _ =>
                {
                    AnsiConsole.MarkupLine($"[grey]Stream completed for vehicle '{cfg.CallSign}'[/]");
                },
                AwaitOperation.Sequential,
                false,
                true,
                1
            );
        try
        {
            await tcs.Task;
            if (cts.IsCancellationRequested)
                throw new TaskCanceledException();
        }
        catch (TaskCanceledException)
        {
            AnsiConsole.MarkupLine($"[yellow]Cancelled segment {index} of vehicle {cfg.CallSign}[/]");
        }
        finally
        {
            subscription.Dispose();
        }

        return currentDistance;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _cancellationTokenSource.Dispose();
            _disposed = true;
        }
    }
}