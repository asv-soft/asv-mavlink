using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    public interface ILoggingServer:IDisposable
    {
        int MaxDataLength { get; }
        Task SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset, byte[] data);
    }

    public class LoggingServer: ILoggingServer
    {
        private static readonly int _maxDataLength = new LoggingDataPayload().Data.Length;
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkServerIdentity _identity;
        private readonly CancellationTokenSource _disposableCancel = new CancellationTokenSource();

        public LoggingServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (seq == null) throw new ArgumentNullException(nameof(seq));

            _connection = connection;
            _seq = seq;
            _identity = identity;
        }

        public int MaxDataLength => _maxDataLength;

        public Task SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset, byte[] data)
        {
            if (data.Length > _maxDataLength)
            {
                throw new ArgumentException($"Data is too long (max size {_maxDataLength})", nameof(data));
            }

            var packet = new LoggingDataPacket
            {
                ComponentId = _identity.ComponentId,
                SystemId = _identity.SystemId,
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = _seq.GetNextSequenceNumber(),
                Payload =
                {
                    TargetComponent = targetComponentId,
                    TargetSystem = targetSystemId,
                    FirstMessageOffset = firstMessageOffset,
                    Length = (byte) data.Length,
                    Sequence = seq,
                    Data = data
                }
            };
            //data.CopyTo(packet.Payload.Data, 0);
            return _connection.Send(packet, _disposableCancel.Token);
        }

        public void Dispose()
        {
            _disposableCancel?.Cancel(false);
            _disposableCancel?.Dispose();
        }
    }
}
