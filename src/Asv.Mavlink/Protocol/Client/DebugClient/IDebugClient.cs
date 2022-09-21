using System;
using System.Collections.Generic;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public interface IDebugClient:IDisposable
    {
        IObservable<KeyValuePair<string,float>> NamedFloatValue { get; }
        IObservable<KeyValuePair<string, int>> NamedIntValue { get; }
        IRxValue<DebugFloatArrayPayload> DebugFloatArray { get; }
    }
}
