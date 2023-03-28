using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvGbs;

namespace Asv.Mavlink
{
    public interface IAsvGbsServer
    {
        void Start(TimeSpan statusRate);
        Task Set(Action<AsvGbsOutStatusPayload> changeCallback);
    }
}