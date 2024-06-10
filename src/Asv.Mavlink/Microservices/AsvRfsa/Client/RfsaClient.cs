using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.AsvRfsa;
using DynamicData;
using NLog;

namespace Asv.Mavlink;

public class RfsaClientConfig
{
    public int MaxTimeToWaitForResponseForListMs { get; set; } = 5000;

}

public class RfsaClient :MavlinkMicroserviceClient, IRfsaClient
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private volatile uint _seq;
    private readonly ISourceCache<SignalInfo, ushort> _signals;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private ushort _lastSignalId = ushort.MaxValue;
    private readonly object _sync = new();
    private ulong _currentFrameId;
    private readonly SortedList<ushort,AsvRfsaSignalDataPayload> _frameBuffer = new();

    public RfsaClient(RfsaClientConfig config, IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq)
        : base("RFSA", connection, identity, seq)
    {
        _signals = new SourceCache<SignalInfo, ushort>(x=>x.Id).DisposeItWith(Disposable);
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        OnSignalInfo
            .Do(x =>
            {
                
                
            })
            .Subscribe(_signals.AddOrUpdate)
            .DisposeItWith(Disposable);
        Signals = _signals.Connect().RefCount();
        InternalFilter<AsvRfsaSignalDataPacket>().Subscribe(InternalOnDataRecv).DisposeItWith(Disposable);
        OnStreamOptions = InternalFilter<AsvRfsaStreamResponsePacket>().Select(x => new StreamOptions(x));
    }
    public IObservable<SignalInfo> OnSignalInfo =>
        InternalFilter<AsvRfsaSignalInfoPacket>().Select(x => new SignalInfo(x.Payload));
    
    public async Task<bool> ReadAllSignalInfo(IProgress<double> progress = null, CancellationToken cancel = default)
    {
        var lastUpdate = DateTime.Now;
        _signals.Clear();
        using var request = OnSignalInfo
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestId =  (byte)(Interlocked.Increment(ref _seq) % 255);
        var requestAck = await InternalCall<AsvRfsaSignalResponsePayload, AsvRfsaSignalRequestPacket, AsvRfsaSignalResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.TargetSystem = Identity.TargetSystemId;
            x.Payload.RequestId = requestId;
            x.Payload.Skip = 0;
            x.Payload.Count = ushort.MaxValue;
        }, x=>x.Payload.RequestId == requestId, x=>x.Payload, cancel: cancel).ConfigureAwait(false);
        if (requestAck.Result == AsvRfsaRequestAck.AsvRfsaRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvRfsaRequestAck.AsvRfsaRequestAckFail) 
            throw new Exception("Request fail");

        while (DateTime.Now - lastUpdate < _maxTimeToWaitForResponseForList && _signals.Count < requestAck.ItemsCount)
        {
            await Task.Delay(_maxTimeToWaitForResponseForList/10, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_signals.Count);
        }
        return _signals.Count == requestAck.ItemsCount;
    }
    

    private void InternalOnDataRecv(AsvRfsaSignalDataPacket data)
    {
        if (data.Payload.PktInFrame == 0)
        {
            Logger.Warn("Recv strange packet with PktInFrame = 0");
            return;
        }
        
        var signalInfo = _signals.Lookup(data.Payload.SignalId);
        if (signalInfo.HasValue == false) return;
        var info = signalInfo.Value;
        var dateTime = MavlinkTypesHelper.FromUnixTimeUs(data.Payload.TimeUnixUsec);
        var stream = data.Payload;
        lock (_sync)
        {
            if (stream.PktInFrame == 1)
            {
                var frameData = ArrayPool<float>.Shared.Rent(info.OneFrameMeasureSize);
                try
                {
                    var frameSpan = frameData.AsSpan(0,info.OneFrameMeasureSize);
                    var readSpan = new ReadOnlySpan<byte>(data.Payload.Data);
                    for (var i = 0; i < info.OneFrameMeasureSize; i++)
                    {
                        frameSpan[i] = RfsaHelper.ReadSignalMeasure(ref readSpan, info);    
                    }
                    OnDataReceived?.Invoke(dateTime, new ReadOnlyMemory<float>(frameData,0,info.OneFrameMeasureSize), info);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                finally
                {
                    ArrayPool<float>.Shared.Return(frameData);
                }
            }
            else
            {
                if (_currentFrameId != stream.TimeUnixUsec)
                {
                    _frameBuffer.Clear();
                    _currentFrameId = stream.TimeUnixUsec;
                }
                if (!_frameBuffer.TryAdd(stream.PktSeq, stream)) return;
                if (_frameBuffer.Count < stream.PktInFrame) return;
                var frameSize = _frameBuffer.Sum(x => x.Value.DataSize);
                var frameData = ArrayPool<byte>.Shared.Rent(frameSize);
                var frameFloatData = ArrayPool<float>.Shared.Rent(info.OneFrameMeasureSize);
                try
                {
                    var index = 0;
                    foreach (var payload in _frameBuffer)
                    {
                        Array.Copy(payload.Value.Data, 0, frameData, index, payload.Value.DataSize);
                        index += payload.Value.DataSize;
                    }
                    _frameBuffer.Clear();
                    var frameSpan = frameFloatData.AsSpan(0,info.OneFrameMeasureSize);
                    var readSpan = new ReadOnlySpan<byte>(frameData,0,frameSize);
                    for (var i = 0; i < info.OneFrameMeasureSize; i++)
                    {
                        frameSpan[i] = RfsaHelper.ReadSignalMeasure(ref readSpan, info);    
                    }
                    OnDataReceived?.Invoke(dateTime, new ReadOnlyMemory<float>(frameFloatData,0,info.OneFrameMeasureSize), info);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(frameData);
                    ArrayPool<float>.Shared.Return(frameFloatData);
                }
            }
        }
    }
    public IObservable<IChangeSet<SignalInfo, ushort>> Signals { get; }
    public OnDataReceivedDelegate OnDataReceived { get; set; }
    public IObservable<StreamOptions> OnStreamOptions { get; }
    public Task<StreamOptions> RequestStream(StreamOptions options, CancellationToken cancel = default)
    {
        return InternalCall<StreamOptions,AsvRfsaStreamRequestPacket,AsvRfsaStreamResponsePacket>(x=>
        {
            x.Payload.TargetSystem = Identity.TargetSystemId;
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.SignalId = options.SignalId;
            x.Payload.Event = options.EventType;
            x.Payload.StreamRate = options.Rate;
        }, x=>x.Payload.SignalId == options.SignalId, x=>new StreamOptions(x), cancel: cancel);
    }
}