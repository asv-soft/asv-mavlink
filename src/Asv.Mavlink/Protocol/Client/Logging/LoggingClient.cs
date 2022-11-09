using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public class LoggingClient:MavlinkMicroserviceClient, ILoggingClient
    {
        private readonly CancellationTokenSource _disposeCancel = new();
        private readonly RxValue<LoggingDataPayload> _loggingData = new();

        public LoggingClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler scheduler):base(connection,identity,seq,"LOG", scheduler)
        {

            Filter<LoggingDataPacket>()
                .Where(_=>_.Payload.TargetSystem == identity.SystemId && _.Payload.TargetComponent == identity.ComponentId)
                .Select(_=>_.Payload)
                .Subscribe(_loggingData, _disposeCancel.Token);
            Disposable.Add(_loggingData);
        }

        public IRxValue<LoggingDataPayload> RawLoggingData => _loggingData;
    }
}
