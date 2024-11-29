using System;
using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink;

public interface IMavlinkContext
{
    
}

public sealed class MavlinkContext : IMavlinkContext
{
    #region Static

    public static IMavlinkContext Create(IProtocolFactory protocol, Action<IMavlinkBuilder> mavlink)
    {
        var builder = new MavlinkBuilder(protocol);
        mavlink(builder);
        return builder.Create();
    }
    public static IMavlinkContext Create(Action<IProtocolBuilder> protocol, Action<IMavlinkBuilder> mavlink)
    {
        var factory = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
            builder.EnableBroadcastFeature<MavlinkMessage>();
            protocol(builder);
        });
        return Create(factory, mavlink);
    }

    #endregion
    
    private readonly IProtocolFactory _protocol;
    private readonly IEnumerable<IClientDeviceProvider> _deviceProviders;
    
    internal MavlinkContext(IProtocolFactory protocol, IEnumerable<IClientDeviceProvider> deviceProviders)
    {
        _protocol = protocol;
        _deviceProviders = deviceProviders;
    }

    
}