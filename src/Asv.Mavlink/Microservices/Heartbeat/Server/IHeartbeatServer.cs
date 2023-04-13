using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IHeartbeatServer:IDisposable
    {
        void Start();
        void Set(Action<HeartbeatPayload> changeCallback);
    }
}