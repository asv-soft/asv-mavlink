using Asv.IO;
using Asv.Mavlink.Common;
using DotNext.Patterns;

namespace Asv.Mavlink;

public class CommandIntPacketFormatter : ProtocolMessageFormatter<CommandIntPacket>, ISingleton<CommandIntPacketFormatter>
{
    protected override string Print(CommandIntPacket packet, PacketFormatting formatting)
    {
        return $"{packet.Name}: {packet.Payload.Command:G}[TARGET:{packet.Payload.TargetSystem}:{packet.Payload.TargetComponent}]({packet.Payload.Param1}, {packet.Payload.Param2}, {packet.Payload.Param3}, {packet.Payload.Param4}, X:{packet.Payload.X}, Y:{packet.Payload.Y}, Z:{packet.Payload.Z})";
    }

    public override string Name => "Mavlink CommandInt";
    public override int Order => int.MaxValue/2;
    public static CommandIntPacketFormatter Instance { get; } = new();
}

public class CommandAckPacketFormatter : ProtocolMessageFormatter<CommandAckPacket>, ISingleton<CommandAckPacketFormatter>
{
    protected override string Print(CommandAckPacket packet, PacketFormatting formatting)
    {
        return $"{packet.Name}: {packet.Payload.Command:G}[TARGET:{packet.Payload.TargetSystem}:{packet.Payload.TargetComponent}]({packet.Payload.Result:G}, progress:{packet.Payload.Progress}, result:{packet.Payload.ResultParam2})";
    }

    public override string Name => "Mavlink CommandAck";
    public override int Order => int.MaxValue/2;
    public static CommandAckPacketFormatter Instance { get; } = new();
}