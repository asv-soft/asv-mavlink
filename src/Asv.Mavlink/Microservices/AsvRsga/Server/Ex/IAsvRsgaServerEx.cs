using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using ObservableCollections;
using R3;


namespace Asv.Mavlink;

public delegate Task<MavResult> RsgaSetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default);
public delegate Task<MavResult> RsgaStartRecord(string name, CancellationToken cancel = default);
public delegate Task<MavResult> RsgaStopRecord(CancellationToken cancel = default);
public delegate IEnumerable<AsvRsgaCustomMode> RsgaGetCompatibility();

public interface IAsvRsgaServerEx: IMavlinkMicroserviceServer
{
    IAsvRsgaServer Base { get; }
    RsgaSetMode? SetMode { get; set; }
    RsgaStartRecord? StartRecord { get; set; }
    RsgaStopRecord? StopRecord { get; set; }
    RsgaGetCompatibility? GetCompatibility { get; set; }
    ValueTask SendMeasure(MavlinkV2Message message, CancellationToken cancel);
    ValueTask SendChart(
        ReadOnlyMemory<double> values,
        RsgaChartSendOptions? options = null,
        CancellationToken cancel = default
    );
}
