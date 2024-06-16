using System;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;

namespace Asv.Mavlink;

public delegate Task<AsvChartOptions> ChartStreamOptionsDelegate(AsvChartOptions options, AsvChartInfo info, CancellationToken cancel = default);

public interface IAsvChartServer
{
    ISourceCache<AsvChartInfo, ushort> Charts { get; }
    Task Send(DateTime time, ReadOnlyMemory<float> data, AsvChartInfo info, CancellationToken cancel = default);
    ChartStreamOptionsDelegate? OnDataRequest { get; set; }
}