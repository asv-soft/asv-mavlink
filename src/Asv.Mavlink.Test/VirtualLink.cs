using System;
using System.Buffers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Test;

public class VirtualLink:DisposableOnceWithCancel
{
    private readonly VirtualDataStream _serverStream;
    private readonly VirtualDataStream _clientStream;
    private readonly IMavlinkV2Connection _server;
    private readonly IMavlinkV2Connection _client;

    public VirtualLink(Func<IPacketV2<IPayload>, bool> clientToServerFilter = null,Func<IPacketV2<IPayload>, bool> serverToClientFilter = null)
    {
        clientToServerFilter ??= _ => true;
        serverToClientFilter ??= _ => true;
        
        _serverStream = new VirtualDataStream("server").DisposeItWith(Disposable);
        _server = MavlinkV2Connection.Create(_serverStream).DisposeItWith(Disposable);
        _clientStream = new VirtualDataStream("client").DisposeItWith(Disposable);
        _client = MavlinkV2Connection.Create(_clientStream).DisposeItWith(Disposable);
        
        var serverToClient = new PacketV2Decoder().DisposeItWith(Disposable);
        MavlinkV2Connection.RegisterDefaultDialects(serverToClient);
        _serverStream.TxPipe.Subscribe(serverToClient.OnData).DisposeItWith(Disposable);
        serverToClient.Where(serverToClientFilter).Select(Serialize).Subscribe(_clientStream.RxPipe)
            .DisposeItWith(Disposable);
        
        var clientToServer = new PacketV2Decoder().DisposeItWith(Disposable);
        MavlinkV2Connection.RegisterDefaultDialects(clientToServer);
        _clientStream.TxPipe.Subscribe(clientToServer.OnData).DisposeItWith(Disposable);
        clientToServer.Where(clientToServerFilter).Select(Serialize).Subscribe(_serverStream.RxPipe)
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
    public IMavlinkV2Connection Server => _server;
    public IMavlinkV2Connection Client => _client;
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
        _rxPipe.Subscribe(_ => Interlocked.Add(ref _rxBytes, _.Length));
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

    public IObserver<byte[]> RxPipe => _rxPipe;
    public IObservable<byte[]> TxPipe => _txPipe;

    public string Name => _name;

    public long RxBytes => _rxBytes;

    public long TxBytes => _txBytes;
}