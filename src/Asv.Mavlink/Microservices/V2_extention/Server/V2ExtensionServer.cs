using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    public class V2ExtensionServer : IV2ExtensionServer
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkServerIdentity _identity;
        private readonly RxValue<V2ExtensionPacket> _onData = new RxValue<V2ExtensionPacket>();
        private readonly CancellationTokenSource _disposeCancel = new CancellationTokenSource();

        public V2ExtensionServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq, MavlinkServerIdentity identity)
        {
            _connection = connection;
            _seq = seq;
            _identity = identity;
            connection
                .Where(_ => _.MessageId == V2ExtensionPacket.PacketMessageId)
                .Cast<V2ExtensionPacket>().Where(_ =>
                    (_.Payload.TargetSystem == 0 || _.Payload.TargetSystem == _identity.SystemId) &&
                    (_.Payload.TargetComponent == 0 || _.Payload.TargetComponent == _identity.ComponentId))
                .Subscribe(_onData,_disposeCancel.Token);
        }

        public void Dispose()
        {
            _disposeCancel.Dispose();
        }

        public IRxValue<V2ExtensionPacket> OnData => _onData;

        public async Task SendData(byte targetSystemId,byte targetComponentId,byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_disposeCancel.Token, cancel);
            await _connection.Send(new V2ExtensionPacket
            {
                ComponenId = _identity.ComponentId,
                SystemId = _identity.SystemId,
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = _seq.GetNextSequenceNumber(),
                Payload =
                {
                    MessageType = messageType,
                    Payload = data,
                    TargetComponent = targetComponentId,
                    TargetSystem = targetSystemId,
                    TargetNetwork = targetNetworkId,
                }
            }, linked.Token).ConfigureAwait(false);

        }
    }

    
}
