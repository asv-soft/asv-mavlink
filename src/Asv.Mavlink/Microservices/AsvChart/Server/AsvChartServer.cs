using System;
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

public class AsvChartServerConfig
{
    public int SendSignalDelayMs { get; set; } = 30;
    public int SendCollectionUpdateMs { get; set; } = 500;
}

public class AsvChartServer: MavlinkMicroserviceServer,IAsvChartServer
{
    private readonly AsvChartServerConfig _config;
    private readonly ILogger _logger;
    private readonly ObservableDictionary<ushort,AsvChartInfo> _charts;
    private volatile ushort _chartHash = 0;
    private int _changeHashLock;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;

    public AsvChartServer(MavlinkIdentity identity, AsvChartServerConfig config, IMavlinkContext core)
        :base(AsvChartHelper.MavlinkMicroserviceName, identity, core)
    {
        _logger = Core.LoggerFactory.CreateLogger<AsvChartServer>();
        _config = config;
        _charts = new ObservableDictionary<ushort,AsvChartInfo>();

        _sub1 = InternalFilter<AsvChartInfoRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Subscribe(OnRequestSignalInfo);
        _sub2 = InternalFilter<AsvChartDataRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Subscribe(OnRequestChartData);
        
        _sub3 = _charts.ObserveChanged().Debounce(TimeSpan.FromMilliseconds(_config.SendCollectionUpdateMs))
            .Subscribe(InternalUpdateCollectionHash);
 
    }

