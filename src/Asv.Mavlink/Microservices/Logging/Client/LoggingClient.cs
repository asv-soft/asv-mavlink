using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class LoggingClient:MavlinkMicroserviceClient, ILoggingClient
    {
        private readonly RxValueBehaviour<LoggingDataPayload?> _loggingData;

        public LoggingClient(
            IMavlinkV2Connection connection, 
            MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq,
            TimeProvider? timeProvider = null,
            IScheduler? scheduler = null,
            ILoggerFactory? logFactory = null):base("LOG", connection, identity, seq, timeProvider,scheduler,logFactory)
        {
            _loggingData = new RxValueBehaviour<LoggingDataPayload?>(default).DisposeItWith(Disposable);
            InternalFilter<LoggingDataPacket>()
                .Where(p=>p.Payload.TargetSystem == identity.SystemId && p.Payload.TargetComponent == identity.ComponentId)
                .Select(p=>p.Payload)
                .Subscribe(_loggingData).DisposeItWith(Disposable);
        }

        public IRxValue<LoggingDataPayload?> RawLoggingData => _loggingData;
    }
}
