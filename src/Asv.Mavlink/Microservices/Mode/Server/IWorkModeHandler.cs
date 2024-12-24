using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IWorkModeHandler:IDisposable
{
    ICustomMode Mode { get; }
    Task Init(CancellationToken cancel);
    Task Destroy(CancellationToken cancel);
}