using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink
{



    public class CommandServer : MavlinkMicroserviceServer, ICommandServer
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkIdentity _identity;
        private readonly ILogger _logger;

        public CommandServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkIdentity identity, IScheduler? rxScheduler,ILogger? logger = null)
            : base("COMMAND", connection, identity, seq, rxScheduler,logger)
        {
            _logger = logger ?? NullLogger.Instance;
            _connection = connection;
            _seq = seq;
            _identity = identity;
            OnCommandLong =
                InternalFilter<CommandLongPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
                    .ObserveOn(Scheduler).Publish().RefCount();
            OnCommandInt = InternalFilter<CommandIntPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent)
                .ObserveOn(Scheduler).Publish().RefCount();
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
