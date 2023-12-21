using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAirTalk;

namespace Asv.Mavlink;

public class AirTalkClient:MavlinkMicroserviceClient, IAirTalkClient
{
    public AirTalkClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq) : base("AIR_TALK", connection, identity, seq)
    {
        
    }


    public Task SendAudioStream(Action<AsvAirTalkAudioStreamPacket> packet, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }
}