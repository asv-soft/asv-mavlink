using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvRfsa;
using NLog;

namespace Asv.Mavlink;

public class RfsaServerConfig
{
    public int SendSignalDelayMs { get; set; } = 0;
    public int SendSignalInfoDelayMs { get; set; } = 0;
}


public class RfsaServer : MavlinkMicroserviceServer, IRfsaServer
{
    private readonly RfsaServerConfig _config;
    private readonly ImmutableDictionary<ushort,SignalInfo> _signal;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public RfsaServer(RfsaServerConfig config, IEnumerable<SignalInfo> signals, IMavlinkV2Connection connection, MavlinkIdentity identity, IPacketSequenceCalculator seq, IScheduler rxScheduler) 
        : base("RFSA", connection, identity, seq, rxScheduler)
    {
        _config = config;
        _signal = signals.ToImmutableDictionary(x=>x.Id, x=>x);
        InternalFilter<AsvRfsaSignalRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Subscribe(OnRequestSignalInfo)
            .DisposeItWith(Disposable);
        InternalFilter<AsvRfsaStreamRequestPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
            .Subscribe(OnRequestSignalInfo)
            .DisposeItWith(Disposable);
    }

    private async void OnRequestSignalInfo(AsvRfsaStreamRequestPacket request)
    {
        try
        {
            if (OnStreamOptions == null)
            {
                await InternalSend<AsvRfsaStreamResponsePacket>(x => { x.Payload.Result = AsvRfsaRequestAck.AsvRfsaRequestAckNotSupported; }).ConfigureAwait(false);
                return;
            }
            var result = await OnStreamOptions.Invoke(new StreamOptions(request), CancellationToken.None).ConfigureAwait(false);
            await InternalSend<AsvRfsaStreamResponsePacket>(x =>
            {
                x.Payload.Result = AsvRfsaRequestAck.AsvRfsaRequestAckOk;
                x.Payload.StreamRate = result.Rate;
                x.Payload.Event = result.EventType;
                x.Payload.SignalId = result.SignalId;
                
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error on request stream options");
            await InternalSend<AsvRfsaStreamResponsePacket>(x =>
            {
                x.Payload.Result = AsvRfsaRequestAck.AsvRfsaRequestAckFail;
            }).ConfigureAwait(false);    
        }
    }
    
    private async void OnRequestSignalInfo(AsvRfsaSignalRequestPacket request)
    {
        var requestId = request.Payload.RequestId;
        await InternalSend<AsvRfsaSignalResponsePacket>(x =>
        {
            x.Payload.RequestId = requestId;
            x.Payload.ItemsCount = (ushort)Math.Max(0, _signal.Count - request.Payload.Skip);
            x.Payload.Result = AsvRfsaRequestAck.AsvRfsaRequestAckOk;
        }).ConfigureAwait(false);
        var signals = _signal.Skip(request.Payload.Skip).Take(request.Payload.Count);
        foreach (var signal in signals)
        {
            await InternalSend<AsvRfsaSignalInfoPacket>(x => signal.Value.Fill(x.Payload)).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
                await Task.Delay(_config.SendSignalInfoDelayMs).ConfigureAwait(false);
        }
    }

    public IReadOnlyDictionary<ushort, SignalInfo> Signals => _signal;

    public async Task Send(DateTime time, ReadOnlyMemory<float> data, SignalInfo info,CancellationToken cancel = default)
    {
        if (data.Length % info.OneMeasureByteSize != 0)
        {
            throw new ArgumentException("Data length must be multiple of one measure byte size", nameof(data));
        }
        if (data.Length != info.OneFrameMeasureSize)
        {
            throw new ArgumentException("Data length must be equal to one frame measure size", nameof(data));
        }
        var fullPackets = info.OneFrameByteSize / AsvRfsaSignalDataPayload.DataMaxItemsCount;
        var lastPacketSize = info.OneFrameByteSize % AsvRfsaSignalDataPayload.DataMaxItemsCount;
        var size = AsvRfsaSignalDataPayload.DataMaxItemsCount / info.OneMeasureByteSize;
        var timeValue = MavlinkTypesHelper.ToUnixTimeUs(time);
        var packetCount = fullPackets + (lastPacketSize > 0 ? 1 : 0);
        var seq = -1; // first packet increment will be 0
        for (var i = 0; i < fullPackets; i++)
        {
            var copyI = i; // copy to local variable !!!
            await InternalSend<AsvRfsaSignalDataPacket>(pkt =>
            {
                var span = pkt.Payload.Data.AsSpan();
                var localRange = data.Span.Slice(copyI*size, size);
                for (var j = 0; j < size; j++)
                {
                    RfsaHelper.WriteSignalMeasure(ref span, info, localRange[j]);
                }
                pkt.Payload.TimeUnixUsec = timeValue;
                pkt.Payload.SignalId = info.Id;
                pkt.Payload.PktSeq = (ushort)Interlocked.Increment(ref seq);
                pkt.Payload.DataSize = (byte)(size * info.OneMeasureByteSize);
                pkt.Payload.PktInFrame = (ushort)packetCount;
            }, cancel).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
                await Task.Delay(_config.SendSignalDelayMs, cancel).ConfigureAwait(false);
        }
        if (lastPacketSize > 0)
        {
            var lastSize = lastPacketSize / info.OneMeasureByteSize;
            await InternalSend<AsvRfsaSignalDataPacket>(pkt =>
            {
                var span = pkt.Payload.Data.AsSpan();
                var localRange = data.Span.Slice(fullPackets*size, lastSize);
                for (var j = 0; j < lastSize; j++)
                {
                    RfsaHelper.WriteSignalMeasure(ref span, info, localRange[j]);
                }
                pkt.Payload.TimeUnixUsec = timeValue;
                pkt.Payload.SignalId = info.Id;
                pkt.Payload.PktSeq = (ushort)Interlocked.Increment(ref seq);
                pkt.Payload.DataSize = (byte)(lastSize * info.OneMeasureByteSize);
                pkt.Payload.PktInFrame = (ushort)packetCount;
            }, cancel).ConfigureAwait(false);
            if (_config.SendSignalDelayMs > 0)
                await Task.Delay(_config.SendSignalDelayMs, cancel).ConfigureAwait(false);
        }
        
    }

    public OnStreamOptionsDelegate OnStreamOptions { get; set; }
}