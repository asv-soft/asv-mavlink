using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvChart;
using Microsoft.Extensions.Logging;
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
    private readonly SourceCache<AsvChartInfo,ushort> _charts;
    private volatile ushort _chartHash = 0;
    private int _changeHashLock;
    private readonly IDisposable _disposeIt;

    public AsvChartServer(MavlinkIdentity identity, AsvChartServerConfig config, ICoreServices core)
        :base("CHART", identity, core)
    {
        _logger = Core.Log.CreateLogger<AsvChartServer>();
        _config = config;
        _charts = new SourceCache<AsvChartInfo, ushort>(x => x.Id);

        var d1 = InternalFilter<AsvChartInfoRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Subscribe(OnRequestSignalInfo);
        var d2 = InternalFilter<AsvChartDataRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Subscribe(OnRequestChartData);
        
        // TODO: must using TimeProvider
        var d3= _charts.Connect().Sample(TimeSpan.FromMilliseconds(_config.SendCollectionUpdateMs))
            .Subscribe(InternalUpdateCollectionHash);

        _disposeIt = Disposable.Combine(_charts,d2,d1,d3); 
    }

    private async void InternalUpdateCollectionHash(IChangeSet<AsvChartInfo, ushort> changeSet)
    {
        if (Interlocked.Exchange(ref _changeHashLock, 1) == 1) return;
        try
        {
            var hash = new HashCode();
            foreach (var chart in _charts.Items)
            {
                hash.Add(chart.InfoHash);
            }
            _chartHash = AsvChartTypeHelper.GetHashCode(hash);
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


    public ISourceCache<AsvChartInfo, ushort> Charts => _charts;

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
                    AsvChartTypeHelper.WriteSignalMeasure(ref span, info, localRange[j]);
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
                    AsvChartTypeHelper.WriteSignalMeasure(ref span, info, localRange[j]);
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

            var info = _charts.Lookup(request.Payload.ChatId);
            if (info.HasValue == false)
            {
                await InternalSend<AsvChartDataResponsePacket>(x => { x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckNotFound; }).ConfigureAwait(false);
                return;
            }
            
            var result = await OnDataRequest.Invoke(new AsvChartOptions(request),info.Value, CancellationToken.None).ConfigureAwait(false);
            await InternalSend<AsvChartDataResponsePacket>(x =>
            {
                x.Payload.Result = AsvChartRequestAck.AsvChartRequestAckOk;
                x.Payload.DataRate = result.Rate;
                x.Payload.DataTrigger = result.Trigger;
                x.Payload.ChatId = result.ChartId;
                x.Payload.ChatInfoHash = info.Value.InfoHash;
                
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
        var charts = _charts.Items.Skip(request.Payload.Skip).Take(request.Payload.Count);
        foreach (var chart in charts)
        {
            await InternalSend<AsvChartInfoPacket>(x => chart.Fill(x.Payload)).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
                await Task.Delay(_config.SendSignalDelayMs).ConfigureAwait(false);
        }
    }

    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}