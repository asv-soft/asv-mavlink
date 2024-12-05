using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

public interface IParamsExtClient: IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets the observable sequence of ParamExtValuePayload objects.
    /// </summary>
    Observable<ParamExtValuePayload> OnParamExtValue { get; }
    
    /// <summary>
    /// Gets the observable sequence of ParamExtAckPayload objects.
    /// </summary>
    Observable<ParamExtAckPayload> OnParamExtAck { get; }

    /// <summary>
    /// Sends request to read all parameters.
    /// </summary>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>A task representing the asynchronous request of all parameters.</returns>
    ValueTask SendRequestList(CancellationToken cancel = default);

    /// <summary>
    /// Sends request to read a single parameter by its ID.
    /// </summary>
    /// <param name="name">The ID of the parameter to read.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>Requested parameter value.</returns>
    Task<ParamExtValuePayload> Read(string name, CancellationToken cancel = default);

    /// <summary>
    /// Sends request to read a single parameter by its index.
    /// </summary>
    /// <param name="index">The index of the parameter to read.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>Requested parameter value.</returns>
    Task<ParamExtValuePayload> Read(ushort index, CancellationToken cancel = default);
    
    /// <summary>
    /// Sends request to write a single parameter with the specified name, type, and value.
    /// </summary>
    /// <param name="name">The ID of the parameter to write.</param>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>Operation acknowledgement.</returns>
    Task<ParamExtAckPayload> Write(string name, MavParamExtType type, char[] value, CancellationToken cancel = default);
}