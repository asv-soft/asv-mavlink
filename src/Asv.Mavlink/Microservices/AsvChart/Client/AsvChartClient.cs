using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvChart;

using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
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
    private readonly ObservableDictionary<ushort,AsvChartInfo> _charts;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly object _sync = new();
    private ulong _currentFrameId;
    private readonly SortedList<ushort,AsvChartDataPayload> _frameBuffer = new();
    private ushort _lastCollectionHash;
    private readonly ReactiveProperty<bool> _isSynced;
    private readonly Subject<AsvChartInfo> _onChartInfo;
    private readonly Subject<AsvChartOptions> _onStreamOptions;
    private readonly Subject<AsvChartInfoUpdatedEventPayload> _onUpdateEvent;

    public AsvChartClient(MavlinkClientIdentity identity,AsvChartClientConfig config,ICoreServices core)
        : base("CHART", identity,core)
    {
        _logger = core.LoggerFactory.CreateLogger<AsvChartClient>();
        _charts = new ObservableDictionary<ushort,AsvChartInfo>();
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        _onChartInfo = new Subject<AsvChartInfo>();
        _sub1 = InternalFilter<AsvChartInfoPacket>().Select(x => new AsvChartInfo(x.Payload))
            .Subscribe(_onChartInfo.AsObserver());
        _sub2 = OnChartInfo
            .Subscribe(x=>_charts[x.Id] = x);
        _sub3 = InternalFilter<AsvChartDataPacket>().Subscribe(InternalOnDataRecv);
        _onStreamOptions = new Subject<AsvChartOptions>();
        _sub4 = InternalFilter<AsvChartDataResponsePacket>().Select(x => new AsvChartOptions(x))
            .Subscribe(_onStreamOptions.AsObserver());

        _onUpdateEvent = new Subject<AsvChartInfoUpdatedEventPayload>();
        _sub5 = InternalFilter<AsvChartInfoUpdatedEventPacket>().Select(x => x.Payload)
            .Subscribe(_onUpdateEvent.AsObserver());
        _isSynced = new ReactiveProperty<bool>(false);
        _sub6 = OnUpdateEvent.Select(x=>x.ChatListHash == _lastCollectionHash)
            .Subscribe(_isSynced.AsObserver());
    }
    public async Task<bool> ReadAllInfo(IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        var lastUpdate = Core.TimeProvider.GetTimestamp();
        _charts.Clear();
        using var request = OnChartInfo
            .Do(_=>lastUpdate = Core.TimeProvider.GetTimestamp())
            .Subscribe();
        var requestId =  (byte)(Interlocked.Increment(ref _seq) % 255);
        var requestAck = await InternalCall<AsvChartInfoResponsePayload, AsvChartInfoRequestPacket, AsvChartInfoResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.Target.ComponentId;
            x.Payload.TargetSystem = Identity.Target.SystemId;
            x.Payload.RequestId = requestId;
            x.Payload.Skip = 0;
            x.Payload.Count = ushort.MaxValue;
        }, x=>x.Payload.RequestId == requestId, x=>x.Payload, cancel: cancel).ConfigureAwait(false);
        if (requestAck.Result == AsvChartRequestAck.AsvChartRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvChartRequestAck.AsvChartRequestAckFail) 
            throw new Exception("Request fail");
        
        while (Core.TimeProvider.GetElapsedTime(lastUpdate) < _maxTimeToWaitForResponseForList && _charts.Count < requestAck.ItemsCount)
        {
            await Task.Delay(_maxTimeToWaitForResponseForList/10, Core.TimeProvider, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_charts.Count);
        }
        var result = _charts.Count == requestAck.ItemsCount;
        _lastCollectionHash = requestAck.ChatListHash;
        _isSynced.OnNext(result);
        return result;
    }

    public Task<AsvChartOptions> RequestStream(AsvChartOptions options, CancellationToken cancel = default)
    {
        if (_charts.TryGetValue(options.ChartId, out var info) == false) throw new Exception($"Chart with id {options.ChartId} not found");
        return InternalCall<AsvChartOptions,AsvChartDataRequestPacket,AsvChartDataResponsePacket>(x=>
        {
            x.Payload.TargetSystem = Identity.Target.SystemId;
            x.Payload.TargetComponent = Identity.Target.ComponentId;
            x.Payload.ChatId = options.ChartId;
            x.Payload.DataTrigger = options.Trigger;
            x.Payload.DataRate = options.Rate;
            x.Payload.ChatInfoHash = info.InfoHash;
        }, x=>x.Payload.ChartId == options.ChartId, x=>new AsvChartOptions(x), cancel: cancel);
    }

    public IReadOnlyObservableDictionary<ushort, AsvChartInfo> Charts => _charts;

    public Observable<AsvChartInfo> OnChartInfo => _onChartInfo;

    public OnDataReceivedDelegate? OnDataReceived { get; set; }

    public Observable<AsvChartOptions> OnStreamOptions => _onStreamOptions;

    public Observable<AsvChartInfoUpdatedEventPayload> OnUpdateEvent => _onUpdateEvent;

    public ReadOnlyReactiveProperty<bool> IsSynced => _isSynced;

    private void InternalOnDataRecv(AsvChartDataPacket data)
    {
        if (data.Payload.PktInFrame == 0)
        {
            _logger.LogWarning("Recv strange packet with PktInFrame = 0");
            return;
        }
        
        
        if (_charts.TryGetValue(data.Payload.ChatId, out var signalInfo) == false) return;
        
        if (signalInfo.InfoHash != data.Payload.ChatInfoHash)
        {
            _logger.ZLogWarning($"Recv data for chart {data.Payload.ChatId} with different hash {data.Payload.ChatInfoHash} != {signalInfo.InfoHash}");
            return;
        }
        
        var info = signalInfo;
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

    #region Dispose

    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;
    private readonly IDisposable _sub5;
    private readonly IDisposable _sub6;
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            OnDataReceived = null;
            _isSynced.Dispose();
            _onChartInfo.Dispose();
            _onStreamOptions.Dispose();
            _onUpdateEvent.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
            _sub4.Dispose();
            _sub5.Dispose();
            _sub6.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        OnDataReceived = null;
        await CastAndDispose(_isSynced).ConfigureAwait(false);
        await CastAndDispose(_onChartInfo).ConfigureAwait(false);
        await CastAndDispose(_onStreamOptions).ConfigureAwait(false);
        await CastAndDispose(_onUpdateEvent).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);
        await CastAndDispose(_sub4).ConfigureAwait(false);
        await CastAndDispose(_sub5).ConfigureAwait(false);
        await CastAndDispose(_sub6).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}