using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class MavlinkV2WrapFeatureHelper
{
    public static void RegisterMavlinkV2WrapFeature(this IProtocolFeatureBuilder builder,IProtocolMessageFactory<MavlinkMessage, int>? factory = null)
    {
        builder.Register(new MavlinkV2WrapFeature(factory));
    }
}

public class MavlinkV2WrapFeature(IProtocolMessageFactory<MavlinkMessage, int>? factory = null) : IProtocolFeature
{
    private readonly IProtocolMessageFactory<MavlinkMessage,int> _factory = factory ?? MavlinkV2MessageFactory.Instance;
    public const string IdKey = "MavlinkV2WrapFeature";
    public const ushort V2ExtensionMessageId = 32535;

    public string Name => $"Mavlink {nameof(V2ExtensionPacket)} wrapper";
    public string Description => $"Wrap packages to {nameof(V2ExtensionPacket)}";
    public string Id => IdKey;
    public int Priority => 0;

    public ValueTask<IProtocolMessage?> ProcessRx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        if (connection is not IProtocolPort) return ValueTask.FromResult<IProtocolMessage?>(message);
        if (message is not V2ExtensionPacket mavlink) return ValueTask.FromResult<IProtocolMessage?>(message);
        if (mavlink.Payload.MessageType != V2ExtensionMessageId) return ValueTask.FromResult<IProtocolMessage?>(message);
        var messageId = MavlinkV2Protocol.GetMessageId(mavlink.Payload.Payload,0);
        var innerMessage = _factory.Create((ushort)messageId);
        if (innerMessage == null) return ValueTask.FromResult<IProtocolMessage?>(message);
        innerMessage.Deserialize(mavlink.Payload.Payload);
        return ValueTask.FromResult<IProtocolMessage?>(innerMessage);
    }

    public ValueTask<IProtocolMessage?> ProcessTx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        if (connection is not IProtocolEndpoint && connection is not VirtualPort) return ValueTask.FromResult<IProtocolMessage?>(message);
        if (message is not MavlinkV2Message mavlink) return ValueTask.FromResult<IProtocolMessage?>(message);

        if (mavlink.WrapToV2Extension == false)
        {
            return ValueTask.FromResult<IProtocolMessage?>(message);
        }

        mavlink.TryGetTargetId(out var systemId, out var componentId);
        var wrappedPacket = new V2ExtensionPacket
        {
            IncompatFlags = mavlink.IncompatFlags,
            CompatFlags = mavlink.CompatFlags,
            Sequence = mavlink.Sequence,
            SystemId = mavlink.SystemId,
            ComponentId = mavlink.ComponentId,
            Payload =
            {
                // broadcast
                TargetComponent = componentId,
                TargetSystem = systemId,
                TargetNetwork = 0,
                MessageType = V2ExtensionMessageId,
            },
            Tags = message.Tags
        };
        // copy all tags
        wrappedPacket.Tags.AddRange(mavlink.Tags);
        
        
        return ValueTask.FromResult<IProtocolMessage?>(wrappedPacket);

    }

    public void Register(IProtocolConnection connection)
    {
        
    }

    public void Unregister(IProtocolConnection connection)
    {
        
    }

    
}