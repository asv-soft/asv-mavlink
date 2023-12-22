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

    Task SendAudio(byte[] pcmRawAudioData, int dataSize, CancellationToken cancel);
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

    public AudioDevice(IAudioCodecFactory factory, 
        AudioCodecInfo outputCodecInfo, 
        AsvAudioOnlinePacket packet, 
        Func<Action<AsvAudioStreamPacket>, CancellationToken, Task> sendPacketDelegate,
        OnRecvAudioDelegate onRecvAudioDelegate)
    {
        if (outputCodecInfo == null) throw new ArgumentNullException(nameof(outputCodecInfo));
        if (packet == null) throw new ArgumentNullException(nameof(packet));
        var factory1 = factory ?? throw new ArgumentNullException(nameof(factory));
        _sendPacketDelegate = sendPacketDelegate ?? throw new ArgumentNullException(nameof(sendPacketDelegate));
        _onRecvAudioDelegate = onRecvAudioDelegate ?? throw new ArgumentNullException(nameof(onRecvAudioDelegate));
        SystemId = packet.SystemId;
        ComponentId = packet.ComponentId;
        FullId = packet.FullId;
        _name = new RxValue<string>(MavlinkTypesHelper.GetString(packet.Payload.Name)).DisposeItWith(Disposable);
        _onLinePing = new Subject<Unit>().DisposeItWith(Disposable);
        _encoder = factory1.CreateEncoder(outputCodecInfo).DisposeItWith(Disposable);
        if (factory1.TryFindCodec(packet.Payload.Format, AsvAudioHelper.GetSampleRate(packet.Payload.SampleRate), AsvAudioHelper.GetChannels(packet.Payload.Channels),packet.Payload.Codec,packet.Payload.CodecCfg, out var codecInfo) == false)
        {
            throw new Exception($"Codec {packet.Payload.Codec} not supported");
        }
        _decoder = factory1.CreateDecoder(codecInfo).DisposeItWith(Disposable);
        
        Touch();
    }
    public byte SystemId { get; }
    public byte ComponentId { get; }
    public ushort FullId { get; }
    public IObservable<Unit> OnLinePing => _onLinePing;
    public IRxValue<string> Name => _name;
    
    public async Task SendAudio(byte[] pcmRawAudioData, int dataSize, CancellationToken cancel)
    {
        byte[] encodedData = null!;
        var frameIndex = (byte)(Interlocked.Increment(ref _frameCounter) % 255);
        try
        {
            int encodedSize;
            
            lock (_encoder)
            {
                encodedData = ArrayPool<byte>.Shared.Rent(_encoder.MaxEncodedSize);
                _encoder.Encode(pcmRawAudioData,dataSize,encodedData,_encoder.MaxEncodedSize,out encodedSize);    
            }
            var fullPackets = encodedSize / AsvAudioHelper.MaxPacketStreamData;
            var lastPacketSize = encodedSize % AsvAudioHelper.MaxPacketStreamData;
            byte packetIndex = 0;
            for (var i = 0; i < fullPackets; i++)
            {
                var index = packetIndex;
                await _sendPacketDelegate(p =>
                {
                    p.Payload.FrameSeq = frameIndex;
                    p.Payload.TargetSystem = SystemId;
                    p.Payload.TargetComponent = ComponentId;
                    p.Payload.PktSeq = index;
                    p.Payload.DataSize = AsvAudioHelper.MaxPacketStreamData;
                    Array.Copy(encodedData,index * AsvAudioHelper.MaxPacketStreamData,p.Payload.Data,0,AsvAudioHelper.MaxPacketStreamData);
                }, cancel).ConfigureAwait(false);
                packetIndex++;
            }
            if (lastPacketSize > 0)
            {
                await _sendPacketDelegate(p =>
                {
                    p.Payload.PktSeq = packetIndex;
                    p.Payload.DataSize = (byte)lastPacketSize;
                    Array.Copy(encodedData,packetIndex * AsvAudioHelper.MaxPacketStreamData,p.Payload.Data,0,lastPacketSize);
                }, cancel).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(encodedData);            
        }
        
    }

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
                var outputBuffer = ArrayPool<byte>.Shared.Rent(_decoder.MaxDecodedSize);
                try
                {
                    _decoder.Decode(stream.Data, stream.DataSize, outputBuffer, out var decodedSize);
                    _onRecvAudioDelegate(this,outputBuffer, decodedSize);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(outputBuffer);
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
                var outputBuffer = ArrayPool<byte>.Shared.Rent(_decoder.MaxDecodedSize);
                try
                {
                    _decoder.Decode(frameData, frameSize, outputBuffer, out var decodedSize);
                    _onRecvAudioDelegate(this,outputBuffer, decodedSize);
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