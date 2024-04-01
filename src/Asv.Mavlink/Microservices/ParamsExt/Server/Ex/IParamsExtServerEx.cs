using System;

namespace Asv.Mavlink;

public interface IParamsExtServerEx
{
    IObservable<Exception> OnError { get; }
    
    IObservable<ParamExtChangedEvent> OnUpdated { get; }

    MavParamExtValue this[string name] { get; set; }

    MavParamExtValue this[IMavParamExtTypeMetadata param] { get; set; }
}