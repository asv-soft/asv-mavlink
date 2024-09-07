using System;
using System.Buffers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

public class VirtualMavlinkConnection:DisposableOnceWithCancel
{
    public VirtualMavlinkConnection(Func<IPacketV2<IPayload>, bool> clientToServerFilter = null,Func<IPacketV2<IPayload>, bool> serverToClientFilter = null,Action<IPacketDecoder<IPacketV2<IPayload>>> registerDialects = null)
    {
        clientToServerFilter ??= _ => true;
        serverToClientFilter ??= _ => true;
        var dialects = registerDialects ?? MavlinkV2Connection.RegisterDefaultDialects;
        
        var serverStream = new VirtualDataStream("server").DisposeItWith(Disposable);
        Server = MavlinkV2Connection.Create(serverStream,dialects,false,TaskPoolScheduler.Default).DisposeItWith(Disposable);
        var clientStream = new VirtualDataStream("client").DisposeItWith(Disposable);
        Client = MavlinkV2Connection.Create(clientStream,dialects,false,TaskPoolScheduler.Default).DisposeItWith(Disposable);
        
        var serverToClient = new PacketV2Decoder().DisposeItWith(Disposable);
        dialects(serverToClient);
        serverStream.TxPipe.Subscribe(serverToClient.OnData).DisposeItWith(Disposable);
        serverToClient.Where(serverToClientFilter).Select(Serialize).Subscribe(clientStream.RxPipe)
            .DisposeItWith(Disposable);
        
        var clientToServer = new PacketV2Decoder().DisposeItWith(Disposable);
        dialects(clientToServer);
        clientStream.TxPipe.Subscribe(clientToServer.OnData).DisposeItWith(Disposable);
        clientToServer.Where(clientToServerFilter).Select(Serialize).Subscribe(serverStream.RxPipe)
            .DisposeItWith(Disposable);
        
        
    }

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
}

public class VirtualDataStream : DisposableOnceWithCancel, IDataStream
{
    private readonly string _name;
    private long _rxBytes;
    private long _txBytes;
    private readonly Subject<byte[]> _txPipe;
    private readonly Subject<byte[]> _rxPipe;

    public VirtualDataStream(string name)
    {
        _name = name;
        _txPipe = new Subject<byte[]>().DisposeItWith(Disposable);
        _rxPipe = new Subject<byte[]>().DisposeItWith(Disposable);
        _rxPipe.Subscribe(b => Interlocked.Add(ref _rxBytes, b.Length));
    }

    public IDisposable Subscribe(IObserver<byte[]> observer)
    {
        return _rxPipe.Subscribe(observer);
    }

    public Task<bool> Send(byte[] data, int count, CancellationToken cancel)
    {
        return Task.Run(() =>
        {
            Interlocked.Add(ref _txBytes, count);
            var dataToSend = new byte[count];
            Array.Copy(data, dataToSend, count);
            _txPipe.OnNext(dataToSend);
            return true;
        }, cancel);
    }

    public Task<bool> Send(ReadOnlyMemory<byte> data, CancellationToken cancel)
    {
        return Task.Run(() =>
        {
            Interlocked.Add(ref _txBytes, data.Length);
            _txPipe.OnNext(data.ToArray());
            return true;
        }, cancel);
    }

    public IObserver<byte[]> RxPipe => _rxPipe;
    public IObservable<byte[]> TxPipe => _txPipe;

    public string Name => _name;

    public long RxBytes => _rxBytes;

    public long TxBytes => _txBytes;
}