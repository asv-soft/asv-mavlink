using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public delegate Task<MavResult> RsgaSetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default);
public delegate IEnumerable<AsvRsgaCustomMode> RsgaGetCompatibility();

public interface IAsvRsgaServerEx
{
    IAsvRsgaServer Base { get; }
    RsgaSetMode? SetMode { get; set; }
    RsgaGetCompatibility? GetCompatibility { get; set; }
}