using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

/// <summary>
/// Represents a server for sending ADS-B vehicle payloads.
/// </summary>
public interface IAdsbVehicleServer : IMavlinkMicroserviceServer
{
    /// <summary>
    /// Sends a payload to the ADSB vehicle.
    /// </summary>
    ValueTask Send(Action<AdsbVehiclePayload> fillCallback, CancellationToken cancel);
}