using System;
using Asv.IO;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a packet decoder that decodes byte data into frames of type TFrame.
    /// </summary>
    /// <typeparam name="TFrame">The type of frames that this decoder produces.</typeparam>
    public interface IPacketDecoder<TFrame>: IDisposable, IAsyncDisposable
        where TFrame: ISpanSerializable
    {
        /// <summary>
        /// Callback method called when new data is received.
        /// </summary>
        /// <param name="data">The received data as a byte array.</param>
        void OnData(byte[] data);
        Observable<DeserializePackageException> OutError { get; }
        Observable<TFrame> OnPacket { get; }

        /// <summary>
        /// Registers a factory function for creating <typeparamref name="TFrame"/> instances.
        /// </summary>
        /// <typeparam name="TFrame">The type of the frame to register.</typeparam>
        /// <param name="factory">A factory function that creates instances of <typeparamref name="TFrame"/>.</param>
        void Register(Func<TFrame> factory);

        /// <summary>
        /// Creates a new instance of MavlinkMessage with the specified ID.
        /// </summary>
        /// <param name="id">The ID for the packet.</param>
        /// <returns>A new instance of MavlinkMessage with the specified ID.</returns>
        MavlinkMessage? Create(int id);
    }
}
