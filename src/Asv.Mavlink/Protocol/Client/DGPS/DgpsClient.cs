using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Client
{
    public class DgpsClient : MavlinkMicroserviceClient, IDgpsClient
    {

        private readonly int _maxMessageLength = new GpsRtcmDataPayload().Data.Length;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _seqNumber;

        public DgpsClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq):base(connection,identity,seq,"DGPS")
        {
        
        }

        public async Task SendRtcmData(byte[] data, int length, CancellationToken cancel)
        {
            if (length > _maxMessageLength * 4)
            {
                _logger.Error($"RTCM message for DGPS is too large '{length}'");
                return;
            }

            // number of packets we need, including a termination packet if needed
            var pktCount = length / _maxMessageLength + 1;
            if (pktCount >= 4)
            {
                pktCount = 4;
            }

            for (var i = 0; i < pktCount; i++)
            {
                await InternalSend<GpsRtcmDataPacket>(pkt =>
                {
                    // 1 means message is fragmented
                    pkt.Payload.Flags = (byte)(pktCount > 1 ? 1 : 0);
                    //  next 2 bits are the fragment ID
                    pkt.Payload.Flags += (byte)((i & 0x3) << 1);
                    // the remaining 5 bits are used for the sequence ID
                    pkt.Payload.Flags += (byte)((Interlocked.Increment(ref _seqNumber) & 0x1f) << 3);

                    var dataLength = Math.Min(length - i * _maxMessageLength, _maxMessageLength);
                    var dataArray = new byte[dataLength];
                    Array.Copy(data, i * _maxMessageLength, dataArray, 0, dataLength);
                    pkt.Payload.Data = dataArray;

                    pkt.Payload.Len = (byte)dataLength;
                }, cancel).ConfigureAwait(false);

            }

        }
    }
}
