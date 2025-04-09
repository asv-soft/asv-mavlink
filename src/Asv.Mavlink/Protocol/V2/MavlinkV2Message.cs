using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

public abstract class MavlinkV2Message : MavlinkMessage
{
    private readonly Signature _signature = new();
    
    public override ProtocolInfo Protocol => MavlinkV2Protocol.Info;
    
    public byte IncompatFlags { get; set; }
    
    public byte CompatFlags { get; set; }
    
    public abstract bool WrapToV2Extension { get; }
    
    public ISignature Signature => _signature;
    
    public int GetMaxByteSize() => GetPayload().GetMaxByteSize() + Signature.GetMaxByteSize() + 12 /*HEADER*/;
    
    public override int GetByteSize() => GetPayload().GetByteSize() + (Signature.IsPresent ? MavlinkV2Protocol.SignatureByteSize : 0) + 12 /*HEADER*/;

    public override void Serialize(ref Span<byte> buffer)
    {
        var fillBuffer = buffer[..];
        var payloadBuffer = buffer[10..];
        var originLength = payloadBuffer.Length;
        GetPayload().Serialize(ref payloadBuffer);
        var payloadSize = originLength - payloadBuffer.Length;
        var originPayloadBuffer = buffer.Slice(10, payloadSize);
        
        // try to truncate empty bytes from the end of payload buffer. See https://mavlink.io/en/guide/serialization.html#payload_truncation
        if (payloadSize > 1) /*The first byte of the payload is never truncated, even if the payload consists entirely of zeros.*/
        {
            var startIndex = payloadSize - 1;
            for (var i = startIndex; i >=1 ; i--)
            {
                if (originPayloadBuffer[i] != 0)
                {
                    break;
                }
                --payloadSize;
            }
        }
        
        BinSerialize.WriteByte(ref fillBuffer, MavlinkV2Protocol.MagicMarkerV2);
        BinSerialize.WriteByte(ref fillBuffer, (byte)payloadSize);
        BinSerialize.WriteByte(ref fillBuffer, IncompatFlags);
        BinSerialize.WriteByte(ref fillBuffer, CompatFlags);
        BinSerialize.WriteByte(ref fillBuffer, Sequence);
        BinSerialize.WriteByte(ref fillBuffer, SystemId);
        BinSerialize.WriteByte(ref fillBuffer, ComponentId);
        BinSerialize.WriteByte(ref fillBuffer, (byte)(Id & 0xFF));
        BinSerialize.WriteByte(ref fillBuffer, (byte)((Id >> 8) & 0xFF));
        BinSerialize.WriteByte(ref fillBuffer, (byte)((Id >> 16) & 0xFF));
        var crcBuff = buffer.Slice(1,9);
        var crc = X25Crc.Accumulate(ref crcBuff, X25Crc.CrcSeed);
        crcBuff = buffer.Slice(10, payloadSize);
        crc = X25Crc.Accumulate(ref crcBuff, crc);
        crc = X25Crc.Accumulate(GetCrcExtra(), crc);
        fillBuffer = fillBuffer[payloadSize..];
        BinSerialize.WriteUShort(ref fillBuffer, crc);
        if (Signature.IsPresent)
        {
            Signature.Serialize(ref fillBuffer);
        }
        // Debug.Assert((payloadSize + Signature.ByteSize + PacketV2Helper.PacketV2FrameSize) == (buffer.Length - fillBuffer.Length));
        buffer = fillBuffer;
    }

    public override void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        var crcBuffer = buffer[1..];
            
            var stx = BinSerialize.ReadByte(ref buffer);
            if (stx != MavlinkV2Protocol.MagicMarkerV2)
                throw new MavlinkException(string.Format(RS.WheelKnownConstant_VerifyStx_Unknown_STX_value, MavlinkV2Protocol.MagicMarkerV2, stx));

            var payloadSize = BinSerialize.ReadByte(ref buffer);
            IncompatFlags = BinSerialize.ReadByte(ref buffer);
            CompatFlags = BinSerialize.ReadByte(ref buffer);
            Sequence = BinSerialize.ReadByte(ref buffer);
            SystemId = BinSerialize.ReadByte(ref buffer);
            ComponentId = BinSerialize.ReadByte(ref buffer);
            int messageId = BinSerialize.ReadByte(ref buffer);
            messageId |= BinSerialize.ReadByte(ref buffer) << 8;
            messageId |= BinSerialize.ReadByte(ref buffer) << 16;
            
            if (messageId != Id)
                throw new MavlinkException(string.Format(RS.PacketV2_Deserialize_Error_message_id_type, Id, messageId));
            
            var crcStartBuffer = buffer.Slice(payloadSize, 2);
            var crc = BinSerialize.ReadUShort(ref crcStartBuffer);

            var originSize = GetPayload().GetMinByteSize();
            if (payloadSize < originSize)
            {
                // this is Empty-Byte Payload Truncation https://mavlink.io/en/guide/serialization.html#payload_truncation
                var data = ArrayPool<byte>.Shared.Rent(originSize);
                var span = new Span<byte>(data,0,originSize);
                buffer[..payloadSize].CopyTo(span);
                var zeroByteNotEnough = originSize - payloadSize;
                for (var i = 0; i < zeroByteNotEnough; i++)
                {
                    data[payloadSize + i] = 0;
                }

                try
                {
                    var readOnly = new ReadOnlySpan<byte>(data, 0, originSize);
                    GetPayload().Deserialize(ref readOnly);
                    // Debug.Assert(readOnly.Length == 0);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(data);
                }
                
            }
            else
            {
                var payloadBuffer = buffer[..payloadSize];
                GetPayload().Deserialize(ref payloadBuffer);
                //Debug.Assert(payloadBuffer.Length == 0);
            }
            buffer = buffer[payloadSize..]; // skip payload
            
            var crcBuffer1 = crcBuffer[..9];
            var crcBuffer2 = crcBuffer.Slice(9, payloadSize);
            var calcCrc = X25Crc.Accumulate(ref crcBuffer1, X25Crc.CrcSeed);
            calcCrc = X25Crc.Accumulate(ref crcBuffer2, calcCrc);
            calcCrc = X25Crc.Accumulate(GetCrcExtra(), calcCrc);
            if (crc != calcCrc)
                throw new MavlinkException(string.Format(RS.PacketV2Helper_VerifyCrc_Bad_X25Crc, calcCrc, crc));

            buffer = buffer[2..]; // skip CRC
            
            if ((IncompatFlags & 0x01) != 0)
            {
                Signature.Deserialize(ref buffer);
            }
    }
}

public abstract class MavlinkV2Message<TPayload> : MavlinkV2Message
    where TPayload : IPayload
{
    public abstract TPayload Payload { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override IPayload GetPayload() => Payload;
    
    public virtual MavlinkFieldInfo[] Fields { get; }
}