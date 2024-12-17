using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink
{
    public class CommandServer : MavlinkMicroserviceServer, ICommandServer
    {
        private readonly ILogger<CommandServer> _logger;
        private readonly Subject<CommandLongPacket> _onCommandLong;
        private readonly Subject<CommandIntPacket> _onCommandInt;
        private readonly IDisposable _obs1;
        private readonly IDisposable _obs2;

        public CommandServer(MavlinkIdentity identity,IMavlinkContext core)
            : base(Command.MicroserviceTypeName, identity,core)
        {
            _logger = core.LoggerFactory.CreateLogger<CommandServer>();
            _onCommandLong = new Subject<CommandLongPacket>();
            _obs1 = InternalFilter<CommandLongPacket>(
                    p => p.Payload.TargetSystem, 
                    p => p.Payload.TargetComponent
            ).Subscribe(_onCommandLong.AsObserver());
            _onCommandInt = new Subject<CommandIntPacket>();
            _obs2 = InternalFilter<CommandIntPacket>(
                    p => p.Payload.TargetSystem, 
                    p => p.Payload.TargetComponent
            ).Subscribe(_onCommandInt.AsObserver());

        }

        public Observable<CommandLongPacket> OnCommandLong => _onCommandLong;

        public Observable<CommandIntPacket> OnCommandInt => _onCommandInt;

        public ValueTask SendCommandAck(
            MavCmd cmd, 
            DeviceIdentity responseTarget, 
            CommandResult result,
            CancellationToken cancel = default
        )
        {
            return InternalSend<CommandAckPacket>(p =>
            {
                p.Payload.Command = cmd;
                p.Payload.Result = result.ResultCode;
                p.Payload.TargetSystem = responseTarget.SystemId;
                p.Payload.TargetComponent = responseTarget.ComponentId;
                p.Payload.Progress = result.Progress ?? 0;
                p.Payload.ResultParam2 = result.ResultParam2 ?? 0;
            }, cancel);
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onCommandLong.Dispose();
                _onCommandInt.Dispose();
                _obs1.Dispose();
                _obs2.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_onCommandLong).ConfigureAwait(false);
            await CastAndDispose(_onCommandInt).ConfigureAwait(false);
            await CastAndDispose(_obs1).ConfigureAwait(false);
            await CastAndDispose(_obs2).ConfigureAwait(false);

            await base.DisposeAsyncCore().ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }

        #endregion
    }


}
