using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvChart;
using DynamicData;
using R3;

namespace Asv.Mavlink;

public delegate void OnDataReceivedDelegate(DateTime time, ReadOnlyMemory<float> data, AsvChartInfo info);

public interface IAsvChartClient:IMavlinkMicroserviceClient
{
    Task<bool> ReadAllInfo(IProgress<double> progress = null, CancellationToken cancel = default);
    Task<AsvChartOptions> RequestStream(AsvChartOptions options, CancellationToken cancel = default);
    IObservable<IChangeSet<AsvChartInfo,ushort>> Charts { get; }
    IObservable<AsvChartInfo> OnChartInfo { get; }
    OnDataReceivedDelegate OnDataReceived { get; set; }
    IObservable<AsvChartOptions> OnStreamOptions { get; }
    IObservable<AsvChartInfoUpdatedEventPayload> OnUpdateEvent { get; }
    ReadOnlyReactiveProperty<bool> IsSynced { get; }
    
    Task<AsvChartOptions> RequestStream(ushort signalId, AsvChartDataTrigger trigger, float rateMs, CancellationToken cancel = default)
    {
        return RequestStream(new AsvChartOptions(signalId,trigger, rateMs), cancel);
    }
}