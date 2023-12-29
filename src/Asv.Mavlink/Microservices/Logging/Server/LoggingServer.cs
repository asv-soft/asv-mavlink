using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    /// <summary>
    /// Represents a logging server that is responsible for sending logging data.
    /// </summary>
    public interface ILoggingServer:IDisposable
    {
        /// <summary>
        /// Gets the maximum data length for the property.
        /// </summary>
        /// <returns>The maximum data length as an integer.</returns>
        int MaxDataLength { get; }

        /// <summary>
        /// Sends logging data to the specified target system and component.
        /// </summary>
        /// <param name="targetSystemId">The target system ID.</param>
        /// <param name="targetComponentId">The target component ID.</param>
        /// <param name="seq">The sequence number.</param>
        /// <param name="firstMessageOffset">The offset of the first message.</param>
        /// <param name="data">The logging data to be sent.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset, byte[] data);
    }

    /// <summary>
    /// Represents a logging server that sends logging data through a Mavlink connection.
    /// </summary>
    public class LoggingServer: MavlinkMicroserviceServer, ILoggingServer
    {
        /// <summary>
        /// Maximum length of the data payload for logging.
        /// </summary>
        private static readonly int _maxDataLength = new LoggingDataPayload().Data.Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingServer"/> class.
        /// </summary>
        /// <param name="connection">The Mavlink connection.</param>
        /// <param name="seq">The packet sequence calculator.</param>
        /// <param name="identity">The server identity.</param>
        /// <param name="scheduler">The scheduler for processing incoming packets.</param>
        public LoggingServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity, IScheduler scheduler) : base("LOG",connection,identity, seq,scheduler)
        {
        }

        /// <summary>
        /// Gets the maximum length of the data.
        /// </summary>
        /// <value>
        /// The maximum length of the data.
        /// </value>
        public int MaxDataLength => _maxDataLength;

        /// Sends logging data to the target system and component.
        /// @param targetSystemId The target system ID to send the logging data to.
        /// @param targetComponentId The target component ID to send the logging data to.
        /// @param seq The sequence of the logging data.
        /// @param firstMessageOffset The offset of the first message in the data.
        /// @param data The logging data to send.
        /// @throws ArgumentException If the length of the data is greater than the maximum allowed size (_maxDataLength).
        /// @returns A Task representing the asynchronous sending of the logging data.
        /// /
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
