using Asv.IO;

namespace Asv.Mavlink
{
    public interface IPacket<out TPayload>: ISpanSerializable
        where TPayload:IPayload
    {
        /// <summary>
        /// Maximum size in bytes
        /// </summary>
        int GetMaxByteSize();
        /// <summary>
        /// Protocol magic marker
        /// </summary>
        byte Magic { get; }
        /// <summary>
        /// Sequence of packet
        /// </summary>
        byte Sequence { get; set; }
        /// <summary>
        /// ID of message sender system/aircraft
        /// </summary>
        byte SystemId { get; set; }
        /// <summary>
        /// ID of the message sender component
        /// </summary>
        byte ComponentId { get; set; }
        /// <summary>
        /// Message Id
        /// </summary>
        int MessageId { get; }
        /// <summary>
        /// Message payload
        /// </summary>
        TPayload Payload { get; }
        
    }
}
