using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public static class IpBasedSystemIdMapperFeatureHelper
{
    public static void RegisterMavlinkIpBasedIdMapperFeature(this IProtocolFeatureBuilder builder, IdConverterDelegate idConverter)
    {
        builder.Register(new MavlinkIpBasedIdMapperFeature(idConverter));
    }
}

public delegate MavlinkIdentity IdConverterDelegate(IPEndPoint endPoint, MavlinkIdentity originalId);

public class MavlinkIpBasedIdMapperFeature(IdConverterDelegate idConverter) : IProtocolFeature
{
    public const string ToTagKey = "mavlink_id_to";
    public const string FromTagKey = "mavlink_id_from";
    
    public const string IdKey = nameof(MavlinkIpBasedIdMapperFeature);
    public string Name => $"Mavlink {nameof(MavlinkIpBasedIdMapperFeature)}";
    public string Description => $"A protocol feature that remaps the SystemId and ComponentId of MAVLink packets based on the IP address of the incoming port (Endpoint). " +
                                 $"This allows multiple drones with identical IDs to be distinguished by assigning unique identifiers derived from their connection source.";
    public string Id => IdKey;
    public int Priority => 0;
    
    public ValueTask<IProtocolMessage?> ProcessRx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        // only mavlink v2 messages are supported
        if (message is not MavlinkV2Message msg)
        {
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        IPEndPoint? endPoint = null;
        // if the connection is a UDP socket, we can get the remote endpoint directly
        if (connection is UdpSocketProtocolEndpoint udp)
        {
            endPoint = udp.RemoteEndPoint;
        }
        // if the connection is a TCP server socket, we can get the remote endpoint from the socket
        if (connection is TcpServerSocketProtocolEndpoint tcp)
        {
            endPoint = tcp.Socket.RemoteEndPoint as IPEndPoint;
        }

        if (endPoint == null)
        {
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        
        // use the id converter to get the new ID based on the endpoint`
        var toIdentity = idConverter(endPoint, msg.FullId);
        connection.Tags[FromTagKey] = msg.FullId.FullId;
        connection.Tags[ToTagKey] = toIdentity.FullId;
        // convert the ID to the new one
        msg.SystemId = toIdentity.SystemId;
        msg.ComponentId = toIdentity.ComponentId;

        return ValueTask.FromResult<IProtocolMessage?>(msg);
    }

    public ValueTask<IProtocolMessage?> ProcessTx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        // this feature is only for mavlink v2 messages
        if (message is not MavlinkV2Message msg)
        {
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        
        // this feature is only for endpoints that
        if (connection is not IProtocolEndpoint)
        {
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        
        // we need to modify the message only if it has target ID
        if (msg.TryGetTargetId(out var toSystemId, out var toComponentId) == false)
        {
            // this is not a mavlink message with target ID, so we can skip it
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        
        // now we need to check if the endpoint has tags for FromTagKey and ToTagKey
        var toId = connection.Tags[ToTagKey];
        if (toId is not ushort toFullId)
        {
            // if the ToTagKey is not set, we can skip the message
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        
        var toIdentity = new MavlinkIdentity(toFullId);
        if (toIdentity.SystemId != toSystemId || toIdentity.ComponentId != toComponentId)
        {
            // if the target ID is different from the one in the message, we mustn't send it
            return ValueTask.FromResult<IProtocolMessage?>(null);       
        }
        // ok, we have the target ID and we need to convert it to the original ID
        var fromId = connection.Tags[FromTagKey];
        if (fromId is not ushort fromFullId)
        {
            // if the target ID is not the same as the one in the message, we can skip the message
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }
        var fromIdentity = new MavlinkIdentity(fromFullId);
        msg.TrySetTargetId(fromIdentity.SystemId, fromIdentity.ComponentId);
        // we have converted the target ID to the original one, so we can return the message
        return ValueTask.FromResult<IProtocolMessage?>(msg);
    }

    public void Register(IProtocolConnection connection)
    {
        
    }

    public void Unregister(IProtocolConnection connection)
    {
        
    }

}