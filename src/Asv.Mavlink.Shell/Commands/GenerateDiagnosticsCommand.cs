#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Cfg.Json;
using Asv.IO;
using Asv.Mavlink.Diagnostic.Server;
using ConsoleAppFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

[JsonConverter(typeof(StringEnumConverter))]
internal enum MetricType
{
    Int,
    Float,
    FloatArray,
    SByteArray,
}

internal class Metric
{
    public required string Name { get; init; }
    public required MetricType Type { get; init; }
    public required string Max { get; init; }
    public required string Min { get; init; }

    public int RandomIntValue()
    {
        var minInt = Convert.ToInt32(Min);
        var maxInt = Convert.ToInt32(Max);
        
        return Random.Shared.Next(minInt, maxInt);
    }

    public float RandomFloatValue()
    {
        var minFloat = Convert.ToSingle(Min);
        var maxFloat = Convert.ToSingle(Max);
        
        return Random.Shared.NextSingle() * (maxFloat - minFloat) + minFloat;
    }

    public float[] RandomFloatArrayValue()
    {
        var minInt = Convert.ToInt32(Min);
        var maxInt = Convert.ToInt32(Max);
        
        var arraySize = Random.Shared.Next(minInt, maxInt);
        
        var array = new float[arraySize];
        for (var i = 0; i < arraySize; i++)
        {
            array[i] = RandomFloatValue();
        }

        return array;
    }

    public sbyte[] RandomSByteArrayValue()
    {
        var minInt = Convert.ToInt32(Min);
        var maxInt = Convert.ToInt32(Max);
        
        var arraySize = Random.Shared.Next(minInt, maxInt);
        
        var array = new sbyte[arraySize];
        for (var i = 0; i < arraySize; i++)
        {
            array[i] = (sbyte) Random.Shared.Next(-128, 127);
        }

        return array;
    }
    
    public class MetricEqualityComparer : IEqualityComparer<Metric>
    {
        public bool Equals(Metric? x, Metric? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }
            
            return x.Name == y.Name && x.Type == y.Type;
        }

        public int GetHashCode(Metric obj)
        {
            return obj.Name.GetHashCode() ^ obj.Type.GetHashCode();
        }
    }
}

internal class DiagnosticsConfig
{
    internal ISet<Metric> Metrics { get; private set; } = new HashSet<Metric>(new Metric.MetricEqualityComparer());
}

public class GenerateDiagnosticsCommand
{
    private const string DefaultConfigFileName = "exampleConfig.json";
    private static readonly string DefaultConfigFileFullPath = Path.Join(
        Directory.GetCurrentDirectory(),
        DefaultConfigFileName
    );

    private readonly ISet<Metric> _exampleMetrics = new HashSet<Metric>(new Metric.MetricEqualityComparer())
    {
        new Metric
        {
            Name = "Metric int",
            Type = MetricType.Int,
            Max = $"{int.MinValue}",
            Min = $"{int.MaxValue}",
        },
        new Metric
        { 
            Name = "Metric float", 
            Type = MetricType.Float, 
            Max = $"{float.MaxValue}", 
            Min = $"{float.MinValue}",
        },
        // TODO: Create examples for other types later
    };
    
    private MavlinkRouter _router;
    private DiagnosticsConfig _config;
    private IDiagnosticServer? _server;
    private uint _refreshRate = 30;
    
    /// <summary>
    /// Command creates fake diagnostics data from file and opens a mavlink connection.
    /// </summary>
    /// <param name="connectionString">-cs, Address of the generator</param>
    /// <param name="configFilePath">-cfp, location of the config file for the generator</param>
    /// <returns></returns>
    [Command("generate-diagnostics")]
    public int Run(string? connectionString = null, string? configFilePath = null)
    {
        RunAsync(connectionString, configFilePath).Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }

