using Spectre.Console;
using System.Collections.Generic;

namespace Asv.Mavlink.Shell;

public class LogRenderer : ITableRenderer
{
    private readonly Queue<string> _logQueue = new();
    private Table _logTable = new Table().AddColumn("Log");

    public void Add(string message)
    {
        _logQueue.Enqueue(message);
        
        if (_logQueue.Count > 10)
        {
            _logQueue.Dequeue();
        }
    }

    public void Update()
    {
        _logTable = new Table().AddColumn("Log");
        
        foreach (var msg in _logQueue)
        {
            _logTable.AddRow(msg);
        }
    }

    public Table Render() => _logTable;
}