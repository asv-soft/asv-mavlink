using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink
{
    public class CommandProtocolConfig
    {
        public int CommandTimeoutMs { get; set; } = 5000;
        public int CommandAttempt { get; set; } = 5;
    }

    public class CommandClient : MavlinkMicroserviceClient, ICommandClient
    {
        private readonly ILogger _logger; 
        private readonly CommandProtocolConfig _config;

        public CommandClient(MavlinkClientIdentity identity, CommandProtocolConfig config,ICoreServices core)
            :base("COMMAND", identity, core)
        {
            _logger = core.Log.CreateLogger<CommandClient>();
            _config = config ?? throw new ArgumentNullException(nameof(config));
            OnCommandAck = InternalFilter<CommandAckPacket>().Select(p => p.Payload);
        }

        public Task SendCommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue,
            float param1, float param2,
            float param3, float param4, int x, int y, float z, CancellationToken cancel)
        {
            _logger.ZLogTrace($"{LogSend}{command:G}({frame:G},{param1},{param2},{param3},{param4},{x},{y},{z},{current},{autocontinue})");
            return InternalSend<CommandIntPacket>((packet) =>
            {
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
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

        public async Task<CommandAckPayload> CommandInt(MavCmd command, MavFrame frame, bool current, bool autocontinue, float param1, float param2,
            float param3, float param4, int x, int y, float z,  CancellationToken cancel)
        {
            _logger.ZLogTrace($"{LogSend}{command:G}({frame:G},{param1},{param2},{param3},{param4},{x},{y},{z},{current},{autocontinue})");
            var result = await InternalCall<CommandAckPayload, CommandIntPacket, CommandAckPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
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
            }, p=>p.Payload.Command == command, p=>p.Payload,_config.CommandAttempt,null, _config.CommandTimeoutMs,cancel).ConfigureAwait(false);
            _logger.ZLogTrace($"{LogRecv}{command:G}({frame:G},{param1},{param2},{param3},{param4},{x},{y},{z},{current},{autocontinue}) => {result.Result:G} {result.Progress} {result.ResultParam2})");
            return result;
        }

        public async Task SendCommandLong(MavCmd command, float param1, float param2, float param3,
            float param4, float param5, float param6, float param7, CancellationToken cancel)
        {
            await InternalSend<CommandLongPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                packet.Payload.Confirmation = 0;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Param5 = param5;
                packet.Payload.Param6 = param6;
                packet.Payload.Param7 = param7;
            }, cancel).ConfigureAwait(false);
            _logger.ZLogTrace($"{LogSend}{command:G}{param1},{param1},{param2},{param3},{param4},{param5},{param6},{param7}");
        }

        public IObservable<CommandAckPayload> OnCommandAck { get; }

        public async Task<CommandAckPayload> CommandLong(Action<CommandLongPayload> edit, CancellationToken cancel = default)
        {
            var command = (MavCmd)0;
            string commandTxt = String.Empty;
            var result = await InternalCall<CommandAckPayload, CommandLongPacket, CommandAckPacket>((packet) =>
            {
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                packet.Payload.Confirmation = 0;
                edit(packet.Payload);
                command = packet.Payload.Command;
                commandTxt = $"{command:G}({packet.Payload.Param1},{packet.Payload.Param1},{packet.Payload.Param2},{packet.Payload.Param3},{packet.Payload.Param4},{packet.Payload.Param5},{packet.Payload.Param6},{packet.Payload.Param7})";
                _logger.ZLogTrace($"{LogSend}{commandTxt}");
            }, p => p.Payload.Command == command, p => p.Payload, _config.CommandAttempt, (p,att)=>p.Payload.Confirmation = (byte)att, _config.CommandTimeoutMs, cancel).ConfigureAwait(false);
            _logger.ZLogTrace($"{LogRecv}{commandTxt} => {result.Result:G} {result.Progress} {result.ResultParam2})");
            return result;
        }

        public async Task<CommandAckPayload> CommandLong(MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel)
        {
            _logger.ZLogTrace($"{LogSend}{command:G}({param1},{param1},{param2},{param3},{param4},{param5},{param6},{param7})");
            var result = await InternalCall<CommandAckPayload, CommandLongPacket, CommandAckPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                packet.Payload.Confirmation = 0;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Param5 = param5;
                packet.Payload.Param6 = param6;
                packet.Payload.Param7 = param7;
            }, p => p.Payload.Command == command, p => p.Payload, _config.CommandAttempt, (p,att)=>p.Payload.Confirmation = (byte)att, _config.CommandTimeoutMs, cancel).ConfigureAwait(false);
            _logger.ZLogTrace($"{LogRecv}{command:G}({param1},{param2},{param3},{param4},{param5},{param6},{param7}) => {result.Result:G} {result.Progress} {result.ResultParam2})");
            return result;
        }

        public async Task<TAnswerPacket> CommandLongAndWaitPacket<TAnswerPacket>(MavCmd command, float param1, float param2, float param3,
            float param4, float param5, float param6, float param7, CancellationToken cancel) 
            where TAnswerPacket : IPacketV2<IPayload>, new()
        {
            _logger.ZLogTrace($"{LogSend}{command:G}({param1},{param1},{param2},{param3},{param4},{param5},{param6},{param7})");
            var result = await InternalCall<TAnswerPacket, CommandLongPacket, TAnswerPacket>((packet) =>
            {
                packet.Payload.Command = command;
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                packet.Payload.Confirmation = 0;
                packet.Payload.Param1 = param1;
                packet.Payload.Param2 = param2;
                packet.Payload.Param3 = param3;
                packet.Payload.Param4 = param4;
                packet.Payload.Param5 = param5;
                packet.Payload.Param6 = param6;
                packet.Payload.Param7 = param7;
            }, _ => true, v => v, _config.CommandAttempt, (p,att)=>p.Payload.Confirmation = (byte)att, _config.CommandTimeoutMs, cancel).ConfigureAwait(false);
            _logger.ZLogTrace($"{LogRecv}{command:G}({param1},{param2},{param3},{param4},{param5},{param6},{param7}) => {result.Name})");
            return result;
        }
        
    }
}
