using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink
{
    public class DgpsClient : MavlinkMicroserviceClient, IDgpsClient
    {
        private readonly int _maxMessageLength = new GpsRtcmDataPayload().Data.Length;
        private readonly ILogger _logger;

        // 5-битный счетчик последовательности; используем int для Interlocked
        private uint _seqCounter;

        public DgpsClient(MavlinkClientIdentity identity, IMavlinkContext core)
            : base(DgpsMixin.MicroserviceTypeName, identity, core)
        {
            _logger = core.LoggerFactory.CreateLogger<DgpsClient>();
        }

        public async Task SendRtcmData(byte[] data, int length, CancellationToken cancel)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));
            if (length < 0 || length > data.Length)
                throw new ArgumentOutOfRangeException(nameof(length), "Invalid RTCM length");

            // Протокольный предел: максимум 4 фрагмента на одно сообщение
            if (length > _maxMessageLength * 4)
            {
                _logger.ZLogError($"RTCM message for DGPS is too large: {length}");
                return;
            }

            // Округление вверх
            var pktCount = (length + _maxMessageLength - 1) / _maxMessageLength;
            if (pktCount == 0) pktCount = 1;          // пустое сообщение — отправим 0 длины
            if (pktCount > 4) pktCount = 4;           // жесткий лимит на фрагменты

            for (var i = 0; i < pktCount; i++)
            {
                cancel.ThrowIfCancellationRequested();

                var offset = i * _maxMessageLength;
                var dataLength = Math.Min(length - offset, _maxMessageLength);
                if (dataLength < 0) dataLength = 0;

                await InternalSend<GpsRtcmDataPacket>(pkt =>
                {
                    // Сборка флагов
                    byte flags = 0;
                    if (pktCount > 1) flags |= 0b00000001;               // fragmented
                    flags |= (byte)((i & 0x3) << 1);                      // fragment id (2 бита)

                    var seq = Interlocked.Increment(ref _seqCounter);     // потокобезопасно
                    seq %= 31;                                       // 5 бит
                    flags |= (byte)((seq & 0x1F) << 3);                   // sequence id (5 бит)

                    pkt.Payload.Flags = flags;

                    if (dataLength > 0)
                        Array.Copy(data, offset, pkt.Payload.Data, 0, dataLength);

                    pkt.Payload.Len = (byte)dataLength;
                }, cancel).ConfigureAwait(false);
            }
        }
    }
}
