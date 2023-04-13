using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;

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

        public AsvGbsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,MavlinkServerIdentity identity,AsvGbsServerConfig config, IScheduler rxScheduler) 
            : base(connection,identity,seq,"GBS", rxScheduler)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _transponder =
                new MavlinkPacketTransponder<AsvGbsOutStatusPacket, AsvGbsOutStatusPayload>(connection, identity, seq)
                    .DisposeItWith(Disposable);
        }

        public void Start()
        {
            _transponder.Start(TimeSpan.FromMilliseconds(_config.StatusRateMs));
        }

        public void Set(Action<AsvGbsOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }
    }
}