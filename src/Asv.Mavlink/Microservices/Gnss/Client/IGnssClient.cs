using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IGnssClient
{
    IRxValue<GpsRawIntPayload> Main { get; }
    IRxValue<Gps2RawPayload> Additional { get; }
}