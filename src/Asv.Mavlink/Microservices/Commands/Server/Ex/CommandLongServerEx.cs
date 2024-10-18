using System;
using System.Reactive.Concurrency;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class CommandLongServerEx : CommandServerEx<CommandLongPacket>
{
    public CommandLongServerEx(
        ICommandServer server,
        TimeProvider? timeProvider = null, 
        IScheduler? scheduler= null, 
        ILoggerFactory? loggerFactory= null) 
        : base(server, server.OnCommandLong, p=>(ushort)p.Payload.Command, p=>p.Payload.Confirmation, timeProvider, scheduler, loggerFactory)
    {
        
    }

    
}