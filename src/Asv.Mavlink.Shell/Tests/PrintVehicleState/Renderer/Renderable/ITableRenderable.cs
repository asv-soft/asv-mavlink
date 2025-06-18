using Spectre.Console.Rendering;

namespace Asv.Mavlink.Shell;

public interface ITableRenderable
{
    string Name { get; }
    IRenderable Render();
    void Update();
}