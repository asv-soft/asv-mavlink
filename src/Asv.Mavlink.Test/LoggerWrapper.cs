using System;
using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class TestLoggerFactory(ITestOutputHelper testOutputHelper, string prefix) : ILoggerFactory
{
    private readonly string _prefix = prefix;

    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(testOutputHelper, $"{prefix}.{categoryName}");
    }

    public void AddProvider(ILoggerProvider provider)
    {
        
    }
}

public class TestLogger(ITestOutputHelper testOutputHelper, string? categoryName) : ILogger
{
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => Disposable.Empty;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        try
        {
            testOutputHelper.WriteLine($"{DateTime.Now:HH:mm:ss.fff,15} |={ConvertToStr(logLevel)}=| {categoryName,-8} | {formatter(state, exception)}");
        }
        catch
        {
            // This can happen when the test is not active
        }
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