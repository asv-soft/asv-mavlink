using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public interface IPv2PowerCycle
    {
        IRxValue<bool> IsRebootRequired { get; }
        IRxValue<bool> CanReboot { get; }
        IRxValue<bool> CanPowerOff { get; }
        Task PowerOff(CancellationToken none);
        Task Reboot(CancellationToken none);
    }
}
