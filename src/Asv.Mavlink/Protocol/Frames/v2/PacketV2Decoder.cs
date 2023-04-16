using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;
using Asv.Common;
using NLog;

namespace Asv.Mavlink;

public class PacketV2Decoder : DisposableOnceWithCancel, IPacketDecoder<IPacketV2<IPayload>>
{
    private readonly byte[] _buffer = new byte[PacketV2Helper.PacketV2MaxSize];
    private DecodeStep _decodeStep;
    private int _bufferIndex;
    private int _bufferStopIndex;
    private readonly Dictionary<int, Func<IPacketV2<IPayload>>> _dict = new();
    private readonly Subject<DeserializePackageException> _decodeErrorSubject;
    private readonly Subject<IPacketV2<IPayload>> _packetSubject;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly object _sync = new object();

    private enum DecodeStep
    {
        Sync,
        Length,
        FillBuffer,
        FillSignature
    }

    public PacketV2Decoder()
    {
        // the first byte is always STX
        _buffer[0] = PacketV2Helper.MagicMarkerV2;
        _decodeErrorSubject = new Subject<DeserializePackageException>().DisposeItWith(Disposable);
        _packetSubject = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);
    }

    public void OnData(byte[] buffer)
    {
        lock (_sync)
        {
            foreach (var value in buffer)
            {
                try
                {
                    _decodeStep = _decodeStep switch
                    {
                        DecodeStep.Sync => SyncStep(value),
                        DecodeStep.Length => GetLengthStep(value),
                        DecodeStep.FillBuffer => FillBufferStep(value),
                        DecodeStep.FillSignature => SignatureStep(value),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                catch (Exception e)
                {
                    _logger.Fatal($"Fatal error to decode packet:{e.Message}");
                    _decodeStep = DecodeStep.Sync;
                    Debug.Assert(false, e.Message);
                }
            }
        }
    }

    private DecodeStep SignatureStep(byte value)
    {
        _buffer[_bufferIndex] = value;
        if (_bufferIndex <= (_bufferStopIndex + PacketV2Helper.SignatureByteSize))
        {
            TryDecodePacket(_bufferIndex);
            return DecodeStep.Sync;
        }
        return DecodeStep.FillSignature;
    }

    private DecodeStep SyncStep(byte value)
    {
        // if found STX => next step is geting length
        return value != PacketV2Helper.MagicMarkerV2 ? DecodeStep.Sync : DecodeStep.Length;
    }

    private DecodeStep GetLengthStep(byte value)
    {
        // for (var i = 1; i < _buffer.Length; i++)
        // {
        //     _buffer[i] = 0;
        // }
        _buffer[1] = value;
            
        _bufferStopIndex = PacketV2Helper.PaylodStartIndexInFrame + /*payload length*/value + 2 /*crc*/;
        _bufferIndex = 2;
        return DecodeStep.FillBuffer;
    }

    private DecodeStep FillBufferStep(byte value)
    {
        _buffer[_bufferIndex] = value;
        ++_bufferIndex;
        if (_bufferIndex < _bufferStopIndex) return DecodeStep.FillBuffer;
            
        // buffer with frames,payload and checksumm filled => check if signature present
        // https://mavlink.io/en/guide/message_signing.html
        if (PacketV2Helper.CheckSignaturePresent(_buffer,0))
        {
            return DecodeStep.FillSignature;
        }
        // packet without sync
        TryDecodePacket(_bufferIndex);
        return DecodeStep.Sync;
    }

    private IPacketV2<IPayload> CreatePacket(int messageId)
    {
        return _dict.TryGetValue(messageId, out var func) ? func() : null;
    }

    private void TryDecodePacket(int size)
    {
        var messageId = PacketV2Helper.GetMessageId(_buffer,0);
        var packet = CreatePacket(messageId);
        if (packet == null)
        {
            _decodeErrorSubject.OnNext(new MessageIdNotFoundException(messageId));
            return;
        }
        try
        {
            var span = new ReadOnlySpan<byte>(_buffer,0,size);
            packet.Deserialize(ref span);
        }
        catch (Exception exception)
        {
            _decodeErrorSubject.OnNext(new DeserializePackageException(messageId, string.Format(RS.DecoderV2_TryDecodePacket_Error_for_deserialize_mavlink_V2, messageId,exception.Message) , exception) );
            return; 
        }

        try
        {
            _packetSubject.OnNext(packet);
        }
        catch (Exception e)
        {
            _logger.Error( $"Fatal error to publish packet:{e.Message}");
            Debug.Assert(false, e.Message);
        }
            
    }

    public IObservable<DeserializePackageException> OutError => _decodeErrorSubject;

    public void Register(Func<IPacketV2<IPayload>> factory)
    {
        var pkt = factory();
        _dict.Add(pkt.MessageId,factory);
    }

    public IPacketV2<IPayload> Create(int id)
    {
        return _dict.TryGetValue(id, out var factory) == false ? null : factory();
    }

    public IDisposable Subscribe(IObserver<IPacketV2<IPayload>> observer)
    {
        return _packetSubject.IgnoreObserverExceptions().Subscribe(observer);
    }



       
}