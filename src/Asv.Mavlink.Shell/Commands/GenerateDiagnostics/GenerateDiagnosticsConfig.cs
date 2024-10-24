using System.Collections.Generic;

namespace Asv.Mavlink.Shell;

public class GenerateDiagnosticsConfig
{
    public static readonly GenerateDiagnosticsConfig Default = new()
    {
        Metrics = {
            new Metric
            {
                Name = "Metric int",
                Type = MetricType.Int,
                Max = $"{1000}",
                Min = $"{-1000}",
            },
            new Metric
            { 
                Name = "Metric float", 
                Type = MetricType.Float, 
                Max = $"{4000.0}", 
                Min = $"{-4000.0}",
            },
            // TODO: Create examples for other types later
        },
        ServerMaxSendIntervalMs = 100,
        SystemId = 1,
        ComponentId = 2,
    };
    
    public int ServerMaxSendIntervalMs { get; set; }
    public ISet<Metric> Metrics { get; set; } = new HashSet<Metric>(Metric.MetricEqualityComparer.Instance.Value);
    public byte SystemId { get; set; }
    public byte ComponentId { get; set; }
}