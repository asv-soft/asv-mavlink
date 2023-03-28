using System;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;

namespace Asv.Mavlink
{
    public interface IAsvGbsClient:IDisposable
    {
        IRxValue<AsvGbsOutStatusPayload> Status { get; }
        
    }
}