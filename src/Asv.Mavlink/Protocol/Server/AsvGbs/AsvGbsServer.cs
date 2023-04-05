using System;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;

namespace Asv.Mavlink
{
    

    public class AsvGbsServer:DisposableOnceWithCancel, IAsvGbsServer
    {
        private readonly MavlinkPacketTransponder<AsvGbsOutStatusPacket,AsvGbsOutStatusPayload> _transponder;

        public AsvGbsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,MavlinkServerIdentity identity)
        {
            _transponder =
                new MavlinkPacketTransponder<AsvGbsOutStatusPacket, AsvGbsOutStatusPayload>(connection, identity, seq)
                    .DisposeItWith(Disposable);
        }

        

        public void Start(TimeSpan statusRate)
        {
            _transponder.Start(statusRate);
        }

        public void Set(Action<AsvGbsOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }
    }
}