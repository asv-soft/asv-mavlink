using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink
{
    public class V2ExtensionServer : MavlinkMicroserviceServer, IV2ExtensionServer
    {
        

        private readonly ReactiveProperty<V2ExtensionPacket> _onData;
        private readonly IDisposable _sub1;

        public V2ExtensionServer(MavlinkIdentity identity,ICoreServices core )
        :base("V2EXT",identity,core)
        {
            _onData = new ReactiveProperty<V2ExtensionPacket>();
            _sub1 = core.Connection.Filter<V2ExtensionPacket>()
                .Where(identity,(p,i) =>
                    (p.Payload.TargetSystem == 0 || p.Payload.TargetSystem == i.SystemId) &&
                    (p.Payload.TargetComponent == 0 || p.Payload.TargetComponent == i.ComponentId))
                .Subscribe(_onData.AsObserver());
        }

        public ReadOnlyReactiveProperty<V2ExtensionPacket> OnData => _onData;

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

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onData.Dispose();
                _sub1.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_onData).ConfigureAwait(false);
            await CastAndDispose(_sub1).ConfigureAwait(false);

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
