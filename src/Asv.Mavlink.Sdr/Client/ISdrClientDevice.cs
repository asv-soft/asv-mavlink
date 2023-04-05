using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public interface ISdrClientDevice
{
    IRxValue<AsvSdrCustomMode> CustomMode { get; }
    
}