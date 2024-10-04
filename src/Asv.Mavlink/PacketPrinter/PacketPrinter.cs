using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink;

namespace Asv.Mavlink;

public class PacketPrinter
{
    private ImmutableList<IPacketPrinterHandler> _packetHandlers;
    public PacketPrinter(IEnumerable<IPacketPrinterHandler> packetPrinterHandlers)
    {
        _packetHandlers = packetPrinterHandlers.OrderBy(_ => _.Order).ToImmutableList();
    }
    public string Print(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        if (packet is null)
            throw new NullReferenceException($"Cannot read object {packet}");
        foreach (var handler in from handler in _packetHandlers let can = handler.CanPrint(packet) where can select handler)
        {
            return handler.Print(packet, formatting);
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