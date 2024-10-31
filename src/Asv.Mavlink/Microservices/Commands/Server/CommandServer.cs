using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{



    public class CommandServer : MavlinkMicroserviceServer, ICommandServer
    {
        private readonly ILogger<CommandServer> _logger;

        public CommandServer(MavlinkIdentity identity,ICoreServices core)
            : base("COMMAND", identity,core)
        {
            _logger = core.Log.CreateLogger<CommandServer>();
            OnCommandLong =
                InternalFilter<CommandLongPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
                    .Publish().RefCount();
            OnCommandInt = InternalFilter<CommandIntPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
                .Publish().RefCount();
        }

        public IObservable<CommandLongPacket> OnCommandLong { get; }
        public IObservable<CommandIntPacket> OnCommandInt { get; }

        public Task SendCommandAck(MavCmd cmd, DeviceIdentity responseTarget, CommandResult result,
            CancellationToken cancel = default)
        {
            return InternalSend<CommandAckPacket>(p =>
            {
                p.Payload.Command = cmd;
                p.Payload.Result = result.ResultCode;
                p.Payload.TargetSystem = responseTarget?.SystemId ?? 0;
                p.Payload.TargetComponent = responseTarget?.ComponentId ?? 0;
                p.Payload.Progress = result.Progress ?? 0;
                p.Payload.ResultParam2 = result.ResultParam2 ?? 0;
            }, cancel);
        }

    }


}
