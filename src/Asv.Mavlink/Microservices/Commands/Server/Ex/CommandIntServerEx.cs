using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class CommandIntServerEx : CommandServerEx<CommandIntPacket>
{
    public CommandIntServerEx(ICommandServer server) 
        : base(server, server.OnCommandInt, _=>(ushort)_.Payload.Command, _=>0)
    {
        
    }
}