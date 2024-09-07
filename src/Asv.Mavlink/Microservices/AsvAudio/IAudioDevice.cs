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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class PacketCounter(byte initialCounter = 0)
{
    /// <summary>
    /// Проверяет, является ли новый счетчик инкрементом предыдущего.
    /// Возвращает количество пропущенных значений, если они есть.
    /// </summary>
    /// <param name="newCounter">Новое значение счетчика пакета.</param>
    /// <returns>Количество пропущенных значений (0, если последовательность не нарушена).</returns>
    public int CheckIncrement(byte newCounter)
    {
        int missedPackets = 0;

        // Проверяем переход через 255
        if (initialCounter == 255 && newCounter == 0)
        {
            missedPackets = 0; // Ничего не пропущено, корректный переход через границу
        }
        // Если новый счетчик больше предыдущего на 1
        else if (newCounter == initialCounter + 1)
        {
            missedPackets = 0; // Последовательность не нарушена
        }
        // Если новый счетчик меньше предыдущего (переполнение через 255) или пропущены пакеты
        else
        {
            // Рассчитываем количество пропущенных пакетов
            if (newCounter > initialCounter)
            {
                missedPackets = newCounter - initialCounter - 1;
            }
            else
            {
                // Учитываем переполнение через 255
                missedPackets = (256 - initialCounter) + (newCounter - 1);
            }
        }

        // Обновляем последний счетчик
        initialCounter = newCounter;

        // Возвращаем количество пропущенных пакетов
        return missedPackets;
    }
}

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
    private readonly ILogger _logger;
    private readonly Func<Action<AsvAudioStreamPacket>, CancellationToken, Task> _sendPacketDelegate;
    private readonly OnRecvAudioDelegate _onRecvAudioDelegate;
    private readonly RxValue<string> _name;
    private long _lastHit;
    private readonly Subject<Unit> _onLinePing;
    private readonly IAudioDecoder _decoder;
    private readonly IAudioEncoder _encoder;
    private uint _frameCounter = 0;
    private readonly SortedDictionary<byte, AsvAudioStreamPayload> _frameBuffer = new();
    
    private readonly object _sync = new();
    private readonly Subject<ReadOnlyMemory<byte>> _inputEncoderAudioStream;
    private readonly Subject<ReadOnlyMemory<byte>> _inputDecoderAudioStream;
    private PacketCounter? _counter;
    private int _lastFrameSeq;

    public AudioDevice(IAudioCodecFactory factory, 
        AsvAudioCodec outputCodecInfo, 
        AsvAudioOnlinePacket packet, 
        Func<Action<AsvAudioStreamPacket>, 
            CancellationToken, Task> sendPacketDelegate,
        OnRecvAudioDelegate onRecvAudioDelegate,
        ILogger? logger = null)
    {
        _logger = logger ?? NullLogger.Instance;
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
        if (IsDisposed) return;
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
            _logger.ZLogError(e,$"Error to send audio packet:{e.Message}");
        }
        
    }
    
    public void SendAudio(ReadOnlyMemory<byte> pcmRawAudioData)
    {
        if (IsDisposed) return;
        lock (_sync)
        {
            _inputEncoderAudioStream.OnNext(pcmRawAudioData);
        }
    }

    public AsvAudioCodec RxCodec { get; }

    private void Touch()
    {
        if (IsDisposed) return;
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
        if (IsDisposed) return;
        if (stream.PktInFrame == 0)
        {
            _logger.LogWarning("Recv strange packet with PktInFrame = 0");
            return;
        }

        if (_lastFrameSeq == stream.FrameSeq)
        {
            // skip same frame
            return;
        }
        lock (_sync)
        {
            if (stream.PktInFrame == 1)
            {
                _counter ??= new PacketCounter((byte)(stream.FrameSeq - 1));
                var missed = _counter.CheckIncrement(stream.FrameSeq);
                try
                {
                    if (missed > 0)
                    {
                        for (var i = 0; i < missed; i++)
                        {
                            _logger.ZLogTrace($"Missed packets {missed}");
                            _inputDecoderAudioStream.OnNext(ReadOnlyMemory<byte>.Empty);        
                        }
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
            }
            else
            {
                if (_lastFrameSeq != stream.FrameSeq)
                {
                    _frameBuffer.Clear();
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
                    _logger.ZLogError(e,$"Error to process audio stream packet:{e.Message}");
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(frameData);
                }
            }
            _lastFrameSeq = stream.FrameSeq;
        }
    }
}