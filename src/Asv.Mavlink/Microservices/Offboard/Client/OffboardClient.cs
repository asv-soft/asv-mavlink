using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    
    public class OffboardClient : MavlinkMicroserviceClient, IOffboardClient
    {
        public OffboardClient(
            IMavlinkV2Connection connection, 
            MavlinkClientIdentity config,
            IPacketSequenceCalculator seq,
            TimeProvider? timeProvider = null,
            IScheduler? rxScheduler = null,
            ILoggerFactory? logFactory = null)
            :base("OFFBOARD", connection, config, seq,timeProvider,rxScheduler,logFactory)
        {
        }

        public Task SetPositionTargetLocalNed(uint timeBootMs, MavFrame coordinateFrame, PositionTargetTypemask typeMask, float x,
            float y, float z, float vx, float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate,
            CancellationToken cancel)
        {
            return InternalSend<SetPositionTargetLocalNedPacket>(p =>
            {
                p.Payload.TimeBootMs = timeBootMs;
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                p.Payload.CoordinateFrame = coordinateFrame;
                p.Payload.TypeMask = typeMask;
                p.Payload.X = x;
                p.Payload.Y = y;
                p.Payload.Z = z;
                p.Payload.Vx = vx;
                p.Payload.Vy = vy;
                p.Payload.Vz = vz;
                p.Payload.Afx = afx;
                p.Payload.Afy = afy;
                p.Payload.Afz = afz;
                p.Payload.Yaw = yaw;
                p.Payload.YawRate = yawRate;
            }, cancel);
        }
        
    }
}
