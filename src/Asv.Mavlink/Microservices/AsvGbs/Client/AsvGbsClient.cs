using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink
{
    public class AsvGbsClient:MavlinkMicroserviceClient, IAsvGbsClient
    {
        private readonly ReactiveProperty<AsvGbsOutStatusPayload> _status;
        private readonly IDisposable _statusSubscribe;

        public AsvGbsClient(MavlinkClientIdentity identity,ICoreServices core) 
            : base("GBS", identity, core)
        {
            _status = new ReactiveProperty<AsvGbsOutStatusPayload>(new AsvGbsOutStatusPayload());
            _statusSubscribe = InternalFilter<AsvGbsOutStatusPacket>().Select(p=>p.Payload).Subscribe(_status);
        }
        public ReadOnlyReactiveProperty<AsvGbsOutStatusPayload> RawStatus => _status;

        public override void Dispose()
        {
            _statusSubscribe.Dispose();
            _status.Dispose();
            base.Dispose();
        }
    }
    
    
}