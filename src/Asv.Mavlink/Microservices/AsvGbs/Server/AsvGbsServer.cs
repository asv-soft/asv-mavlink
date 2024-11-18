using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class AsvGbsServerConfig
    {
        public uint StatusRateMs { get; set; } = 1000;
    }

    public class AsvGbsServer:MavlinkMicroserviceServer, IAsvGbsServer
    {
        private readonly AsvGbsServerConfig _config;
        private readonly MavlinkPacketTransponder<AsvGbsOutStatusPacket,AsvGbsOutStatusPayload> _transponder;

        public AsvGbsServer(
            MavlinkIdentity identity,
            AsvGbsServerConfig config, 
            ICoreServices core)
            : base("GBS", identity, core)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _transponder =
                new MavlinkPacketTransponder<AsvGbsOutStatusPacket, AsvGbsOutStatusPayload>(identity, core);
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(_config.StatusRateMs),TimeSpan.FromMilliseconds(_config.StatusRateMs));
        }

        public void Set(Action<AsvGbsOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

        public Task SendDgps(Action<GpsRtcmDataPacket> changeCallback, CancellationToken cancel = default)
        {
            return InternalSend(changeCallback, cancel);
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