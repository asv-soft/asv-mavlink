using System.Collections.Generic;
using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink;

public interface IMavlinkBuilder
{
    IProtocolFactory Protocol { get; }
    void SetIdentity(MavlinkIdentity selfIdentity);
    void SetPacketSequence(IPacketSequenceCalculator sequence);
    void SetConnection(IProtocolConnection connection);
    void SetConfiguration(IConfiguration configuration);
}

public interface IMavlinkClientBuilder:IMavlinkBuilder
{
    void RegisterClientDevice(IClientDeviceProvider deviceProvider);
    IMavlinkClientContext Create();
}

internal class MavlinkBuilder : IMavlinkBuilder
{
    private readonly List<IClientDeviceProvider> _deviceProviders = new();
    private IPacketSequenceCalculator? _sequence;

    public MavlinkBuilder(IProtocolFactory protocol)
    {
        Protocol = protocol;
    }
    public IMavlinkContext Create()
    {
        var deviceFactory = new ClientDeviceFactory(_deviceProviders);
        return new MavlinkContext(this.Create(), _deviceProviders);
    }

    public void RegisterMavlinkDevice(IClientDeviceProvider deviceProvider)
    {
        _deviceProviders.Add(deviceProvider);
    }

    public IProtocolFactory Protocol { get; }
    public void SetPacketSequence(IPacketSequenceCalculator sequence)
    {
        _sequence = sequence;
    }
}