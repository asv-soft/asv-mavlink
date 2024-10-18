using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{

    public class AsvGbsServerConfig
    {
        public int StatusRateMs { get; set; } = 1000;
    }

    public class AsvGbsServer:MavlinkMicroserviceServer, IAsvGbsServer
    {
        private readonly AsvGbsServerConfig _config;
        private readonly MavlinkPacketTransponder<AsvGbsOutStatusPacket,AsvGbsOutStatusPayload> _transponder;

        public AsvGbsServer(
            IMavlinkV2Connection connection, 
            IPacketSequenceCalculator seq,
            MavlinkIdentity identity,
            AsvGbsServerConfig config, 
            TimeProvider? timeProvider = null,
            IScheduler? rxScheduler = null,
            ILoggerFactory? logFactory = null)
            : base("GBS", connection, identity, seq, timeProvider, rxScheduler,logFactory)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _transponder =
                new MavlinkPacketTransponder<AsvGbsOutStatusPacket, AsvGbsOutStatusPayload>(connection, identity, seq,timeProvider,logFactory)
                    .DisposeItWith(Disposable);
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(500),TimeSpan.FromMilliseconds(_config.StatusRateMs));
        }

        public void Set(Action<AsvGbsOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

        public Task SendDgps(Action<GpsRtcmDataPacket> changeCallback, CancellationToken cancel = default)
        {
            return InternalSend(changeCallback, cancel);
        }
    }
}