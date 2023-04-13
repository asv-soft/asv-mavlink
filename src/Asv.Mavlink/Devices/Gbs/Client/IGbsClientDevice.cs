using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IGbsClientDevice
{
    IHeartbeatClient Heartbeat { get; }
    ICommandClient Command { get; }
    IAsvGbsExClient Gbs { get; }
}