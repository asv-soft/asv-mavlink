using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink.Shell;

public class UiRenderer
{
    private readonly List<ITableRenderer> _modules;

    public UiRenderer(params ITableRenderer[] modules)
    {
        _modules = modules.ToList();
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var table = new Table().Expand().AddColumns(Enumerable.Repeat("Column", _modules.Count).ToArray());

        foreach (var renderer in _modules)
        {
            table.AddColumn(" ");
        }

        var row = _modules.Select(m => m.Render()).ToArray();
        table.AddRow(row);

        await AnsiConsole.Live(table).AutoClear(true).StartAsync(async ctx =>
        {
            while (!ct.IsCancellationRequested)
            {
                foreach (var module in _modules)
                {
                    module.Update();
                }

                for (int i = 0; i < _modules.Count; i++)
                {
                    table.UpdateCell(0, i, _modules[i].Render());
                }

                ctx.Refresh();
                await Task.Delay(1000, ct);
            }
        });
    }
}