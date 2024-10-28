using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using R3;

namespace Asv.Mavlink;

public sealed class VirtualMavlinkConnection:IDisposable, IAsyncDisposable
{
    private readonly IDisposable _disposeItAll;

    public VirtualMavlinkConnection(Func<IPacketV2<IPayload>, bool>? clientToServerFilter = null,Func<IPacketV2<IPayload>, bool>? serverToClientFilter = null,Action<IPacketDecoder<IPacketV2<IPayload>>>? registerDialects = null)
    {
        ClientToServerFilter = clientToServerFilter ?? (_ => true);
        ServerToClientFilter = serverToClientFilter ?? (_ => true);
        var builder = Disposable.CreateBuilder();
        var dialects = registerDialects ?? MavlinkV2Connection.RegisterDefaultDialects;
        
        var serverStream = new VirtualDataStream("server").AddTo(ref builder);
        Server = MavlinkV2Connection.Create(serverStream,dialects,false).AddTo(ref builder);
        var clientStream = new VirtualDataStream("client").AddTo(ref builder);
        Client = MavlinkV2Connection.Create(clientStream,dialects,false).AddTo(ref builder);
        
        var serverToClient = new PacketV2Decoder().AddTo(ref builder);
        dialects(serverToClient);
        serverStream.TxPipe.Subscribe(serverToClient.OnData).AddTo(ref builder);
        serverToClient.OnPacket.Where(ClientToServerFilter).Select(Serialize).Subscribe(clientStream.RxPipe)
            .AddTo(ref builder);
        
        var clientToServer = new PacketV2Decoder().AddTo(ref builder);
        dialects(clientToServer);
        clientStream.TxPipe.Subscribe(clientToServer.OnData).AddTo(ref builder);
        clientToServer.OnPacket.Where(ServerToClientFilter).Select(Serialize).Subscribe(serverStream.RxPipe)
            .AddTo(ref builder);
        
        _disposeItAll = builder.Build();
        
    }

    public Func<IPacketV2<IPayload>, bool> ClientToServerFilter { get; set; }
    public Func<IPacketV2<IPayload>, bool> ServerToClientFilter { get; set; }
    private byte[] Serialize(IPacketV2<IPayload> packet)
    {
        var data = ArrayPool<byte>.Shared.Rent(packet.GetMaxByteSize());
        try
        {
            var span = new Span<byte>(data);
            var size = span.Length;
            packet.Serialize(ref span);
            size -= span.Length;
            var result = new byte[size];
            Array.Copy(data, result, size);
            return result;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }
    public IMavlinkV2Connection Server { get; }

    public IMavlinkV2Connection Client { get; }

    public void Dispose()
    {
        _disposeItAll.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposeItAll is IAsyncDisposable disposeItAllAsyncDisposable)
            await disposeItAllAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            _disposeItAll.Dispose();
    }
}

public sealed class VirtualDataStream : IDataStream, IDisposable,IAsyncDisposable
{
    private long _rxBytes;
    private long _txBytes;
    private readonly Subject<byte[]> _txPipe;
    private readonly Subject<byte[]> _rxPipe;
    private readonly IDisposable _sub1;

    public VirtualDataStream(string name)
    {
        Name = name;
        _txPipe = new Subject<byte[]>();
        _rxPipe = new Subject<byte[]>();
        _sub1 = _rxPipe
            .Subscribe(this,(b, ctx) => Interlocked.Add(ref ctx._rxBytes, b.Length));
    }

    public IDisposable Subscribe(IObserver<byte[]> observer)
    {
        return _rxPipe.Subscribe(observer.ToObserver());
    }

    public Task<bool> Send(byte[] data, int count, CancellationToken cancel)
    {
        Interlocked.Add(ref _txBytes, count);
        var dataToSend = new byte[count];
        Array.Copy(data, dataToSend, count);
        _txPipe.OnNext(dataToSend);
        return Task.FromResult(true);
    }

    public Task<bool> Send(ReadOnlyMemory<byte> data, CancellationToken cancel)
    {
        Interlocked.Add(ref _txBytes, data.Length);
        _txPipe.OnNext(data.ToArray());
        return Task.FromResult(true);
    }

    public Observer<byte[]> RxPipe => _rxPipe.AsObserver();
    public Observable<byte[]> TxPipe => _txPipe;

    public string Name { get; }

    public long RxBytes => _rxBytes;

    public long TxBytes => _txBytes;

    #region Dispose
    public void Dispose()
    {
        _txPipe.Dispose();
        _rxPipe.Dispose();
        _sub1.Dispose();
    }
    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_txPipe).ConfigureAwait(false);
        await CastAndDispose(_rxPipe).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }
    #endregion
}