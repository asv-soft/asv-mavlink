using System;
using Asv.Mavlink.V2.AsvGbs;

namespace Asv.Mavlink
{
    public interface IAsvGbsServer
    {
        void Start();
        void Set(Action<AsvGbsOutStatusPayload> changeCallback);
    }
}