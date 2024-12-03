using System;
using System.Threading.Tasks;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink
{
    public class MavlinkHeartbeatServerConfig
    {
        public int HeartbeatRateMs { get; set; } = 1000;
    }

    public class HeartbeatServer(MavlinkIdentity identity, MavlinkHeartbeatServerConfig config, ICoreServices core)
        : MavlinkMicroserviceServer(Heartbeat.MicroserviceName, identity, core), IHeartbeatServer
    {
        private readonly MavlinkPacketTransponder<HeartbeatPacket> _transponder = new(identity, core);

        public void Set(Action<HeartbeatPayload> changeCallback)
        {
            _transponder.Set(x=>changeCallback(x.Payload));
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(config.HeartbeatRateMs), TimeSpan.FromMilliseconds(config.HeartbeatRateMs));
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transponder.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            if (_transponder is IAsyncDisposable transponderAsyncDisposable)
                await transponderAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                _transponder.Dispose();

            await base.DisposeAsyncCore().ConfigureAwait(false);
        }

        #endregion
    }
}
