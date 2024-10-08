﻿using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;

namespace Asv.Mavlink
{
    public class AsvGbsClient:MavlinkMicroserviceClient, IAsvGbsClient
    {
        private readonly RxValue<AsvGbsOutStatusPayload> _status;
        public AsvGbsClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler scheduler) 
            : base("GBS", connection, identity, seq)
        {
            _status = new RxValue<AsvGbsOutStatusPayload>().DisposeItWith(Disposable);
            InternalFilter<AsvGbsOutStatusPacket>().Select(p=>p.Payload).Subscribe(_status).DisposeItWith(Disposable);
        }
        public IRxValue<AsvGbsOutStatusPayload> RawStatus => _status;
    }
}