using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class MavlinkV2WrapFeatureHelper
{
    public static void RegisterMavlinkV2WrapFeature(this IProtocolFeatureBuilder builder,IProtocolMessageFactory<MavlinkMessage, ushort>? factory = null)
    {
        builder.Register(new MavlinkV2WrapFeature(factory));
    }
}

public class MavlinkV2WrapFeature(IProtocolMessageFactory<MavlinkMessage, ushort>? factory = null) : IProtocolFeature
{
    private readonly IProtocolMessageFactory<MavlinkMessage,ushort> _factory = factory ?? MavlinkV2MessageFactory.Instance;
    public const string IdKey = "MavlinkV2WrapFeature";
    public const ushort V2ExtensionMessageId = 32535;

    public string Name => $"Mavlink {nameof(V2ExtensionPacket)} wrapper";
    public string Description => $"Wrap packages to {nameof(V2ExtensionPacket)}";
    public string Id => IdKey;
    public int Priority => 0;

    public async ValueTask<IProtocolMessage?> ProcessRx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        if (connection is not IProtocolPort) return message;
        if (message is not V2ExtensionPacket mavlink) return message;
        if (mavlink.Payload.MessageType != V2ExtensionMessageId) return message;
        var messageId = MavlinkV2Protocol.GetMessageId(mavlink.Payload.Payload,0);
        var innerMessage = _factory.Create((ushort)messageId);
        if (innerMessage == null) return message;
        await innerMessage.Deserialize(mavlink.Payload.Payload).ConfigureAwait(false);
        return innerMessage;
    }

    public async ValueTask<IProtocolMessage?> ProcessTx(IProtocolMessage message, IProtocolConnection connection, CancellationToken cancel)
    {
        if (connection is not IProtocolEndpoint && connection is not VirtualPort) return message;
        if (message is not MavlinkV2Message mavlink) return message;

        if (mavlink.WrapToV2Extension == false)
        {
            return message;
        }
        
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
                TargetComponent = 0,
                TargetSystem = 0,
                TargetNetwork = 0,
                MessageType = V2ExtensionMessageId
            },
            Tags = message.Tags
        };
        var size = await message.Serialize(wrappedPacket.Payload.Payload).ConfigureAwait(false);
        var arr = wrappedPacket.Payload.Payload;
        Array.Resize(ref arr, size);
        wrappedPacket.Payload.Payload = arr;
        return wrappedPacket;

    }

    public void Register(IProtocolConnection connection)
    {
        
    }

    public void Unregister(IProtocolConnection connection)
    {
        
    }

    
}