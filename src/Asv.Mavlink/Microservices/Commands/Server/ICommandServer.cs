using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    
    public interface ICommandServer
    {
        IObservable<CommandLongPacket> OnCommandLong { get; }
        IObservable<CommandIntPacket> OnCommandInt { get; }
        Task SendCommandAck(MavCmd cmd, DeviceIdentity responseTarget, CommandResult result, CancellationToken cancel = default);
    }

    public static class CommandServerHelper
    {
        public static Task SendCommandAckAccepted(this ICommandServer server, CommandIntPacket req, MavResult result, CancellationToken cancel = default)
        {
            return server.SendCommandAck(req.Payload.Command, new DeviceIdentity(req.SystemId,req.ComponentId), new CommandResult(result), cancel);
        }
        public static Task SendCommandAckAccepted(this ICommandServer server, CommandLongPacket req, MavResult result, CancellationToken cancel = default)
        {
            return server.SendCommandAck(req.Payload.Command, new DeviceIdentity(req.SystemId,req.ComponentId), new CommandResult(result), cancel);
        }
    }
    
    public class CommandResult
    {
        public CommandResult(MavResult resultCode, int result = 0,byte? progress = null,int? resultParam2 = null)
        {
            ResultCode = resultCode;
            Progress = progress;
            Result = result;
            ResultParam2 = resultParam2;
        }

        public int? ResultParam2 { get; set; }
        public MavResult ResultCode { get; }
        public int Result { get; }
        public byte? Progress { get; }
        
    }

    public class DeviceIdentity
    {
        public static readonly DeviceIdentity Broadcast = new() { ComponentId = 0, SystemId = 0 };

        public DeviceIdentity()
        {
            
        }

        public DeviceIdentity(byte systemId,byte componentId)
        {
            SystemId = systemId;
            ComponentId = componentId;
        }

        public byte SystemId { get; set; }
        public byte ComponentId { get; set; }
    }


   

    

   
}
