using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public delegate Task<MavResult> RsgaSetMode(AsvRsgaCustomMode mode,float param2 = float.NaN, float param3 = float.NaN, float param4 = float.NaN, float param5 = float.NaN, float param6 = float.NaN, float param7 = float.NaN, CancellationToken cancel = default);
public delegate IEnumerable<AsvRsgaCustomMode> RsgaGetCompatibility();

public interface IAsvRsgaServerEx
{
    IAsvRsgaServer Base { get; }
    RsgaSetMode? SetMode { get; set; }
    RsgaGetCompatibility? GetCompatibility { get; set; }
}