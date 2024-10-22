using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a server for sending ADS-B vehicle payloads.
/// </summary>
public interface IAdsbVehicleServer : IMavlinkMicroserviceServer
{
    /// <summary>
    /// Sends a payload to the ADSB vehicle.
    /// </summary>
    /// <param name="fillCallback">Action for filling the payload with data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// // Creating a callback to fill the payload
    /// Action<AdsbVehiclePayload> fillCallback = payload =>
    /// {
    /// payload.Id = 1;
    /// payload.Velocity = 100;
    /// // Fill the payload with other necessary data
    /// };
    /// await Send(fillCallback);
    /// </code>
    /// </example>
    Task Send(Action<AdsbVehiclePayload> fillCallback);
}