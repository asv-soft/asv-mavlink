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
    Task Send(Action<AdsbVehiclePayload> fillCallback);
}