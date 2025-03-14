using System;
using System.Buffers;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public class MavlinkV1Parser:ProtocolParser<MavlinkMessage,int>
{
    private State _state;
    private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(MavlinkV2Protocol.PacketV2MaxSize + MavlinkV2Protocol.SignatureByteSize);
    private int _bufferIndex;
    private int _bufferStopIndex;

    public MavlinkV1Parser(
        IProtocolMessageFactory<MavlinkMessage, int> messageFactory, IProtocolContext context, IStatisticHandler? statisticHandler) 
        : base(messageFactory, context, statisticHandler)
    {
        
    }

    enum State
    {
        Sync,
        Length,
        FillBuffer
    }
    
    public override bool Push(byte data)
    {
        switch (_state)
        {
            case State.Sync:
                if (data == MavlinkV1Protocol.MagicMarker)
                {
                    _buffer[0] = data;
                    _state = State.Length;
                }
                return false;
            case State.Length:
                _buffer[1] = data;
                _bufferIndex = 2;
                _bufferStopIndex = MavlinkV1Protocol.PayloadStartIndex + /*payload length*/data + 2 /*crc*/ + 1; 
                _state = State.FillBuffer;
                return false;
            case State.FillBuffer:
                _buffer[_bufferIndex] = data;
                ++_bufferIndex;
                if (_bufferIndex >= _bufferStopIndex)
                {
                    _state = State.Sync;
                    var span = new ReadOnlySpan<byte>(_buffer, 0, _bufferIndex);
                    var messageId = MavlinkV1Protocol.GetMessageId(span);
                    InternalParsePacket((ushort)messageId, ref span,true);
                    return true;
                }
                return false;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Reset()
    {
        _state = State.Sync;
    }

    public override ProtocolInfo Info => MavlinkV1Protocol.Info;
    
    protected override void Dispose(bool disposing)
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        base.Dispose(disposing);
    }
    
    protected override async ValueTask DisposeAsyncCore()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }
}

