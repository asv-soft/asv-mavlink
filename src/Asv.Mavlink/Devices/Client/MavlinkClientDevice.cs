using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;


public class MavlinkClientDeviceConfig : ICustomConfigurable
{
    private HeartbeatClientConfig _heartbeat = new();
    private ClientDeviceConfig _baseConfig = new();

    public HeartbeatClientConfig Heartbeat
    {
        get => _heartbeat;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _heartbeat = value;
        }
    }

    public ClientDeviceConfig BaseConfig
    {
        get => _baseConfig;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _baseConfig = value;
        }
    }

    public virtual void Load(string key, IConfiguration configuration)
    {
        Heartbeat = configuration.Get<HeartbeatClientConfig>();
        BaseConfig = configuration.Get<ClientDeviceConfig>();
    }

    public virtual void Save(string key, IConfiguration configuration)
    {
        configuration.Set(Heartbeat);
        configuration.Set(BaseConfig);
    }
}
public class MavlinkClientDevice:ClientDevice<MavlinkClientDeviceId>
{
    private readonly MavlinkClientDeviceId _id;

    public MavlinkClientDevice(MavlinkClientDeviceId id, MavlinkClientDeviceConfig config, ImmutableArray<IClientDeviceExtender> extenders, IMavlinkContext context) 
        : base(id, config.BaseConfig, extenders, context)
    {
        _id = id;
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(context);
        Core = context;
        Heartbeat = new HeartbeatClient(id.Id,config.Heartbeat, context);
    }

    protected MavlinkClientIdentity Identity => _id.Id;
    protected IMavlinkContext Core { get; }
    public HeartbeatClient Heartbeat { get; }

    public override ILinkIndicator Link => Heartbeat.Link;
    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices([EnumeratorCancellation] CancellationToken cancel)
    {
        cancel.ThrowIfCancellationRequested();
        await Heartbeat.Init(cancel).ConfigureAwait(false);
        yield return Heartbeat; 
    }
}

public abstract class MavlinkClientDeviceFactory<TDevice>(MavlinkIdentity selfId, IPacketSequenceCalculator seq)
    : ClientDeviceFactory<HeartbeatPacket, TDevice, MavlinkClientDeviceId>
    where TDevice : MavlinkClientDevice
{
    public abstract override int Order { get; }
    public abstract string DeviceClass { get; }
    protected override void InternalUpdateDevice(TDevice device, HeartbeatPacket msg)
    {
        // nothing to do
    }

    protected override TDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, IMicroserviceContext context,
        ImmutableArray<IClientDeviceExtender> extenders)
    {
        return InternalCreateDevice(msg, clientDeviceId, extenders, new CoreServices(seq,context));
    }
    
    protected abstract TDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId,
        ImmutableArray<IClientDeviceExtender> extenders, IMavlinkContext context);

    protected abstract bool CheckDevice(HeartbeatPacket msg);
    
    protected override bool InternalTryIdentify(HeartbeatPacket msg, out MavlinkClientDeviceId? deviceId)
    {
        if (CheckDevice(msg))
        {
            deviceId = new MavlinkClientDeviceId(DeviceClass, new MavlinkClientIdentity(selfId, msg.FullId));
            return true;
        }

        deviceId = null;
        return false;
    }
}
