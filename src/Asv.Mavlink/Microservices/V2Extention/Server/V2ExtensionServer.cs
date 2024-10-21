using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink
{
    public class V2ExtensionServer : MavlinkMicroserviceServer, IV2ExtensionServer
    {
        private readonly RxValue<V2ExtensionPacket> _onData;
        private readonly IDisposable _disposeIt;

        public V2ExtensionServer(MavlinkIdentity identity,ICoreServices core )
        :base("V2EXT",identity,core)
        {
            _onData = new RxValue<V2ExtensionPacket>();
            var d1 = core.Connection.Filter<V2ExtensionPacket>()
                .Where(p =>
                    (p.Payload.TargetSystem == 0 || p.Payload.TargetSystem == identity.SystemId) &&
                    (p.Payload.TargetComponent == 0 || p.Payload.TargetComponent == identity.ComponentId))
                .Subscribe(_onData);
            _disposeIt = Disposable.Combine(_onData, d1);
        }

        public IRxValue<V2ExtensionPacket> OnData => _onData;

        public Task SendData(byte targetSystemId,byte targetComponentId,byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
        {
            return InternalSend<V2ExtensionPacket>(packet =>
            {
                packet.Payload.MessageType = messageType;
                packet.Payload.Payload = data;
                packet.Payload.TargetComponent = targetComponentId;
                packet.Payload.TargetSystem = targetSystemId;
                packet.Payload.TargetNetwork = targetNetworkId;
            }, cancel);
        }

        public override void Dispose()
        {
            _disposeIt.Dispose();
            base.Dispose();
        }
    }

    
}
