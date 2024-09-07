using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class V2ExtensionServer : MavlinkMicroserviceServer, IV2ExtensionServer
    {
        private readonly RxValue<V2ExtensionPacket> _onData = new();
        private readonly CancellationTokenSource _disposeCancel = new();

        public V2ExtensionServer(
            IMavlinkV2Connection connection, 
            IPacketSequenceCalculator seq,
            MavlinkIdentity identity, 
            IScheduler? rxScheduler = null,
            ILogger? logger = null)
        :base("V2EXT",connection,identity,seq,rxScheduler,logger)
        {
            _onData.DisposeItWith(Disposable);
            connection
                .Where(v => v.MessageId == V2ExtensionPacket.PacketMessageId)
                .Cast<V2ExtensionPacket>().Where(p =>
                    (p.Payload.TargetSystem == 0 || p.Payload.TargetSystem == identity.SystemId) &&
                    (p.Payload.TargetComponent == 0 || p.Payload.TargetComponent == identity.ComponentId))
                .Subscribe(_onData,_disposeCancel.Token);
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
    }

    
}
