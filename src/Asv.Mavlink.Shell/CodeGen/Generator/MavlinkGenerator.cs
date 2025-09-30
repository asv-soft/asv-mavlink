using System.IO;

namespace Asv.Mavlink.Shell;

public class MavlinkGenerator
{
    public static MavlinkProtocolModel ParseXml(string fileName, Stream stream)
    {
        return new MavlinkParserXml().Parse(fileName, stream);
    }
}