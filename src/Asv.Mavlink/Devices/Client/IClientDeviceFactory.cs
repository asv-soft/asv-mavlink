using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

/// <summary>
/// Used to create a client device by providing a heartbeat packet.
/// </summary>
public interface IClientDeviceProvider
{
    int Order { get; }
    bool CanCreateDevice(HeartbeatPacket packet);
    IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core);
}

/// <summary>
/// Used to create a client device.
/// </summary>
public interface IClientDeviceFactory
{
    IClientDevice? Create(HeartbeatPacket packet, ICoreServices core);
}

public class ClientDeviceFactory : IClientDeviceFactory
{
    public const int DefaultOrder = 100;
    public const int MinimumOrder = int.MinValue;
    
    private readonly MavlinkIdentity _selfIdentity;
    private readonly ImmutableArray<IClientDeviceProvider> _providers;

    public ClientDeviceFactory(MavlinkIdentity selfIdentity, IEnumerable<IClientDeviceProvider> providers)
    {
        _selfIdentity = selfIdentity;
        _providers = [..providers.OrderByDescending(x => x.Order)];
    }
    public IClientDevice? Create(HeartbeatPacket packet, ICoreServices core)
    {
        foreach (var provider in _providers)
        {
            if (provider.CanCreateDevice(packet))
            {
                return provider.CreateDevice(packet,new MavlinkClientIdentity(_selfIdentity.SystemId,_selfIdentity.ComponentId,packet.SystemId,packet.ComponentId), core);
            }
        }
        return null;
    }
}