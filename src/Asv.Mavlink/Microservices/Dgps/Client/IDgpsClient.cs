using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink
{
    public interface IDgpsClient
    {
        Task SendRtcmData(byte[] data, int length, CancellationToken cancel);
    }
}