using System;

namespace Asv.Mavlink;



public interface IParamsServerEx
{
    IObservable<Exception> OnError { get; }
    IObservable<ParamChangedEvent> OnUpdated { get; }
    MavParamValue this[string name] { get; set; }
    MavParamValue this[IMavParamTypeMetadata param] { get; set; }
}

public static class ParamsServerExHelper
{
    
}

