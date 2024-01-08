using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

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
        /// <summary>
        /// Gets the base server.
        /// </summary>
        IAsvGbsServer Base { get; }

        /// <summary>
        /// Gets the custom mode.
        /// </summary>
        IRxEditableValue<AsvGbsCustomMode> CustomMode { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        IRxEditableValue<GeoPoint> Position { get; }

        /// <summary>
        /// Gets the accuracy in meters.
        /// </summary>
        IRxEditableValue<double> AccuracyMeter { get; }

        /// <summary>
        /// Gets the number of seconds since the observation.
        /// </summary>
        IRxEditableValue<ushort> ObservationSec { get; }

        /// <summary>
        /// Gets the DGPS rate.
        /// </summary>
        IRxEditableValue<ushort> DgpsRate { get; }

        /// <summary>
        /// Gets the number of all satellites.
        /// </summary>
        IRxEditableValue<byte> AllSatellites { get; }

        /// <summary>
        /// Gets the number of GAL satellites.
        /// </summary>
        IRxEditableValue<byte> GalSatellites { get; }

        /// <summary>
        /// Gets the number of Beidou satellites.
        /// </summary>
        IRxEditableValue<byte> BeidouSatellites { get; }

        /// <summary>
        /// Gets the number of Glonass satellites.
        /// </summary>
        IRxEditableValue<byte> GlonassSatellites { get; }

        /// <summary>
        /// Gets the number of GPS satellites.
        /// </summary>
        IRxEditableValue<byte> GpsSatellites { get; }

        /// <summary>
        /// Gets the number of QZSS satellites.
        /// </summary>
        IRxEditableValue<byte> QzssSatellites { get; }

        /// <summary>
        /// Gets the number of SBAS satellites.
        /// </summary>
        IRxEditableValue<byte> SbasSatellites { get; }

        /// <summary>
        /// Gets the number of IMES satellites.
        /// </summary>
        IRxEditableValue<byte> ImesSatellites { get; }

        /// <summary>
        /// Gets or sets the delegate for starting auto mode.
        /// </summary>
        StartAutoModeDelegate StartAutoMode { set; }

        /// <summary>
        /// Gets or sets the delegate for starting fixed mode.
        /// </summary>
        StartFixedModeDelegate StartFixedMode { set; }

        /// <summary>
        /// Gets or sets the delegate for starting idle mode.
        /// </summary>
        StartIdleModeDelegate StartIdleMode { set; }

        /// <summary>
        /// Method to send RTCM data.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="length">The length of the data.</param>
        /// <param name="cancel">Cancellation token.</param>
        Task SendRtcmData(byte[] data, int length, CancellationToken cancel);
    }
}