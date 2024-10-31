using System;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink
{
    public class LoggingClient:MavlinkMicroserviceClient, ILoggingClient
    {
        private readonly ReactiveProperty<LoggingDataPayload?> _loggingData;
        private readonly IDisposable _filter;

        public LoggingClient(MavlinkClientIdentity identity, ICoreServices core):base("LOG", identity, core)
        {
            _loggingData = new ReactiveProperty<LoggingDataPayload?>(default);
            _filter = InternalFilter<LoggingDataPacket>()
                .Where(p => p.Payload.TargetSystem == identity.Self.SystemId &&
                            p.Payload.TargetComponent == identity.Self.ComponentId)
                .Select(p => p.Payload)
                .Subscribe(_loggingData);
            
        }

        public ReadOnlyReactiveProperty<LoggingDataPayload?> RawLoggingData => _loggingData;

        public override void Dispose()
        {
            _loggingData.Dispose();
            _filter.Dispose();
            base.Dispose();
        }
    }
    
    
}
