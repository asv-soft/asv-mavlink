using System;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    public class LoggingClient:MavlinkMicroserviceClient, ILoggingClient
    {
        private readonly ReactiveProperty<LoggingDataPayload> _loggingData;
        private readonly IDisposable _filter;

        public LoggingClient(MavlinkClientIdentity identity, ICoreServices core):base("LOG", identity, core)
        {
            _loggingData = new ReactiveProperty<LoggingDataPayload>();
            _filter = InternalFilter<LoggingDataPacket>()
                .Where(p => p.Payload.TargetSystem == identity.Self.SystemId &&
                            p.Payload.TargetComponent == identity.Self.ComponentId)
                .Select(p => p.Payload)
                .Subscribe(_loggingData.AsObserver());
            
        }

        public ReadOnlyReactiveProperty<LoggingDataPayload> RawLoggingData => _loggingData;

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _loggingData.Dispose();
                _filter.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_loggingData).ConfigureAwait(false);
            await CastAndDispose(_filter).ConfigureAwait(false);

            await base.DisposeAsyncCore().ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }

        #endregion

    }
    
    
}
