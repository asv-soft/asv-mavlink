namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a packet in the V2 protocol.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    public interface IPacketV2<out TPayload>:IPacket<TPayload>
        where TPayload:IPayload
    {
        /// <summary>
        /// Gets or sets the value of the Tag property.
        /// </summary>
        /// <value>
        /// The value of the Tag property.
        /// </value>
        object Tag { get; set; }

        /// <summary>
        /// Calculates the CrcEtra value.
        /// </summary>
        /// <returns>
        /// The calculated CrcEtra value as a byte.
        /// </returns>
        byte GetCrcEtra();

        /// <summary>
        /// Gets or sets the flags that must be understood.
        /// </summary>
        /// <remarks>
        /// These flags provide information that must be understood by the application.
        /// </remarks>
        byte IncompatFlags { get; set; }

        /// <summary>
        /// Gets or sets the flags that can be ignored if not understood.
        /// </summary>
        /// <value>
        /// A <see cref="byte"/> value representing the flags.
        /// </value>
        byte CompatFlags { get; set; }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        ISignature Signature { get; }

        /// <summary>
        /// Gets the name of the message.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Devices on the network cannot understand unknown messages ( cause https://github.com/mavlink/mavlink/issues/1166 ) and will not forward them.
        /// This flag indicates that the message is not standard and we need to wrap this message into a V2_EXTENSION message for the routers to successfully transmit over the network.
        /// </summary>
        bool WrapToV2Extension { get; }
        
    }
}
