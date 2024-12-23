using System;
using System.Threading.Tasks;
using Asv.Mavlink.Common;


namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a logging server that sends logging data through a Mavlink connection.
    /// </summary>
    public class LoggingServer(MavlinkIdentity identity, IMavlinkContext core)
        : MavlinkMicroserviceServer(LoggingHelper.MicroserviceName, identity, core), ILoggingServer
    {
        /// <summary>
        /// Maximum length of the data payload for logging.
        /// </summary>
        private static readonly int _maxDataLength = new LoggingDataPayload().Data.Length;


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
        public ValueTask SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset, byte[] data)
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
