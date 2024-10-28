using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Diagnostic.Server;
using ConsoleAppFramework;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class GenerateDiagnostics
{
    private string _file = "exampleDiagnosticsConfig.json";
    private MavlinkRouter _router;
    private IDiagnosticServer? _server;
    private uint _refreshRate;
    
    /// <summary>
    /// Command creates fake diagnostics data from file and opens a mavlink connection.
    /// </summary>
    /// <param name="cfgPath">-cfg, location of the config file for the generator</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    /// <returns></returns>
    [Command("generate-diagnostics")]
    public int Run(string? cfgPath = null, uint refreshRate = 2000)
    {
        _file = cfgPath ?? _file;
        _refreshRate = refreshRate;
        
        RunAsync().Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }

    private async Task RunAsync()
    {
        AnsiConsole.MarkupLine($"[blue]info[/]: Check config file exist: [green]{_file}[/]");
        
        if (!File.Exists(_file))
        {
            AnsiConsole.MarkupLine($"[yellow]warn[/]: Creating default config file: {_file}");
            var json = JsonConvert.SerializeObject(GenerateDiagnosticsConfig.Default, Formatting.Indented);
            await File.WriteAllTextAsync(_file, json);
        }

        var cfg = JsonConvert.DeserializeObject<GenerateDiagnosticsConfig>(await File.ReadAllTextAsync(_file));

        if (cfg is null)
        {
            AnsiConsole.MarkupLine("[red]error[/]: Unable to load cfg[/]");
            return;
        }
        
        if (cfg.Metrics.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]error[/]: Metrics setting not found in the configuration file: [yellow]{_file}[/]");
            return;
        }
        
        AnsiConsole.MarkupLine("[blue]info[/]: Sort metrics by name");
        cfg.Metrics = cfg.Metrics
            .OrderBy(x => x.Name)
            .ToHashSet(Metric.MetricEqualityComparer.Instance.Value);
        
        var table = new Table();
        table.Title("[[ [yellow]Diagnostics info[/] ]]");
        table.AddColumn("Name");
        table.AddColumn("Type");
        table.AddColumn("Value");

        try
        {
            AnsiConsole.MarkupLine($"[blue]info[/]: Start Diagnostics server with SystemId: [yellow]{cfg.SystemId}[/], ComponentId: [yellow]{cfg.ComponentId}[/]");
            _server = SetUpServer(cfg);
            
            await AnsiConsole.Live(table)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Cropping(VerticalOverflowCropping.Top)
                .StartAsync(async ctx =>
                {
                    var runForever = true;
                    Console.CancelKeyPress += (_, _) => { runForever = false; };

                    while (runForever)
                    {
                        table.Rows.Clear();
                        await RenderRows(table, cfg.Metrics);
                        ctx.Refresh();
                        await Task.Delay(TimeSpan.FromMilliseconds(_refreshRate));
                    }
                });
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]error[/]: {e.Message}");
        }
        finally
        {
            AnsiConsole.MarkupLine("[blue]info[/]: Done");
            _router.Dispose();
        }
    }
    
    private async Task RenderRows(Table table, ISet<Metric> metrics)
    {
        foreach (var metric in metrics)
        {
            switch (metric.Type)
            {
                case MetricType.Int:
                    var valueInt = metric.RandomIntValue();

                    await _server!.Send(metric.Name, valueInt);
                    
                    table.AddRow(
                        $"[red]{metric.Name}[/]",
                        $"{metric.Type.ToString()}",
                        $"[green]{valueInt}[/]"
                    );
                    break;
                case MetricType.Float:
                    var valueFloat = metric.RandomFloatValue();
                    
                    await _server!.Send(metric.Name, valueFloat);

                    table.AddRow(
                        $"[red]{metric.Name}[/]",
                        $"{metric.Type.ToString()}",
                        $"[green]{valueFloat}[/]"
                    );
                    break;
                case MetricType.FloatArray:
                    var valueFloatArray = metric.RandomFloatArrayValue();

                    await _server!.Send(metric.Name, (ushort)Random.Shared.Next(0, ushort.MaxValue),
                        valueFloatArray);

                    table.AddRow(
                        $"[red]{metric.Name}[/]",
                        $"{metric.Type.ToString()}",
                        $"[green]{string.Join("; ", valueFloatArray)}[/]"
                    );
                    break;
                case MetricType.SByteArray:
                    throw new NotImplementedException("SByte array is not supported right now");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private IDiagnosticServer SetUpServer(GenerateDiagnosticsConfig config)
    {
        _router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects, publishScheduler: Scheduler.Default);
        _router.WrapToV2ExtensionEnabled = true;
        foreach (var port in config.Ports)
        {
            AnsiConsole.MarkupLine($"[green]Add connection port {port.Name}[/]: [yellow]{port.ConnectionString}[/]");
            _router.AddPort(port);
        }
        
        var server = new DiagnosticServer(
            new DiagnosticServerConfig {MaxSendIntervalMs = config.ServerMaxSendIntervalMs},
            _router, 
            new MavlinkIdentity(config.SystemId, config.ComponentId), 
            new PacketSequenceCalculator(), 
            TimeProvider.System,
            Scheduler.Default
        );

        return server;
    }
}