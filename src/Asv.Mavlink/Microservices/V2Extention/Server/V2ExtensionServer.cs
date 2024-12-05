using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    public sealed class V2ExtensionServer : MavlinkMicroserviceServer, IV2ExtensionServer
    {
        private readonly Subject<V2ExtensionPacket> _onData;
        private readonly IDisposable _sub;

        public V2ExtensionServer(MavlinkIdentity identity,ICoreServices core )
            :base("V2EXT",identity,core)
        {
            _onData = new Subject<V2ExtensionPacket>(); 
            _sub = InternalFilter<V2ExtensionPacket>(x => x.Payload.TargetSystem, x => x.Payload.TargetComponent)
                .Subscribe(_onData.AsObserver());
        }
        public Observable<V2ExtensionPacket> OnData => _onData;
        public ValueTask SendData(byte targetSystemId,byte targetComponentId,byte targetNetworkId,ushort messageType, byte[] data, CancellationToken cancel)
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

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onData.Dispose();
                _sub.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_onData).ConfigureAwait(false);
            await CastAndDispose(_sub).ConfigureAwait(false);

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
