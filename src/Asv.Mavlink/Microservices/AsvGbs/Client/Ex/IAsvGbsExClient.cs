using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using R3;

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
        ReadOnlyReactiveProperty<AsvGbsCustomMode> CustomMode { get; }

        /// <summary>
        /// Gets the geoposition value.
        /// </summary>
        ReadOnlyReactiveProperty<GeoPoint> Position { get; }

        /// <summary>
        /// Gets the accuracy value in meters.
        /// </summary>
        ReadOnlyReactiveProperty<double> AccuracyMeter { get; }

        /// <summary>
        /// Gets the value for observation per second.
        /// </summary>
        ReadOnlyReactiveProperty<ushort> ObservationSec { get; }

        /// <summary>
        /// Gets the rate of DPBS.
        /// </summary>
        ReadOnlyReactiveProperty<ushort> DgpsRate { get; }

        /// <summary>
        /// Gets the number of all satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> AllSatellites { get; }

        /// <summary>
        /// Gets the number of Galileo satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> GalSatellites { get; }

        /// <summary>
        /// Gets the number of Beidou satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> BeidouSatellites { get; }

        /// <summary>
        /// Gets the number of GLONASS satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> GlonassSatellites { get; }

        /// <summary>
        /// Gets the number of GPS satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> GpsSatellites { get; }

        /// <summary>
        /// Gets the number of QZSS satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> QzssSatellites { get; }

        /// <summary>
        /// Gets the number of SBAS satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> SbasSatellites { get; }

        /// <summary>
        /// Gets the number of IMES satellites.
        /// </summary>
        ReadOnlyReactiveProperty<byte> ImesSatellites { get; }

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