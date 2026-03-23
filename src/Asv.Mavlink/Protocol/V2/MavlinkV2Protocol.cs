using System;
using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvChart;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.AsvRfsa;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Avssuas;
using Asv.Mavlink.Common;
using Asv.Mavlink.Csairlink;
using Asv.Mavlink.Cubepilot;
using Asv.Mavlink.Icarous;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.Storm32;
using Asv.Mavlink.Ualberta;
using Asv.Mavlink.Uavionix;

namespace Asv.Mavlink;

/// <summary>
///                         MAVLink 2 packet
/// [STX|LEN|INC|CMP|SEQ|SYS|COMP|MSG_ID| PAYLOAD | CHECKSUM      | SIGNATURE    |
///   0   1   2   3   4   5   6    7-9   10-(10+N)  (11+N)-(12+N)   (13+N)-(26+N)
/// </summary>
public static class MavlinkV2Protocol
{
    public static readonly ProtocolInfo Info = new("MavlinkV2", "Mavlink V2");
    
    /// <summary>
    /// Protocol-specific start-of-text (STX) marker used to indicate the beginning of a new packet. 
    /// Any system that does not understand protocol version will skip the packet.
    /// </summary>
    public const byte MagicMarkerV2 = 0xFD;
    /// <summary>
    /// The maximum packet length is 279 bytes for a signed message that uses the whole payload.
    /// </summary>
    public const int PacketV2MaxSize = 279;
    
    public static bool CheckSignaturePresent(byte[] buffer, int inx)
    {
        return (GetIncompatFlags(buffer,inx) & 0x01) != 0;
    }
    /// <summary>
    /// Size of signature, if present
    /// </summary>
    public const int SignatureByteSize = 13;
    
    public static void SetMessageId(byte[] buffer, int frameStartIndex, int messageId)
    {
        buffer[frameStartIndex + 7] = (byte)(messageId & 0xFF);
        buffer[frameStartIndex + 8] = (byte)((messageId >> 8) & 0xFF);
        buffer[frameStartIndex + 9] = (byte)((messageId >> 16) & 0xFF);
    }

    public static int GetMessageId(byte[] buffer, int frameStartIndex)
    {
        int messageId = buffer[7 + frameStartIndex];
        messageId |= buffer[8 + frameStartIndex] << 8;
        messageId |= buffer[9 + frameStartIndex] << 16;
        return messageId;
    }
    /// <summary>
    /// Packet frame byte size without Payload and Signature
    /// </summary>
    public const int PacketV2FrameSize = 13;
    public const int PayloadStartIndexInFrame = PacketV2FrameSize - /*STX*/1 - 2 /*CRC*/ ;
    public static byte GetIncompatFlags(byte[] buffer, int inx)
    {
        return buffer[inx + 2];
    }
    
    public static void RegisterMavlinkV2Protocol(this IProtocolBuilder builder, IProtocolMessageFactory<MavlinkMessage, int> messageFactory)
    {
        builder.Protocols.Register(Info, (core,stat) => new MavlinkV2Parser(messageFactory, core,stat));
        
        builder.Formatters.Register(FtpPacketFormatter.Instance);
        builder.Formatters.Register(ParamSetFormatter.Instance);
        builder.Formatters.Register(ParamValueFormatter.Instance);
        builder.Formatters.Register(StatusTextFormatter.Instance);
        builder.Formatters.Register(GpsRtcmDataPacketFormatter.Instance);
        builder.Formatters.Register(CommandLongPacketFormatter.Instance);
        builder.Formatters.Register(CommandIntPacketFormatter.Instance);
        builder.Formatters.Register(CommandAckPacketFormatter.Instance);
        builder.Formatters.Register(PayloadAsJsonFormatter.Instance);
    }

    public static IProtocolMessageFactory<MavlinkMessage, int> CreateMessageFactory(Action<ImmutableDictionary<int,Func<MavlinkMessage>>.Builder>? configure = null)
    {
        configure ??= RegisterDefaultDialects;
        var builder = ImmutableDictionary.CreateBuilder<int, Func<MavlinkMessage>>();
        configure(builder);
        return new ProtocolMessageFactory<MavlinkMessage, int>(Info, builder.ToImmutable());
    }
    
    private static void RegisterDefaultDialects(this ImmutableDictionary<int, Func<MavlinkMessage>>.Builder builder)
    {
        builder.RegisterMinimalDialect();
        builder.RegisterCommonDialect();
        builder.RegisterArdupilotmegaDialect();
        builder.RegisterIcarousDialect();
        builder.RegisterUalbertaDialect();
        builder.RegisterStorm32Dialect();
        builder.RegisterAvssuasDialect();
        builder.RegisterUavionixDialect();
        builder.RegisterCubepilotDialect();
        builder.RegisterCsairlinkDialect();
        builder.RegisterAsvGbsDialect();
        builder.RegisterAsvSdrDialect();
        builder.RegisterAsvAudioDialect();
        builder.RegisterAsvRadioDialect();
        builder.RegisterAsvRfsaDialect();
        builder.RegisterAsvChartDialect();
        builder.RegisterAsvRsgaDialect();
    }
}