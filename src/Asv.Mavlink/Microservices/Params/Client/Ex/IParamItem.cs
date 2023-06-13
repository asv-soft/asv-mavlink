using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IParamItem
{
    ushort Index { get;}
    string Name { get; }
    MavParamType Type { get; }
    ParamDescription Info { get; }
    IRxValue<bool> IsSynced { get; }
    IRxEditableValue<MavParamValue> Value { get; }
    Task Read(CancellationToken cancel = default);
    Task Write(CancellationToken cancel = default);
    
}