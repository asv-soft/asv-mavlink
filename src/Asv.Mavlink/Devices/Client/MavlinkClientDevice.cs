using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly MavlinkClientDeviceConfig _config;
    private readonly ProxyLinkIndicator _link;

    public MavlinkClientDevice(MavlinkClientDeviceId id, MavlinkClientDeviceConfig config, ImmutableArray<IClientDeviceExtender> extenders, IMavlinkContext context) 
        : base(id, config.BaseConfig, extenders, context)
    {
        _id = id;
        _config = config;
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(context);
        Core = context;
        _link = new ProxyLinkIndicator(LinkState.Connected);

    }

    protected MavlinkClientIdentity Identity => _id.Id;
    protected IMavlinkContext Core { get; }

    public override ILinkIndicator Link => _link;

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices([EnumeratorCancellation] CancellationToken cancel)
    {
        yield return await Task.Run(() =>
        {
            cancel.ThrowIfCancellationRequested();
            var heartBeat = new HeartbeatClient(_id.Id, _config.Heartbeat, Core);
            _link.UpdateSource(heartBeat.Link);
            return heartBeat;
        }, cancel).ConfigureAwait(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _link.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _link.DisposeAsync().ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);
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
