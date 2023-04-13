using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IAsvGbsExClient:IAsvGbsCommon
{
    IAsvGbsClient Base { get; }
}