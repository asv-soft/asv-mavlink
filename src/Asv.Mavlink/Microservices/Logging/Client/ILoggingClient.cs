using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface ILoggingClient:IDisposable
    {
        IRxValue<LoggingDataPayload> RawLoggingData { get; }
    }
}
