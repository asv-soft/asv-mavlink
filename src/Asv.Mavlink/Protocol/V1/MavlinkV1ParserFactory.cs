using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink;

public class MavlinkV1MessageFactory : IProtocolMessageFactory<MavlinkMessage, int>
{
    public static MavlinkV1MessageFactory Instance { get; } = new();

    private MavlinkV1MessageFactory()
    {
        
    }
    
    public MavlinkMessage? Create(int id)
    {
        return null;
    }

    public IEnumerable<int> GetSupportedIds()
    {
        yield break;
    }

    public ProtocolInfo Info => MavlinkV2Protocol.Info;
}