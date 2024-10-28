using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class ParamsServer: MavlinkMicroserviceServer, IParamsServer
    {
        public ParamsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkIdentity identity,
            TimeProvider? timeProvider = null,
            IScheduler? scheduler = null,
            ILoggerFactory? logFactory = null)
            : base("PARAM",connection, identity, seq, timeProvider, scheduler,logFactory)
        {
            OnParamRequestRead =
                InternalFilter<ParamRequestReadPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);
            OnParamRequestList =
                InternalFilter<ParamRequestListPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);
            OnParamSet =
                InternalFilter<ParamSetPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

        }
        
        public Task SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default)
        {
            return InternalSend<ParamValuePacket>(p=>changeCallback(p.Payload), cancel);
        }

        public IObservable<ParamRequestReadPacket> OnParamRequestRead { get; }
        public IObservable<ParamRequestListPacket> OnParamRequestList { get; }
        public IObservable<ParamSetPacket> OnParamSet { get; }
        
    }
}
