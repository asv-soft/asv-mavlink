using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
    public interface IAsvSdrServer
    {
        void Start(TimeSpan statusRate);
        Task Set(Action<AsvSdrOutStatusPayload> changeCallback);
    }
}