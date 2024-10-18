using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class AsvGbsClient:MavlinkMicroserviceClient, IAsvGbsClient
    {
        private readonly RxValueBehaviour<AsvGbsOutStatusPayload> _status;
        public AsvGbsClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, TimeProvider? timeProvider = null, IScheduler? scheduler = null,ILoggerFactory? logFactory = null) 
            : base("GBS", connection, identity, seq,timeProvider,scheduler,logFactory)
        {
            _status = new RxValueBehaviour<AsvGbsOutStatusPayload>(new AsvGbsOutStatusPayload()).DisposeItWith(Disposable);
            InternalFilter<AsvGbsOutStatusPacket>().Select(p=>p.Payload).Subscribe(_status).DisposeItWith(Disposable);
        }
        public IRxValue<AsvGbsOutStatusPayload> RawStatus => _status;
    }
}