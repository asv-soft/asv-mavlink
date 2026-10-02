using System;
using Asv.Common;
using Asv.Mavlink.AsvRsga;
using R3;

namespace Asv.Mavlink;

public class RsgaChartSource : DisposableOnce
{
    private readonly IDisposable _disposeIt;

    public RsgaChartSource(
        AsvRsgaRttChartType chartType,
        Observable<RsgaChartFrame> rawStream,
        RsgaChartFrame initValue
    )
    {
        ChartType = chartType;
        NavId = $"chart_{chartType:G}";
        var builder = default(DisposableBuilder);
        Stream = rawStream
            .Where(chartType, (x, type) => x.ChartType == type)
            .ToReadOnlyReactiveProperty(initValue)
            .AddTo(ref builder);
        _disposeIt = builder.Build();
    }

    public string NavId { get; }
    public AsvRsgaRttChartType ChartType { get; }
    public ReadOnlyReactiveProperty<RsgaChartFrame> Stream { get; }

    protected override void InternalDisposeOnce()
    {
        _disposeIt.Dispose();
    }
}
