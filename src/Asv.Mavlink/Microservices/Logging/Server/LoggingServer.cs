using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    public interface ILoggingServer:IDisposable
    {
        int MaxDataLength { get; }
        Task SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset, byte[] data);
    }

    public class LoggingServer: MavlinkMicroserviceServer, ILoggingServer
    {
        private static readonly int _maxDataLength = new LoggingDataPayload().Data.Length;

        public LoggingServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity, IScheduler scheduler) : base("LOG",connection,identity, seq,scheduler)
        {
        }

        public int MaxDataLength => _maxDataLength;

        public Task SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset, byte[] data)
        {
            if (data.Length > _maxDataLength)
            {
                throw new ArgumentException($"Data is too long (max size {_maxDataLength})", nameof(data));
            }
            return InternalSend<LoggingDataPacket>(packet =>
            {
                packet.Payload.TargetComponent = targetComponentId;
                packet.Payload.TargetSystem = targetSystemId;
                packet.Payload.FirstMessageOffset = firstMessageOffset;
                packet.Payload.Length = (byte)data.Length;
                packet.Payload.Sequence = seq;
                data.CopyTo(packet.Payload.Data, 0);
            });
        }

    }
}
