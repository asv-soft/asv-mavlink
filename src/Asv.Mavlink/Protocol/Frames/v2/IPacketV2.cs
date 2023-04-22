namespace Asv.Mavlink
{
    public interface IPacketV2<out TPayload>:IPacket<TPayload>
        where TPayload:IPayload
    {
        object Tag { get; set; }
        /// <summary>
        /// CrcEtra
        /// </summary>
        byte GetCrcEtra();
        /// <summary>
        /// flags that must be understood
        /// </summary>
        byte IncompatFlags { get; set; }
        /// <summary>
        /// flags that can be ignored if not understood
        /// </summary>
        byte CompatFlags { get; set; }
        /// <summary>
        /// Signature, optional
        /// </summary>
        ISignature Signature { get; }
        /// <summary>
        /// Message name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Devices in network could not understand unknown messages (cause https://github.com/mavlink/mavlink/issues/1166) and will not forwarding it.
        /// This flag indicates that the message is not standard and we need to wrap this message into a V2_EXTENSION message for the routers to successfully transmit over the network.
        /// </summary>
        bool WrapToV2Extension { get; }
        
    }
}
