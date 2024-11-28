using System;
using Asv.IO;

namespace Asv.Mavlink;

public static class MavlinkV1Protocol
{
    public static readonly ProtocolInfo Info = new("MavlinkV1", "Mavlink V1");
    
    public const byte MagicMarker = 0xFE;
    public const int PacketMaxSize = 263;
    public const int PayloadStartIndex = 5;
    
    public static void RegisterMavlinkV1Protocol(this IProtocolBuilder builder)
    {
        builder.RegisterProtocol(Info, (core,stat) => new MavlinkV1Parser(MavlinkV1MessageFactory.Instance, core,stat));
    }
    
    public static int GetMessageId(ReadOnlySpan<byte> buffer)
    {
        return buffer[5];
    }

    public static ushort CalculateChecksum(ReadOnlySpan<byte> buffer, byte crcExtra)
    {
        var crc = X25Crc.Accumulate(ref buffer, X25Crc.CrcSeed);
        crc = X25Crc.Accumulate(crcExtra, crc);
        return crc;
    }
}