    private async Task RunAsync(string? connectionString, string? customConfigFilePath = null)
    {
        var cfgJson = new JsonOneFileConfiguration(
            customConfigFilePath ?? DefaultConfigFileFullPath,
            true,
            null
        );

        if (customConfigFilePath is null)
        {
            var json = JsonConvert.SerializeObject(_exampleMetrics, Formatting.Indented);
            await File.WriteAllTextAsync(DefaultConfigFileFullPath, json);
        }

        var cfg = cfgJson.Get<DiagnosticsConfig>();

        if (connectionString is not null)
        {
            _server = SetUpConnection(connectionString);
        }

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
                        await RenderRows(table, cfg.Metrics);
                        ctx.Refresh();
                        await Task.Delay(TimeSpan.FromMilliseconds(_refreshRate));
                    } 
                });
        }
        finally
        {
            AnsiConsole.Markup("\nDone");
            _router.Dispose();
        }
    }
    
    private async Task RenderRows(Table table, ISet<Metric> metrics)
    {
        foreach (var metric in metrics)
        {
            switch (metric.Type)
            {
                case MetricType.Int:
                    var valueInt = metric.RandomIntValue();

                    if (_server is not null)
                    {
                        await _server.Send(metric.Name, valueInt);
                    }
                    
                    table.AddRow(
                        $"[red]{metric}[/]",
                        $"{metric.Type.ToString()}",
                        $"[green]{valueInt}[/]"
                    );
                    break;
                case MetricType.Float:
                    var valueFloat = metric.RandomFloatValue();
                    
                    if (_server is not null)
                    {
                        await _server.Send(metric.Name, valueFloat);
                    }

                    table.AddRow(
                        $"[red]{metric}[/]",
                        $"{metric.Type.ToString()}",
                        $"[green]{valueFloat}[/]"
                    );
                    break;
                case MetricType.FloatArray:
                    var valueFloatArray = metric.RandomFloatArrayValue();

                    if (_server is not null)
                    {
                        await _server.Send(metric.Name, (ushort)Random.Shared.Next(0, ushort.MaxValue),
                            valueFloatArray);
                    }

                    table.AddRow(
                        $"[red]{metric}[/]",
                        $"{metric.Type.ToString()}",
                        $"[green]{string.Join("; ", valueFloatArray)}[/]"
                    );
                    break;
                case MetricType.SByteArray:
                    throw new NotImplementedException("SByte array is not supported right now");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private async Task ParseCfgAsync(StreamReader reader, HashSet<Metric> metrics) //TODO: if cfg works with json => delete method
    {
        var metricSb = new StringBuilder();
        while (reader.EndOfStream)
        {
            var b = reader.Read();

            string? name = null;
            MetricType? type = null;
            string? max = null;
            string? min = null;

            switch (b)
            {
                case '_' when name is null:
                    name = metricSb.ToString();
                    metrics.Clear();
                    continue;
                case '_' when type is null:
                    // type = metricSb.ToString();
                    metrics.Clear();
                    continue;
                case '_' when max is null:
                    max = metricSb.ToString();
                    metrics.Clear();
                    continue;
                case '_' when min is null:
                    min = metricSb.ToString();
                    metrics.Clear();
                    continue;
                case ';':
                    if (name is null || type is null || max is null || min is null)
                    {
                        throw new FormatException("Invalid format of the config file");
                    }

                    var metric = new Metric
                    {
                        Name = name,
                        Type = type.Value,
                        Max = max,
                        Min = min
                    };
                    metrics.Add(metric);
                    
                    continue;
                default:
                    metricSb.Append((char)b);
                    continue;
            }
        }
    }

    private string CreateDefaultConfigFile() //TODO: if cfg works with json => delete method
    {
        // var configFilePath = Path.Join(Directory.GetCurrentDirectory(), _config.DefaultConfigFileName);
        //
        // if (!File.Exists(configFilePath))
        // {
        //     File.Create(configFilePath);
        // }
        //
        // return configFilePath;
        throw new NotImplementedException();
    }

    private IDiagnosticServer SetUpConnection(string? connectionString)
    {
        _router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects, publishScheduler: Scheduler.Default);
        _router.WrapToV2ExtensionEnabled = true;
        var portConfig = new MavlinkPortConfig
        {
            ConnectionString = connectionString,
            IsEnabled = true,
            Name = "DiagnosticsServer"
        };
        _router.AddPort(portConfig);
        
        var cfg = new DiagnosticServerConfig();
        var serverId = new MavlinkIdentity(1, 2); // TODO: find out how to define unique id
        
        var server = new DiagnosticServer(
            cfg,
            _router, 
            serverId, 
            new PacketSequenceCalculator(), 
            TimeProvider.System,
            Scheduler.Default
        );

        return server;
    }
}