using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    
    public class OffboardClient : MavlinkMicroserviceClient, IOffboardClient
    {
        public OffboardClient(IMavlinkV2Connection connection, MavlinkClientIdentity config,
            IPacketSequenceCalculator seq):base("OFFBOARD", connection, config, seq)
        {
        }

        public Task SetPositionTargetLocalNed(uint timeBootMs, MavFrame coordinateFrame, PositionTargetTypemask typeMask, float x,
            float y, float z, float vx, float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate,
            CancellationToken cancel)
        {
            return InternalSend<SetPositionTargetLocalNedPacket>(_ =>
            {
                _.Payload.TimeBootMs = timeBootMs;
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.CoordinateFrame = coordinateFrame;
                _.Payload.TypeMask = typeMask;
                _.Payload.X = x;
                _.Payload.Y = y;
                _.Payload.Z = z;
                _.Payload.Vx = vx;
                _.Payload.Vy = vy;
                _.Payload.Vz = vz;
                _.Payload.Afx = afx;
                _.Payload.Afy = afy;
                _.Payload.Afz = afz;
                _.Payload.Yaw = yaw;
                _.Payload.YawRate = yawRate;
            }, cancel);
        }
        
    }
}
