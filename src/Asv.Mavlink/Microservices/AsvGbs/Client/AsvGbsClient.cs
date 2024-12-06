using System;
using System.Threading.Tasks;
using Asv.Mavlink.AsvGbs;
using R3;

namespace Asv.Mavlink
{
    public class AsvGbsClient:MavlinkMicroserviceClient, IAsvGbsClient
    {
        public AsvGbsClient(MavlinkClientIdentity identity,IMavlinkContext core) 
            : base("GBS", identity, core)
        {
            RawStatus = InternalFilter<AsvGbsOutStatusPacket>().Select(p => p?.Payload)
                .ToReadOnlyReactiveProperty();
        }
        public ReadOnlyReactiveProperty<AsvGbsOutStatusPayload?> RawStatus { get; }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RawStatus.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            if (RawStatus is IAsyncDisposable rawStatusAsyncDisposable)
                await rawStatusAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                RawStatus.Dispose();

            await base.DisposeAsyncCore().ConfigureAwait(false);
        }

        #endregion
    }
}