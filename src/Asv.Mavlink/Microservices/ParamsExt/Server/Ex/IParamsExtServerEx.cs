using System;
using R3;

namespace Asv.Mavlink;

public interface IParamsExtServerEx
{
    Observable<Exception> OnError { get; }
    
    Observable<ParamExtChangedEvent> OnUpdated { get; }

    MavParamExtValue this[string name] { get; set; }

    MavParamExtValue this[IMavParamExtTypeMetadata param] { get; set; }
}