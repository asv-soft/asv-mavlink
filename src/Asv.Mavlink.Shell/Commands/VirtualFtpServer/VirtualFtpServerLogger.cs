using System;
using Microsoft.Extensions.Logging;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class VirtualFtpServerLoggerFactory(string prefix) : ILoggerFactory
{
    private readonly string _prefix = prefix;

    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new VirtualFtpServerLogger($"{prefix}.{categoryName}");
    }

    public void AddProvider(ILoggerProvider provider)
    {
        
    }
}

public class VirtualFtpServerLogger : ILogger
{
    private readonly string? _categoryName;

    public VirtualFtpServerLogger(string? categoryName)
    {
        _categoryName = categoryName;
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => Disposable.Empty;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        try
        {
            var colorStr = SyncWithLogLevelColor(logLevel);
            var formatterStr = Markup.Escape($"{formatter(state, exception)}");
            AnsiConsole.MarkupLine($@"[orange4]{DateTime.Now:HH:mm:ss}[/] [{colorStr}]|={ConvertToStr(logLevel)}=|[/] {_categoryName,-8} [{colorStr}]|[/] {formatterStr}");
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]error[/]:!!! Unknown logger error !!![/]");
        }
    }

    private string SyncWithLogLevelColor(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "blue",
            LogLevel.Debug => "blue",
            LogLevel.Information => "blue",
            LogLevel.Warning => "yellow",
            LogLevel.Error => "red",
            LogLevel.Critical => "maroon",
            LogLevel.None => "silver",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }

    private string ConvertToStr(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "TRC",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRT",
            LogLevel.None => "NON",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
}
