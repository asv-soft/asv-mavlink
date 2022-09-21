using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink.Payload
{
    


    public interface IPv2ServerBaseInterface
    {
        void UpdateDeviceInfo(Action<Pv2DeviceInfo> editCallback);
        IEnumerable<IWorkModeFactory> Modes { get; }
        Task SetMode(byte modeIndex, CancellationToken cancel = default);
        Task SetMode(IWorkModeFactory mode);
    }
}
