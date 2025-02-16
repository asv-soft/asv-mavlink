using System;
using System.Text;
using System.Text.Json.Serialization;
using Asv.IO;
using Asv.Mavlink.Common;
using DotNext.Patterns;


namespace Asv.Mavlink;

public class StatusTextFormatter : IProtocolMessageFormatter, ISingleton<StatusTextFormatter>
{
    public static StatusTextFormatter Instance { get; } = new();

    private StatusTextFormatter()
    {
        
    }
    
    public int Order => int.MaxValue/2;
    public bool CanPrint(IProtocolMessage packet)
    {
        return packet is StatustextPacket;
    }

    public string Print(IProtocolMessage message, PacketFormatting formatting)
    {
        
        var packet = message as StatustextPacket;
        if (packet == null) throw new ArgumentException("Packet is not a StatustextPacket!");
        var sb = new StringBuilder();
        switch (formatting)
        {
            case PacketFormatting.Inline:
                sb.Append("{");
                sb.Append("\"Severity\":");
                sb.Append($"{packet.Payload.Severity},");
                sb.Append("\"Text\":");
                sb.Append($"\"{MavlinkTypesHelper.GetString(packet.Payload.Text)}\"");
                sb.Append("}");
                break;
            case PacketFormatting.Indented:
                sb.Append("{\n");
                sb.Append("\"Severity\": ");
                sb.Append($"{packet.Payload.Severity},\n");
                sb.Append("\"Text\": ");
                sb.Append($"\"{MavlinkTypesHelper.GetString(packet.Payload.Text)}\"\n");
                sb.Append("}");
                break;
            default:
                throw new ArgumentException("Wrong packet formatting!");
        }

        return sb.ToString();
    }

    public string Name => "StatusText";
    
}