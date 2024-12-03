using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;


public class MavlinkClientDeviceConfig:ClientDeviceConfig
{
    public HeartbeatClientConfig Heartbeat { get; set; } = new();
}
public class MavlinkClientDevice:ClientDevice<MavlinkClientDeviceId>
{
    private readonly MavlinkClientDeviceId _id;

    public MavlinkClientDevice(MavlinkClientDeviceId id, MavlinkClientDeviceConfig config, ImmutableArray<IClientDeviceExtender> extenders, ICoreServices context) 
        : base(id, config, extenders, context)
    {
        _id = id;
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(context);
        Core = context;
        Heartbeat = new HeartbeatClient(id.Id,config.Heartbeat, context);
    }

    protected MavlinkClientIdentity Identity => _id.Id;
    protected ICoreServices Core { get; }
    protected HeartbeatClient Heartbeat { get; }

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

    protected override TDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, IDeviceContext context,
        ImmutableArray<IClientDeviceExtender> extenders)
    {
        return InternalCreateDevice(msg, clientDeviceId, extenders, new CoreServices(seq,context));
    }
    
    protected abstract TDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId,
        ImmutableArray<IClientDeviceExtender> extenders, ICoreServices context);

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
