using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvChart;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class AsvChartClientConfig
{
    public double MaxTimeToWaitForResponseForListMs { get; set; } = 5000;
}

public class AsvChartClient: MavlinkMicroserviceClient, IAsvChartClient
{
    private readonly ILogger _logger;
    private volatile uint _seq;
    private readonly SourceCache<AsvChartInfo,ushort> _signals;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly object _sync = new();
    private ulong _currentFrameId;
    private readonly SortedList<ushort,AsvChartDataPayload> _frameBuffer = new();
    private ushort _lastCollectionHash;
    private readonly RxValue<bool> _isSynced;

    public AsvChartClient(
        AsvChartClientConfig config,
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null): base("CHART", connection, identity, seq,scheduler, logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _signals = new SourceCache<AsvChartInfo, ushort>(x=>x.Id).DisposeItWith(Disposable);
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        OnChartInfo = InternalFilter<AsvChartInfoPacket>().Select(x => new AsvChartInfo(x.Payload));
        OnChartInfo
            .Subscribe(_signals.AddOrUpdate)
            .DisposeItWith(Disposable);
        Charts = _signals.Connect().RefCount();
        InternalFilter<AsvChartDataPacket>().Subscribe(InternalOnDataRecv).DisposeItWith(Disposable);
        OnStreamOptions = InternalFilter<AsvChartDataResponsePacket>().Select(x => new AsvChartOptions(x));
        OnUpdateEvent = InternalFilter<AsvChartInfoUpdatedEventPacket>().Select(x => x.Payload);
        _isSynced = new RxValue<bool>(false).DisposeItWith(Disposable);
        OnUpdateEvent.Select(x=>x.ChatListHash == _lastCollectionHash).Subscribe(_isSynced).DisposeItWith(Disposable);
    }
    public async Task<bool> ReadAllInfo(IProgress<double> progress = null, CancellationToken cancel = default)
    {
        var lastUpdate = DateTime.Now;
        _signals.Clear();
        using var request = OnChartInfo
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestId =  (byte)(Interlocked.Increment(ref _seq) % 255);
        var requestAck = await InternalCall<AsvChartInfoResponsePayload, AsvChartInfoRequestPacket, AsvChartInfoResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.TargetSystem = Identity.TargetSystemId;
            x.Payload.RequestId = requestId;
            x.Payload.Skip = 0;
            x.Payload.Count = ushort.MaxValue;
        }, x=>x.Payload.RequestId == requestId, x=>x.Payload, cancel: cancel).ConfigureAwait(false);
        if (requestAck.Result == AsvChartRequestAck.AsvChartRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvChartRequestAck.AsvChartRequestAckFail) 
            throw new Exception("Request fail");
        
        while (DateTime.Now - lastUpdate < _maxTimeToWaitForResponseForList && _signals.Count < requestAck.ItemsCount)
        {
            await Task.Delay(_maxTimeToWaitForResponseForList/10, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_signals.Count);
        }
        var result = _signals.Count == requestAck.ItemsCount;
        _lastCollectionHash = requestAck.ChatListHash;
        _isSynced.OnNext(result);
        return result;
    }

    public Task<AsvChartOptions> RequestStream(AsvChartOptions options, CancellationToken cancel = default)
    {
        var info = _signals.Lookup(options.ChartId);
        if (info.HasValue == false) throw new Exception($"Chart with id {options.ChartId} not found");
        return InternalCall<AsvChartOptions,AsvChartDataRequestPacket,AsvChartDataResponsePacket>(x=>
        {
            x.Payload.TargetSystem = Identity.TargetSystemId;
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.ChatId = options.ChartId;
            x.Payload.DataTrigger = options.Trigger;
            x.Payload.DataRate = options.Rate;
            x.Payload.ChatInfoHash = info.Value.InfoHash;
        }, x=>x.Payload.ChatId == options.ChartId, x=>new AsvChartOptions(x), cancel: cancel);
    }

    public IObservable<IChangeSet<AsvChartInfo, ushort>> Charts { get; }
    public IObservable<AsvChartInfo> OnChartInfo { get; }
    public OnDataReceivedDelegate OnDataReceived { get; set; }
    public IObservable<AsvChartOptions> OnStreamOptions { get; }
    public IObservable<AsvChartInfoUpdatedEventPayload> OnUpdateEvent { get; }
    public IRxValue<bool> IsSynced => _isSynced;

    private void InternalOnDataRecv(AsvChartDataPacket data)
    {
        if (data.Payload.PktInFrame == 0)
        {
            _logger.LogWarning("Recv strange packet with PktInFrame = 0");
            return;
        }
        
        var signalInfo = _signals.Lookup(data.Payload.ChatId);
        if (signalInfo.HasValue == false) return;
        
        if (signalInfo.Value.InfoHash != data.Payload.ChatInfoHash)
        {
            _logger.ZLogWarning($"Recv data for chart {data.Payload.ChatId} with different hash {data.Payload.ChatInfoHash} != {signalInfo.Value.InfoHash}");
            return;
        }
        
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
                        frameSpan[i] = AsvChartTypeHelper.ReadSignalMeasure(ref readSpan, info);    
                    }
                    OnDataReceived?.Invoke(dateTime, new ReadOnlyMemory<float>(frameData,0,info.OneFrameMeasureSize), info);
                }
                catch (Exception e)
                {
                    _logger.ZLogError(e, $"Error on read data:{e.Message}");
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
                        frameSpan[i] = AsvChartTypeHelper.ReadSignalMeasure(ref readSpan, info);    
                    }
                    OnDataReceived?.Invoke(dateTime, new ReadOnlyMemory<float>(frameFloatData,0,info.OneFrameMeasureSize), info);
                }
                catch (Exception e)
                {
                    _logger.ZLogError(e, $"Error on read data:{e.Message}");
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(frameData);
                    ArrayPool<float>.Shared.Return(frameFloatData);
                }
            }
        }
    }
}