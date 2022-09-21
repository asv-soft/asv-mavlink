using System.IO;

namespace Asv.Mavlink.Shell
{
    public interface IMavlinkParser
    {
        MavlinkProtocolModel Parse(string fileName, Stream data);
    }
}
