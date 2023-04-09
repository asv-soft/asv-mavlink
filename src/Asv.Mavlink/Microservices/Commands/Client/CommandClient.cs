using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class CommandProtocolConfig
    {
        public int CommandTimeoutMs { get; set; } = 5000;
    }

    public class CommandClient : MavlinkMicroserviceClient, ICommandClient
    {
        private readonly CommandProtocolConfig _config;

        public CommandClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, CommandProtocolConfig config, IScheduler scheduler):base(connection,identity,seq,"COMMAND", scheduler)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task SendCommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue,
            float param1, float param2,
            float param3, float param4, int x, int y, float z, CancellationToken cancel)
        {
            return InternalSend<CommandIntPacket>((packet) =>
            {
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                packet.Payload.Command = command;
                packet.Payload.Frame = frame;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Current = (byte)(current ? 1 : 0);
                packet.Payload.Autocontinue = (byte)(autocontinue ? 1 : 0);
                packet.Payload.X = x;
                packet.Payload.Y = y;
                packet.Payload.Z = z;
            },cancel);
        }

        public Task<CommandAckPayload> CommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue, float param1, float param2,
            float param3, float param4, int x, int y, float z, int attemptCount, CancellationToken cancel)
        {
            return InternalCall<CommandAckPayload, CommandIntPacket, CommandAckPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                packet.Payload.Frame = frame;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Current = (byte)(current ? 1 : 0);
                packet.Payload.Autocontinue = (byte)(autocontinue ? 1 : 0);
                packet.Payload.X = x;
                packet.Payload.Y = y;
                packet.Payload.Z = z;
            }, _=>_.Payload.Command == command, _=>_.Payload,attemptCount,null, _config.CommandTimeoutMs,cancel);
        }

        public async Task SendCommandLong(MavCmd command, float param1, float param2, float param3,
            float param4, float param5, float param6, float param7, CancellationToken cancel)
        {
            await InternalSend<CommandLongPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                packet.Payload.Confirmation = 0;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Param5 = param5;
                packet.Payload.Param6 = param6;
                packet.Payload.Param7 = param7;
            }, cancel).ConfigureAwait(false);
        }

        public Task<CommandAckPayload> CommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, int attemptCount, CancellationToken cancel)
        {
            return InternalCall<CommandAckPayload, CommandLongPacket, CommandAckPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                packet.Payload.Confirmation = 0;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Param5 = param5;
                packet.Payload.Param6 = param6;
                packet.Payload.Param7 = param7;
            }, _ => _.Payload.Command == command, _ => _.Payload, attemptCount, (_,att)=>_.Payload.Confirmation = (byte)att, _config.CommandTimeoutMs, cancel);
        }
        
    }
}
