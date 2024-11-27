using System;
using System.Text;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class StatusTextHandler : IPacketPrinterHandler
{
    public int Order => int.MaxValue/2;
    public bool CanPrint(IPacketV2<IPayload> packet)
    {
        if (packet == null) throw new ArgumentException("Incoming packet was not initialized!");
        return packet.MessageId == StatustextPacket.PacketMessageId;
    }

    public string Print(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        if (packet == null) throw new ArgumentException("Incoming packet was not initialized!");

        var sb = new StringBuilder();

        var payload = packet.Payload as StatustextPayload;

        switch (formatting)
        {
            case PacketFormatting.None:
                sb.Append("{");
                sb.Append("\"Severity\":");
                sb.Append($"{payload.Severity},");
                sb.Append("\"Text\":");
                sb.Append($"\"{MavlinkTypesHelper.GetString(payload.Text)}\"");
                sb.Append("}");
                break;
            case PacketFormatting.Indented:
                sb.Append("{\n");
                sb.Append("\"Severity\": ");
                sb.Append($"{payload.Severity},\n");
                sb.Append("\"Text\": ");
                sb.Append($"\"{MavlinkTypesHelper.GetString(payload.Text)}\"\n");
                sb.Append("}");
                break;
            default:
                throw new ArgumentException("Wrong packet formatting!");
        }

        return sb.ToString();
    }
    
}