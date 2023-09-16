using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class V2ExtensionServer : MavlinkMicroserviceServer, IV2ExtensionServer
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkServerIdentity _identity;
        private readonly RxValue<V2ExtensionPacket> _onData = new();
        private readonly CancellationTokenSource _disposeCancel = new();

        public V2ExtensionServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity, IScheduler rxScheduler)
        :base("V2EXT",connection,identity,seq,rxScheduler)
        {
            _connection = connection;
            _seq = seq;
            _identity = identity;
            _onData.DisposeItWith(Disposable);
            connection
                .Where(_ => _.MessageId == V2ExtensionPacket.PacketMessageId)
                .Cast<V2ExtensionPacket>().Where(_ =>
                    (_.Payload.TargetSystem == 0 || _.Payload.TargetSystem == _identity.SystemId) &&
                    (_.Payload.TargetComponent == 0 || _.Payload.TargetComponent == _identity.ComponentId))
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
