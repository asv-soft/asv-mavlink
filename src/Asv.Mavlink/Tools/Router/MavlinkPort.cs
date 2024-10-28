using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using R3;

namespace Asv.Mavlink;

public sealed class MavlinkPort : IDisposable, IAsyncDisposable
{
    private readonly MavlinkPortConfig _config;
    private int _deserializeError;
    private long _txPackets;
    private readonly IDisposable _sub;
    public Guid Id { get; }

    public MavlinkPort(MavlinkPortConfig config, Action<IPacketDecoder<IPacketV2<IPayload>>> register, Guid id)
    {
        _config = config;
        Id = id;
        Port = PortFactory.Create(config.ConnectionString, config.IsEnabled);
        Connection = new MavlinkV2Connection(Port, register);
        _sub = Connection.DeserializePackageErrors.Subscribe(_ => Interlocked.Increment(ref _deserializeError));
    }

    public IMavlinkV2Connection Connection { get; }
    public IPort Port { get; }

       
    internal Task<bool> InternalSendSerializedPacket(byte[] data, int count, CancellationToken cancel)
    {
        // we have to manually increment the packet counter because we serialize the packet once into an array of bytes and send
        Interlocked.Increment(ref _txPackets);
        return Port.Send(data, count, cancel);
    }
    internal Task<bool> InternalSendSerializedPacket(ReadOnlyMemory<byte> data, CancellationToken cancel)
    {
        // we have to manually increment the packet counter because we serialize the packet once into an array of bytes and send
        Interlocked.Increment(ref _txPackets);
        return Port.Send(data, cancel);
    }

    public MavlinkPortInfo GetInfo()
    {
        return new MavlinkPortInfo
        {
            Id = Id,
            Name = _config.Name,
            ConnectionString = _config.ConnectionString,
            State = Port.State.Value,
            RxBytes = Port.RxBytes,
            TxBytes = Port.TxBytes,
            RxPackets = Connection.RxPackets,
            // we have to manually increment the packet counter because we serialize the packet once into an array of bytes and send
            TxPackets = Interlocked.Read(ref _txPackets),
            SkipPackets = Connection.SkipPackets,
            DeserializationErrors = _deserializeError,
            Description = Port.ToString(),
            LastException = Port.Error.Value,
            Type = Port.PortType,
            IsEnabled = Port?.IsEnabled.Value,
            PacketLossChance = _config.PacketLossChance
        };
    }

    public MavlinkPortConfig GetConfig()
    {
        return new MavlinkPortConfig
        {
            ConnectionString = _config.ConnectionString,
            Name = _config.Name,
            IsEnabled = Port.IsEnabled.Value
        };
    }

    public void Dispose()
    {
        _sub.Dispose();
        Connection.Dispose();
        Port.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_sub).ConfigureAwait(false);
        await Connection.DisposeAsync().ConfigureAwait(false);
        await CastAndDispose(Port).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }
}