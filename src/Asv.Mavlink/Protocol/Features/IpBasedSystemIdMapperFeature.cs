using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public static class IpBasedSystemIdMapperFeatureHelper
{
    public static void RegisterMavlinkV2WrapFeature(this IProtocolFeatureBuilder builder)
    {
        builder.Register(new IpBasedSystemIdMapperFeature());
    }
}

public class IpBasedSystemIdMapperFeature() : IProtocolFeature
{
    public const string IdKey = nameof(IpBasedSystemIdMapperFeature);
    public const ushort V2ExtensionMessageId = 32535;

    public string Name => $"Mavlink {nameof(IpBasedSystemIdMapperFeature)}";
    public string Description => $"A protocol feature that remaps the SystemId and ComponentId of MAVLink packets based on the IP address of the incoming port (Endpoint). " +
                                 $"This allows multiple drones with identical IDs to be distinguished by assigning unique identifiers derived from their connection source.";
    public string Id => IdKey;
    public int Priority => 0;
    
    public ValueTask<IProtocolMessage?> ProcessRx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
       
    }

    public ValueTask<IProtocolMessage?> ProcessTx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }

    public void Register(IProtocolConnection connection)
    {
        if (connection is IProtocolEndpoint endpoint)
        {
            
        }
    }

    public void Unregister(IProtocolConnection connection)
    {
        throw new System.NotImplementedException();
    }

}