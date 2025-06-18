using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Asv.Mavlink.Shell;

public class LogRenderable : ITableRenderable
{
    public const string RenderableName = "Journal";
    
    private const string TableName = "Log";
    private const int MaxCount = 10;
    private readonly Queue<string> _logQueue = new();
    private Table _logTable = new Table().AddColumn(TableName);

    public void Add(string message)
    {
        _logQueue.Enqueue(message);
        
        if (_logQueue.Count > MaxCount)
        {
            _logQueue.Dequeue();
        }
    }

    public void Update()
    {
        _logTable = new Table().AddColumn(TableName);
        
        foreach (var msg in _logQueue)
        {
            _logTable.AddRow(msg);
        }
    }

    public string Name => RenderableName;
    public IRenderable Render() => _logTable;
}