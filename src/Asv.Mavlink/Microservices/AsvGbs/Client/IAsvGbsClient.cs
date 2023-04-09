using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;

namespace Asv.Mavlink
{
    public interface IAsvGbsClient
    {
        IRxValue<AsvGbsOutStatusPayload> RawStatus { get; }
        
    }
}