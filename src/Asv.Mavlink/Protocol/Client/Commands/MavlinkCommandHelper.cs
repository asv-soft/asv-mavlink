using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public static class MavlinkCommandHelper
    {
        public static Task GetHomePosition(this IMavlinkCommandClient src, CancellationToken cancel)
        {
            return src.CommandLong(MavCmd.MavCmdGetHomePosition, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
                float.NaN, float.NaN, 1, cancel);
        }

       
    }

   
}