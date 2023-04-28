using Asv.IO;

namespace Asv.Mavlink
{
    public interface IPayload:ISpanSerializable
    {
        /// <summary>
        /// Maximum size of payload
        /// </summary>
        byte GetMaxByteSize();
        // <summary>
        /// Minimum size of payload
        /// </summary>
        byte GetMinByteSize();
        /// <summary>
        /// Calculate current size of payload by sum of all fields (include arrays)
        /// </summary>
        /// <returns></returns>
        byte GetCurrentByteSize();
    }
}
