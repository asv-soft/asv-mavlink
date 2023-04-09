using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class CommandLongServerList : CommandServerListBase<CommandLongPacket>
{
    public CommandLongServerList(ICommandServer server) 
        : base(server, server.OnCommandLong, _=>(ushort)_.Payload.Command, _=>_.Payload.Confirmation)
    {
        
    }
}