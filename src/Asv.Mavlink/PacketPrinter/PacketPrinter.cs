using System;
using System.Collections.Generic;
using Asv.Mavlink;

namespace Asv.Mavlink;

public class PacketPrinter(IEnumerable<IPacketPrinterHandler> packetPrinterHandlers)
{
    public string Print(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        foreach (var handler in packetPrinterHandlers)
        {
           var can = handler.CanPrint(packet);
           if (can)
           {
               return handler.Print(packet);
           }
        }

        throw new ArgumentOutOfRangeException($"Not found accesible handler for {packet.Name}");
    }
}

public interface IPacketPrinterHandler
{
    int Order { get; }
    bool CanPrint(IPacketV2<IPayload> packet);
    string Print(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None);
}
public enum PacketFormatting
{
    None,
    Indented,
}