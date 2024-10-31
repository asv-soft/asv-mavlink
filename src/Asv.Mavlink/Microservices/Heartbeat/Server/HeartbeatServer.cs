using System;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink
{
    public class MavlinkHeartbeatServerConfig
    {
        public int HeartbeatRateMs { get; set; } = 1000;
    }

    public class HeartbeatServer: MavlinkMicroserviceServer,IHeartbeatServer
    {
        private readonly MavlinkHeartbeatServerConfig _config;
        private readonly MavlinkPacketTransponder<HeartbeatPacket, HeartbeatPayload> _transponder;
        public HeartbeatServer(MavlinkIdentity identity, MavlinkHeartbeatServerConfig config, ICoreServices core) 
            : base("HEARTBEAT", identity, core)
        {
            _config = config;
            _transponder = new MavlinkPacketTransponder<HeartbeatPacket, HeartbeatPayload>(identity, core);

        }

        public void Set(Action<HeartbeatPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(_config.HeartbeatRateMs));
        }

        public override void Dispose()
        {
            _transponder.Dispose();
            base.Dispose();
        }
    }
}
