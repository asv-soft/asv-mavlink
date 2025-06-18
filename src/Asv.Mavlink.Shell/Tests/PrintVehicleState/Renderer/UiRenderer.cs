using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink.Shell;

public class UiRenderer(params ITableRenderable[] modules)
{
    private readonly List<ITableRenderable> _modules = modules.ToList();

    public Task RunAsync(CancellationToken ct)
    {
        var table = new Table().Expand().AddColumns(_modules.Select(module => module.Name).ToArray());

        var row = _modules.Select(m => m.Render()).ToArray();
        table.AddRow(row);

        return AnsiConsole.Live(table).AutoClear(true).StartAsync(async ctx =>
        {
            while (!ct.IsCancellationRequested)
            {
                foreach (var module in _modules)
                {
                    module.Update();
                }

                for (var i = 0; i < _modules.Count; i++)
                {
                    table.UpdateCell(0, i, _modules[i].Render());
                }

                ctx.Refresh();
                await Task.Delay(1000, ct);
            }
        });
    }
}