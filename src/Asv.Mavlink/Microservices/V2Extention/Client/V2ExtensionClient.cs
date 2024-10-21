using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink
{
    public class V2ExtensionClient : MavlinkMicroserviceClient, IV2ExtensionClient
    {
        private readonly MavlinkClientIdentity _identity;
        private readonly RxValue<V2ExtensionPacket> _onData = new();
        public static readonly int StaticMaxDataSize = new V2ExtensionPayload().GetMaxByteSize();
        private readonly IDisposable _disposeIt;

        public V2ExtensionClient(MavlinkClientIdentity identity, 
            ICoreServices core):base("V2EXT", identity, core)
        {
            _identity = identity;
            var d1 = InternalFilter<V2ExtensionPacket>().Subscribe(_onData);
            _disposeIt = Disposable.Combine(_onData, d1);
        }
       
        public int MaxDataSize => StaticMaxDataSize;

        public IRxValue<V2ExtensionPacket> OnData => _onData;

        public Task SendData(byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
        {
            return InternalSend<V2ExtensionPacket>(p =>
            {
                p.Payload.MessageType = messageType;
                p.Payload.Payload = data;
                p.Payload.TargetComponent = _identity.Target.ComponentId;
                p.Payload.TargetSystem = _identity.Target.SystemId;
                p.Payload.TargetNetwork = targetNetworkId;
            }, cancel);
            
        }
        
        public override void Dispose()
        {
            _disposeIt.Dispose();
            base.Dispose();
        }
    }

    
}
