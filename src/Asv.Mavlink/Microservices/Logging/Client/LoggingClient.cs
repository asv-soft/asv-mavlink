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
        private readonly CancellationTokenSource _disposeCancel = new();
        private readonly RxValue<LoggingDataPayload> _loggingData = new();

        public LoggingClient(
            IMavlinkV2Connection connection, 
            MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq,
            IScheduler? scheduler = null,
            ILogger? logger = null):base("LOG", connection, identity, seq,scheduler,logger)
        {

            InternalFilter<LoggingDataPacket>()
                .Where(p=>p.Payload.TargetSystem == identity.SystemId && p.Payload.TargetComponent == identity.ComponentId)
                .Select(p=>p.Payload)
                .Subscribe(_loggingData, _disposeCancel.Token);
            Disposable.Add(_loggingData);
        }

        public IRxValue<LoggingDataPayload> RawLoggingData => _loggingData;
    }
}
