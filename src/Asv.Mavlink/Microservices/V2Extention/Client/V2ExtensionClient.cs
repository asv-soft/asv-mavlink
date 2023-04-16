using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class V2ExtensionClient : MavlinkMicroserviceClient, IV2ExtensionClient
    {
        private readonly MavlinkClientIdentity _identity;
        private readonly RxValue<V2ExtensionPacket> _onData = new();
        public static readonly int StaticMaxDataSize = new V2ExtensionPayload().GetMaxByteSize();

        public V2ExtensionClient(IMavlinkV2Connection connection,MavlinkClientIdentity identity, IPacketSequenceCalculator seq, IScheduler scheduler):base("V2EXT", connection, identity, seq, scheduler)
        {
            _identity = identity;
            InternalFilter<V2ExtensionPacket>().Subscribe(_onData).DisposeItWith(Disposable);
        }
       
        public int MaxDataSize => StaticMaxDataSize;

        public IRxValue<V2ExtensionPacket> OnData => _onData;

        public Task SendData(byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
        {
            return InternalSend<V2ExtensionPacket>(_ =>
            {
                _.Payload.MessageType = messageType;
                _.Payload.Payload = data;
                _.Payload.TargetComponent = _identity.TargetComponentId;
                _.Payload.TargetSystem = _identity.TargetSystemId;
                _.Payload.TargetNetwork = targetNetworkId;
            }, cancel);
            
        }
    }

    
}
