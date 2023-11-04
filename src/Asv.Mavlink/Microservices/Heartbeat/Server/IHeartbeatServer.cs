using System;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink
{
    public interface IHeartbeatServer:IDisposable
    {
        void Start();
        void Set(Action<HeartbeatPayload> changeCallback);
    }
}
