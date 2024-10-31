using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

/// Represents a client for handling parameters.
/// /
public interface IParamsClient: IMavlinkMicroserviceClient
{
    /// <summary>
    /// Property that represents an observable sequence of ParamValuePayload objects.
    /// </summary>
    /// <remarks>
    /// The OnParamValue property can be subscribed to in order to receive ParamValuePayload objects when they are available.
    /// Each ParamValuePayload object represents a value update for a specific parameter.
    /// Subscribers can be notified multiple times as new ParamValuePayload objects are emitted.
    /// </remarks>
    /// <seealso cref="ParamValuePayload"/>
    /// <seealso cref="IObservable{T}"/>
    Observable<ParamValuePayload> OnParamValue { get; }

    /// <summary>
    /// Sends a request list asynchronously.
    /// </summary>
    /// <param name="cancel">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendRequestList(CancellationToken cancel = default);

    /// <summary>
    /// Reads the value of a specified name.
    /// </summary>
    /// <param name="name">The name of the value to read.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains a <see cref="ParamValuePayload"/> object representing the payload of the value read.</returns>
    Task<ParamValuePayload> Read(string name, CancellationToken cancel = default);

    /// <summary>
    /// Reads the value of a parameter at the specified index.
    /// </summary>
    /// <param name="index">The index of the parameter to read.</param>
    /// <param name="cancel">The cancellation token used to cancel the operation (optional).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a ParamValuePayload object.</returns>
    Task<ParamValuePayload> Read(ushort index, CancellationToken cancel = default);

    /// <summary>
    /// Writes a value to a parameter.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="cancel">(Optional) The cancellation token.</param>
    /// <returns>A task that represents the asynchronous write operation. The task result contains a ParamValuePayload.</returns>
    /// <remarks>
    /// This method writes the specified value to the parameter identified by its name and type. It returns a task that
    /// represents the asynchronous write operation. The task result contains a ParamValuePayload object containing the
    /// updated value of the parameter.
    /// </remarks>
    Task<ParamValuePayload> Write(string name, MavParamType type, float value, CancellationToken cancel = default);
}