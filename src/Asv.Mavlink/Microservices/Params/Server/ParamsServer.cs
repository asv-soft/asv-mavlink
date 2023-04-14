using System;
using System.Reactive.Concurrency;
using System.Threading;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    public class ParamsServer: MavlinkMicroserviceServer, IParamsServer
    {
        private int _isSending;

        public ParamsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity, IScheduler scheduler)
            : base("PARAM",connection, identity, seq, scheduler)
        {
            // Subscribe<ParamRequestListPacket, ParamRequestListPayload>(OnRequestList);
            
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
