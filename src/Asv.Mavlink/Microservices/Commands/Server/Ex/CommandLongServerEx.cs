using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class CommandLongServerEx : CommandServerEx<CommandLongPacket>
{
    public CommandLongServerEx(ICommandServer server) 
        : base(server, server.OnCommandLong, p=>(ushort)p.Payload.Command, p=>p.Payload.Confirmation)
    {
        
    }
}