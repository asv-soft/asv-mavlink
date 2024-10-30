using System.Collections.Generic;
using Asv.Mavlink.Diagnostic.Server;

namespace Asv.Mavlink.Shell;

public class GenerateDiagnosticsConfig
{
    public static readonly GenerateDiagnosticsConfig Default = new()
    {
        Ports = new []
        {
            new MavlinkPortConfig
            {
                ConnectionString = "tcp://127.0.0.1:7342",
                Name = "to Client",
                IsEnabled = true
            }
        },
        Metrics = {
            new Metric
            {
                Name = "M_int",
                Type = MetricType.Int,
                Max = $"{1000}",
                Min = $"{-1000}",
            },
            new Metric
            { 
                Name = "M_float", 
                Type = MetricType.Float, 
                Max = $"{4000.0}", 
                Min = $"{-4000.0}",
            },
            // TODO: Create examples for other types later
        },
        SystemId = 1,
        ComponentId = 241,
        ServerConfig = new DiagnosticServerConfig()
        {
            IsEnabled = true,
            MaxSendIntervalMs = 100
        },
    };
    
    public required DiagnosticServerConfig ServerConfig { get; set; }
    public required MavlinkPortConfig[] Ports { get; set; }
    public ISet<Metric> Metrics { get; set; } = new HashSet<Metric>(Metric.MetricEqualityComparer.Instance.Value);
    public required byte SystemId { get; set; }
    public required byte ComponentId { get; set; }
}