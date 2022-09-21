using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink.Client
{
    public interface IDgpsClient
    {
        Task SendRtcmData(byte[] data, int length, CancellationToken cancel);
    }
}