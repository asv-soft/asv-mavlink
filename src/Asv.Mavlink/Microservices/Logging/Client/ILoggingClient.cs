using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a logging client that enables interaction with logging data.
    /// </summary>
    public interface ILoggingClient:IDisposable
    {
        /// <summary>
        /// Gets the raw logging data property.
        /// </summary>
        /// <value>
        /// The raw logging data property.
        /// </value>
        /// <remarks>
        /// This property provides access to the raw logging data in the form of an <see cref="IRxValue{T}"/> where T is <see cref="LoggingDataPayload"/>.
        /// </remarks>
        IRxValue<LoggingDataPayload?> RawLoggingData { get; }
    }
}
