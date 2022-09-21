using System.IO;

namespace Asv.Mavlink.Shell
{
    public class MavlinkGenerator
    {
        public static MavlinkProtocolModel ParseXml(string fileName, Stream strm)
        {
            return new MavlinkParserXml().Parse(fileName,strm);
        }
    }
}
