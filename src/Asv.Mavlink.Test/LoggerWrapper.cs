using System;
using Microsoft.Extensions.Logging;
using R3;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class TestLoggerFactory(ITestOutputHelper testOutputHelper, TimeProvider time, string prefix) : ILoggerFactory
{
    public TimeProvider Time { get; } = time;
    private readonly string _prefix = prefix;

    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(testOutputHelper,Time, $"{prefix}.{categoryName}");
    }

    public void AddProvider(ILoggerProvider provider)
    {
        
    }
}

public class TestLogger(ITestOutputHelper testOutputHelper, TimeProvider time, string? categoryName) : ILogger
{
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => Disposable.Empty;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        try
        {
            testOutputHelper.WriteLine($"{time.GetLocalNow().DateTime:HH:mm:ss.fff,15} |={ConvertToStr(logLevel)}=| {categoryName,-8} | {formatter(state, exception)}");
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