    private async void InternalUpdateCollectionHash(CollectionChangedEvent<KeyValuePair<ushort, AsvChartInfo>> changeSet)
    {
        if (Interlocked.Exchange(ref _changeHashLock, 1) == 1) return;
        try
        {
            var hash = new HashCode();
            foreach (var chart in _charts.Select(x=>x.Value))
            {
                hash.Add(chart.InfoHash);
            }
            _chartHash = AsvChartHelper.GetHashCode(hash);
            await InternalSend<AsvChartInfoUpdatedEventPacket>(x =>
            {
                x.Payload.ChatListHash = _chartHash;
                x.Payload.ChartCount = (ushort)_charts.Count;
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error on update collection hash:{e.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _changeHashLock, 0);
        }
    }


    public ObservableDictionary<ushort,AsvChartInfo> Charts => _charts;

    public async Task Send(DateTime time, ReadOnlyMemory<float> data, AsvChartInfo info, CancellationToken cancel = default)
    {
        if (data.Length % info.OneMeasureByteSize != 0)
        {
            throw new ArgumentException("Data length must be multiple of one measure byte size", nameof(data));
        }
        if (data.Length != info.OneFrameMeasureSize)
        {
            throw new ArgumentException("Data length must be equal to one frame measure size", nameof(data));
        }
        var fullPackets = info.OneFrameByteSize / AsvChartDataPayload.DataMaxItemsCount;
        var lastPacketSize = info.OneFrameByteSize % AsvChartDataPayload.DataMaxItemsCount;
        var size = AsvChartDataPayload.DataMaxItemsCount / info.OneMeasureByteSize;
        var timeValue = MavlinkTypesHelper.ToUnixTimeUs(time);
        var packetCount = fullPackets + (lastPacketSize > 0 ? 1 : 0);
        var seq = -1; // first packet increment will be 0
        for (var i = 0; i < fullPackets; i++)
        {
            var copyI = i; // copy to local variable !!!
            await InternalSend<AsvChartDataPacket>(pkt =>
            {
                var span = pkt.Payload.Data.AsSpan();
                var localRange = data.Span.Slice(copyI*size, size);
                for (var j = 0; j < size; j++)
                {
                    AsvChartHelper.WriteSignalMeasure(ref span, info, localRange[j]);
                }
                pkt.Payload.TimeUnixUsec = timeValue;
                pkt.Payload.ChatId = info.Id;
                pkt.Payload.PktSeq = (ushort)Interlocked.Increment(ref seq);
                pkt.Payload.DataSize = (byte)(size * info.OneMeasureByteSize);
                pkt.Payload.PktInFrame = (ushort)packetCount;
                pkt.Payload.ChatInfoHash = info.InfoHash;
            }, cancel).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
                await Task.Delay(_config.SendSignalDelayMs, cancel).ConfigureAwait(false);
        }
        if (lastPacketSize > 0)
        {
            var lastSize = lastPacketSize / info.OneMeasureByteSize;
            await InternalSend<AsvChartDataPacket>(pkt =>
            {
                var span = pkt.Payload.Data.AsSpan();
                var localRange = data.Span.Slice(fullPackets*size, lastSize);
                for (var j = 0; j < lastSize; j++)
                {
                    AsvChartHelper.WriteSignalMeasure(ref span, info, localRange[j]);
                }
                pkt.Payload.TimeUnixUsec = timeValue;
                pkt.Payload.ChatId = info.Id;
                pkt.Payload.PktSeq = (ushort)Interlocked.Increment(ref seq);
                pkt.Payload.DataSize = (byte)(lastSize * info.OneMeasureByteSize);
                pkt.Payload.PktInFrame = (ushort)packetCount;
                pkt.Payload.ChatInfoHash = info.InfoHash;
            }, cancel).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
                await Task.Delay(_config.SendSignalDelayMs, cancel).ConfigureAwait(false);
        }
    }

    public ChartStreamOptionsDelegate? OnDataRequest { get; set; }
    
    private async void OnRequestChartData(AsvChartDataRequestPacket request)
    {
        try
        {
            if (OnDataRequest == null)
            {
                await InternalSend<AsvChartDataResponsePacket>(x => { x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckNotSupported; }).ConfigureAwait(false);
                return;
            }

            if (_charts.TryGetValue(request.Payload.ChatId, out var info) == false)
            {
                await InternalSend<AsvChartDataResponsePacket>(x => { x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckNotFound; }).ConfigureAwait(false);
                return;
            }
            
            var result = await OnDataRequest.Invoke(new AsvChartOptions(request),info, CancellationToken.None).ConfigureAwait(false);
            await InternalSend<AsvChartDataResponsePacket>(x =>
            {
                x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckOk;
                x.Payload.DataRate = result.Rate;
                x.Payload.DataTrigger = result.Trigger;
                x.Payload.ChartId = result.ChartId;
                x.Payload.ChatInfoHash = info.InfoHash;
                
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error on request stream options:{e.Message}");
            await InternalSend<AsvChartDataResponsePacket>(x =>
            {
                x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckFail;
            }).ConfigureAwait(false);    
        }
    }
    
    private async void OnRequestSignalInfo(AsvChartInfoRequestPacket request)
    {
        var requestId = request.Payload.RequestId;
        await InternalSend<AsvChartInfoResponsePacket>(x =>
        {
            x.Payload.RequestId = requestId;
            x.Payload.ItemsCount = (ushort)Math.Max(0, _charts.Count - request.Payload.Skip);
            x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckOk;
            x.Payload.ChatListHash = _chartHash;
        }).ConfigureAwait(false);
        var charts = _charts.Select(x=>x.Value).Skip(request.Payload.Skip).Take(request.Payload.Count);

        var delay = TimeSpan.FromMilliseconds(_config.SendSignalDelayMs);
        var ts = Core.TimeProvider.GetTimestamp();    
        foreach (var chart in charts)
        {
            await InternalSend<AsvChartInfoPacket>(x => chart.Fill(x.Payload)).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
            {
                var tcs = new TaskCompletionSource<bool>();
                await using var timer = Core.TimeProvider.CreateTimer(_ =>
                {
                    tcs.SetResult(true);
                }, null, TimeSpan.Zero, delay);

                await tcs.Task.ConfigureAwait(false);
            }
        }
    }
    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);

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