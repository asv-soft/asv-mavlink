using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAirTalk;

namespace Asv.Mavlink;

public interface IAirTalkClient
{
    Task SendAudioStream(Action<AsvAirTalkAudioStreamPacket> packet, CancellationToken cancel = default);
}