using System;
using Asv.IO;

namespace Asv.Mavlink
{
    public interface IPacketDecoder<TFrame>: IDisposable, IObservable<TFrame>
        where TFrame: ISpanSerializable
    {
        void OnData(byte data);
        IObservable<DeserializePackageException> OutError { get; }
        void Register(Func<TFrame> factory);
        IPacketV2<IPayload> Create(int id);
    }
}
