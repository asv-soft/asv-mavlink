using System;
using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink
{



    public class CommandServer : MavlinkMicroserviceServer, ICommandServer
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkServerIdentity _identity;

        public CommandServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity, IScheduler rxScheduler)
            : base(connection, identity, seq, "COMMAND", rxScheduler)
        {
            _connection = connection;
            _seq = seq;
            _identity = identity;
            OnCommandLong =
                InternalFilter<CommandLongPacket>(_ => _.Payload.TargetSystem, _ => _.Payload.TargetComponent)
                    .ObserveOn(Scheduler).Publish().RefCount();
            OnCommandInt = InternalFilter<CommandIntPacket>(_ => _.Payload.TargetSystem, _ => _.Payload.TargetComponent)
                .ObserveOn(Scheduler).Publish().RefCount();
        }

        public IObservable<CommandLongPacket> OnCommandLong { get; }
        public IObservable<CommandIntPacket> OnCommandInt { get; }

        public Task SendCommandAck(MavCmd cmd, DeviceIdentity responseTarget, CommandResult result,
            CancellationToken cancel = default)
        {
            return InternalSend<CommandAckPacket>(_ =>
            {
                _.Payload.Command = cmd;
                _.Payload.Result = result.ResultCode;
                _.Payload.TargetSystem = responseTarget?.SystemId ?? 0;
                _.Payload.TargetComponent = responseTarget?.ComponentId ?? 0;
                _.Payload.Progress = result.Progress ?? 0;
                _.Payload.ResultParam2 = result.ResultParam2 ?? 0;
            }, cancel);
        }

    }


}