using Asv.IO;

namespace Asv.Mavlink;

public partial class MavlinkV2MessageFactory : IProtocolMessageFactory<MavlinkMessage, ushort>
{
    public static MavlinkV2MessageFactory Instance { get; } = new();

    private MavlinkV2MessageFactory()
    {
        
    }

    
    public MavlinkMessage? Create(ushort id)
    {
        return null;
    }
    
    public ProtocolInfo Info => MavlinkV2Protocol.Info;
}