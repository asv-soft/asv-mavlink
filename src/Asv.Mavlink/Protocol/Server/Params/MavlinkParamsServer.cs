using System;
using System.Threading;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    public interface IMavlinkParamsServer
    {
        
    }

    public class MavlinkParamsServer: MavlinkMicroserviceBase, IMavlinkParamsServer
    {
        private int _isSending;

        public MavlinkParamsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq, MavlinkServerIdentity identity)
            : base(connection, seq, identity)
        {
            Subscribe<ParamRequestListPacket, ParamRequestListPayload>(OnRequestList);
            
        }

        private void OnRequestList(ParamRequestListPacket obj)
        {
            if (Interlocked.CompareExchange(ref _isSending, 1, 0) != 0) return;
            try
            {

            }
            catch (Exception)
            {

            }
            finally
            {
                Interlocked.Exchange(ref _isSending, 0);
            }
        }



    }
}
