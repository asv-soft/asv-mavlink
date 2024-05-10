#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using NLog;

namespace Asv.Mavlink;

public interface IAudioDevice
{
    ushort FullId { get; }
    IObservable<Unit> OnLinePing { get; }
    IRxValue<string> Name { get; }
    void SendAudio(ReadOnlyMemory<byte> pcmRawAudioData);
    AsvAudioCodec RxCodec { get; }
}


public class AudioDevice : DisposableOnceWithCancel, IAudioDevice
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly Func<Action<AsvAudioStreamPacket>, CancellationToken, Task> _sendPacketDelegate;
    private readonly OnRecvAudioDelegate _onRecvAudioDelegate;
    private readonly RxValue<string> _name;
    private long _lastHit;
    private readonly Subject<Unit> _onLinePing;
    private readonly IAudioDecoder _decoder;
    private readonly IAudioEncoder _encoder;
    private uint _frameCounter = 0;
    private readonly SortedDictionary<byte, AsvAudioStreamPayload> _frameBuffer = new();
    private int _currentFrameId;
    private readonly object _sync = new();
    private readonly Subject<ReadOnlyMemory<byte>> _inputEncoderAudioStream;
    private readonly Subject<ReadOnlyMemory<byte>> _inputDecoderAudioStream;

    public AudioDevice(IAudioCodecFactory factory, 
        AsvAudioCodec outputCodecInfo, 
        AsvAudioOnlinePacket packet, 
        Func<Action<AsvAudioStreamPacket>, CancellationToken, Task> sendPacketDelegate,
        OnRecvAudioDelegate onRecvAudioDelegate)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));
        if (packet == null) throw new ArgumentNullException(nameof(packet));
        _inputEncoderAudioStream = new Subject<ReadOnlyMemory<byte>>().DisposeItWith(Disposable);
        _inputDecoderAudioStream = new Subject<ReadOnlyMemory<byte>>().DisposeItWith(Disposable);
        
        _sendPacketDelegate = sendPacketDelegate ?? throw new ArgumentNullException(nameof(sendPacketDelegate));
        _onRecvAudioDelegate = onRecvAudioDelegate ?? throw new ArgumentNullException(nameof(onRecvAudioDelegate));
        SystemId = packet.SystemId;
        ComponentId = packet.ComponentId;
        FullId = packet.FullId;
        _name = new RxValue<string>(MavlinkTypesHelper.GetString(packet.Payload.Name)).DisposeItWith(Disposable);
        _onLinePing = new Subject<Unit>().DisposeItWith(Disposable);
        _encoder = factory.CreateEncoder(outputCodecInfo,_inputEncoderAudioStream).DisposeItWith(Disposable);
        _encoder.Subscribe(InternalSendEncodedAudio).DisposeItWith(Disposable);
        _decoder = factory.CreateDecoder(packet.Payload.Codec,_inputDecoderAudioStream).DisposeItWith(Disposable);
        _decoder.Subscribe(x=>_onRecvAudioDelegate(this,x)).DisposeItWith(Disposable);
        
        RxCodec = packet.Payload.Codec;
        Touch();
    }
    public byte SystemId { get; }
    public byte ComponentId { get; }
    public ushort FullId { get; }
    public IObservable<Unit> OnLinePing => _onLinePing;
    public IRxValue<string> Name => _name;
    
    private async void InternalSendEncodedAudio(ReadOnlyMemory<byte> encodedData)
    {
        
        var frameIndex = (byte)(Interlocked.Increment(ref _frameCounter) % 255);
        try
        {
            var fullPackets = encodedData.Length / AsvAudioHelper.MaxPacketStreamData;
            var lastPacketSize = encodedData.Length % AsvAudioHelper.MaxPacketStreamData;
            byte packetIndex = 0;
            var packetsInFrame = (fullPackets + (lastPacketSize > 0 ? 1 : 0));
            if (packetsInFrame == 0) return; // no data to send
            if (packetsInFrame > byte.MaxValue) throw new Exception($"Too many packets in frame. Expected: less then {byte.MaxValue}, actual: {packetsInFrame}");
            for (var i = 0; i < fullPackets; i++)
            {
                var index = packetIndex;
                await _sendPacketDelegate(p =>
                {
                    p.Payload.FrameSeq = frameIndex;
                    p.Payload.TargetSystem = SystemId;
                    p.Payload.TargetComponent = ComponentId;
                    p.Payload.PktInFrame = (byte)packetsInFrame;
                    p.Payload.PktSeq = index;
                    p.Payload.DataSize = AsvAudioHelper.MaxPacketStreamData;
                    encodedData.Slice(index * AsvAudioHelper.MaxPacketStreamData,AsvAudioHelper.MaxPacketStreamData).CopyTo(p.Payload.Data);
                }, DisposeCancel).ConfigureAwait(false);
                packetIndex++;
            }
            if (lastPacketSize > 0)
            {
                await _sendPacketDelegate(p =>
                {
                    p.Payload.FrameSeq = frameIndex;
                    p.Payload.TargetSystem = SystemId;
                    p.Payload.TargetComponent = ComponentId;
                    p.Payload.PktInFrame = (byte)packetsInFrame;
                    p.Payload.PktSeq = packetIndex;
                    p.Payload.DataSize = (byte)lastPacketSize;
                    encodedData.Slice(packetIndex * AsvAudioHelper.MaxPacketStreamData,lastPacketSize).CopyTo(p.Payload.Data);
                }, DisposeCancel).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        
    }
    
    public void SendAudio(ReadOnlyMemory<byte> pcmRawAudioData)
    {
        lock (_sync)
        {
            _inputEncoderAudioStream.OnNext(pcmRawAudioData);
        }
    }

    public AsvAudioCodec RxCodec { get; }

    private void Touch()
    {
        Interlocked.Exchange(ref _lastHit, DateTime.Now.ToBinary());
        _onLinePing.OnNext(Unit.Default);
    }
    public DateTime GetLastHit()
    {
        var lastHit = Interlocked.CompareExchange(ref _lastHit, 0, 0);
        return DateTime.FromBinary(lastHit);
    }
    internal void Update(AsvAudioOnlinePayload pktPayload)
    {
        Touch();
        _name.OnNext(MavlinkTypesHelper.GetString(pktPayload.Name));
    }

    public void OnInputAudioStream(AsvAudioStreamPayload stream)
    {
        if (stream.PktInFrame == 0)
        {
            Logger.Warn("Recv strange packet with PktInFrame = 0");
            return;
        }

        lock (_sync)
        {
            if (stream.PktInFrame == 1)
            {
                try
                {
                    _inputDecoderAudioStream.OnNext(new ReadOnlyMemory<byte>(stream.Data,0,stream.DataSize));
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            else
            {
                if (_currentFrameId != stream.FrameSeq)
                {
                    _frameBuffer.Clear();
                    _currentFrameId = stream.FrameSeq;
                }
                if (!_frameBuffer.TryAdd(stream.PktSeq, stream)) return;
                if (_frameBuffer.Count < stream.PktInFrame) return;
                var frameSize = _frameBuffer.Sum(x => x.Value.DataSize);
                var frameData = ArrayPool<byte>.Shared.Rent(frameSize);
                var index = 0;
                foreach (var payload in _frameBuffer)
                {
                    Array.Copy(payload.Value.Data, 0, frameData, index, payload.Value.DataSize);
                    index += payload.Value.DataSize;
                }
                _frameBuffer.Clear();
                try
                {
                    _inputDecoderAudioStream.OnNext(new ReadOnlyMemory<byte>(frameData,0, frameSize));
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(frameData);
                }
            }
        }
    }
}