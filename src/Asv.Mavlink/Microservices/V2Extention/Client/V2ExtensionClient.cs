using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class V2ExtensionClient : MavlinkMicroserviceClient, IV2ExtensionClient
    {
        private readonly MavlinkClientIdentity _identity;
        private readonly RxValue<V2ExtensionPacket> _onData = new();
        public static readonly int StaticMaxDataSize = new V2ExtensionPayload().GetMaxByteSize();

        public V2ExtensionClient(
            IMavlinkV2Connection connection,
            MavlinkClientIdentity identity, 
            IPacketSequenceCalculator seq,
            TimeProvider? timeProvider = null,
            IScheduler? scheduler = null,
            ILoggerFactory? logFactory = null):base("V2EXT", connection, identity, seq, timeProvider,scheduler,logFactory)
        {
            _identity = identity;
            InternalFilter<V2ExtensionPacket>().Subscribe(_onData).DisposeItWith(Disposable);
        }
       
        public int MaxDataSize => StaticMaxDataSize;

        public IRxValue<V2ExtensionPacket> OnData => _onData;

        public Task SendData(byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
        {
            return InternalSend<V2ExtensionPacket>(p =>
            {
                p.Payload.MessageType = messageType;
                p.Payload.Payload = data;
                p.Payload.TargetComponent = _identity.TargetComponentId;
                p.Payload.TargetSystem = _identity.TargetSystemId;
                p.Payload.TargetNetwork = targetNetworkId;
            }, cancel);
            
        }
    }

    
}
