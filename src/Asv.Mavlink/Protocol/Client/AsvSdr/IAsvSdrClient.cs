using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public interface IAsvSdrClient
{
    IRxValue<AsvSdrOutStatusPayload> Status { get; }
}