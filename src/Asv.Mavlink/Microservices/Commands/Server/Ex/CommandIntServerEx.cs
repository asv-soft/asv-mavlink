using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public class CommandIntServerEx(ICommandServer server)
    : CommandServerEx<CommandIntPacket>(server, server.OnCommandInt, p => (ushort)p.Payload.Command, _ => 0);