using Asv.Cfg;
using Asv.Mavlink.Common;
using Asv.Mavlink.Diagnostic.Server;

namespace Asv.Mavlink.Diagnostic;

public static class DiagnosticHelper
{
    public const string MicroserviceTypeName = "DIAG";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterDiagnostic(this IServerDeviceBuilder builder)
    {
        builder.Register<IDiagnosticServer>((identity, context,config) => new DiagnosticServer(identity, config.Get<DiagnosticServerConfig>(),  context));
        return builder;
    }
   
    public static IServerDeviceBuilder RegisterDiagnostic(this IServerDeviceBuilder builder, DiagnosticServerConfig config)
    {
        builder
            .Register<IDiagnosticServer>((identity, context,_) =>  new DiagnosticServer(identity,config,context));
        return builder;
    }

    public static IDiagnosticServer GetDiagnostic(this IServerDevice factory) 
        => factory.Get<IDiagnosticServer>();

    #endregion
}