using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;

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
        public HeartbeatServer(
            IMavlinkV2Connection connection, 
            IPacketSequenceCalculator seq, 
            MavlinkIdentity identity, 
            MavlinkHeartbeatServerConfig config, 
            IScheduler? rxScheduler = null,
            ILogger? logger = null) 
            : base("HEARTBEAT", connection, identity, seq, rxScheduler,logger)
        {
            _config = config;
            _transponder = new MavlinkPacketTransponder<HeartbeatPacket,HeartbeatPayload>(connection, identity, seq)
                .DisposeItWith(Disposable);
        }

        public void Set(Action<HeartbeatPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(_config.HeartbeatRateMs));
        }
    }
}
