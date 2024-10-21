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
        private readonly IDisposable _filter;

        public LoggingClient(MavlinkClientIdentity identity, ICoreServices core):base("LOG", identity, core)
        {
            _loggingData = new RxValueBehaviour<LoggingDataPayload?>(default);
            _filter = InternalFilter<LoggingDataPacket>()
                .Where(p => p.Payload.TargetSystem == identity.Self.SystemId &&
                            p.Payload.TargetComponent == identity.Self.ComponentId)
                .Select(p => p.Payload)
                .Subscribe(_loggingData);
            
        }

        public IRxValue<LoggingDataPayload?> RawLoggingData => _loggingData;

        public override void Dispose()
        {
            _loggingData.Dispose();
            _filter.Dispose();
            base.Dispose();
        }
    }
    
    
}
