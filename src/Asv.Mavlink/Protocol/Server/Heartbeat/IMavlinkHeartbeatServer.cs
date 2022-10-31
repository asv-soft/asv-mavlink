using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IMavlinkHeartbeatServer:IDisposable
    {
        void Start();
        Task Set(Action<HeartbeatPayload> changeCallback);
    }
}
