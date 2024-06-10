using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRfsa;
using DynamicData;

namespace Asv.Mavlink;

public delegate void OnDataReceived(DateTime time, ReadOnlyMemory<float> data, SignalInfo info);
public interface IRfsaClient
{
    Task<bool> ReadAllSignalInfo(IProgress<double> progress = null, CancellationToken cancel = default); 
    IObservable<IChangeSet<SignalInfo,ushort>> Signals { get; }
    IObservable<SignalInfo> OnSignalInfo { get; }
    OnDataReceived OnDataReceived { get; set; }
}