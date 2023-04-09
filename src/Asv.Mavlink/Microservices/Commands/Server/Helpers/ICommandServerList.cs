using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface ICommandServerList<out TArgPacket>
{
    CommandDelegate<TArgPacket> this[MavCmd cmd] { set; }
}

public delegate Task<CommandResult> CommandDelegate<in TArgPacket>(DeviceIdentity from, TArgPacket args, CancellationToken cancel);