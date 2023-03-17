using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IAsvGbsServer
    {
        void Init(TimeSpan statusRate, IAsvGbsClient localImplementation);
    }
}