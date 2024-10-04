using System;
using Newtonsoft.Json;

namespace Asv.Mavlink;

public class DefaultPacketHandler : IPacketPrinterHandler
{
    public int Order => int.MaxValue;
    public bool CanPrint(IPacketV2<IPayload> packet)
    {
        return true;
    }

    public string Print(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        var canConvert = packet != null;
        if (packet == null) throw new ArgumentException("Incoming packet was not initialized!");
        if (!canConvert) throw new ArgumentException("Converter can not convert incoming packet!");
        
        return $"Type: {packet.Name}, Source: {packet.SystemId}:{packet.ComponentId}, Size:{packet.GetByteSize()}, Sequence: {packet.Sequence:000} Message:{ConvertPacket(packet, formatting)}";
    }
    private static string ConvertPacket(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        var result = formatting switch
        {
            PacketFormatting.None => JsonConvert.SerializeObject(packet.Payload, Formatting.None),
            PacketFormatting.Indented => JsonConvert.SerializeObject(packet.Payload, Formatting.Indented),
            _ => throw new ArgumentException("Wrong packet formatting!")
        };

        return result;
    }
}