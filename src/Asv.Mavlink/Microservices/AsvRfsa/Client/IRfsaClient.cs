using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRfsa;
using DynamicData;

namespace Asv.Mavlink;

public delegate void OnDataReceivedDelegate(DateTime time, ReadOnlyMemory<float> data, SignalInfo info);
public interface IRfsaClient
{
    Task<bool> ReadAllSignalInfo(IProgress<double> progress = null, CancellationToken cancel = default); 
    IObservable<IChangeSet<SignalInfo,ushort>> Signals { get; }
    IObservable<SignalInfo> OnSignalInfo { get; }
    OnDataReceivedDelegate OnDataReceived { get; set; }
    IObservable<StreamOptions> OnStreamOptions { get; }
    Task<StreamOptions> RequestStream(StreamOptions options, CancellationToken cancel = default);
    Task<StreamOptions> RequestStream(ushort signalId, AsvRfsaStreamArg eventType, float rateMs, CancellationToken cancel = default)
    {
        return RequestStream(new StreamOptions(signalId,eventType, rateMs), cancel);
    }
}

public class StreamOptions(ushort signalId, AsvRfsaStreamArg eventType, float rateMs)
{
    internal StreamOptions(AsvRfsaStreamResponsePacket packet):this(packet.Payload.SignalId,packet.Payload.Event,packet.Payload.StreamRate)
    {
    }

    internal StreamOptions(AsvRfsaStreamRequestPacket packet):this(packet.Payload.SignalId,packet.Payload.Event,packet.Payload.StreamRate)
    {
    
    }

    public float Rate { get;  } = rateMs;
    public AsvRfsaStreamArg EventType { get; } = eventType;
    public ushort SignalId { get; } = signalId;
}