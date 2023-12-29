using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a GPS client.
    /// </summary>
    public interface IDgpsClient
    {
        /// <summary>
        /// Sends RTCM data to a specific destination. </summary> <param name="data">The byte array containing the RTCM data to be sent.</param> <param name="length">The length of the RTCM data in bytes.</param> <param name="cancel">A CancellationToken that can be used to cancel the operation.</param> <returns>
        /// A Task representing the asynchronous operation. The Task completes when the RTCM data has been successfully sent or an error occurs. </returns>
        /// /
        Task SendRtcmData(byte[] data, int length, CancellationToken cancel);
    }
}