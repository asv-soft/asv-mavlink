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
    ParamDescription Description { get; }
    IRxValue<bool> IsSynced { get; }
    IRxEditableValue<decimal> Value { get; }
   
    Task Read(CancellationToken cancel = default);
    Task Write(CancellationToken cancel = default);
}