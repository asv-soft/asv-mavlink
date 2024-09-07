using Asv.Mavlink.Diagnostic.Client;

namespace Asv.Mavlink;



public interface IRsgaClientDevice:IClientDevice
{
    IParamsClientEx Params { get; }
    IAsvChartClient Charts { get; }
    ICommandClient Command { get; }
    IDiagnosticClient Diagnostic { get; }
    IAsvRsgaClientEx Rsga { get; }
    
}

