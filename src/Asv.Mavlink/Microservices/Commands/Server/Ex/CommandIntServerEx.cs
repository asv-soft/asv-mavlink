using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class CommandIntServerEx : CommandServerEx<CommandIntPacket>
{
    public CommandIntServerEx(ICommandServer server) 
        : base(server, server.OnCommandInt, p=>(ushort)p.Payload.Command, _=>0)
    {
        
    }
}