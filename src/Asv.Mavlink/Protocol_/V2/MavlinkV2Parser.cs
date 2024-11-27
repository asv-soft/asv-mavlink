using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading.Tasks;
using Asv.IO;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class MavlinkV2Parser:ProtocolParser<MavlinkMessage,ushort>
{
    

    private DecodeStep _decodeStep;
    private readonly ILogger<MavlinkV2Parser> _logger;
    private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(MavlinkV2Protocol.PacketV2MaxSize + MavlinkV2Protocol.SignatureByteSize);
    private int _bufferStopIndex;
    private int _bufferIndex;

    public MavlinkV2Parser(
        IProtocolMessageFactory<MavlinkMessage, ushort> messageFactory, IProtocolContext context, IStatisticHandler? statisticHandler) 
        : base(messageFactory, context, statisticHandler)
    {
        _logger = context.LoggerFactory.CreateLogger<MavlinkV2Parser>();
    }
    private enum DecodeStep
    {
        Sync,
        Length,
        FillBuffer,
        FillSignature
    }
    
    public override bool Push(byte value)
    {
        try
        {
            switch (_decodeStep)
            {
                case DecodeStep.Sync:
                    if (value == MavlinkV2Protocol.MagicMarkerV2)
                    {
                        _buffer[0] = value;
                        _decodeStep = DecodeStep.Length;
                    }
                    return false;
                case DecodeStep.Length:
                    _buffer[1] = value;
                    _bufferStopIndex = MavlinkV2Protocol.PayloadStartIndexInFrame + /*payload length*/value + 2 /*crc*/;
                    _bufferIndex = 2;
                    _decodeStep = DecodeStep.FillBuffer;
                    return false;
                    break;
                case DecodeStep.FillBuffer:
                    _buffer[_bufferIndex] = value;
                    ++_bufferIndex;
                    if (_bufferIndex >= MavlinkV2Protocol.PacketV2MaxSize)
                    {
                        _decodeStep = DecodeStep.Sync;
                        return false;
                    }
                    if (_bufferIndex < _bufferStopIndex) return false;
                    // buffer with frames,payload and checksumm filled => check if signature present
                    // https://mavlink.io/en/guide/message_signing.html
                    if (MavlinkV2Protocol.CheckSignaturePresent(_buffer,0))
                    {
                        _decodeStep = DecodeStep.FillSignature;
                        return false;
                    }
                    _decodeStep = DecodeStep.Sync;
                    TryDecodePacket(_bufferIndex);
                    return true;
                case DecodeStep.FillSignature:
                    _buffer[_bufferIndex] = value;
                    ++_bufferIndex;
                    if (_bufferIndex >= MavlinkV2Protocol.PacketV2MaxSize + MavlinkV2Protocol.SignatureByteSize)
                    {
                        _decodeStep = DecodeStep.Sync;
                        return false;
                    }
                    if (_bufferIndex <= (_bufferStopIndex + MavlinkV2Protocol.SignatureByteSize))
                    {
                        TryDecodePacket(_bufferIndex);
                        _decodeStep = DecodeStep.Sync;
                        return true;
                    }
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e,$"Fatal error to decode packet:{e.Message}");
            _decodeStep = DecodeStep.Sync;
            Debug.Assert(false, e.Message);
            return false;
        }
    }

    private void TryDecodePacket(int size)
    {
        var messageId = MavlinkV2Protocol.GetMessageId(_buffer,0);
        var span = new ReadOnlySpan<byte>(_buffer, 0, size);
        InternalParsePacket((ushort)messageId,ref span, true );
    }

    public override void Reset()
    {
        _decodeStep = DecodeStep.Sync;
    }

    public override ProtocolInfo Info => MavlinkV2Protocol.Info;

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
