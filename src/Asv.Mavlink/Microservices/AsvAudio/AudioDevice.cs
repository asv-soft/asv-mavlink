using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;


namespace Asv.Mavlink;

public class AudioDevice : IAudioDevice, IDisposable,IAsyncDisposable
{
    private readonly ILogger _logger;
    private readonly Func<Action<AsvAudioStreamPacket>, CancellationToken, Task> _sendPacketDelegate;
    private readonly ICoreServices _core;
    private long _lastHit;
    private uint _frameCounter;
    private readonly object _sync = new();
    private PacketCounter? _counter;
    private int _lastFrameSeq;
    private int _lastPacketSync;
    private readonly IAudioEncoder _encoder;
    private readonly IAudioDecoder _decoder;
    private readonly ReactiveProperty<string> _name;
    private readonly CancellationTokenSource _disposeCancel;
    private readonly Subject<Unit> _onLinePing;
    private readonly Subject<ReadOnlyMemory<byte>> _inputEncoderAudioStream;
    private readonly Subject<ReadOnlyMemory<byte>> _inputDecoderAudioStream;
    private readonly SortedDictionary<byte, AsvAudioStreamPayload> _frameBuffer = new();

    public AudioDevice(IAudioCodecFactory factory, 
        AsvAudioCodec outputCodecInfo, 
        AsvAudioOnlinePacket packet, 
        Func<Action<AsvAudioStreamPacket>, 
            CancellationToken, Task> sendPacketDelegate,
        OnRecvAudioDelegate onRecvAudioDelegate,
        ICoreServices core)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(sendPacketDelegate);
        ArgumentNullException.ThrowIfNull(onRecvAudioDelegate);
        ArgumentNullException.ThrowIfNull(core);
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(packet);
        _disposeCancel = new CancellationTokenSource();
        _logger = core.Log.CreateLogger<AudioDevice>();
        _inputEncoderAudioStream = new Subject<ReadOnlyMemory<byte>>();
        _inputDecoderAudioStream = new Subject<ReadOnlyMemory<byte>>();
        _sendPacketDelegate = sendPacketDelegate ?? throw new ArgumentNullException(nameof(sendPacketDelegate));
        _core = core;
        FullId = packet.FullId;
        _name = new ReactiveProperty<string>(MavlinkTypesHelper.GetString(packet.Payload.Name));
        _onLinePing = new Subject<Unit>();
        _encoder = factory.CreateEncoder(outputCodecInfo,_inputEncoderAudioStream);
        _sub1 = _encoder.Output.Subscribe(InternalSendEncodedAudio);
        _decoder = factory.CreateDecoder(packet.Payload.Codec,_inputDecoderAudioStream);
        _sub2 = _decoder.Output.Subscribe(x=>onRecvAudioDelegate(this,x));
        RxCodec = packet.Payload.Codec;
        Touch();
    }
  
    public MavlinkIdentity FullId { get; }
    public Observable<Unit> OnLinePing => _onLinePing;
    public ReadOnlyReactiveProperty<string> Name => _name;
    
    private async void InternalSendEncodedAudio(ReadOnlyMemory<byte> encodedData)
    {
        if (_isDisposed) return;
        var frameIndex = (byte)(Interlocked.Increment(ref _frameCounter) % 255);
        try
        {
            var fullPackets = encodedData.Length / AsvAudioHelper.MaxPacketStreamData;
            var lastPacketSize = encodedData.Length % AsvAudioHelper.MaxPacketStreamData;
            byte packetIndex = 0;
            var packetsInFrame = fullPackets + (lastPacketSize > 0 ? 1 : 0);
            if (packetsInFrame == 0) return; // no data to send
            if (packetsInFrame > byte.MaxValue) throw new Exception($"Too many packets in frame. Expected: less then {byte.MaxValue}, actual: {packetsInFrame}");
            for (var i = 0; i < fullPackets; i++)
            {
                var index = packetIndex;
                await _sendPacketDelegate(p =>
                {
                    p.Payload.FrameSeq = frameIndex;
                    p.Payload.TargetSystem = FullId.SystemId;
                    p.Payload.TargetComponent = FullId.ComponentId;
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
                    p.Payload.TargetSystem = FullId.SystemId;
                    p.Payload.TargetComponent = FullId.ComponentId;
                    p.Payload.PktInFrame = (byte)packetsInFrame;
                    p.Payload.PktSeq = packetIndex;
                    p.Payload.DataSize = (byte)lastPacketSize;
                    encodedData.Slice(packetIndex * AsvAudioHelper.MaxPacketStreamData,lastPacketSize).CopyTo(p.Payload.Data);
                }, DisposeCancel).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,$"Error to send audio packet:{e.Message}");
        }
        
    }

    private CancellationToken DisposeCancel => _disposeCancel.Token;

    public void SendAudio(ReadOnlyMemory<byte> pcmRawAudioData)
    {
        if (_isDisposed) return;
        lock (_sync)
        {
            _inputEncoderAudioStream.OnNext(pcmRawAudioData);
        }
    }

    public AsvAudioCodec RxCodec { get; }

    private void Touch()
    {
        if (_isDisposed) return;
        Interlocked.Exchange(ref _lastHit, _core.TimeProvider.GetTimestamp());
        _onLinePing.OnNext(Unit.Default);
    }
    public long GetLastHit()
    {
        return Interlocked.CompareExchange(ref _lastHit, 0, 0);
    }
    internal void Update(AsvAudioOnlinePayload pktPayload)
    {
        Touch();
        _name.OnNext(MavlinkTypesHelper.GetString(pktPayload.Name));
    }

    public void OnInputAudioStream(AsvAudioStreamPayload stream)
    {
        if (_isDisposed) return;
        if (stream.PktInFrame == 0)
        {
            _logger.LogWarning("Recv strange packet with PktInFrame = 0");
            return;
        }

        if (_lastFrameSeq == stream.FrameSeq && _lastPacketSync == stream.PktSeq)
        {
            // skip same frame
            return;
        }
        _lastPacketSync = stream.PktSeq;
        lock (_sync)
        {
            if (stream.PktInFrame == 1)
            {
                _counter ??= new PacketCounter(stream.FrameSeq);
                
                var missed = _counter.CheckIncrement(stream.FrameSeq);
                try
                {
                    if (missed > 0)
                    {
                        _logger.ZLogTrace($"Missed packets {missed}");
                        _inputDecoderAudioStream.OnNext(ReadOnlyMemory<byte>.Empty);
                    }
                    else
                    {
                        _inputDecoderAudioStream.OnNext(new ReadOnlyMemory<byte>(stream.Data,0,stream.DataSize));   
                    }
                    
                }
                catch (Exception e)
                {
                    _logger.ZLogError(e,$"Error to process audio stream packet:{e.Message}");
                }
                _lastFrameSeq = stream.FrameSeq;
            }
            else
            {
                if (_lastFrameSeq != stream.FrameSeq)
                {
                    _frameBuffer.Clear();
                }
                _lastFrameSeq = stream.FrameSeq;
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
                    _logger.ZLogError(e,$"Error to process audio stream packet:{e.Message}");
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(frameData);
                }
            }
            
        }
    }

    #region Dispose

    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    
    private bool _isDisposed;

    public void Dispose()
    {
        _isDisposed = true;
        _frameBuffer.Clear();
        _disposeCancel.Cancel(false);
        _disposeCancel.Dispose();
        _name.Dispose();
        _onLinePing.Dispose();
        _inputEncoderAudioStream.Dispose();
        _inputDecoderAudioStream.Dispose();
        _encoder.Dispose();
        _decoder.Dispose();
        _sub1.Dispose();
        _sub2.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        _isDisposed = true;
        _frameBuffer.Clear();
        _disposeCancel.Cancel(false);
        _disposeCancel.Dispose();
        await CastAndDispose(_name).ConfigureAwait(false);
        await CastAndDispose(_onLinePing).ConfigureAwait(false);
        await CastAndDispose(_inputEncoderAudioStream).ConfigureAwait(false);
        await CastAndDispose(_inputDecoderAudioStream).ConfigureAwait(false);
        await _encoder.DisposeAsync().ConfigureAwait(false);
        await CastAndDispose(_decoder).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);

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