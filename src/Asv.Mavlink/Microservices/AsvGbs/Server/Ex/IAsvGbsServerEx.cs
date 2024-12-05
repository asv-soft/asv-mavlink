using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Delegate for starting auto mode.
    /// </summary>
    /// <param name="duration">The duration.</param>
    /// <param name="accuracy">The accuracy.</param>
    /// <param name="cancel">Cancellation token.</param>
    public delegate Task<MavResult> StartAutoModeDelegate(float duration, float accuracy, CancellationToken cancel);

    /// <summary>
    /// Delegate for starting fixed mode.
    /// </summary>
    /// <param name="geoPoint">The point of interest.</param>
    /// <param name="accuracy">The accuracy.</param>
    /// <param name="cancel">Cancellation token.</param>
    public delegate Task<MavResult> StartFixedModeDelegate(GeoPoint geoPoint, float accuracy, CancellationToken cancel);

    /// <summary>
    /// Delegate for starting idle mode.
    /// </summary>
    /// <param name="cancel">Cancellation token.</param>
    public delegate Task<MavResult> StartIdleModeDelegate(CancellationToken cancel);

    /// <summary>
    /// Extended interface for ASV GBS server.
    /// </summary>
    public interface IAsvGbsServerEx
    {
        public void Start();
        /// <summary>
        /// Gets the base server.
        /// </summary>
        IAsvGbsServer Base { get; }

        /// <summary>
        /// Gets the custom mode.
        /// </summary>
        ReactiveProperty<AsvGbsCustomMode> CustomMode { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        ReactiveProperty<GeoPoint> Position { get; }

        /// <summary>
        /// Gets the accuracy in meters.
        /// </summary>
        ReactiveProperty<double> AccuracyMeter { get; }

        /// <summary>
        /// Gets the number of seconds since the observation.
        /// </summary>
        ReactiveProperty<ushort> ObservationSec { get; }

        /// <summary>
        /// Gets the DGPS rate.
        /// </summary>
        ReactiveProperty<ushort> DgpsRate { get; }

        /// <summary>
        /// Gets the number of all satellites.
        /// </summary>
        ReactiveProperty<byte> AllSatellites { get; }

        /// <summary>
        /// Gets the number of GAL satellites.
        /// </summary>
        ReactiveProperty<byte> GalSatellites { get; }

        /// <summary>
        /// Gets the number of Beidou satellites.
        /// </summary>
        ReactiveProperty<byte> BeidouSatellites { get; }

        /// <summary>
        /// Gets the number of Glonass satellites.
        /// </summary>
        ReactiveProperty<byte> GlonassSatellites { get; }

        /// <summary>
        /// Gets the number of GPS satellites.
        /// </summary>
        ReactiveProperty<byte> GpsSatellites { get; }

        /// <summary>
        /// Gets the number of QZSS satellites.
        /// </summary>
        ReactiveProperty<byte> QzssSatellites { get; }

        /// <summary>
        /// Gets the number of SBAS satellites.
        /// </summary>
        ReactiveProperty<byte> SbasSatellites { get; }

        /// <summary>
        /// Gets the number of IMES satellites.
        /// </summary>
        ReactiveProperty<byte> ImesSatellites { get; }

        /// <summary>
        /// Gets or sets the delegate for starting auto mode.
        /// </summary>
        StartAutoModeDelegate? StartAutoMode { set; }

        /// <summary>
        /// Gets or sets the delegate for starting fixed mode.
        /// </summary>
        StartFixedModeDelegate? StartFixedMode { set; }

        /// <summary>
        /// Gets or sets the delegate for starting idle mode.
        /// </summary>
        StartIdleModeDelegate? StartIdleMode { set; }

        /// <summary>
        /// Method to send RTCM data.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="length">The length of the data.</param>
        /// <param name="cancel">Cancellation token.</param>
        Task SendRtcmData(byte[] data, int length, CancellationToken cancel);
    }
}