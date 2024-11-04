using System;
using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public delegate Task<AsvChartOptions> ChartStreamOptionsDelegate(AsvChartOptions options, AsvChartInfo info, CancellationToken cancel = default);

public interface IAsvChartServer:IMavlinkMicroserviceServer
{
    ObservableDictionary<ushort,AsvChartInfo> Charts { get; }
    Task Send(DateTime time, ReadOnlyMemory<float> data, AsvChartInfo info, CancellationToken cancel = default);
    ChartStreamOptionsDelegate? OnDataRequest { get; set; }
}