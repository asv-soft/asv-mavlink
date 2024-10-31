using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class CommandLongServerEx(ICommandServer server) : CommandServerEx<CommandLongPacket>(server,
    server.OnCommandLong, p => (ushort)p.Payload.Command, p => p.Payload.Confirmation);