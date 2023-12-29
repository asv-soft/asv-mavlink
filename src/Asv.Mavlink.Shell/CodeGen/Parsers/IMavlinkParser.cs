using System.IO;

namespace Asv.Mavlink.Shell
{
    /// <summary>
    /// Represents a MAVLink parser. This interface provides methods to parse MAVLink protocol messages.
    /// </summary>
    public interface IMavlinkParser
    {
        /// <summary>
        /// Parses the Mavlink protocol model from the given file and stream.
        /// </summary>
        /// <param name="fileName">The name of the file to parse.</param>
        /// <param name="data">The stream containing the protocol data.</param>
        /// <returns>The parsed Mavlink protocol model.</returns>
        MavlinkProtocolModel Parse(string fileName, Stream data);
    }
}
