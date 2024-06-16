using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IRfsaClientDevice:IClientDevice
{
    IParamsClientEx Params { get; }
    IAsvChartClient Charts { get; }
    ICommandClient Command { get; }
    IDiagnosticClient Diagnostic { get; }
    Task<MavResult> Enable(ulong frequencyHz,uint spanHz, CancellationToken cancel = default);
    Task<MavResult> Disable(CancellationToken cancel = default);
}