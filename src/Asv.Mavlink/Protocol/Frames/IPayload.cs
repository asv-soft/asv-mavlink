using Asv.IO;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents an interface for a payload object.
    /// </summary>
    public interface IPayload : ISizedSpanSerializable
    {
        /// <summary>
        /// Maximum size of payload
        /// </summary>
        byte GetMaxByteSize();

        /// <summary>
        /// Returns the minimum size of the payload.
        /// </summary>
        /// <returns>The minimum size of the payload.</returns>
        byte GetMinByteSize();
    }
}
