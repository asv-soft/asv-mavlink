#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Diagnostic.Server;
using ConsoleAppFramework;

namespace Asv.Mavlink.Shell;

file enum MetricType
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
        public bool Equals(Metric x, Metric y)
        {
            return x.Name == y.Name && x.Type == y.Type;
        }

        public int GetHashCode(Metric obj)
        {
            return obj.Name.GetHashCode() ^ obj.Type.GetHashCode();
        }
    }
}

public class DiagnosticsConfig
{
    public string DefaultConfigFileName { get; set; } = "exampleConfig.txt";
}

public class GenerateDiagnosticsCommand
{
    private MavlinkRouter _router;
    private DiagnosticsConfig _config;
    
    /// <summary>
    /// Command creates fake diagnostics data from file and opens a mavlink connection.
    /// </summary>
    /// <param name="connectionString">-cs, Address of the generator</param>
    /// <param name="configFilePath">-cfp, location of the config file for the generator</param>
    /// <returns></returns>
    [Command("generate-diagnostics")]
    public int Run(string? connectionString, string? configFilePath = null)
    {
        try
        {
            RunAsync(connectionString, configFilePath).Wait();
            ConsoleAppHelper.WaitCancelPressOrProcessExit();
            return 0;
        }
        catch (Exception)
        {
            return 1;
        }
    }

    private async Task RunAsync(string? connectionString, string? customConfigFilePath = null)
    {
        var metrics = new HashSet<Metric>(new Metric.MetricEqualityComparer());

        var cfgFilePath = customConfigFilePath ?? CreateDefaultConfigFile();

        if (!File.Exists(cfgFilePath))
        {
            throw new FileNotFoundException("Could not find config file", cfgFilePath);
        }
        
        using var reader = new StreamReader(cfgFilePath);
        await ParseCfgAsync(reader, metrics);
        
        if (connectionString is not null)
        {
            var server = SetUpConnection(connectionString);

            var runForever = true;
            Console.CancelKeyPress += (_, _) =>
            {
                runForever = false;
            };

            ushort arrayId = 0;
            while (runForever)
            {
                foreach (var metric in metrics)
                {
                    switch (metric.Type)
                    {
                        case MetricType.Int:
                            await server.Send(metric.Name, metric.RandomIntValue());
                            break;
                        case MetricType.Float:
                            await server.Send(metric.Name, metric.RandomFloatValue());
                            break;
                        case MetricType.FloatArray:
                            await server.Send(metric.Name, arrayId++, metric.RandomFloatArrayValue());
                            break;
                        case MetricType.SByteArray:
                            throw new NotImplementedException("SByte array is not supported right now");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }

    private async Task ParseCfgAsync(StreamReader reader, HashSet<Metric> metrics)
    {
        var metricSb = new StringBuilder();
        while (reader.EndOfStream)
        {
            var b = reader.Read();

            if (b == ';')
            {
                
                
                metricSb.Clear();
                continue;
            }
            
        }
    }

    private string CreateDefaultConfigFile()
    {
        var configFilePath = Path.Join(Directory.GetCurrentDirectory(), _config.DefaultConfigFileName);

        if (!File.Exists(configFilePath))
        {
            File.Create(configFilePath);
        }

        return configFilePath;
    }

    private DiagnosticServer SetUpConnection(string? connectionString)
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
        var serverId = new MavlinkIdentity(1, 2); // TODO: Узнать как определить уникальный айдишник
        
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