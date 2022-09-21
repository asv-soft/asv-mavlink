namespace Asv.Mavlink
{
    public interface IPacketV2<out TPayload>:IPacket<TPayload>
        where TPayload:IPayload
    {
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

        
    }
}
