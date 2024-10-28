using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink
{
    public class DgpsClient : MavlinkMicroserviceClient, IDgpsClient
    {

        private readonly int _maxMessageLength = new GpsRtcmDataPayload().Data.Length;
        private readonly ILogger _logger;
        private uint _seqNumber;

        public DgpsClient(
            IMavlinkV2Connection connection, 
            MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq,
            TimeProvider? timeProvider = null,
            IScheduler? scheduler = null,
            ILoggerFactory? logFactory = null)
            :base("DGPS", connection, identity, seq, timeProvider, scheduler, logFactory)
        {
            logFactory ??= NullLoggerFactory.Instance;
            _logger = logFactory.CreateLogger<DgpsClient>();
        }

        public async Task SendRtcmData(byte[] data, int length, CancellationToken cancel)
        {
            if (length > _maxMessageLength * 4)
            {
                _logger.ZLogError($"RTCM message for DGPS is too large '{length}'");
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
                var i1 = i;
                await InternalSend<GpsRtcmDataPacket>(pkt =>
                {
                    // 1 means message is fragmented
                    pkt.Payload.Flags = (byte)(pktCount > 1 ? 1 : 0);
                    //  next 2 bits are the fragment ID
                    pkt.Payload.Flags += (byte)((i1 & 0x3) << 1);
                    // the remaining 5 bits are used for the sequence ID
                    var increment = Interlocked.Increment(ref _seqNumber) % 31;
                    pkt.Payload.Flags += (byte)((increment& 0x1f) << 3);

                    var dataLength = Math.Min(length - i1 * _maxMessageLength, _maxMessageLength);
                    var dataArray = new byte[dataLength];
                    Array.Copy(data, i1 * _maxMessageLength, dataArray, 0, dataLength);
                    pkt.Payload.Data = dataArray;

                    pkt.Payload.Len = (byte)dataLength;
                }, cancel).ConfigureAwait(false);
            }

        }
    }
}
