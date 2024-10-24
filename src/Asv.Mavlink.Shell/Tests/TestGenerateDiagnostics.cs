using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Diagnostic.Client;
using ConsoleAppFramework;
using DynamicData;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class TestGenerateDiagnosticsCommand
{
    private string _connectionString;
    private uint _refreshRate;
    private DiagnosticClient _client;

    private ReadOnlyObservableCollection<INamedProbe<int>>? _intItems;
    private ReadOnlyObservableCollection<INamedProbe<float>>? _floatItems;
    
    /// <summary>
    /// Command creates fake diagnostics data from file and opens a mavlink connection.
    /// </summary>
    /// <param name="connectionString">-cs, The address of the connection to the mavlink diagnostic server</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    /// <returns></returns>
    [Command("test-diagnostics")]
    public int Run(string connectionString, uint refreshRate = 1000)
    {
        _connectionString = connectionString;
        _refreshRate = refreshRate;
        
        RunAsync().Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }
    
    private async Task RunAsync()
    {
        using var router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects, publishScheduler: Scheduler.Default);
        router.WrapToV2ExtensionEnabled = true;
        router.AddPort(new MavlinkPortConfig
        {
            ConnectionString = _connectionString,
            Name = "Diagnostic Client",
            IsEnabled = true
        });
        
        _client = new DiagnosticClient(
            new DiagnosticClientConfig(),
            router, 
            new MavlinkClientIdentity
            {
                SystemId = 3,
                ComponentId = 4,
                TargetSystemId = 1,
                TargetComponentId = 2
            }, 
            new PacketSequenceCalculator(), 
            TimeProvider.System,
            Scheduler.Default
        );

        _client.IntProbes
            .Do(_ => { })
            .Bind(out _intItems)
            .DisposeMany()
            .Subscribe();
        _client.FloatProbes
            .Do(_ => { })
            .Bind(out _floatItems)
            .DisposeMany()
            .Subscribe();
        
        var table = new Table();
        table.Title("[[ [yellow]Diagnostics info[/] ]]");
        table.AddColumn("Name");
        table.AddColumn("Type");
        table.AddColumn("Value");

        try
        {
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
                        RenderRows(table);
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
        }
    }
    
    private void RenderRows(Table table)
    {
        if (_intItems is not null)
        {
            foreach (var item in _intItems)
            {
                table.AddRow(
                    $"[red]{item.Name}[/]",
                    "int",
                    $"[green]{item.Value}[/]"
                );
            }
        }
        
        if (_floatItems is not null)
        {
            foreach (var item in _floatItems)
            {
                table.AddRow(
                    $"[red]{item.Name}[/]",
                    "float",
                    $"[green]{item.Value}[/]"
                );
            }
        }
    }
}