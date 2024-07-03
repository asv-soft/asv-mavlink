using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;



public interface IRsgaClientDevice
{
    IParamsClientEx Params { get; }
    IAsvChartClient Charts { get; }
    ICommandClient Command { get; }
    IDiagnosticClient Diagnostic { get; }
    IAsvRsgaClientEx Rsga { get; }
    
}

