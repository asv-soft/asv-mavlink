using System;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public class AsvSdrServer:DisposableOnceWithCancel, IAsvSdrServer
    {
        private readonly MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload> _transponder;

        public AsvSdrServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,MavlinkServerIdentity identity)
        {
            _transponder =
                new MavlinkPacketTransponder<AsvSdrOutStatusPacket, AsvSdrOutStatusPayload>(connection, identity, seq)
                    .DisposeItWith(Disposable);
        }

        public void Start(TimeSpan statusRate)
        {
            _transponder.Start(statusRate);
        }

        public Task Set(Action<AsvSdrOutStatusPayload> changeCallback)
        {
            return _transponder.Set(changeCallback);
        }
    }
}