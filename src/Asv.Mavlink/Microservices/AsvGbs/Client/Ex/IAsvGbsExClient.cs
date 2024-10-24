using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Interface for the ASV GBS Extended Client.
    /// </summary>
    public interface IAsvGbsExClient:IMavlinkMicroserviceClient
    {
        /// <summary>
        /// Gets the base client for the ASV GBS.
        /// </summary>
        IAsvGbsClient Base { get; }

        /// <summary>
        /// Gets the custom mode value.
        /// </summary>
        IRxValue<AsvGbsCustomMode> CustomMode { get; }

        /// <summary>
        /// Gets the geoposition value.
        /// </summary>
        IRxValue<GeoPoint> Position { get; }

        /// <summary>
        /// Gets the accuracy value in meters.
        /// </summary>
        IRxValue<double> AccuracyMeter { get; }

        /// <summary>
        /// Gets the value for observation per second.
        /// </summary>
        IRxValue<ushort> ObservationSec { get; }

        /// <summary>
        /// Gets the rate of DPBS.
        /// </summary>
        IRxValue<ushort> DgpsRate { get; }

        /// <summary>
        /// Gets the number of all satellites.
        /// </summary>
        IRxValue<byte> AllSatellites { get; }

        /// <summary>
        /// Gets the number of Galileo satellites.
        /// </summary>
        IRxValue<byte> GalSatellites { get; }

        /// <summary>
        /// Gets the number of Beidou satellites.
        /// </summary>
        IRxValue<byte> BeidouSatellites { get; }

        /// <summary>
        /// Gets the number of GLONASS satellites.
        /// </summary>
        IRxValue<byte> GlonassSatellites { get; }

        /// <summary>
        /// Gets the number of GPS satellites.
        /// </summary>
        IRxValue<byte> GpsSatellites { get; }

        /// <summary>
        /// Gets the number of QZSS satellites.
        /// </summary>
        IRxValue<byte> QzssSatellites { get; }

        /// <summary>
        /// Gets the number of SBAS satellites.
        /// </summary>
        IRxValue<byte> SbasSatellites { get; }

        /// <summary>
        /// Gets the number of IMES satellites.
        /// </summary>
        IRxValue<byte> ImesSatellites { get; }

        /// <summary>
        /// Start auto mode asynchronously.
        /// </summary>
        Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel);

        /// <summary>
        /// Start fixed mode asynchronously.
        /// </summary>
        Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel);

        /// <summary>
        /// Start idle mode asynchronously.
        /// </summary>
        Task<MavResult> StartIdleMode(CancellationToken cancel);
    }
}