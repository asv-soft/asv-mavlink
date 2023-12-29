using System;
using Asv.IO;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a packet decoder that decodes byte data into frames of type TFrame.
    /// </summary>
    /// <typeparam name="TFrame">The type of frames that this decoder produces.</typeparam>
    public interface IPacketDecoder<TFrame>: IDisposable, IObservable<TFrame>
        where TFrame: ISpanSerializable
    {
        /// <summary>
        /// Callback method called when new data is received.
        /// </summary>
        /// <param name="data">The received data as a byte array.</param>
        void OnData(byte[] data);

        /// <summary>
        /// Gets the property representing the output errors as an IObservable of DeserializePackageException.
        /// </summary>
        /// <remarks>
        /// The OutError property provides access to any errors that occur during the deserialization process.
        /// The errors are encapsulated in instances of the DeserializePackageException class.
        /// The property returns an IObservable<DeserializePackageException> object, which allows consuming code to observe the errors.
        /// </remarks>
        /// <returns>
        /// An IObservable<DeserializePackageException> representing the output errors during deserialization.
        /// </returns>
        IObservable<DeserializePackageException> OutError { get; }

        /// <summary>
        /// Registers a factory function for creating <typeparamref name="TFrame"/> instances.
        /// </summary>
        /// <typeparam name="TFrame">The type of the frame to register.</typeparam>
        /// <param name="factory">A factory function that creates instances of <typeparamref name="TFrame"/>.</param>
        void Register(Func<TFrame> factory);

        /// <summary>
        /// Creates a new instance of IPacketV2<IPayload> with the specified ID.
        /// </summary>
        /// <param name="id">The ID for the packet.</param>
        /// <returns>A new instance of IPacketV2<IPayload> with the specified ID.</returns>
        IPacketV2<IPayload> Create(int id);
    }
}
