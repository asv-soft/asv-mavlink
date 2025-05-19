using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    public class V2ExtensionClient : MavlinkMicroserviceClient, IV2ExtensionClient
    {
        
        public static readonly int StaticMaxDataSize = new V2ExtensionPayload().GetMaxByteSize();
        
        
        private readonly MavlinkClientIdentity _identity;
        private readonly ReactiveProperty<V2ExtensionPacket> _onData;
        private readonly IDisposable _subscribe;

        public V2ExtensionClient(MavlinkClientIdentity identity, 
            IMavlinkContext core):base(V2Extension.MicroserviceTypeName, identity, core)
        {
            _identity = identity;
            _onData = new ReactiveProperty<V2ExtensionPacket>();
            _subscribe = InternalFilter<V2ExtensionPacket>().Subscribe(_onData.AsObserver());
        }
       
        public int MaxDataSize => StaticMaxDataSize;

        public ReadOnlyReactiveProperty<V2ExtensionPacket> OnData => _onData;

        public ValueTask SendData(byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
        {
            return InternalSend<V2ExtensionPacket>(p =>
            {
                p.Payload.MessageType = messageType;
                data.CopyTo(p.Payload.Payload, 0);
                p.Payload.TargetComponent = _identity.Target.ComponentId;
                p.Payload.TargetSystem = _identity.Target.SystemId;
                p.Payload.TargetNetwork = targetNetworkId;
            }, cancel);
            
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onData.Dispose();
                _subscribe.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_onData).ConfigureAwait(false);
            await CastAndDispose(_subscribe).ConfigureAwait(false);

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
