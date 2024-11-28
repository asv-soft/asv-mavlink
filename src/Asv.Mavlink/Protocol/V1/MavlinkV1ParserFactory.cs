using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink;

public class MavlinkV1MessageFactory : IProtocolMessageFactory<MavlinkMessage, ushort>
{
    public static MavlinkV1MessageFactory Instance { get; } = new();

    private MavlinkV1MessageFactory()
    {
        
    }
    
    public MavlinkMessage? Create(ushort id)
    {
        return null;
    }

    public IEnumerable<ushort> GetSupportedIds()
    {
        yield break;
    }

    public ProtocolInfo Info => MavlinkV2Protocol.Info;
}