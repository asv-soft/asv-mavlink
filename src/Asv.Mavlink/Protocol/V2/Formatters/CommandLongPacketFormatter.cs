using Asv.IO;
using Asv.Mavlink.Common;
using DotNext.Patterns;

namespace Asv.Mavlink;

public class CommandLongPacketFormatter : ProtocolMessageFormatter<CommandLongPacket>, ISingleton<CommandLongPacketFormatter>
{
    protected override string Print(CommandLongPacket packet, PacketFormatting formatting)
    {
        return $"{packet.Name}: {packet.Payload.Command:G}[REPLY:{packet.Payload.Confirmation},TARGET:{packet.Payload.TargetSystem}:{packet.Payload.TargetComponent}]({packet.Payload.Param1}, {packet.Payload.Param2}, {packet.Payload.Param3}, {packet.Payload.Param4}, {packet.Payload.Param5}, {packet.Payload.Param6}, {packet.Payload.Param7})";
    }

    public override string Name => "Mavlink CommandLong";
    public override int Order => int.MaxValue/2;
    public static CommandLongPacketFormatter Instance { get; } = new();
}