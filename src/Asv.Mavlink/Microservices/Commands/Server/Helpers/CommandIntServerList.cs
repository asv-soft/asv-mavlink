using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class CommandIntServerList : CommandServerListBase<CommandIntPacket>
{
    public CommandIntServerList(ICommandServer server) 
        : base(server, server.OnCommandInt, _=>(ushort)_.Payload.Command, _=>0)
    {
        
    }
}