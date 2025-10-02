using Spectre.Console;

namespace Asv.Mavlink.Shell;

public interface ITableRenderer
{
    Table Render();
    void Update();
}