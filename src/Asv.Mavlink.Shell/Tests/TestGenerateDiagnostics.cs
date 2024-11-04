using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Diagnostic.Client;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class TestGenerateDiagnosticsCommand
{
    private string _connectionString;
    private byte _targetSystemId;
    private byte _targetComponentId;
    
    private uint _refreshRate;
    private DiagnosticClient _client;
    private IList<(string, string, string)>? _unitedItems = new List<(string, string, string)>();

    /// <summary>
    /// Command creates diagnostic client that retrieves diagnostic data.
    /// </summary>
    /// <param name="connectionString">-cs, The address of the connection to the mavlink diagnostic server</param>
    /// <param name="targetSystemId">-tsid, Server's system id</param>
    /// <param name="targetComponentId">-tcid, Server's component id</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    /// <returns></returns>
    [Command("test-diagnostics")]
    public int Run(string connectionString = "tcp://127.0.0.1:7341", byte targetSystemId = 255, byte targetComponentId = 255, uint refreshRate = 1000)
    {
        _connectionString = connectionString;
        _targetSystemId = targetSystemId;
        _targetComponentId = targetComponentId;
        _refreshRate = refreshRate;
        
        RunAsync().Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }
    
    private async Task RunAsync()
    {
        using var router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects);
        router.WrapToV2ExtensionEnabled = true;
        router.AddPort(new MavlinkPortConfig
        {
            ConnectionString = _connectionString,
            Name = "to Server",
            IsEnabled = true
        });

        var core = new CoreServices(router);
        
        _client = new DiagnosticClient(new MavlinkClientIdentity(3,4,_targetSystemId,_targetComponentId), new DiagnosticClientConfig(),core);
        
        
        
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
                        UniteItems();
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
        if (_unitedItems is null)
        {
            return;
        }

        foreach (var item in _unitedItems)
        {
            table.AddRow(
                $"[red]{item.Item1}[/]",
                item.Item2,
                $"[green]{item.Item3}[/]"
            );
        }
    }

    private void UniteItems()
    {
        _unitedItems?.Clear();
        foreach (var item in _client.IntProbes.Select(x=>x.Value))
        {
            _unitedItems?.Add((
                item.Name, 
                "Int", 
                item.Value.CurrentValue.Item2.ToString()
            ));
        }
        
        foreach (var item in _client.FloatProbes.Select(x=>x.Value))
        {
            _unitedItems?.Add((
                item.Name, 
                "Float", 
                item.Value.CurrentValue.Item2.ToString(CultureInfo.CurrentCulture)
            ));
        }

        _unitedItems = _unitedItems?.OrderBy(i => i.Item1).ToList();
    }
}